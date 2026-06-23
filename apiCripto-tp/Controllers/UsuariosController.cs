using apiCripto_tp.Data;
using apiCripto_tp.Models;
using apiCripto_tp.Models.Dtos;
using apiCripto_tp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCripto_tp.Controllers
{
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _contexto;

        public UsuariosController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nombre = dto.Nombre_Usuario.Trim();

            bool existe = await _contexto.Usuarios.AnyAsync(u => u.Nombre_Usuario == nombre);
            if (existe)
                return Conflict(new { error = "El nombre de usuario ya está en uso." });

            var usuario = new Usuarios
            {
                Nombre_Usuario = nombre,
                Contrasenia = PasswordHasher.Hash(dto.Contrasenia)
            };

            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();

            return Ok(new LoginRespuestaDto
            {
                Ok = true,
                Mensaje = "Usuario registrado.",
                Nombre_Usuario = usuario.Nombre_Usuario
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Nombre_Usuario == dto.Nombre_Usuario.Trim());

            if (usuario is null || !PasswordHasher.Verify(dto.Contrasenia, usuario.Contrasenia))
            {
                return Unauthorized(new LoginRespuestaDto
                {
                    Ok = false,
                    Mensaje = "Usuario o contraseña incorrectos."
                });
            }

            return Ok(new LoginRespuestaDto
            {
                Ok = true,
                Mensaje = "Login correcto.",
                Nombre_Usuario = usuario.Nombre_Usuario
            });
        }
    }
}
