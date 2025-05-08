using System;
using SGIC_APP.Application.UI;
using SGIC_APP.Infrastructure.Repositories;

namespace SGIC_APP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string connectionString = "Server=localhost;Database=NJSIGC;User=root;Password=123456;";
                var clienteRepository = new ImplDtoCliente(connectionString);
                var uiPrincipal = new UIPrincipal(clienteRepository);
                uiPrincipal.MostrarMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Presione cualquier tecla para salir...");
                Console.ReadKey();
            }
        }
    }
}