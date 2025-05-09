using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Application.UI
{
    public class UIProveedorDto
    {
        private readonly IDtoProveedor<Proveedor> _proveedorRepository;

        public UIProveedorDto(IDtoProveedor<Proveedor> proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE PROVEEDORES ===");
                Console.WriteLine("1. Listar Proveedores");
                Console.WriteLine("2. Buscar Proveedor");
                Console.WriteLine("3. Crear Proveedor");
                Console.WriteLine("4. Actualizar Proveedor");
                Console.WriteLine("5. Eliminar Proveedor");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            ListarProveedores();
                            break;
                        case "2":
                            BuscarProveedor();
                            break;
                        case "3":
                            CrearProveedor();
                            break;
                        case "4":
                            ActualizarProveedor();
                            break;
                        case "5":
                            EliminarProveedor();
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

        private void ListarProveedores()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE PROVEEDORES ===\n");

            var proveedores = _proveedorRepository.ObtenerTodos();
            foreach (var proveedor in proveedores)
            {
                Console.WriteLine($"ID: {proveedor.TerceroId}");
                Console.WriteLine($"Nombre: {proveedor.Nombre} {proveedor.Apellidos}");
                Console.WriteLine($"Email: {proveedor.Email}");
                Console.WriteLine($"Teléfono: {proveedor.Telefono}");
                Console.WriteLine($"Descuento: {proveedor.Descuento}%");
                Console.WriteLine($"Día de Pago: {proveedor.DiaPago}");
                Console.WriteLine($"Fecha de Registro: {proveedor.FechaRegistro:dd/MM/yyyy}");
                Console.WriteLine("------------------------");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void BuscarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR PROVEEDOR ===\n");
            Console.Write("Ingrese el ID del proveedor: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var proveedor = _proveedorRepository.ObtenerPorId(id.ToString());
                if (proveedor != null)
                {
                    Console.WriteLine("\nInformación del Proveedor:");
                    Console.WriteLine($"ID: {proveedor.TerceroId}");
                    Console.WriteLine($"Nombre: {proveedor.Nombre} {proveedor.Apellidos}");
                    Console.WriteLine($"Email: {proveedor.Email}");
                    Console.WriteLine($"Teléfono: {proveedor.Telefono}");
                    Console.WriteLine($"Descuento: {proveedor.Descuento}%");
                    Console.WriteLine($"Día de Pago: {proveedor.DiaPago}");
                    Console.WriteLine($"Fecha de Registro: {proveedor.FechaRegistro:dd/MM/yyyy}");
                }
                else
                {
                    Console.WriteLine("\nNo se encontró el proveedor.");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR PROVEEDOR ===\n");

            var proveedor = new Proveedor();

            Console.Write("Nombre: ");
            proveedor.Nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido");

            Console.Write("Apellidos: ");
            proveedor.Apellidos = Console.ReadLine() ?? throw new ArgumentException("Los apellidos son requeridos");

            Console.Write("Email: ");
            proveedor.Email = Console.ReadLine() ?? throw new ArgumentException("El email es requerido");

            Console.Write("Teléfono: ");
            proveedor.Telefono = Console.ReadLine() ?? throw new ArgumentException("El teléfono es requerido");

            Console.Write("Tipo de Teléfono: ");
            proveedor.TipoTelefono = Console.ReadLine() ?? throw new ArgumentException("El tipo de teléfono es requerido");

            Console.Write("Tipo de Documento ID: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
                throw new ArgumentException("Tipo de documento inválido");
            proveedor.TipoDocId = tipoDocId;

            Console.Write("Tipo de Tercero ID: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoTerceroId))
                throw new ArgumentException("Tipo de tercero inválido");
            proveedor.TipoTerceroId = tipoTerceroId;

            Console.Write("Ciudad ID: ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
                throw new ArgumentException("Ciudad inválida");
            proveedor.CiudadId = ciudadId;

            Console.Write("Descuento (%): ");
            if (!double.TryParse(Console.ReadLine(), out double descuento))
                throw new ArgumentException("Descuento inválido");
            proveedor.Descuento = descuento;

            Console.Write("Día de Pago: ");
            if (!int.TryParse(Console.ReadLine(), out int diaPago))
                throw new ArgumentException("Día de pago inválido");
            proveedor.DiaPago = diaPago;

            _proveedorRepository.Crear(proveedor);
            Console.WriteLine("\nProveedor creado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PROVEEDOR ===\n");
            Console.Write("Ingrese el ID del proveedor a actualizar: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var proveedor = _proveedorRepository.ObtenerPorId(id.ToString());
                if (proveedor != null)
                {
                    Console.Write("Nombre: ");
                    proveedor.Nombre = Console.ReadLine() ?? proveedor.Nombre;

                    Console.Write("Apellidos: ");
                    proveedor.Apellidos = Console.ReadLine() ?? proveedor.Apellidos;

                    Console.Write("Email: ");
                    proveedor.Email = Console.ReadLine() ?? proveedor.Email;

                    Console.Write("Teléfono: ");
                    proveedor.Telefono = Console.ReadLine() ?? proveedor.Telefono;

                    Console.Write("Tipo de Teléfono: ");
                    proveedor.TipoTelefono = Console.ReadLine() ?? proveedor.TipoTelefono;

                    Console.Write("Tipo de Documento ID: ");
                    if (int.TryParse(Console.ReadLine(), out int tipoDocId))
                        proveedor.TipoDocId = tipoDocId;

                    Console.Write("Tipo de Tercero ID: ");
                    if (int.TryParse(Console.ReadLine(), out int tipoTerceroId))
                        proveedor.TipoTerceroId = tipoTerceroId;

                    Console.Write("Ciudad ID: ");
                    if (int.TryParse(Console.ReadLine(), out int ciudadId))
                        proveedor.CiudadId = ciudadId;

                    Console.Write("Descuento (%): ");
                    if (double.TryParse(Console.ReadLine(), out double descuento))
                        proveedor.Descuento = descuento;

                    Console.Write("Día de Pago: ");
                    if (int.TryParse(Console.ReadLine(), out int diaPago))
                        proveedor.DiaPago = diaPago;

                    _proveedorRepository.Actualizar(proveedor);
                    Console.WriteLine("\nProveedor actualizado exitosamente.");
                }
                else
                {
                    Console.WriteLine("\nNo se encontró el proveedor.");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PROVEEDOR ===\n");
            Console.Write("Ingrese el ID del proveedor a eliminar: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var proveedor = _proveedorRepository.ObtenerPorId(id.ToString());
                if (proveedor != null)
                {
                    Console.WriteLine($"\n¿Está seguro de eliminar al proveedor {proveedor.Nombre} {proveedor.Apellidos}? (S/N)");
                    var confirmacion = Console.ReadLine()?.ToUpper();
                    if (confirmacion == "S")
                    {
                        _proveedorRepository.Eliminar(id.ToString());
                        Console.WriteLine("\nProveedor eliminado exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine("\nOperación cancelada.");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo se encontró el proveedor.");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 