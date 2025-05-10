using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Infrastructure.Repositories
{
    public class ImplDtoPais : IDtoPais<Pais>
    {
        private readonly string _connectionString;

        public ImplDtoPais(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Pais> ObtenerTodos()
        {
            var paises = new List<Pais>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM pais ORDER BY id DESC", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paises.Add(new Pais
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre")
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener los países: {ex.Message}");
            }
            return paises;
        }

        public Pais? ObtenerPorId(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("SELECT * FROM pais WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Pais
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al obtener el país: {ex.Message}");
            }
            return null;
        }

        public void Crear(Pais pais)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("INSERT INTO pais (nombre) VALUES (@nombre)", connection))
                    {
                        command.Parameters.AddWithValue("@nombre", pais.Nombre);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al crear el país: {ex.Message}");
            }
        }

        public void Actualizar(Pais pais)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("UPDATE pais SET nombre = @nombre WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", pais.Id);
                        command.Parameters.AddWithValue("@nombre", pais.Nombre);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error al actualizar el país: {ex.Message}");
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("DELETE FROM pais WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451) // Error de clave foránea
                    throw new Exception("No se puede eliminar el país porque tiene regiones asociadas. Elimine o reasigne las regiones primero.");
                throw new Exception($"Error al eliminar el país: {ex.Message}");
            }
        }
    }
} 