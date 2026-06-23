using apiCripto_tp.Data;
using apiCripto_tp.Models;
using apiCripto_tp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static apiCripto_tp.Services.CriptoYaPrecio;

namespace apiCripto_tp.Controllers
{
    public class TransaccionesController : ControllerBase
    {
        private readonly ApplicationDbContext _contexto;
        private readonly ICriptoYaService _criptoya;

        private static readonly string[] AccionesValidas = { "purchase", "sale" };

        public TransaccionesController(ApplicationDbContext contexto, ICriptoYaService criptoya)
        {
            _contexto = contexto;
            _criptoya = criptoya;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearTransaccionDto dto)
        {
            if (dto is null)
                return BadRequest(new { error = "Cuerpo vacío." });

            var code = (dto.CryptoCode ?? "").Trim().ToLower();
            var action = (dto.Action ?? "").Trim().ToLower();

            if (!AccionesValidas.Contains(action))
                return BadRequest(new { error = "La acción debe ser 'purchase' o 'sale'." });

            if (dto.CryptoAmount <= 0)
                return BadRequest(new { error = "La cantidad debe ser mayor a 0." });

            var cripto = await _contexto.Cryptos.FirstOrDefaultAsync(c => c.Codigo == code);
            if (cripto is null)
                return BadRequest(new { error = $"La criptomoneda '{code}' no está soportada." });

            if (!TryParseFecha(dto.Datetime, out var fecha))
                return BadRequest(new { error = "Fecha inválida. Usá el formato 'YYYY-MM-DD HH:mm' o ISO." });

            if (action == "sale")
            {
                var tenencia = await CalcularTenenciaAsync(code);
                if (dto.CryptoAmount > tenencia)
                    return BadRequest(new
                    {
                        error = $"No podés vender {dto.CryptoAmount} {cripto.Simbolo}: " +
                                $"solo tenés {tenencia} {cripto.Simbolo}."
                    });
            }


            var precio = await _criptoya.ObtenerPrecioAsync(code);
            if (precio is null)
                return StatusCode(502, new { error = "No se pudo obtener el precio desde CriptoYa." });


            decimal precioUnitario = action == "purchase" ? precio.TotalAsk : precio.TotalBid;
            decimal montoTotal = Math.Round(precioUnitario * dto.CryptoAmount, 2);

            var transaccion = new Transaccion
            {
                CryptoCode = code,
                Action = action,
                CryptoAmount = dto.CryptoAmount,
                Money = montoTotal,
                Datetime = fecha
            };

            _contexto.Transacciones.Add(transaccion);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(Obtener), new { id = transaccion.Id }, AMapDto(transaccion));
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var lista = await _contexto.Transacciones
                .OrderByDescending(t => t.Datetime)
                .Select(t => AMapDto(t))
                .ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var t = await _contexto.Transacciones.FindAsync(id);
            if (t is null)
                return NotFound(new { error = "Transacción no encontrada." });

            return Ok(AMapDto(t));
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Editar(int id, [FromBody] EditarTransaccionDto dto)
        {
            var t = await _contexto.Transacciones.FindAsync(id);
            if (t is null)
                return NotFound(new { error = "Transacción no encontrada." });

            if (dto.CryptoCode is not null) t.CryptoCode = dto.CryptoCode.Trim().ToLower();
            if (dto.Action is not null) t.Action = dto.Action.Trim().ToLower();
            if (dto.CryptoAmount is not null) t.CryptoAmount = dto.CryptoAmount.Value;
            if (dto.Money is not null) t.Money = dto.Money.Value;
            if (dto.Datetime is not null) t.Datetime = dto.Datetime.Value;

            await _contexto.SaveChangesAsync();
            return Ok(AMapDto(t));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Borrar(int id)
        {
            var t = await _contexto.Transacciones.FindAsync(id);
            if (t is null)
                return NotFound(new { error = "Transacción no encontrada." });

            _contexto.Transacciones.Remove(t);
            await _contexto.SaveChangesAsync();
            return Ok(new { estado = "ok" });
        }

        [HttpGet("estado")]
        public async Task<IActionResult> Estado()
        {
            var transacciones = await _contexto.Transacciones.ToListAsync();
            var cryptos = await _contexto.Cryptos.ToListAsync();

            var resultado = new EstadoActualDto();

            var porCripto = transacciones
                .GroupBy(t => t.CryptoCode)
                .Select(g => new
                {
                    Code = g.Key,
                    Cantidad = g.Where(x => x.Action == "purchase").Sum(x => x.CryptoAmount)
                             - g.Where(x => x.Action == "sale").Sum(x => x.CryptoAmount)
                })
                .Where(x => x.Cantidad > 0);

            foreach (var item in porCripto)
            {
                var cripto = cryptos.FirstOrDefault(c => c.Codigo == item.Code);
                var precio = await _criptoya.ObtenerPrecioAsync(item.Code);
                if (precio is null) continue;

                decimal dinero = Math.Round(item.Cantidad * precio.TotalBid, 2);

                resultado.Cryptos.Add(new EstadoCryptoDto
                {
                    CryptoCode = item.Code,
                    Nombre = cripto?.Nombre ?? item.Code,
                    Cantidad = item.Cantidad,
                    Dinero = dinero
                });
                resultado.Total += dinero;
            }

            return Ok(resultado);
        }

        private async Task<decimal> CalcularTenenciaAsync(string code)
        {
            var compras = await _contexto.Transacciones
                .Where(t => t.CryptoCode == code && t.Action == "purchase")
                .SumAsync(t => (decimal?)t.CryptoAmount) ?? 0;

            var ventas = await _contexto.Transacciones
                .Where(t => t.CryptoCode == code && t.Action == "sale")
                .SumAsync(t => (decimal?)t.CryptoAmount) ?? 0;

            return compras - ventas;
        }

        private static bool TryParseFecha(string texto, out DateTime fecha)
        {
            texto = (texto ?? "").Trim();
            string[] formatos =
            {
                "yyyy-MM-dd HH:mm", "yyyy-MM-ddTHH:mm", "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-ddTHH:mm:ss", "dd-MM-yyyy HH:mm", "dd/MM/yyyy HH:mm"
            };

            if (DateTime.TryParseExact(texto, formatos, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out fecha))
                return true;


            return DateTime.TryParse(texto, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal, out fecha);
        }

        private static TransaccionDto AMapDto(Transaccion t) => new()
        {
            Id = t.Id,
            CryptoCode = t.CryptoCode,
            Action = t.Action,
            CryptoAmount = t.CryptoAmount,
            Money = t.Money,
            Datetime = t.Datetime
        };
    }
}
