using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGIC_APP.Domain.dto
{
    public class ClienteDto
    {
        // Terceros
        public int Id { get; set; }

        public string? nombre { get; set; }

        public string? apellidos { get; set; }

        public string? email { get; set; }

        public int tipo_doc_id { get; set; }

        public int tipo_tercero_id { get; set; }

        // Clientes

        public DateTime fecha_nac { get; set; }

        public DateTime ultima_compra { get; set; }
    }
}