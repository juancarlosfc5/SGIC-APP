using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoEmpleado : IDtoEmpleado<EmpleadoDto>
    {
        private readonly string _connectionString;

        public ImplDtoEmpleado(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<EmpleadoDto> ObtenerTodos()
        {
            var empleados = new List<EmpleadoDto>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT t.*, e.fecha_ingreso, e.salario_base, e.eps_id, e.arl_id
                        FROM tercero t
                        INNER JOIN empleado e ON t.id = e.tercero_id
                        ORDER BY t.id";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empleado = new EmpleadoDto
                            {
                                TerceroId = reader["id"]?.ToString() ?? string.Empty,
                                Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                Apellidos = reader["apellidos"]?.ToString() ?? string.Empty,
                                Email = reader["email"]?.ToString() ?? string.Empty,
                                TipoDocId = reader["tipo_doc_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_doc_id"]) : 0,
                                TipoTerceroId = reader["tipo_tercero_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_tercero_id"]) : 0,
                                CiudadId = reader["ciudad_id"] != DBNull.Value ? Convert.ToInt32(reader["ciudad_id"]) : 0
                            };

                            if (reader["fecha_ingreso"] != DBNull.Value)
                                empleado.FechaIngreso = Convert.ToDateTime(reader["fecha_ingreso"]);

                            if (reader["salario_base"] != DBNull.Value)
                                empleado.SalarioBase = Convert.ToDouble(reader["salario_base"]);

                            if (reader["eps_id"] != DBNull.Value)
                                empleado.EpsId = Convert.ToInt32(reader["eps_id"]);

                            if (reader["arl_id"] != DBNull.Value)
                                empleado.ArlId = Convert.ToInt32(reader["arl_id"]);

                            empleados.Add(empleado);
                        }
                    }
                }
            }
            return empleados;
        }

        public EmpleadoDto? ObtenerPorId(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT t.*, e.fecha_ingreso, e.salario_base, e.eps_id, e.arl_id
                        FROM tercero t
                        INNER JOIN empleado e ON t.id = e.tercero_id
                        WHERE t.id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var empleado = new EmpleadoDto
                            {
                                TerceroId = reader["id"]?.ToString() ?? string.Empty,
                                Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                Apellidos = reader["apellidos"]?.ToString() ?? string.Empty,
                                Email = reader["email"]?.ToString() ?? string.Empty,
                                TipoDocId = reader["tipo_doc_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_doc_id"]) : 0,
                                TipoTerceroId = reader["tipo_tercero_id"] != DBNull.Value ? Convert.ToInt32(reader["tipo_tercero_id"]) : 0,
                                CiudadId = reader["ciudad_id"] != DBNull.Value ? Convert.ToInt32(reader["ciudad_id"]) : 0
                            };

                            if (reader["fecha_ingreso"] != DBNull.Value)
                                empleado.FechaIngreso = Convert.ToDateTime(reader["fecha_ingreso"]);

                            if (reader["salario_base"] != DBNull.Value)
                                empleado.SalarioBase = Convert.ToDouble(reader["salario_base"]);

                            if (reader["eps_id"] != DBNull.Value)
                                empleado.EpsId = Convert.ToInt32(reader["eps_id"]);

                            if (reader["arl_id"] != DBNull.Value)
                                empleado.ArlId = Convert.ToInt32(reader["arl_id"]);

                            return empleado;
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
                        FROM empleado e
                        INNER JOIN tercero t ON e.tercero_id = t.id
                        WHERE t.id = @id";
                    command.Parameters.AddWithValue("@id", terceroId);

                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public void Crear(EmpleadoDto empleado)
        {
            if (ExisteTerceroId(empleado.TerceroId))
            {
                throw new Exception($"El TerceroId '{empleado.TerceroId}' ya está registrado como empleado. Por favor, utilice otro ID.");
            }

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

                            // Insertar en tercero
                            command.CommandText = @"
                                INSERT INTO tercero (id, nombre, apellidos, email, tipo_doc_id, tipo_tercero_id, ciudad_id)
                                VALUES (@id, @nombre, @apellidos, @email, @tipo_doc_id, @tipo_tercero_id, @ciudad_id)";

                            command.Parameters.AddWithValue("@id", empleado.TerceroId);
                            command.Parameters.AddWithValue("@nombre", empleado.Nombre);
                            command.Parameters.AddWithValue("@apellidos", empleado.Apellidos);
                            command.Parameters.AddWithValue("@email", empleado.Email);
                            command.Parameters.AddWithValue("@tipo_doc_id", empleado.TipoDocId);
                            command.Parameters.AddWithValue("@tipo_tercero_id", empleado.TipoTerceroId);
                            command.Parameters.AddWithValue("@ciudad_id", empleado.CiudadId);

                            command.ExecuteNonQuery();

                            // Insertar en empleado
                            command.Parameters.Clear();
                            command.CommandText = @"
                                INSERT INTO empleado (tercero_id, fecha_ingreso, salario_base, eps_id, arl_id)
                                VALUES (@tercero_id, @fecha_ingreso, @salario_base, @eps_id, @arl_id)";

                            command.Parameters.AddWithValue("@tercero_id", empleado.TerceroId);
                            command.Parameters.AddWithValue("@fecha_ingreso", empleado.FechaIngreso ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@salario_base", empleado.SalarioBase);
                            command.Parameters.AddWithValue("@eps_id", empleado.EpsId ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@arl_id", empleado.ArlId ?? (object)DBNull.Value);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al crear el empleado: " + ex.Message, ex);
                    }
                }
            }
        }

        public void Actualizar(EmpleadoDto empleado)
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

                            command.Parameters.AddWithValue("@id", empleado.TerceroId);
                            command.Parameters.AddWithValue("@nombre", empleado.Nombre);
                            command.Parameters.AddWithValue("@apellidos", empleado.Apellidos);
                            command.Parameters.AddWithValue("@email", empleado.Email);
                            command.Parameters.AddWithValue("@tipo_doc_id", empleado.TipoDocId);
                            command.Parameters.AddWithValue("@tipo_tercero_id", empleado.TipoTerceroId);
                            command.Parameters.AddWithValue("@ciudad_id", empleado.CiudadId);

                            command.ExecuteNonQuery();

                            // Actualizar empleado
                            command.Parameters.Clear();
                            command.CommandText = @"
                                UPDATE empleado SET 
                                    fecha_ingreso = @fecha_ingreso,
                                    salario_base = @salario_base,
                                    eps_id = @eps_id,
                                    arl_id = @arl_id
                                WHERE tercero_id = @tercero_id";

                            command.Parameters.AddWithValue("@tercero_id", empleado.TerceroId);
                            command.Parameters.AddWithValue("@fecha_ingreso", empleado.FechaIngreso ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@salario_base", empleado.SalarioBase);
                            command.Parameters.AddWithValue("@eps_id", empleado.EpsId ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@arl_id", empleado.ArlId ?? (object)DBNull.Value);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al actualizar el empleado: " + ex.Message, ex);
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

                            // Eliminar teléfonos asociados
                            command.CommandText = "DELETE FROM tercero_telefono WHERE tercero_id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();

                            // Eliminar de cliente si existe
                            command.Parameters.Clear();
                            command.CommandText = "DELETE FROM cliente WHERE tercero_id = @id";
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();

                            // Eliminar de empleado
                            command.Parameters.Clear();
                            command.CommandText = "DELETE FROM empleado WHERE tercero_id = @id";
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
                        throw new Exception("Error al eliminar el empleado: " + ex.Message, ex);
                    }
                }
            }
        }
    }
} 