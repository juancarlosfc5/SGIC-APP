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
                        SELECT t.*, c.fecha_nac, c.fecha_ultima_compra
                        FROM tercero t
                        INNER JOIN cliente c ON t.id = c.tercero_id
                        ORDER BY t.id";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cliente = new ClienteDto
                            {
                                TerceroId = reader["id"]?.ToString() ?? string.Empty,
                                Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                Apellidos = reader["apellidos"]?.ToString() ?? string.Empty,
                                Email = reader["email"]?.ToString() ?? string.Empty,
                                TipoDocId = reader["tipo_doc_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_doc_id"]) : 0,
                                TipoTerceroId = reader["tipo_tercero_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_tercero_id"]) : 0,
                                CiudadId = reader["ciudad_id"] != DBNull.Value ? Convert.ToInt32(reader["ciudad_id"]) : 0
                            };

                            if (reader["fecha_nac"] != DBNull.Value)
                                cliente.FechaNacimiento = Convert.ToDateTime(reader["fecha_nac"]);

                            if (reader["fecha_ultima_compra"] != DBNull.Value)
                                cliente.FechaUltimaCompra = Convert.ToDateTime(reader["fecha_ultima_compra"]);

                            clientes.Add(cliente);
                        }
                    }
                }
            }
            return clientes;
        }

        public ClienteDto ObtenerPorId(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT t.*, c.fecha_nac, c.fecha_ultima_compra
                        FROM tercero t
                        LEFT JOIN cliente c ON t.id = c.tercero_id
                        WHERE t.id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var cliente = new ClienteDto
                            {
                                TerceroId = reader["id"]?.ToString() ?? string.Empty,
                                Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                Apellidos = reader["apellidos"]?.ToString() ?? string.Empty,
                                Email = reader["email"]?.ToString() ?? string.Empty,
                                TipoDocId = reader["tipo_doc_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_doc_id"]) : 0,
                                TipoTerceroId = reader["tipo_tercero_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_tercero_id"]) : 0,
                                CiudadId = reader["ciudad_id"] != DBNull.Value ? Convert.ToInt32(reader["ciudad_id"]) : 0
                            };

                            if (reader["fecha_nac"] != DBNull.Value)
                                cliente.FechaNacimiento = Convert.ToDateTime(reader["fecha_nac"]);

                            if (reader["fecha_ultima_compra"] != DBNull.Value)
                                cliente.FechaUltimaCompra = Convert.ToDateTime(reader["fecha_ultima_compra"]);

                            return cliente;
                        }
                    }
                }
            }
            return null;
        }

        private bool ExisteTerceroId(string terceroId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT COUNT(*) 
                        FROM tercero 
                        WHERE id = @id";
                    command.Parameters.AddWithValue("@id", terceroId);

                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public void Crear(ClienteDto cliente)
        {
            if (ExisteTerceroId(cliente.TerceroId))
            {
                throw new Exception($"El TerceroId '{cliente.TerceroId}' ya está registrado en el sistema. Por favor, utilice otro ID.");
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "sp_crear_cliente";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_tercero_id", cliente.TerceroId);
                    command.Parameters.AddWithValue("@p_nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@p_apellidos", cliente.Apellidos);
                    command.Parameters.AddWithValue("@p_email", cliente.Email);
                    command.Parameters.AddWithValue("@p_tipo_doc_id", cliente.TipoDocId);
                    command.Parameters.AddWithValue("@p_tipo_tercero_id", cliente.TipoTerceroId);
                    command.Parameters.AddWithValue("@p_ciudad_id", cliente.CiudadId);
                    command.Parameters.AddWithValue("@p_fecha_nac", cliente.FechaNacimiento ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@p_fecha_ultima_compra", cliente.FechaUltimaCompra ?? (object)DBNull.Value);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception("Error al crear el cliente: " + ex.Message, ex);
                    }
                }
            }
        }

        public void Actualizar(ClienteDto cliente)
        {
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

                            // Actualizar tercero
                            command.CommandText = @"
                                UPDATE tercero SET 
                                    nombre = @nombre,
                                    apellidos = @apellidos,
                                    email = @email,
                                    tipo_doc_id = @tipo_doc_id,
                                    tipo_tercero_id = @tipo_tercero_id,
                                    ciudad_id = @ciudad_id
                                WHERE id = @id";

                            command.Parameters.AddWithValue("@id", cliente.TerceroId);
                            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                            command.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                            command.Parameters.AddWithValue("@email", cliente.Email);
                            command.Parameters.AddWithValue("@tipo_doc_id", cliente.TipoDocId);
                            command.Parameters.AddWithValue("@tipo_tercero_id", cliente.TipoTerceroId);
                            command.Parameters.AddWithValue("@ciudad_id", cliente.CiudadId);

                            command.ExecuteNonQuery();

                            // Actualizar cliente
                            command.Parameters.Clear();
                            command.CommandText = @"
                                UPDATE cliente SET 
                                    fecha_nac = @fecha_nac,
                                    fecha_ultima_compra = @fecha_ultima_compra
                                WHERE tercero_id = @tercero_id";

                            command.Parameters.AddWithValue("@tercero_id", cliente.TerceroId);
                            command.Parameters.AddWithValue("@fecha_nac", cliente.FechaNacimiento ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@fecha_ultima_compra", cliente.FechaUltimaCompra ?? (object)DBNull.Value);

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

                            // Verificar si existe en la tabla cliente
                            command.CommandText = "SELECT COUNT(*) FROM cliente WHERE tercero_id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            int count = Convert.ToInt32(command.ExecuteScalar());

                            if (count == 0)
                            {
                                throw new Exception("El ID proporcionado no corresponde a un cliente registrado.");
                            }

                            // Eliminar de cliente
                            command.Parameters.Clear();
                            command.CommandText = "DELETE FROM cliente WHERE tercero_id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();

                            // Eliminar de tercero solo si no está siendo usado por empleado
                            command.Parameters.Clear();
                            command.CommandText = @"
                                DELETE FROM tercero 
                                WHERE id = @id 
                                AND NOT EXISTS (SELECT 1 FROM empleado WHERE tercero_id = @id)";
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