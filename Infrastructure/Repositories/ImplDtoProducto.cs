using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoProducto : IDtoProducto<Producto>
    {
        private readonly string _connectionString;

        public ImplDtoProducto(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            var productos = new List<Producto>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT * FROM producto
                        ORDER BY id DESC", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    Stock = reader.GetInt32("stock"),
                                    StockMin = reader.GetInt32("stock_min"),
                                    StockMax = reader.GetInt32("stock_max"),
                                    CreatedAt = reader.GetDateTime("created_at"),
                                    UpdatedAt = reader.GetDateTime("updated_at"),
                                    Barcode = reader.GetString("barcode"),
                                    PrecioCompra = reader.GetDouble("precio_compra"),
                                    PrecioVenta = reader.GetDouble("precio_venta"),
                                    CategoriaId = reader.GetInt32("categoria_id"),
                                    ProveedorId = reader.GetInt32("proveedor_id"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
                                    Activo = reader.GetBoolean("activo")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener los productos: {ex.Message}");
            }
            return productos;
        }

        public Producto? ObtenerPorId(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT * FROM producto 
                        WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Producto
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    Stock = reader.GetInt32("stock"),
                                    StockMin = reader.GetInt32("stock_min"),
                                    StockMax = reader.GetInt32("stock_max"),
                                    CreatedAt = reader.GetDateTime("created_at"),
                                    UpdatedAt = reader.GetDateTime("updated_at"),
                                    Barcode = reader.GetString("barcode"),
                                    PrecioCompra = reader.GetDouble("precio_compra"),
                                    PrecioVenta = reader.GetDouble("precio_venta"),
                                    CategoriaId = reader.GetInt32("categoria_id"),
                                    ProveedorId = reader.GetInt32("proveedor_id"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
                                    Activo = reader.GetBoolean("activo")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener el producto: {ex.Message}");
            }
            return null;
        }

        public void Crear(Producto producto)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        INSERT INTO producto (nombre, stock, stock_min, stock_max, created_at, updated_at, barcode, 
                                           precio_compra, precio_venta, categoria_id, proveedor_id, descripcion, activo)
                        VALUES (@nombre, @stock, @stock_min, @stock_max, @created_at, @updated_at, @barcode,
                               @precio_compra, @precio_venta, @categoria_id, @proveedor_id, @descripcion, @activo)", connection))
                    {
                        command.Parameters.AddWithValue("@nombre", producto.Nombre);
                        command.Parameters.AddWithValue("@stock", producto.Stock);
                        command.Parameters.AddWithValue("@stock_min", producto.StockMin);
                        command.Parameters.AddWithValue("@stock_max", producto.StockMax);
                        command.Parameters.AddWithValue("@created_at", DateTime.Now);
                        command.Parameters.AddWithValue("@updated_at", DateTime.Now);
                        command.Parameters.AddWithValue("@barcode", producto.Barcode);
                        command.Parameters.AddWithValue("@precio_compra", producto.PrecioCompra);
                        command.Parameters.AddWithValue("@precio_venta", producto.PrecioVenta);
                        command.Parameters.AddWithValue("@categoria_id", producto.CategoriaId);
                        command.Parameters.AddWithValue("@proveedor_id", producto.ProveedorId);
                        command.Parameters.AddWithValue("@descripcion", (object?)producto.Descripcion ?? DBNull.Value);
                        command.Parameters.AddWithValue("@activo", producto.Activo);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al crear el producto: {ex.Message}");
            }
        }

        public void Actualizar(Producto producto)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        UPDATE producto 
                        SET nombre = @nombre,
                            stock = @stock,
                            stock_min = @stock_min,
                            stock_max = @stock_max,
                            updated_at = @updated_at,
                            barcode = @barcode,
                            precio_compra = @precio_compra,
                            precio_venta = @precio_venta,
                            categoria_id = @categoria_id,
                            proveedor_id = @proveedor_id,
                            descripcion = @descripcion,
                            activo = @activo
                        WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", producto.Id);
                        command.Parameters.AddWithValue("@nombre", producto.Nombre);
                        command.Parameters.AddWithValue("@stock", producto.Stock);
                        command.Parameters.AddWithValue("@stock_min", producto.StockMin);
                        command.Parameters.AddWithValue("@stock_max", producto.StockMax);
                        command.Parameters.AddWithValue("@updated_at", DateTime.Now);
                        command.Parameters.AddWithValue("@barcode", producto.Barcode);
                        command.Parameters.AddWithValue("@precio_compra", producto.PrecioCompra);
                        command.Parameters.AddWithValue("@precio_venta", producto.PrecioVenta);
                        command.Parameters.AddWithValue("@categoria_id", producto.CategoriaId);
                        command.Parameters.AddWithValue("@proveedor_id", producto.ProveedorId);
                        command.Parameters.AddWithValue("@descripcion", (object?)producto.Descripcion ?? DBNull.Value);
                        command.Parameters.AddWithValue("@activo", producto.Activo);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al actualizar el producto: {ex.Message}");
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("DELETE FROM producto WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al eliminar el producto: {ex.Message}");
            }
        }

        public IEnumerable<Producto> ObtenerPorCategoria(int categoriaId)
        {
            var productos = new List<Producto>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT p.*, c.nombre as categoria_nombre, pr.nombre as proveedor_nombre 
                        FROM productos p
                        LEFT JOIN categorias c ON p.categoria_id = c.id
                        LEFT JOIN proveedores pr ON p.proveedor_id = pr.tercero_id
                        WHERE p.categoria_id = @categoriaId
                        ORDER BY p.id DESC", connection))
                    {
                        command.Parameters.AddWithValue("@categoriaId", categoriaId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    Stock = reader.GetInt32("stock"),
                                    StockMin = reader.GetInt32("stock_min"),
                                    StockMax = reader.GetInt32("stock_max"),
                                    CreatedAt = reader.GetDateTime("created_at"),
                                    UpdatedAt = reader.GetDateTime("updated_at"),
                                    Barcode = reader.GetString("barcode"),
                                    PrecioCompra = reader.GetDouble("precio_compra"),
                                    PrecioVenta = reader.GetDouble("precio_venta"),
                                    CategoriaId = reader.GetInt32("categoria_id"),
                                    ProveedorId = reader.GetInt32("proveedor_id"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
                                    Activo = reader.GetBoolean("activo")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener los productos por categoría: {ex.Message}");
            }
            return productos;
        }

        public IEnumerable<Producto> ObtenerPorProveedor(int proveedorId)
        {
            var productos = new List<Producto>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT p.*, c.nombre as categoria_nombre, pr.nombre as proveedor_nombre 
                        FROM productos p
                        LEFT JOIN categorias c ON p.categoria_id = c.id
                        LEFT JOIN proveedores pr ON p.proveedor_id = pr.tercero_id
                        WHERE p.proveedor_id = @proveedorId
                        ORDER BY p.id DESC", connection))
                    {
                        command.Parameters.AddWithValue("@proveedorId", proveedorId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    Stock = reader.GetInt32("stock"),
                                    StockMin = reader.GetInt32("stock_min"),
                                    StockMax = reader.GetInt32("stock_max"),
                                    CreatedAt = reader.GetDateTime("created_at"),
                                    UpdatedAt = reader.GetDateTime("updated_at"),
                                    Barcode = reader.GetString("barcode"),
                                    PrecioCompra = reader.GetDouble("precio_compra"),
                                    PrecioVenta = reader.GetDouble("precio_venta"),
                                    CategoriaId = reader.GetInt32("categoria_id"),
                                    ProveedorId = reader.GetInt32("proveedor_id"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
                                    Activo = reader.GetBoolean("activo")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener los productos por proveedor: {ex.Message}");
            }
            return productos;
        }

        public IEnumerable<Producto> ObtenerProductosBajoStock()
        {
            var productos = new List<Producto>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT p.*, c.nombre as categoria_nombre, pr.nombre as proveedor_nombre 
                        FROM productos p
                        LEFT JOIN categorias c ON p.categoria_id = c.id
                        LEFT JOIN proveedores pr ON p.proveedor_id = pr.tercero_id
                        WHERE p.stock <= p.stock_min
                        ORDER BY p.id DESC", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    Stock = reader.GetInt32("stock"),
                                    StockMin = reader.GetInt32("stock_min"),
                                    StockMax = reader.GetInt32("stock_max"),
                                    CreatedAt = reader.GetDateTime("created_at"),
                                    UpdatedAt = reader.GetDateTime("updated_at"),
                                    Barcode = reader.GetString("barcode"),
                                    PrecioCompra = reader.GetDouble("precio_compra"),
                                    PrecioVenta = reader.GetDouble("precio_venta"),
                                    CategoriaId = reader.GetInt32("categoria_id"),
                                    ProveedorId = reader.GetInt32("proveedor_id"),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
                                    Activo = reader.GetBoolean("activo")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener los productos con bajo stock: {ex.Message}");
            }
            return productos;
        }

        public void ActualizarStock(int id, int cantidad)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        UPDATE producto 
                        SET stock = stock + @cantidad,
                            updated_at = @updated_at
                        WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@cantidad", cantidad);
                        command.Parameters.AddWithValue("@updated_at", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al actualizar el stock del producto: {ex.Message}");
            }
        }
    }
} 