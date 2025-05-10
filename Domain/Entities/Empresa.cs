using System;

namespace SGIC_APP.Domain.Entities
{
    public class Empresa
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public int CiudadId { get; set; }
        public DateTime FechaReg { get; set; }
    }
} 