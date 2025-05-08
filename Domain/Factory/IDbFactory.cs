using System;
using SGICAPP.Domain.Ports;

namespace SGICAPP.Domain.Factory;

public interface IDbFactory
{
    IClienteRepository CrearClienteRepository();
    
}