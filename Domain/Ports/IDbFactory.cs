using SGIC_APP.Domain.dto;

namespace SGIC_APP.Domain.Ports
{
    public interface IDbFactory
    {
        IDtoCliente<ClienteDto> CrearClienteDtoRepository();
    }
} 