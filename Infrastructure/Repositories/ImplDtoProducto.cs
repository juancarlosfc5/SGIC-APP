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
                                Barcode = reader.GetString("barcode")
                            });
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
                                    Barcode = reader.GetString("barcode")
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
                        INSERT INTO producto (nombre, stock, stock_min, stock_max, created_at, updated_at, barcode)
                        VALUES (@nombre, @stock, @stock_min, @stock_max, @created_at, @updated_at, @barcode)", connection))
                    {
                        command.Parameters.AddWithValue("@nombre", producto.Nombre);
                        command.Parameters.AddWithValue("@stock", producto.Stock);
                        command.Parameters.AddWithValue("@stock_min", producto.StockMin);
                        command.Parameters.AddWithValue("@stock_max", producto.StockMax);
                        command.Parameters.AddWithValue("@created_at", producto.CreatedAt);
                        command.Parameters.AddWithValue("@updated_at", producto.UpdatedAt);
                        command.Parameters.AddWithValue("@barcode", producto.Barcode);
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
                            barcode = @barcode
                        WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", producto.Id);
                        command.Parameters.AddWithValue("@nombre", producto.Nombre);
                        command.Parameters.AddWithValue("@stock", producto.Stock);
                        command.Parameters.AddWithValue("@stock_min", producto.StockMin);
                        command.Parameters.AddWithValue("@stock_max", producto.StockMax);
                        command.Parameters.AddWithValue("@updated_at", producto.UpdatedAt);
                        command.Parameters.AddWithValue("@barcode", producto.Barcode);
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
            // Esta función ya no tiene sentido porque la tabla producto no tiene categoria_id
            // Devuelvo una lista vacía o lanzo una excepción si lo prefieres
            return new List<Producto>();
        }

        public IEnumerable<Producto> ObtenerPorProveedor(int proveedorId)
        {
            // Esta función ya no tiene sentido porque la tabla producto no tiene proveedor_id
            // Devuelvo una lista vacía o lanzo una excepción si lo prefieres
            return new List<Producto>();
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
                        SELECT * FROM producto
                        WHERE stock <= stock_min
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
                                    Barcode = reader.GetString("barcode")
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