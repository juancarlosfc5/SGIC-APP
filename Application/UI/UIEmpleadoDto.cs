using System;
using System.Collections.Generic;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;
using System.Linq;

namespace SGIC_APP.Application.UI
{
    public class UIEmpleadoDto
    {
        private readonly IDtoEmpleado<EmpleadoDto> _empleadoRepository;
        private static readonly Dictionary<int, string> TiposTercero = new Dictionary<int, string>
        {
            { 1, "Cliente" },
            { 2, "Empleado" },
            { 3, "Proveedor" }
        };

        public UIEmpleadoDto(IDtoEmpleado<EmpleadoDto> empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE EMPLEADOS ===");
                Console.WriteLine("1. Listar Empleados");
                Console.WriteLine("2. Crear Empleado");
                Console.WriteLine("3. Actualizar Empleado");
                Console.WriteLine("4. Eliminar Empleado");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            ListarEmpleados();
                            break;
                        case "2":
                            CrearEmpleado();
                            break;
                        case "3":
                            ActualizarEmpleado();
                            break;
                        case "4":
                            EliminarEmpleado();
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

        private void ListarEmpleados()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE EMPLEADOS ===\n");

            var empleados = _empleadoRepository.ObtenerTodos();
            if (!empleados.Any())
            {
                Console.WriteLine("No hay empleados registrados.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            foreach (var empleado in empleados)
            {
                Console.WriteLine("=== INFORMACIÓN DEL EMPLEADO ===");
                Console.WriteLine($"ID: {empleado.TerceroId}");
                Console.WriteLine($"Nombre Completo: {empleado.NombreCompleto}");
                Console.WriteLine($"Email: {empleado.Email}");
                Console.WriteLine($"Teléfono: {empleado.Telefono} ({empleado.TipoTelefono})");
                Console.WriteLine($"Salario Base: ${empleado.SalarioBase:N2}");
                Console.WriteLine($"Fecha de Ingreso: {empleado.FechaIngreso:dd/MM/yyyy}");
                if (empleado.EpsId.HasValue)
                    Console.WriteLine($"EPS ID: {empleado.EpsId}");
                if (empleado.ArlId.HasValue)
                    Console.WriteLine($"ARL ID: {empleado.ArlId}");
                Console.WriteLine("Estado: " + (empleado.Activo ? "Activo" : "Inactivo"));
                Console.WriteLine("------------------------\n");
            }

            Console.WriteLine($"Total de empleados: {empleados.Count()}");
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void BuscarEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR EMPLEADO ===\n");
            Console.Write("Ingrese el ID del empleado: ");

            var id = Console.ReadLine();
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nID inválido.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var empleado = _empleadoRepository.ObtenerPorId(id);
            if (empleado != null)
            {
                Console.WriteLine("\nInformación del Empleado:");
                Console.WriteLine($"ID: {empleado.TerceroId}");
                Console.WriteLine($"Nombre: {empleado.Nombre} {empleado.Apellidos}");
                Console.WriteLine($"Email: {empleado.Email}");
                Console.WriteLine($"Teléfono: {empleado.Telefono}");
                Console.WriteLine($"Tipo de Teléfono: {empleado.TipoTelefono}");
                Console.WriteLine($"Salario Base: ${empleado.SalarioBase:N2}");
                Console.WriteLine($"Fecha de Ingreso: {empleado.FechaIngreso:dd/MM/yyyy}");
            }
            else
            {
                Console.WriteLine("\nNo se encontró el empleado.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR EMPLEADO ===\n");

            Console.Write("Nombre: ");
            var nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido");

            Console.Write("Apellidos: ");
            var apellidos = Console.ReadLine() ?? throw new ArgumentException("Los apellidos son requeridos");

            Console.Write("Email: ");
            var email = Console.ReadLine() ?? throw new ArgumentException("El email es requerido");

            Console.Write("Teléfono: ");
            var telefono = Console.ReadLine() ?? throw new ArgumentException("El teléfono es requerido");

            Console.Write("Tipo de Teléfono: ");
            var tipoTelefono = Console.ReadLine() ?? throw new ArgumentException("El tipo de teléfono es requerido");

            Console.Write("Tipo de Documento ID: ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
                throw new ArgumentException("Tipo de documento inválido");

            Console.WriteLine("\nTipos de Tercero disponibles:");
            foreach (var tipo in TiposTercero)
            {
                Console.WriteLine($"{tipo.Key} - {tipo.Value}");
            }
            Console.Write("\nSeleccione el Tipo de Tercero (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int tipoTerceroId) || !TiposTercero.ContainsKey(tipoTerceroId))
                throw new ArgumentException("Tipo de tercero inválido. Debe seleccionar uno de los tipos listados.");

            Console.Write("Ciudad ID: ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
                throw new ArgumentException("Ciudad inválida");

            Console.Write("Salario Base: ");
            if (!double.TryParse(Console.ReadLine(), out double salarioBase))
                throw new ArgumentException("Salario inválido");

            Console.Write("Fecha de Ingreso (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaIngreso))
                throw new ArgumentException("Fecha inválida");

            var empleado = new EmpleadoDto
            {
                TerceroId = DateTime.Now.ToString("yyyyMMddHHmmss"),
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                Telefono = telefono,
                TipoTelefono = tipoTelefono,
                TipoDocId = tipoDocId,
                TipoTerceroId = tipoTerceroId,
                CiudadId = ciudadId,
                SalarioBase = salarioBase,
                FechaIngreso = fechaIngreso
            };

            _empleadoRepository.Crear(empleado);
            Console.WriteLine("\nEmpleado creado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR EMPLEADO ===\n");
            Console.Write("Ingrese el ID del empleado a actualizar: ");

            var id = Console.ReadLine();
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nID inválido.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var empleado = _empleadoRepository.ObtenerPorId(id);
            if (empleado != null)
            {
                Console.Write("Nombre: ");
                empleado.Nombre = Console.ReadLine() ?? empleado.Nombre;

                Console.Write("Apellidos: ");
                empleado.Apellidos = Console.ReadLine() ?? empleado.Apellidos;

                Console.Write("Email: ");
                empleado.Email = Console.ReadLine() ?? empleado.Email;

                Console.Write("Teléfono: ");
                empleado.Telefono = Console.ReadLine() ?? empleado.Telefono;

                Console.Write("Tipo de Teléfono: ");
                empleado.TipoTelefono = Console.ReadLine() ?? empleado.TipoTelefono;

                Console.Write("Tipo de Documento ID: ");
                if (int.TryParse(Console.ReadLine(), out int tipoDocId))
                    empleado.TipoDocId = tipoDocId;

                Console.Write("Tipo de Tercero ID: ");
                if (int.TryParse(Console.ReadLine(), out int tipoTerceroId))
                    empleado.TipoTerceroId = tipoTerceroId;

                Console.Write("Ciudad ID: ");
                if (int.TryParse(Console.ReadLine(), out int ciudadId))
                    empleado.CiudadId = ciudadId;

                Console.Write("Salario Base: ");
                if (double.TryParse(Console.ReadLine(), out double salarioBase))
                    empleado.SalarioBase = salarioBase;

                Console.Write("Fecha de Ingreso (dd/MM/yyyy): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fechaIngreso))
                    empleado.FechaIngreso = fechaIngreso;

                _empleadoRepository.Actualizar(empleado);
                Console.WriteLine("\nEmpleado actualizado exitosamente.");
            }
            else
            {
                Console.WriteLine("\nNo se encontró el empleado.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR EMPLEADO ===\n");
            Console.Write("Ingrese el ID del empleado a eliminar: ");

            var id = Console.ReadLine();
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nID inválido.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var empleado = _empleadoRepository.ObtenerPorId(id);
            if (empleado != null)
            {
                Console.WriteLine($"\n¿Está seguro de eliminar al empleado {empleado.Nombre} {empleado.Apellidos}? (S/N)");
                var confirmacion = Console.ReadLine()?.ToUpper();
                if (confirmacion == "S")
                {
                    _empleadoRepository.Eliminar(id);
                    Console.WriteLine("\nEmpleado eliminado exitosamente.");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            else
            {
                Console.WriteLine("\nNo se encontró el empleado.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 