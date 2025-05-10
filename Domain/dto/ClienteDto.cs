using System;
using System.ComponentModel.DataAnnotations;

namespace SGIC_APP.Domain.dto
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public required string TerceroId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string Email { get; set; }
        public int TipoTerceroId { get; set; }
        public int CiudadId { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaUltimaCompra { get; set; }
        public string? Telefono { get; set; }
        public string? TipoTelefono { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int TipoDocId { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellidos}";
    }
}