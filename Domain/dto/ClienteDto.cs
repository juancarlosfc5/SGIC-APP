using System;
using System.ComponentModel.DataAnnotations;

namespace SGIC_APP.Domain.dto
{
    public class ClienteDto
    {
        public string? TerceroId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [StringLength(50, ErrorMessage = "Los apellidos no pueden tener más de 50 caracteres")]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(80, ErrorMessage = "El email no puede tener más de 80 caracteres")]
        public string? Email { get; set; }

        public string? Telefono { get; set; }
        public string? TipoTelefono { get; set; }

        [Required(ErrorMessage = "El tipo de documento es requerido")]
        public int TipoDocId { get; set; }

        [Required(ErrorMessage = "El tipo de tercero es requerido")]
        public int TipoTerceroId { get; set; }

        [Required(ErrorMessage = "La ciudad es requerida")]
        public int CiudadId { get; set; }

        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaUltimaCompra { get; set; }

        public string? Direccion { get; set; }
        public bool Activo { get; set; } = true;

        public string NombreCompleto => $"{Nombre} {Apellidos}";
    }
}