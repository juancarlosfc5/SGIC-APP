using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SGICAPP.Infrastructure.Mysql;
using SGIC_APP.Domain.dto;
using SGIC_APP.Domain.Ports;

namespace SGICAPP.Infrastructure.Repositories
{
    public class ImpDtoCliente : IDtoCliente<ClienteDto>
    {
        private readonly ConexionSingleton _conexion;

        public ImpDtoCliente(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<ClienteDto> ObtenerTodos()
        {
            var lista = new List<ClienteDto>();
            var connection = _conexion.ObtenerConexion();

            string query = @"
                SELECT 
                    t.id, t.nombre, t.apellidos, t.email, 
                    t.tipo_doc_id, t.tipo_tercero_id, 
                    c.fecha_nac, c.fecha_ultima_compra
                FROM cliente c
                INNER JOIN tercero t ON c.tercero_id = t.id";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new ClienteDto
                {
                    Id = reader.GetInt32("id"),
                    nombre = reader.GetString("nombre"),
                    apellidos = reader.GetString("apellidos"),
                    email = reader.GetString("email"),
                    tipo_doc_id = reader.GetInt32("tipo_doc_id"),
                    tipo_tercero_id = reader.GetInt32("tipo_tercero_id"),
                    fecha_nac = reader.GetDateTime("fecha_nac"),
                    ultima_compra = reader.GetDateTime("fecha_ultima_compra")
                });
            }

            return lista;
        }

        public void Crear(ClienteDto clienteDto)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "INSERT INTO cliente (tercero_id, fecha_nac, fecha_ultima_compra) VALUES (@tercero_id, @fecha_nac, @fecha_ultima_compra)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@tercero_id", clienteDto.Id);
            cmd.Parameters.AddWithValue("@fecha_nac", clienteDto.fecha_nac);
            cmd.Parameters.AddWithValue("@fecha_ultima_compra", clienteDto.ultima_compra);
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(ClienteDto clienteDto)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "UPDATE cliente SET fecha_nac = @fecha_nac, fecha_ultima_compra = @fecha_ultima_compra WHERE tercero_id = @tercero_id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@tercero_id", clienteDto.Id);
            cmd.Parameters.AddWithValue("@fecha_nac", clienteDto.fecha_nac);
            cmd.Parameters.AddWithValue("@fecha_ultima_compra", clienteDto.ultima_compra);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM cliente WHERE id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
