using System.Collections.Generic;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoCliente<T> where T : class
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(string id);
        void Crear(T dto);
        void Actualizar(T dto);
        void Eliminar(string id);
    }
}