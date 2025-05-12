using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoCiudad : IDtoCiudad<Ciudad>
    {
        private readonly string _connectionString;

        public ImplDtoCiudad(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Ciudad> ObtenerTodos()
        {
            var ciudades = new List<Ciudad>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM ciudad ORDER BY id ASC", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ciudades.Add(new Ciudad
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                RegionId = reader.GetInt32("region_id")
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener las ciudades: {ex.Message}");
            }
            return ciudades;
        }

        public Ciudad? ObtenerPorId(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM ciudad WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Ciudad
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    RegionId = reader.GetInt32("region_id")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener la ciudad: {ex.Message}");
            }
            return null;
        }

        public void Crear(Ciudad ciudad)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("INSERT INTO ciudad (nombre, region_id) VALUES (@nombre, @region_id)", connection))
                    {
                        command.Parameters.AddWithValue("@nombre", ciudad.Nombre);
                        command.Parameters.AddWithValue("@region_id", ciudad.RegionId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al crear la ciudad: {ex.Message}");
            }
        }

        public void Actualizar(Ciudad ciudad)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("UPDATE ciudad SET nombre = @nombre, region_id = @region_id WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", ciudad.Id);
                        command.Parameters.AddWithValue("@nombre", ciudad.Nombre);
                        command.Parameters.AddWithValue("@region_id", ciudad.RegionId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al actualizar la ciudad: {ex.Message}");
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("DELETE FROM ciudad WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451) // Error de clave foránea
                    throw new Exception("No se puede eliminar la ciudad porque tiene datos asociados. Elimine o reasigne los datos primero.");
                throw new Exception($"Error al eliminar la ciudad: {ex.Message}");
            }
        }
    }
} 