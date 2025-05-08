using System;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;

namespace SGIC_APP.Application.UI
{
    public class UIPrincipal
    {
        private readonly UIClienteDto _uiClienteDto;

        public UIPrincipal(IDtoCliente<ClienteDto> clienteRepository)
        {
            _uiClienteDto = new UIClienteDto(clienteRepository);
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ PRINCIPAL ===");
                Console.WriteLine("1. Gestión de Clientes");
                Console.WriteLine("0. Salir");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        _uiClienteDto.MostrarMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}