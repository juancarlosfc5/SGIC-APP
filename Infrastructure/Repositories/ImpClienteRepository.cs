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

        string query = "SELECT id, nombre FROM clientes";
        using var mysqlCmd = new MySqlCommand(query, connection);
        using var mysqlReader = mysqlCmd.ExecuteReader();
        while (mysqlReader.Read())
        {
            clientes.Add(new Cliente
            {
                Id = mysqlReader.GetInt32("id"),
                Nombre = mysqlReader.GetString("nombre")
            });
        }
        return clientes;
    }

    public void Crear(Cliente cliente)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO clientes (nombre) VALUES (@nombre)";
        using var mysqlCmd = new MySqlCommand(query, connection);
        mysqlCmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
        mysqlCmd.ExecuteNonQuery();
    }

    public void Actualizar(Cliente cliente)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE clientes SET nombre = @nombre WHERE id = @id";
        using var mysqlCmd = new MySqlCommand(query, connection);
        mysqlCmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
        mysqlCmd.Parameters.AddWithValue("@id", cliente.Id);
        mysqlCmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM clientes WHERE id = @id";
        using var mysqlCmd = new MySqlCommand(query, connection);
        mysqlCmd.Parameters.AddWithValue("@id", id);
        mysqlCmd.ExecuteNonQuery();
    }
}