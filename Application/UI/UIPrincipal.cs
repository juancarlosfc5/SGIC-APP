using System;
using SGIC_APP.Domain.Factory;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Application.UI
{
    public class UIPrincipal
    {
        private readonly IDbFactory _dbFactory;
        private readonly UIClienteDto _uiCliente;
        private readonly UIEmpleadoDto _uiEmpleado;
        private readonly UIProductoDto _uiProducto;
        private readonly UIProveedorDto _uiProveedor;

        public UIPrincipal(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
            _uiCliente = new UIClienteDto(_dbFactory.CrearClienteRepository());
            _uiEmpleado = new UIEmpleadoDto(_dbFactory.CrearEmpleadoRepository());
            _uiProducto = new UIProductoDto(_dbFactory.CrearProductoRepository());
            _uiProveedor = new UIProveedorDto(_dbFactory.CrearProveedorRepository());
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ PRINCIPAL ===");
                Console.WriteLine("1. Gestión de Clientes");
                Console.WriteLine("2. Gestión de Empleados");
                Console.WriteLine("3. Gestión de Productos");
                Console.WriteLine("4. Gestión de Proveedores");
                Console.WriteLine("0. Salir");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            _uiCliente.MostrarMenu();
                            break;
                        case "2":
                            _uiEmpleado.MostrarMenu();
                            break;
                        case "3":
                            _uiProducto.MostrarMenu();
                            break;
                        case "4":
                            _uiProveedor.MostrarMenu();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("\nOpción no válida. Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
}