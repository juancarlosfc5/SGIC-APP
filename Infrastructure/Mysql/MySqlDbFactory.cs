using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;
using SGIC_APP.Infrastructure.Repositories;

namespace SGIC_APP.Infrastructure.Mysql
{
    public class MySqlDbFactory : IDbFactory
    {
        private readonly string _connectionString;

        public MySqlDbFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDtoCliente<ClienteDto> CrearClienteDtoRepository()
        {
            return new ImplDtoCliente(_connectionString);
        }
    }
} 