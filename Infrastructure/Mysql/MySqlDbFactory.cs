using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGIC_APP.Domain.Factory;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;
using SGIC_APP.Domain.Entities;
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

        public IDtoCliente<ClienteDto> CrearClienteRepository()
        {
            return new ImplDtoCliente(_connectionString);
        }

        public IDtoEmpleado<EmpleadoDto> CrearEmpleadoRepository()
        {
            return new ImplDtoEmpleado(_connectionString);
        }

        public IDtoProducto<Producto> CrearProductoRepository()
        {
            return new ImplDtoProducto(_connectionString);
        }

        public IDtoProveedor<Proveedor> CrearProveedorRepository()
        {
            return new ImplDtoProveedor(_connectionString);
        }

        public IDtoPais<Pais> CrearPaisRepository()
        {
            return new ImplDtoPais(_connectionString);
        }

        public IDtoRegion<Region> CrearRegionRepository()
        {
            return new ImplDtoRegion(_connectionString);
        }

        public IDtoCiudad<Ciudad> CrearCiudadRepository()
        {
            return new ImplDtoCiudad(_connectionString);
        }

        public IDtoEmpresa<Empresa> CrearEmpresaRepository()
        {
            return new ImplDtoEmpresa(_connectionString);
        }
    }
} 