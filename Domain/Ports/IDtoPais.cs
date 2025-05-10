using System.Collections.Generic;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoPais<T> where T : Pais
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(int id);
        void Crear(T pais);
        void Actualizar(T pais);
        void Eliminar(int id);
    }
} 