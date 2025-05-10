using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;
using SGIC_APP.Domain.dto;

namespace SGIC_APP.Domain.Factory
{
    public interface IDbFactory
    {
        IDtoCliente<ClienteDto> CrearClienteRepository();
        IDtoEmpleado<EmpleadoDto> CrearEmpleadoRepository();
        IDtoProducto<Producto> CrearProductoRepository();
        IDtoProveedor<Proveedor> CrearProveedorRepository();
        IDtoPais<Pais> CrearPaisRepository();
        IDtoRegion<Region> CrearRegionRepository();
        IDtoCiudad<Ciudad> CrearCiudadRepository();
        IDtoEmpresa<Empresa> CrearEmpresaRepository();
    }
} 