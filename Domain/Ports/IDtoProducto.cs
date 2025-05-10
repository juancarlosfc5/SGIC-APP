using System.Collections.Generic;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Domain.Ports
{
    public interface IDtoProducto<T> where T : Producto
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(int id);
        void Crear(T producto);
        void Actualizar(T producto);
        void Eliminar(int id);
        IEnumerable<T> ObtenerPorCategoria(int categoriaId);
        IEnumerable<T> ObtenerPorProveedor(int proveedorId);
        IEnumerable<T> ObtenerProductosBajoStock();
        void ActualizarStock(int id, int cantidad);
    }
} 