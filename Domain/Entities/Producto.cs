using System;

namespace SGIC_APP.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Barcode { get; set; }
    }
} 