using System.Collections.Generic;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoCiudad<T> where T : Ciudad
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(int id);
        void Crear(T ciudad);
        void Actualizar(T ciudad);
        void Eliminar(int id);
    }
} 