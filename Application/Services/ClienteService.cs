using System;
using SGICAPP.Domain.Entities;
using SGICAPP.Domain.Ports;

namespace SGICAPP.Application.Services;

public class ClienteService
{
    private readonly IClienteRepository _repo;

    public ClienteService(IClienteRepository repo)
    {
        _repo = repo;
    }

    public void MostrarCliente()
        {
            var lista = _repo.ObtenerTodos();
            foreach (var c in lista)
            {
                Console.WriteLine($"ID: {c.Id}, Tercero ID: {c.tercero_id},Fecha Nac: {c.fecha_nac.ToShortDateString()}, Última Compra: {c.fecha_ultima_compra.ToShortDateString()}");
            }
        }

    public void CrearCliente(string terceroId, DateTime fechaNac, DateTime fechaUltimaCompra)
        {
            var nuevoCliente = new Cliente
            {
                tercero_id = terceroId,
                fecha_nac = fechaNac,
                fecha_ultima_compra = fechaUltimaCompra
            };
            _repo.Crear(nuevoCliente);
        }

        public void ActualizarCliente(string? tercero_id, DateTime nuevaFechaNac, DateTime nuevaFechaUltimaCompra)
        {
            var clienteActualizado = new Cliente
            {
                tercero_id = tercero_id,
                fecha_nac = nuevaFechaNac,
                fecha_ultima_compra = nuevaFechaUltimaCompra
            };
            _repo.Actualizar(clienteActualizado);
        }
    public void EliminarCliente(int id)
    {
        _repo.Eliminar(id);
    }
}