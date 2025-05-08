using System;
using SGICAPP.Domain.Factory;
using SGICAPP.Domain.Ports;
using SGICAPP.Infrastructure.Repositories;

namespace SGICAPP.Infrastructure.Mysql;

public class MySqlDbFactory : IDbFactory
{
    private readonly string _connectionString;

    public MySqlDbFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IClienteRepository CrearClienteRepository()
    {
        return new lmpClienteRepository(_connectionString);
    }
}