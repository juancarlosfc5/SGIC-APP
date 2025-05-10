using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoEmpresa : IDtoEmpresa<Empresa>
    {
        private readonly string _connectionString;

        public ImplDtoEmpresa(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Empresa> ObtenerTodos()
        {
            var empresas = new List<Empresa>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM empresa ORDER BY id DESC", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empresas.Add(new Empresa
                            {
                                Id = reader.GetString("id"),
                                Nombre = reader.GetString("nombre"),
                                CiudadId = reader.GetInt32("ciudad_id"),
                                FechaReg = reader.GetDateTime("fecha_reg")
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener las empresas: {ex.Message}");
            }
            return empresas;
        }

        public Empresa? ObtenerPorId(string id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM empresa WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Empresa
                                {
                                    Id = reader.GetString("id"),
                                    Nombre = reader.GetString("nombre"),
                                    CiudadId = reader.GetInt32("ciudad_id"),
                                    FechaReg = reader.GetDateTime("fecha_reg")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener la empresa: {ex.Message}");
            }
            return null;
        }

        public void Crear(Empresa empresa)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("INSERT INTO empresa (id, nombre, ciudad_id, fecha_reg) VALUES (@id, @nombre, @ciudad_id, @fecha_reg)", connection))
                    {
                        command.Parameters.AddWithValue("@id", empresa.Id);
                        command.Parameters.AddWithValue("@nombre", empresa.Nombre);
                        command.Parameters.AddWithValue("@ciudad_id", empresa.CiudadId);
                        command.Parameters.AddWithValue("@fecha_reg", empresa.FechaReg);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al crear la empresa: {ex.Message}");
            }
        }

        public void Actualizar(Empresa empresa)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("UPDATE empresa SET nombre = @nombre, ciudad_id = @ciudad_id, fecha_reg = @fecha_reg WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", empresa.Id);
                        command.Parameters.AddWithValue("@nombre", empresa.Nombre);
                        command.Parameters.AddWithValue("@ciudad_id", empresa.CiudadId);
                        command.Parameters.AddWithValue("@fecha_reg", empresa.FechaReg);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al actualizar la empresa: {ex.Message}");
            }
        }

        public void Eliminar(string id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("DELETE FROM empresa WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451)
                    throw new Exception("No se puede eliminar la empresa porque tiene datos asociados. Elimine o reasigne los datos primero.");
                throw new Exception($"Error al eliminar la empresa: {ex.Message}");
            }
        }
    }
} 