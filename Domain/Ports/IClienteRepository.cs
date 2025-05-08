using System;
using SGIC_APP.Domain.dto;
using SGICAPP.Domain.Entities;

namespace SGICAPP.Domain.Ports;

public interface IClienteRepository : IGenericRepository<Cliente>
{
        List<ClienteDto> ObtenerTodosConTercero();
        void EliminarPorTerceroId(int terceroId);
}