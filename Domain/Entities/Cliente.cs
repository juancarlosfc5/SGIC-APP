using System;

namespace SGICAPP.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public DateTime fecha_nac { get; set; }

    public DateTime fecha_ultima_compra { get; set; }

    public string? tercero_id { get; set; }
}