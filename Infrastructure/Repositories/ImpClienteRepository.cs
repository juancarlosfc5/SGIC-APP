using System;
using SGICAPP.Domain.Entities;
using SGICAPP.Domain.Ports;
using SGICAPP.Infrastructure.Mysql;
using MySql.Data.MySqlClient;

namespace SGICAPP.Infrastructure.Repositories;

public class lmpClienteRepository : IGenericRepository<Cliente>, IClienteRepository
{
    private readonly ConexionSingleton _conexion;

    public lmpClienteRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Cliente> ObtenerTodos()
{
    var clientes = new List<Cliente>();
    var connection = _conexion.ObtenerConexion();

    string query = @"
        SELECT cliente.id, cliente.tercero_id, tercero.nombre, cliente.fecha_nac, cliente.fecha_ultima_compra 
        FROM cliente 
        INNER JOIN tercero ON cliente.tercero_id = tercero.id ";

    using var mysqlCmd = new MySqlCommand(query, connection);
    using var mysqlReader = mysqlCmd.ExecuteReader();
    while (mysqlReader.Read())
    {
        clientes.Add(new Cliente
        {
            Id = mysqlReader.GetInt32("id"),
            tercero_id = mysqlReader.GetString("tercero_id"),
            fecha_nac = mysqlReader.GetDateTime("fecha_nac"),
            fecha_ultima_compra = mysqlReader.GetDateTime("fecha_ultima_compra")
        });
    }
    return clientes;
}


    public void Crear(Cliente cliente)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "INSERT INTO cliente (tercero_id, fecha_nac, fecha_ultima_compra) VALUES (@tercero_id, @fecha_nac, @fecha_ultima_compra)";
            using var mysqlCmd = new MySqlCommand(query, connection);
            mysqlCmd.Parameters.AddWithValue("@tercero_id", cliente.tercero_id);
            mysqlCmd.Parameters.AddWithValue("@fecha_nac", cliente.fecha_nac);
            mysqlCmd.Parameters.AddWithValue("@fecha_ultima_compra", cliente.fecha_ultima_compra);
            mysqlCmd.ExecuteNonQuery();
        }

        public void Actualizar(Cliente cliente)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "UPDATE cliente SET fecha_nac = @fecha_nac, fecha_ultima_compra = @fecha_ultima_compra WHERE tercero_id = @tercero_id";
            using var mysqlCmd = new MySqlCommand(query, connection);
            mysqlCmd.Parameters.AddWithValue("@tercero_id", cliente.tercero_id);
            mysqlCmd.Parameters.AddWithValue("@fecha_nac", cliente.fecha_nac);
            mysqlCmd.Parameters.AddWithValue("@fecha_ultima_compra", cliente.fecha_ultima_compra);
            mysqlCmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "DELETE FROM cliente WHERE id = @id";
            using var mysqlCmd = new MySqlCommand(query, connection);
            mysqlCmd.Parameters.AddWithValue("@id", id);
            mysqlCmd.ExecuteNonQuery();
        }
}