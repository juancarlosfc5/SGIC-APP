using System;
using System.Collections.Generic;
using SGIC_APP.Application.Services;   
using SGIC_APP.Domain.Dto;              
using SGIC_APP.Domain.Ports;            
using SGIC_APP.Application.UI; 

namespace SGIC_APP.Application.UI
{
    public class DtoCliente
    {
        private readonly ClienteDtoService _clienteService;
        private readonly UIClienteDto _clienteDtoUI;

        public DtoCliente(IDbFactory factory)
        {
            _clienteService = new ClienteDtoService(
                factory.CrearTerceroRepository(),
                factory.CrearClienteRepository()
            );

            _clienteDtoUI = new UIClienteDto();
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- MENÚ CLIENTES (DTO) ---");
                Console.WriteLine("1. Mostrar todos");
                Console.WriteLine("2. Crear nuevo");
                Console.WriteLine("3. Actualizar");
                Console.WriteLine("4. Eliminar");
                Console.WriteLine("0. Salir");
                Console.Write("Opción: ");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Clientes registrados.\n");
                        _clienteService.MostrarClientes(); // Este método lo implementaremos después
                        Console.WriteLine("\nPresione una tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        var dto = _clienteDtoUI.CrearClienteDtoDesdeConsola();
                        _clienteService.CrearCliente(dto);
                        Console.WriteLine("\n✅ Cliente creado.");
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("Actualizar cliente (por ID de tercero).");
                        Console.Write("Ingrese ID del tercero: ");
                        int idTercero = int.Parse(Console.ReadLine()!);
                        var dtoActualizar = _clienteDtoUI.CrearClienteDtoDesdeConsola();
                        dtoActualizar.Id = idTercero;
                        _clienteService.ActualizarCliente(dtoActualizar);
                        Console.WriteLine("\n✅ Cliente actualizado.");
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("Eliminar cliente.");
                        Console.Write("Ingrese ID del tercero a eliminar: ");
                        int idEliminar = int.Parse(Console.ReadLine()!);
                        _clienteService.EliminarCliente(idEliminar);
                        Console.WriteLine("\n✅ Cliente eliminado.");
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("❌ Opción inválida.");
                        Console.WriteLine("Presione cualquier tecla para volver al menú...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}