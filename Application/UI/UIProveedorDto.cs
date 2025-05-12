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
                Console.WriteLine("2. Crear Proveedor");
                Console.WriteLine("3. Actualizar Proveedor");
                Console.WriteLine("4. Eliminar Proveedor");
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
                            CrearProveedor();
                            break;
                        case "3":
                            ActualizarProveedor();
                            break;
                        case "4":
                            EliminarProveedor();
                            break;
                        case "0":
                            Console.Clear();
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

        private void CrearProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR PROVEEDOR ===\n");

            var proveedor = new Proveedor();

            // Solicitar TERCERO_ID y verificar si ya existe
            Console.Write("TERCERO_ID: ");
            var terceroId = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(terceroId))
                throw new Exception("El TERCERO_ID es requerido.");
            if (_proveedorRepository.ObtenerPorId(terceroId) != null)
            {
                Console.WriteLine("\nEl número de identificación ingresado ya está registrado como proveedor.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            proveedor.TerceroId = terceroId;

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
            // Listar proveedores existentes
            var proveedores = _proveedorRepository.ObtenerTodos().ToList();
            if (!proveedores.Any())
            {
                Console.WriteLine("No hay proveedores disponibles.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Listado de proveedores disponibles:");
            foreach (var p in proveedores)
            {
                Console.WriteLine($"Tercero_ID: {p.TerceroId} - Nombre: {p.Nombre} {p.Apellidos}");
            }
            Console.WriteLine(new string('-', 50));

            Console.Write("Ingrese el TERCERO_ID del proveedor a actualizar: ");
            var id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nEl TERCERO_ID no puede estar vacío.");
                Console.ReadKey();
                return;
            }

            var proveedor = _proveedorRepository.ObtenerPorId(id);
            if (proveedor == null)
            {
                Console.WriteLine("\nNo se encontró el proveedor.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nDeje en blanco los campos que no desee modificar.\n");

            Console.Write($"Nombre ({proveedor.Nombre}): ");
            var nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre))
                proveedor.Nombre = nombre;

            Console.Write($"Apellidos ({proveedor.Apellidos}): ");
            var apellidos = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(apellidos))
                proveedor.Apellidos = apellidos;

            Console.Write($"Email ({proveedor.Email}): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                proveedor.Email = email;

            Console.Write($"Teléfono ({proveedor.Telefono}): ");
            var telefono = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(telefono))
                proveedor.Telefono = telefono;

            Console.Write($"Tipo de Teléfono ({proveedor.TipoTelefono}): ");
            var tipoTelefono = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoTelefono))
                proveedor.TipoTelefono = tipoTelefono;

            Console.Write($"Tipo de Documento ID ({proveedor.TipoDocId}): ");
            var tipoDocIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoDocIdStr) && int.TryParse(tipoDocIdStr, out int tipoDocId))
                proveedor.TipoDocId = tipoDocId;

            Console.Write($"Tipo de Tercero ID ({proveedor.TipoTerceroId}): ");
            var tipoTerceroIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoTerceroIdStr) && int.TryParse(tipoTerceroIdStr, out int tipoTerceroId))
                proveedor.TipoTerceroId = tipoTerceroId;

            Console.Write($"Ciudad ID ({proveedor.CiudadId}): ");
            var ciudadIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ciudadIdStr) && int.TryParse(ciudadIdStr, out int ciudadId))
                proveedor.CiudadId = ciudadId;

            Console.Write($"Descuento (%) ({proveedor.Descuento}%): ");
            var descuentoStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(descuentoStr) && double.TryParse(descuentoStr, out double descuento))
                proveedor.Descuento = descuento;

            Console.Write($"Día de Pago ({proveedor.DiaPago}): ");
            var diaPagoStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(diaPagoStr) && int.TryParse(diaPagoStr, out int diaPago))
                proveedor.DiaPago = diaPago;

            _proveedorRepository.Actualizar(proveedor);
            Console.WriteLine("\nProveedor actualizado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PROVEEDOR ===\n");
            var proveedores = _proveedorRepository.ObtenerTodos().ToList();
            if (!proveedores.Any())
            {
                Console.WriteLine("No hay proveedores disponibles.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Listado de proveedores disponibles:");
            foreach (var p in proveedores)
            {
                Console.WriteLine($"Tercero_ID: {p.TerceroId} - Nombre: {p.Nombre} {p.Apellidos}");
            }
            Console.WriteLine(new string('-', 50));
            Console.Write("Ingrese el TERCERO_ID del proveedor a eliminar: ");
            var id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nEl TERCERO_ID no puede estar vacío.");
                Console.ReadKey();
                return;
            }
            var proveedor = _proveedorRepository.ObtenerPorId(id);
            if (proveedor == null)
            {
                Console.WriteLine("\nNo se encontró el proveedor.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("\n¿Está seguro que desea eliminar el siguiente proveedor?");
            MostrarProveedor(proveedor);
            Console.Write("\nIngrese 'SI' para confirmar: ");
            if (Console.ReadLine()?.ToUpper() != "SI")
            {
                Console.WriteLine("\nOperación cancelada.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            _proveedorRepository.Eliminar(id);
            Console.WriteLine("\nProveedor eliminado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void MostrarProveedor(Proveedor proveedor)
        {
            Console.WriteLine($"Tercero_ID: {proveedor.TerceroId}");
            Console.WriteLine($"Nombre: {proveedor.Nombre} {proveedor.Apellidos}");
            Console.WriteLine($"Email: {proveedor.Email}");
            Console.WriteLine($"Teléfono: {proveedor.Telefono}");
            Console.WriteLine($"Tipo de Teléfono: {proveedor.TipoTelefono}");
            Console.WriteLine($"Tipo de Documento ID: {proveedor.TipoDocId}");
            Console.WriteLine($"Tipo de Tercero ID: {proveedor.TipoTerceroId}");
            Console.WriteLine($"Ciudad ID: {proveedor.CiudadId}");
            Console.WriteLine($"Descuento: {proveedor.Descuento}%");
            Console.WriteLine($"Día de Pago: {proveedor.DiaPago}");
            Console.WriteLine($"Fecha de Registro: {proveedor.FechaRegistro:dd/MM/yyyy}");
        }
    }
} 