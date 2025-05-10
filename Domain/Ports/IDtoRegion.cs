using System.Collections.Generic;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoRegion<T> where T : Region
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(int id);
        void Crear(T region);
        void Actualizar(T region);
        void Eliminar(int id);
    }
} 