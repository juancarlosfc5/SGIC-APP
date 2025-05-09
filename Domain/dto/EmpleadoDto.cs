using System;

namespace SGIC_APP.Domain.dto
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public required string TerceroId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string Email { get; set; }
        public required string Telefono { get; set; }
        public required string TipoTelefono { get; set; }
        public int TipoDocId { get; set; }
        public int TipoTerceroId { get; set; }
        public int CiudadId { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaContratacion { get; set; }
        public string? Cargo { get; set; }
        public double? Salario { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public double SalarioBase { get; set; }
        public int? EpsId { get; set; }
        public int? ArlId { get; set; }
        public bool Activo { get; set; } = true;
        public string NombreCompleto => $"{Nombre} {Apellidos}";
    }
} 