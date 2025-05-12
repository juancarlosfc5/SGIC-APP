using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoRegion : IDtoRegion<Region>
    {
        private readonly string _connectionString;

        public ImplDtoRegion(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Region> ObtenerTodos()
        {
            var regiones = new List<Region>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM region ORDER BY id ASC", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            regiones.Add(new Region
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                PaisId = reader.GetInt32("pais_id")
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener las regiones: {ex.Message}");
            }
            return regiones;
        }

        public Region? ObtenerPorId(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM region WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Region
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    PaisId = reader.GetInt32("pais_id")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener la región: {ex.Message}");
            }
            return null;
        }

        public void Crear(Region region)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("INSERT INTO region (nombre, pais_id) VALUES (@nombre, @pais_id)", connection))
                    {
                        command.Parameters.AddWithValue("@nombre", region.Nombre);
                        command.Parameters.AddWithValue("@pais_id", region.PaisId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al crear la región: {ex.Message}");
            }
        }

        public void Actualizar(Region region)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("UPDATE region SET nombre = @nombre, pais_id = @pais_id WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", region.Id);
                        command.Parameters.AddWithValue("@nombre", region.Nombre);
                        command.Parameters.AddWithValue("@pais_id", region.PaisId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al actualizar la región: {ex.Message}");
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("DELETE FROM region WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451) // Error de clave foránea
                    throw new Exception("No se puede eliminar la región porque tiene ciudades asociadas. Elimine o reasigne las ciudades primero.");
                throw new Exception($"Error al eliminar la región: {ex.Message}");
            }
        }
    }
} 