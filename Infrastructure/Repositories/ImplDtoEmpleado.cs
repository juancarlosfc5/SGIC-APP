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
                        SELECT t.id, t.nombre, t.apellidos, t.email, t.tipo_doc_id, t.tipo_tercero_id, t.ciudad_id,
                               tt.numero as telefono, tt.tipo as tipo_telefono,
                               e.fecha_ingreso, e.salario_base, e.eps_id, e.arl_id
                        FROM tercero t
                        INNER JOIN empleado e ON t.id = e.tercero_id
                        LEFT JOIN tercero_telefono tt ON t.id = tt.tercero_id
                        WHERE t.tipo_tercero_id = 1
                        ORDER BY t.id DESC";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empleado = new EmpleadoDto
                            {
                                TerceroId = reader.GetString("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellidos = reader.GetString("apellidos"),
                                Email = reader.GetString("email"),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? string.Empty : reader.GetString("telefono"),
                                TipoTelefono = reader.IsDBNull(reader.GetOrdinal("tipo_telefono")) ? string.Empty : reader.GetString("tipo_telefono"),
                                TipoDocId = reader.GetInt32("tipo_doc_id"),
                                TipoTerceroId = reader.GetInt32("tipo_tercero_id"),
                                CiudadId = reader.GetInt32("ciudad_id"),
                                SalarioBase = reader.GetDouble("salario_base")
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("fecha_ingreso")))
                                empleado.FechaIngreso = reader.GetDateTime("fecha_ingreso");

                            if (!reader.IsDBNull(reader.GetOrdinal("eps_id")))
                                empleado.EpsId = reader.GetInt32("eps_id");

                            if (!reader.IsDBNull(reader.GetOrdinal("arl_id")))
                                empleado.ArlId = reader.GetInt32("arl_id");

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
                        SELECT t.id, t.nombre, t.apellidos, t.email, t.tipo_doc_id, t.tipo_tercero_id, t.ciudad_id,
                               tt.numero as telefono, tt.tipo as tipo_telefono,
                               e.fecha_ingreso, e.salario_base, e.eps_id, e.arl_id
                        FROM tercero t
                        INNER JOIN empleado e ON t.id = e.tercero_id
                        LEFT JOIN tercero_telefono tt ON t.id = tt.tercero_id
                        WHERE t.id = @id AND t.tipo_tercero_id = 1";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var empleado = new EmpleadoDto
                            {
                                TerceroId = reader.GetString("id"),
                                Nombre = reader.GetString("nombre"),
                                Apellidos = reader.GetString("apellidos"),
                                Email = reader.GetString("email"),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? string.Empty : reader.GetString("telefono"),
                                TipoTelefono = reader.IsDBNull(reader.GetOrdinal("tipo_telefono")) ? string.Empty : reader.GetString("tipo_telefono"),
                                TipoDocId = reader.GetInt32("tipo_doc_id"),
                                TipoTerceroId = reader.GetInt32("tipo_tercero_id"),
                                CiudadId = reader.GetInt32("ciudad_id"),
                                SalarioBase = reader.GetDouble("salario_base")
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("fecha_ingreso")))
                                empleado.FechaIngreso = reader.GetDateTime("fecha_ingreso");

                            if (!reader.IsDBNull(reader.GetOrdinal("eps_id")))
                                empleado.EpsId = reader.GetInt32("eps_id");

                            if (!reader.IsDBNull(reader.GetOrdinal("arl_id")))
                                empleado.ArlId = reader.GetInt32("arl_id");

                            return empleado;
                        }
                    }
                }
            }
            return null;
        }

        public void Crear(EmpleadoDto dto)
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
                            command.Parameters.AddWithValue("@nombre", dto.Nombre);
                            command.Parameters.AddWithValue("@apellidos", dto.Apellidos);
                            command.Parameters.AddWithValue("@email", dto.Email);
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
                                command.Parameters.AddWithValue("@tipo", dto.TipoTelefono);
                                command.Parameters.AddWithValue("@tercero_id", terceroId);

                                command.ExecuteNonQuery();
                            }

                            // Insertar en empleado
                            command.Parameters.Clear();
                            command.CommandText = @"
                                INSERT INTO empleado (tercero_id, fecha_ingreso, salario_base, eps_id, arl_id)
                                VALUES (@tercero_id, @fecha_ingreso, @salario_base, @eps_id, @arl_id)";

                            command.Parameters.AddWithValue("@tercero_id", terceroId);
                            command.Parameters.AddWithValue("@fecha_ingreso", (object?)dto.FechaIngreso ?? DBNull.Value);
                            command.Parameters.AddWithValue("@salario_base", dto.SalarioBase);
                            command.Parameters.AddWithValue("@eps_id", (object?)dto.EpsId ?? DBNull.Value);
                            command.Parameters.AddWithValue("@arl_id", (object?)dto.ArlId ?? DBNull.Value);

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

        public void Actualizar(EmpleadoDto dto)
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
                                throw new Exception("No se encontró el empleado con el ID especificado");

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
                            command.Parameters.AddWithValue("@nombre", dto.Nombre);
                            command.Parameters.AddWithValue("@apellidos", dto.Apellidos);
                            command.Parameters.AddWithValue("@email", dto.Email);
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
                                command.Parameters.AddWithValue("@tipo", dto.TipoTelefono);
                                command.Parameters.AddWithValue("@tercero_id", dto.TerceroId);

                                command.ExecuteNonQuery();
                            }

                            // Actualizar empleado
                            command.Parameters.Clear();
                            command.CommandText = @"
                                UPDATE empleado 
                                SET fecha_ingreso = @fecha_ingreso,
                                    salario_base = @salario_base,
                                    eps_id = @eps_id,
                                    arl_id = @arl_id
                                WHERE tercero_id = @tercero_id";

                            command.Parameters.AddWithValue("@tercero_id", dto.TerceroId);
                            command.Parameters.AddWithValue("@fecha_ingreso", (object?)dto.FechaIngreso ?? DBNull.Value);
                            command.Parameters.AddWithValue("@salario_base", dto.SalarioBase);
                            command.Parameters.AddWithValue("@eps_id", (object?)dto.EpsId ?? DBNull.Value);
                            command.Parameters.AddWithValue("@arl_id", (object?)dto.ArlId ?? DBNull.Value);

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
                                throw new Exception("No se encontró el empleado con el ID especificado");

                            // Eliminar de empleado
                            command.Parameters.Clear();
                            command.CommandText = "DELETE FROM empleado WHERE tercero_id = @id";
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
                        throw new Exception("Error al eliminar el empleado: " + ex.Message, ex);
                    }
                }
            }
        }
    }
} 