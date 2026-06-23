using System.ComponentModel.DataAnnotations;

namespace apiCripto_tp.Models.Dtos
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string Nombre_Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres")]
        public string Contrasenia { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string Nombre_Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasenia { get; set; } = string.Empty;
    }

    public class LoginRespuestaDto
    {
        public bool Ok { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Nombre_Usuario { get; set; } = string.Empty;
    }
}
