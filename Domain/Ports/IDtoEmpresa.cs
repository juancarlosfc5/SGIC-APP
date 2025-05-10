using System.Collections.Generic;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoEmpresa<T> where T : Empresa
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(string id);
        void Crear(T empresa);
        void Actualizar(T empresa);
        void Eliminar(string id);
    }
} 