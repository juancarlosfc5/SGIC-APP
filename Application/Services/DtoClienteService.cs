using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGIC_APP.Domain.dto;
using SGIC_APP.Domain.Entities;
using SGIC_APP.Domain.Repositories;


namespace SGIC_APP.Application.Services
{
    public class DtoClienteService
    {
        private readonly ITerceroRepository _terceroRepository;
        private readonly IClienteRepository _clienteRepository;

        public ClienteDtoService(ITerceroRepository terceroRepository, IClienteRepository clienteRepository)
        {
            _terceroRepository = terceroRepository;
            _clienteRepository = clienteRepository;
        }

        public void CrearCliente(ClienteDto dto)
        {
            // Crear el tercero
            var tercero = new Tercero
            {
                Nombre = dto.nombre!,
                Apellidos = dto.apellidos!,
                Email = dto.email!,
                TipoDocId = dto.tipo_doc_id,
                TipoTerceroId = dto.tipo_tercero_id
            };

            int nuevoTerceroId = _terceroRepository.Crear(tercero);

            // Crear el cliente
            var cliente = new Cliente
            {
                TerceroId = nuevoTerceroId,
                FechaNacimiento = dto.fecha_nac,
                FechaUltimaCompra = dto.ultima_compra
            };

            _clienteRepository.Crear(cliente);
        }

        public void ActualizarCliente(ClienteDto dto)
        {
            // Actualizar el tercero
            var tercero = new Tercero
            {
                Id = dto.Id,
                Nombre = dto.nombre!,
                Apellidos = dto.apellidos!,
                Email = dto.email!,
                TipoDocId = dto.tipo_doc_id,
                TipoTerceroId = dto.tipo_tercero_id
            };

            _terceroRepository.Actualizar(tercero);

            // Actualizar cliente
            var cliente = new Cliente
            {
                TerceroId = dto.Id,
                FechaNacimiento = dto.fecha_nac,
                FechaUltimaCompra = dto.ultima_compra
            };

            _clienteRepository.Actualizar(cliente);
        }

        public void EliminarCliente(int terceroId)
        {
            _clienteRepository.EliminarPorTerceroId(terceroId);
            _terceroRepository.Eliminar(terceroId);
        }

        public void MostrarClientes()
        {
            var clientes = _clienteRepository.ObtenerTodosConTercero();

            foreach (var c in clientes)
            {
                Console.WriteLine($"ID: {c.TerceroId} | Nombre: {c.NombreCompleto} | Email: {c.Email} | Nacimiento: {c.FechaNacimiento.ToShortDateString()} | Última compra: {c.FechaUltimaCompra.ToShortDateString()}");
            }
        }
    }
}