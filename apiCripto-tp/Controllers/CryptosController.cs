using apiCripto_tp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static apiCripto_tp.Services.CriptoYaPrecio;

namespace apiCripto_tp.Controllers
{
    [ApiController]
    [Route("cryptos")]
    public class CryptosController : ControllerBase
    {
        private readonly ApplicationDbContext _contexto;
        private readonly ICriptoYaService _criptoya;

        public CryptosController(ApplicationDbContext contexto, ICriptoYaService criptoya)
        {
            _contexto = contexto;
            _criptoya = criptoya;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var cryptos = await _contexto.Cryptos
                .OrderBy(c => c.Nombre)
                .Select(c => new { c.Codigo, c.Nombre, c.Simbolo })
                .ToListAsync();

            return Ok(cryptos);
        }

        [HttpGet("{code}/precio")]
        public async Task<IActionResult> Precio(string code)
        {
            var precio = await _criptoya.ObtenerPrecioAsync(code.ToLower());
            if (precio is null)
                return StatusCode(502, new { error = "No se pudo obtener el precio." });

            return Ok(new
            {
                crypto_code = code.ToLower(),
                compra = precio.TotalAsk,
                venta = precio.TotalBid,
                time = precio.Time
            });
        }
    }
}
