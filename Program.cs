using System;
using SGIC_APP.Domain.Factory;
using SGIC_APP.Infrastructure.Mysql;
using SGIC_APP.Application.UI;

namespace SGIC_APP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string connectionString = "Server=localhost;Database=NJSIGC;User=root;Password=123456;";
                IDbFactory dbFactory = new MySqlDbFactory(connectionString);

                var uiCliente = new UIClienteDto(dbFactory.CrearClienteRepository());
                var uiEmpleado = new UIEmpleadoDto(dbFactory.CrearEmpleadoRepository());
                var uiProducto = new UIProductoDto(dbFactory.CrearProductoRepository());
                var uiProveedor = new UIProveedorDto(dbFactory.CrearProveedorRepository());

                var uiPrincipal = new UIPrincipal(dbFactory);
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