using System.Collections.Generic;
using SGIC_APP.Domain.dto;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoEmpleado<T> where T : class
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(string id);
        void Crear(T dto);
        void Actualizar(T dto);
        void Eliminar(string id);
    }
} 