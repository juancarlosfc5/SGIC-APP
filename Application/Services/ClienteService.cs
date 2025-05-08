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
            Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}");
        }
    }

    public void CrearCliente(string nombre)
    {
        _repo.Crear(new Cliente { Nombre = nombre });
    }

    public void ActualizarCliente(int id, string nuevoNombre)
    {
        _repo.Actualizar(new Cliente { Id = id, Nombre = nuevoNombre });
    }

    public void EliminarCliente(int id)
    {
        _repo.Eliminar(id);
    }
}