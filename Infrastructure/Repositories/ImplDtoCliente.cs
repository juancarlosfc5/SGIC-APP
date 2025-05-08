using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoCliente : IDtoCliente<ClienteDto>
    {
        private readonly string _connectionString;

        public ImplDtoCliente(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<ClienteDto> ObtenerTodos()
        {
            var clientes = new List<ClienteDto>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT t.id, t.nombre, t.apellidos, t.email, t.tipo_doc_id, t.tipo_tercero_id, t.ciudad_id,
                               tt.numero as telefono, tt.tipo as tipo_telefono,
                               c.fecha_nac, c.fecha_ultima_compra
                        FROM tercero t
                        INNER JOIN cliente c ON t.id = c.tercero_id
                        LEFT JOIN tercero_telefono tt ON t.id = tt.tercero_id
                        WHERE t.tipo_tercero_id = 1
                        ORDER BY t.id DESC";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new ClienteDto
                            {
                                TerceroId = reader.GetString("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellidos = reader.GetString("apellidos"),
                                Email = reader.GetString("email"),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                                TipoTelefono = reader.IsDBNull(reader.GetOrdinal("tipo_telefono")) ? null : reader.GetString("tipo_telefono"),
                                TipoDocId = reader.GetInt32("tipo_doc_id"),
                                TipoTerceroId = reader.GetInt32("tipo_tercero_id"),
                                CiudadId = reader.GetInt32("ciudad_id"),
                                FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("fecha_nac")) ? null : (DateTime?)reader.GetDateTime("fecha_nac"),
                                FechaUltimaCompra = reader.IsDBNull(reader.GetOrdinal("fecha_ultima_compra")) ? null : (DateTime?)reader.GetDateTime("fecha_ultima_compra")
                            });
                        }
                    }
                }
            }
            return clientes;
        }

        public ClienteDto? ObtenerPorId(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT t.id, t.nombre, t.apellidos, t.email, t.tipo_doc_id, t.tipo_tercero_id, t.ciudad_id,
                               tt.numero as telefono, tt.tipo as tipo_telefono,
                               c.fecha_nac, c.fecha_ultima_compra
                        FROM tercero t
                        INNER JOIN cliente c ON t.id = c.tercero_id
                        LEFT JOIN tercero_telefono tt ON t.id = tt.tercero_id
                        WHERE t.id = @id AND t.tipo_tercero_id = 1";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ClienteDto
                            {
                                TerceroId = reader.GetString("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellidos = reader.GetString("apellidos"),
                                Email = reader.GetString("email"),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                                TipoTelefono = reader.IsDBNull(reader.GetOrdinal("tipo_telefono")) ? null : reader.GetString("tipo_telefono"),
                                TipoDocId = reader.GetInt32("tipo_doc_id"),
                                TipoTerceroId = reader.GetInt32("tipo_tercero_id"),
                                CiudadId = reader.GetInt32("ciudad_id"),
                                FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("fecha_nac")) ? null : (DateTime?)reader.GetDateTime("fecha_nac"),
                                FechaUltimaCompra = reader.IsDBNull(reader.GetOrdinal("fecha_ultima_compra")) ? null : (DateTime?)reader.GetDateTime("fecha_ultima_compra")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Crear(ClienteDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new MySqlCommand())
                        {
                            command.Connection = connection;
                            command.Transaction = transaction;

                            // Generar ID único para el tercero
                            string terceroId = DateTime.Now.ToString("yyyyMMddHHmmss");

                            // Insertar en tercero
                            command.CommandText = @"
                                INSERT INTO tercero (id, nombre, apellidos, email, tipo_doc_id, tipo_tercero_id, ciudad_id)
                                VALUES (@id, @nombre, @apellidos, @email, @tipo_doc_id, @tipo_tercero_id, @ciudad_id)";

                            command.Parameters.AddWithValue("@id", terceroId);
                            command.Parameters.AddWithValue("@nombre", dto.Nombre ?? string.Empty);
                            command.Parameters.AddWithValue("@apellidos", dto.Apellidos ?? string.Empty);
                            command.Parameters.AddWithValue("@email", dto.Email ?? string.Empty);
                            command.Parameters.AddWithValue("@tipo_doc_id", dto.TipoDocId);
                            command.Parameters.AddWithValue("@tipo_tercero_id", dto.TipoTerceroId);
                            command.Parameters.AddWithValue("@ciudad_id", dto.CiudadId);

                            command.ExecuteNonQuery();

                            // Insertar teléfono si existe
                            if (!string.IsNullOrEmpty(dto.Telefono))
                            {
                                command.Parameters.Clear();
                                command.CommandText = @"
                                    INSERT INTO tercero_telefono (numero, tipo, tercero_id)
                                    VALUES (@numero, @tipo, @tercero_id)";

                                command.Parameters.AddWithValue("@numero", dto.Telefono);
                                command.Parameters.AddWithValue("@tipo", dto.TipoTelefono ?? "Celular");
                                command.Parameters.AddWithValue("@tercero_id", terceroId);

                                command.ExecuteNonQuery();
                            }

                            // Insertar en cliente
                            command.Parameters.Clear();
                            command.CommandText = @"
                                INSERT INTO cliente (tercero_id, fecha_nac, fecha_ultima_compra)
                                VALUES (@tercero_id, @fecha_nac, @fecha_ultima_compra)";

                            command.Parameters.AddWithValue("@tercero_id", terceroId);
                            command.Parameters.AddWithValue("@fecha_nac", (object?)dto.FechaNacimiento ?? DBNull.Value);
                            command.Parameters.AddWithValue("@fecha_ultima_compra", (object?)dto.FechaUltimaCompra ?? DBNull.Value);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al crear el cliente: " + ex.Message, ex);
                    }
                }
            }
        }

        public void Actualizar(ClienteDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrEmpty(dto.TerceroId))
                throw new ArgumentException("El ID del tercero es inválido", nameof(dto));

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new MySqlCommand())
                        {
                            command.Connection = connection;
                            command.Transaction = transaction;

                            // Verificar si existe el tercero
                            command.CommandText = "SELECT COUNT(*) FROM tercero WHERE id = @id";
                            command.Parameters.AddWithValue("@id", dto.TerceroId);
                            int count = Convert.ToInt32(command.ExecuteScalar());

                            if (count == 0)
                                throw new Exception("No se encontró el cliente con el ID especificado");

                            // Actualizar tercero
                            command.Parameters.Clear();
                            command.CommandText = @"
                                UPDATE tercero 
                                SET nombre = @nombre,
                                    apellidos = @apellidos,
                                    email = @email,
                                    tipo_doc_id = @tipo_doc_id,
                                    tipo_tercero_id = @tipo_tercero_id,
                                    ciudad_id = @ciudad_id
                                WHERE id = @id";

                            command.Parameters.AddWithValue("@id", dto.TerceroId);
                            command.Parameters.AddWithValue("@nombre", dto.Nombre ?? string.Empty);
                            command.Parameters.AddWithValue("@apellidos", dto.Apellidos ?? string.Empty);
                            command.Parameters.AddWithValue("@email", dto.Email ?? string.Empty);
                            command.Parameters.AddWithValue("@tipo_doc_id", dto.TipoDocId);
                            command.Parameters.AddWithValue("@tipo_tercero_id", dto.TipoTerceroId);
                            command.Parameters.AddWithValue("@ciudad_id", dto.CiudadId);

                            command.ExecuteNonQuery();

                            // Actualizar o insertar teléfono
                            if (!string.IsNullOrEmpty(dto.Telefono))
                            {
                                command.Parameters.Clear();
                                command.CommandText = @"
                                    INSERT INTO tercero_telefono (numero, tipo, tercero_id)
                                    VALUES (@numero, @tipo, @tercero_id)
                                    ON DUPLICATE KEY UPDATE
                                    numero = @numero,
                                    tipo = @tipo";

                                command.Parameters.AddWithValue("@numero", dto.Telefono);
                                command.Parameters.AddWithValue("@tipo", dto.TipoTelefono ?? "Celular");
                                command.Parameters.AddWithValue("@tercero_id", dto.TerceroId);

                                command.ExecuteNonQuery();
                            }

                            // Actualizar cliente
                            command.Parameters.Clear();
                            command.CommandText = @"
                                UPDATE cliente 
                                SET fecha_nac = @fecha_nac,
                                    fecha_ultima_compra = @fecha_ultima_compra
                                WHERE tercero_id = @tercero_id";

                            command.Parameters.AddWithValue("@tercero_id", dto.TerceroId);
                            command.Parameters.AddWithValue("@fecha_nac", (object?)dto.FechaNacimiento ?? DBNull.Value);
                            command.Parameters.AddWithValue("@fecha_ultima_compra", (object?)dto.FechaUltimaCompra ?? DBNull.Value);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al actualizar el cliente: " + ex.Message, ex);
                    }
                }
            }
        }

        public void Eliminar(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("El ID del tercero es inválido", nameof(id));

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new MySqlCommand())
                        {
                            command.Connection = connection;
                            command.Transaction = transaction;

                            // Verificar si existe el tercero
                            command.CommandText = "SELECT COUNT(*) FROM tercero WHERE id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            int count = Convert.ToInt32(command.ExecuteScalar());

                            if (count == 0)
                                throw new Exception("No se encontró el cliente con el ID especificado");

                            // Eliminar de cliente
                            command.Parameters.Clear();
                            command.CommandText = "DELETE FROM cliente WHERE tercero_id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();

                            // Eliminar teléfonos
                            command.Parameters.Clear();
                            command.CommandText = "DELETE FROM tercero_telefono WHERE tercero_id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();

                            // Eliminar de tercero
                            command.Parameters.Clear();
                            command.CommandText = "DELETE FROM tercero WHERE id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al eliminar el cliente: " + ex.Message, ex);
                    }
                }
            }
        }
    }
}