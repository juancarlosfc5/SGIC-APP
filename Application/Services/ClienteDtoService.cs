using System;
using System.Collections.Generic;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;

namespace SGIC_APP.Application.Services
{
    public class ClienteDtoService
    {
        private readonly IDtoCliente<ClienteDto> _clienteRepository;

        public ClienteDtoService(IDtoCliente<ClienteDto> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public void MostrarClientes()
        {
            var clientes = _clienteRepository.ObtenerTodos();
            if (!clientes.Any())
            {
                Console.WriteLine("No hay clientes registrados.");
                return;
            }

            foreach (var cliente in clientes)
            {
                Console.WriteLine($"ID: {cliente.TerceroId}");
                Console.WriteLine($"Nombre: {cliente.NombreCompleto}");
                Console.WriteLine($"Email: {cliente.Email}");
                Console.WriteLine($"Teléfono: {cliente.Telefono}");
                Console.WriteLine($"Tipo de Teléfono: {cliente.TipoTelefono}");
                Console.WriteLine($"Tipo de Documento: {cliente.TipoDocId}");
                Console.WriteLine($"Ciudad: {cliente.CiudadId}");
                Console.WriteLine($"Fecha de Nacimiento: {cliente.FechaNacimiento?.ToShortDateString() ?? "No especificada"}");
                Console.WriteLine($"Última Compra: {cliente.FechaUltimaCompra?.ToShortDateString() ?? "No registrada"}");
                Console.WriteLine("----------------------------------------");
            }
        }

        public ClienteDto? ObtenerClientePorId(string id)
        {
            return _clienteRepository.ObtenerPorId(id);
        }

        public void CrearCliente(ClienteDto cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            _clienteRepository.Crear(cliente);
        }

        public void ActualizarCliente(ClienteDto cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (string.IsNullOrEmpty(cliente.TerceroId))
                throw new ArgumentException("El ID del cliente es requerido", nameof(cliente));

            _clienteRepository.Actualizar(cliente);
        }

        public void EliminarCliente(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("El ID del cliente es requerido", nameof(id));

            _clienteRepository.Eliminar(id);
        }
    }
}