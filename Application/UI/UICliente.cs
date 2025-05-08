using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using SGICAPP.Application.Services;
using SGICAPP.Domain.Factory;

namespace SGICAPP.Application.UI.Clientes;
public class UICliente
{
    private readonly ClienteService clienteService;
    private readonly IDbFactory _factory;
    //var servicio = new ClienteService(factory.CrearClienteRepository());

    public UICliente(IDbFactory factory)
    {
        _factory = factory;
        clienteService = new ClienteService(factory.CrearClienteRepository());

    }

    public void MenuCliente(){
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- MENÚ CLIENTES ---");
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
                    clienteService.MostrarCliente();
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Crear nuevo cliente.");
                    Console.Write("Ingrese el ID del tercero: ");
                    var terceroId = Console.ReadLine();
                    Console.Write("Ingrese la fecha de nacimiento (yyyy-MM-dd): ");
                    var fechaNac = DateTime.Parse(Console.ReadLine()!);
                    Console.Write("Ingrese la fecha de última compra (yyyy-MM-dd): ");
                    var fechaUltima = DateTime.Parse(Console.ReadLine()!);
                    clienteService.CrearCliente(terceroId!, fechaNac, fechaUltima);
                    Console.WriteLine("\nCliente creado.");
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Actualizar cliente.");
                    Console.Write("Ingrese ID del cliente a actualizar: ");
                    int idActualizar = int.Parse(Console.ReadLine()!);
                    Console.Write("Ingrese el ID del tercero: ");
                    var terceroId2 = Console.ReadLine();
                    Console.Write("Nueva fecha de nacimiento (yyyy-MM-dd): ");
                    var fechaNac2 = DateTime.Parse(Console.ReadLine()!);
                    Console.Write("Nueva fecha de última compra (yyyy-MM-dd): ");
                    var fechaUltima2 = DateTime.Parse(Console.ReadLine()!);
                    clienteService.ActualizarCliente(terceroId2, fechaNac2, fechaUltima2);
                    Console.WriteLine("\nCliente actualizado.");
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Eliminar cliente.");
                    Console.Write("Ingrese ID del cliente a eliminar: ");
                    int idEliminar = int.Parse(Console.ReadLine()!);
                    clienteService.EliminarCliente(idEliminar);
                    Console.WriteLine("\nCliente eliminado.");
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