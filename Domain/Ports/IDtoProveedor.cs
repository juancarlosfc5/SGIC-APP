using System.Collections.Generic;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoProveedor<T> where T : Proveedor
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(string id);
        void Crear(T proveedor);
        void Actualizar(T proveedor);
        void Eliminar(string id);
    }
} 