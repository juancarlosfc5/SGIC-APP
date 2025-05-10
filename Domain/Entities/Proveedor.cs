using System;

namespace SGIC_APP.Domain.Entities
{
    public class Proveedor
    {
        public string? TerceroId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? TipoTelefono { get; set; }
        public int TipoDocId { get; set; }
        public int TipoTerceroId { get; set; }
        public int CiudadId { get; set; }
        public double Descuento { get; set; }
        public int DiaPago { get; set; }
        public string? Direccion { get; set; }
        public string? Contacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
} 