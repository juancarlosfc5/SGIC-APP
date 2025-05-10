using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoProveedor : IDtoProveedor<Proveedor>
    {
        private readonly string _connectionString;

        public ImplDtoProveedor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Proveedor> ObtenerTodos()
        {
            var proveedores = new List<Proveedor>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT p.*, t.nombre, t.apellidos, t.email, t.telefono, t.tipo_telefono, 
                               t.tipo_doc_id, t.tipo_tercero_id, t.ciudad_id
                        FROM proveedor p
                        INNER JOIN tercero t ON p.tercero_id = t.id
                        ORDER BY p.tercero_id DESC", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                proveedores.Add(new Proveedor
                                {
                                    TerceroId = reader.GetString("tercero_id"),
                                    Nombre = reader.GetString("nombre"),
                                    Apellidos = reader.GetString("apellidos"),
                                    Email = reader.GetString("email"),
                                    Telefono = reader.GetString("telefono"),
                                    TipoTelefono = reader.GetString("tipo_telefono"),
                                    TipoDocId = reader.GetInt32("tipo_doc_id"),
                                    TipoTerceroId = reader.GetInt32("tipo_tercero_id"),
                                    CiudadId = reader.GetInt32("ciudad_id"),
                                    Descuento = reader.GetDouble("descuento"),
                                    DiaPago = reader.GetInt32("dia_pago"),
                                    FechaRegistro = reader.GetDateTime("fecha_registro")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener los proveedores: {ex.Message}");
            }
            return proveedores;
        }

        public Proveedor? ObtenerPorId(string id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT p.*, t.nombre, t.apellidos, t.email, t.telefono, t.tipo_telefono, 
                               t.tipo_doc_id, t.tipo_tercero_id, t.ciudad_id
                        FROM proveedor p
                        INNER JOIN tercero t ON p.tercero_id = t.id
                        WHERE p.tercero_id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Proveedor
                                {
                                    TerceroId = reader.GetString("tercero_id"),
                                    Nombre = reader.GetString("nombre"),
                                    Apellidos = reader.GetString("apellidos"),
                                    Email = reader.GetString("email"),
                                    Telefono = reader.GetString("telefono"),
                                    TipoTelefono = reader.GetString("tipo_telefono"),
                                    TipoDocId = reader.GetInt32("tipo_doc_id"),
                                    TipoTerceroId = reader.GetInt32("tipo_tercero_id"),
                                    CiudadId = reader.GetInt32("ciudad_id"),
                                    Descuento = reader.GetDouble("descuento"),
                                    DiaPago = reader.GetInt32("dia_pago"),
                                    FechaRegistro = reader.GetDateTime("fecha_registro")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener el proveedor: {ex.Message}");
            }
            return null;
        }

        public void Crear(Proveedor proveedor)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Primero insertar en la tabla tercero
                        using (var command = new MySqlCommand(@"
                            INSERT INTO tercero (nombre, apellidos, email, telefono, tipo_telefono, 
                                              tipo_doc_id, tipo_tercero_id, ciudad_id)
                            VALUES (@nombre, @apellidos, @email, @telefono, @tipo_telefono,
                                   @tipo_doc_id, @tipo_tercero_id, @ciudad_id)", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@nombre", proveedor.Nombre);
                            command.Parameters.AddWithValue("@apellidos", proveedor.Apellidos);
                            command.Parameters.AddWithValue("@email", proveedor.Email);
                            command.Parameters.AddWithValue("@telefono", proveedor.Telefono);
                            command.Parameters.AddWithValue("@tipo_telefono", proveedor.TipoTelefono);
                            command.Parameters.AddWithValue("@tipo_doc_id", proveedor.TipoDocId);
                            command.Parameters.AddWithValue("@tipo_tercero_id", proveedor.TipoTerceroId);
                            command.Parameters.AddWithValue("@ciudad_id", proveedor.CiudadId);

                            command.ExecuteNonQuery();
                            proveedor.TerceroId = command.LastInsertedId.ToString();
                        }

                        // Luego insertar en la tabla proveedor
                        using (var command = new MySqlCommand(@"
                            INSERT INTO proveedor (tercero_id, descuento, dia_pago, fecha_registro)
                            VALUES (@tercero_id, @descuento, @dia_pago, @fecha_registro)", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@tercero_id", proveedor.TerceroId);
                            command.Parameters.AddWithValue("@descuento", proveedor.Descuento);
                            command.Parameters.AddWithValue("@dia_pago", proveedor.DiaPago);
                            command.Parameters.AddWithValue("@fecha_registro", DateTime.Now);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Actualizar(Proveedor proveedor)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Actualizar la tabla tercero
                        using (var command = new MySqlCommand(@"
                            UPDATE tercero 
                            SET nombre = @nombre,
                                apellidos = @apellidos,
                                email = @email,
                                telefono = @telefono,
                                tipo_telefono = @tipo_telefono,
                                tipo_doc_id = @tipo_doc_id,
                                tipo_tercero_id = @tipo_tercero_id,
                                ciudad_id = @ciudad_id
                            WHERE id = @id", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", proveedor.TerceroId);
                            command.Parameters.AddWithValue("@nombre", proveedor.Nombre);
                            command.Parameters.AddWithValue("@apellidos", proveedor.Apellidos);
                            command.Parameters.AddWithValue("@email", proveedor.Email);
                            command.Parameters.AddWithValue("@telefono", proveedor.Telefono);
                            command.Parameters.AddWithValue("@tipo_telefono", proveedor.TipoTelefono);
                            command.Parameters.AddWithValue("@tipo_doc_id", proveedor.TipoDocId);
                            command.Parameters.AddWithValue("@tipo_tercero_id", proveedor.TipoTerceroId);
                            command.Parameters.AddWithValue("@ciudad_id", proveedor.CiudadId);

                            command.ExecuteNonQuery();
                        }

                        // Actualizar la tabla proveedor
                        using (var command = new MySqlCommand(@"
                            UPDATE proveedor 
                            SET descuento = @descuento,
                                dia_pago = @dia_pago
                            WHERE tercero_id = @tercero_id", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@tercero_id", proveedor.TerceroId);
                            command.Parameters.AddWithValue("@descuento", proveedor.Descuento);
                            command.Parameters.AddWithValue("@dia_pago", proveedor.DiaPago);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Eliminar(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Primero eliminar de la tabla proveedor
                        using (var command = new MySqlCommand("DELETE FROM proveedor WHERE tercero_id = @id", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        // Luego eliminar de la tabla tercero
                        using (var command = new MySqlCommand("DELETE FROM tercero WHERE id = @id", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public IEnumerable<Proveedor> ObtenerPorCiudad(int ciudadId)
        {
            var proveedores = new List<Proveedor>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(@"
                        SELECT p.*, t.nombre, t.apellidos, t.email, t.telefono, t.tipo_telefono, 
                               t.tipo_doc_id, t.tipo_tercero_id, t.ciudad_id
                        FROM proveedor p
                        INNER JOIN tercero t ON p.tercero_id = t.id
                        WHERE t.ciudad_id = @ciudadId
                        ORDER BY p.tercero_id DESC", connection))
                    {
                        command.Parameters.AddWithValue("@ciudadId", ciudadId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                proveedores.Add(new Proveedor
                                {
                                    TerceroId = reader.GetString("tercero_id"),
                                    Nombre = reader.GetString("nombre"),
                                    Apellidos = reader.GetString("apellidos"),
                                    Email = reader.GetString("email"),
                                    Telefono = reader.GetString("telefono"),
                                    TipoTelefono = reader.GetString("tipo_telefono"),
                                    TipoDocId = reader.GetInt32("tipo_doc_id"),
                                    TipoTerceroId = reader.GetInt32("tipo_tercero_id"),
                                    CiudadId = reader.GetInt32("ciudad_id"),
                                    Descuento = reader.GetDouble("descuento"),
                                    DiaPago = reader.GetInt32("dia_pago"),
                                    FechaRegistro = reader.GetDateTime("fecha_registro")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener los proveedores por ciudad: {ex.Message}");
            }
            return proveedores;
        }
    }
} 