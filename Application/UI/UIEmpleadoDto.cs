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
                Console.WriteLine("5. Volver al Menú Principal");
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
                        case "5":
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
                Console.WriteLine($"TerceroId: {empleado.TerceroId}");
                Console.WriteLine($"Nombre Completo: {empleado.NombreCompleto}");
                Console.WriteLine($"Email: {empleado.Email}");
                Console.WriteLine($"Salario Base: ${empleado.SalarioBase:N2}");
                Console.WriteLine($"Fecha de Ingreso: {empleado.FechaIngreso:dd/MM/yyyy}");
                if (empleado.EpsId.HasValue)
                    Console.WriteLine($"EPS ID: {empleado.EpsId}");
                if (empleado.ArlId.HasValue)
                    Console.WriteLine($"ARL ID: {empleado.ArlId}");
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
                Console.WriteLine($"TerceroId: {empleado.TerceroId}");
                Console.WriteLine($"Nombre: {empleado.Nombre} {empleado.Apellidos}");
                Console.WriteLine($"Email: {empleado.Email}");
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

            try
            {
                Console.Write("TerceroId: ");
                var terceroId = Console.ReadLine() ?? throw new ArgumentException("El TerceroId es requerido");

                Console.Write("Nombre Completo: ");
                var nombreCompleto = Console.ReadLine() ?? throw new ArgumentException("El nombre completo es requerido");
                var partesNombre = nombreCompleto.Split(' ', 2);
                var nombre = partesNombre[0];
                var apellidos = partesNombre.Length > 1 ? partesNombre[1] : "";

                Console.Write("Email: ");
                var email = Console.ReadLine() ?? throw new ArgumentException("El email es requerido");

                Console.Write("Salario Base: ");
                if (!double.TryParse(Console.ReadLine(), out double salarioBase))
                    throw new ArgumentException("Salario inválido");

                Console.Write("Fecha de Ingreso (dd/MM/yyyy): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaIngreso))
                    throw new ArgumentException("Fecha inválida");

                Console.Write("EPS ID: ");
                if (!int.TryParse(Console.ReadLine(), out int epsId))
                    throw new ArgumentException("EPS ID inválido");

                Console.Write("ARL ID: ");
                if (!int.TryParse(Console.ReadLine(), out int arlId))
                    throw new ArgumentException("ARL ID inválido");

                var empleado = new EmpleadoDto
                {
                    TerceroId = terceroId,
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Email = email,
                    TipoDocId = 1, // Valor por defecto
                    TipoTerceroId = 2, // 2 para empleados
                    CiudadId = 1, // Valor por defecto
                    SalarioBase = salarioBase,
                    FechaIngreso = fechaIngreso,
                    EpsId = epsId,
                    ArlId = arlId
                };

                _empleadoRepository.Crear(empleado);
                Console.WriteLine("\nEmpleado creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
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
                Console.WriteLine("\nDatos actuales del empleado:");
                Console.WriteLine($"TerceroId: {empleado.TerceroId}");
                Console.WriteLine($"Nombre: {empleado.Nombre}");
                Console.WriteLine($"Apellidos: {empleado.Apellidos}");
                Console.WriteLine($"Email: {empleado.Email}");
                Console.WriteLine($"Salario Base: ${empleado.SalarioBase:N2}");
                Console.WriteLine($"Fecha de Ingreso: {empleado.FechaIngreso:dd/MM/yyyy}");
                Console.WriteLine($"EPS ID: {empleado.EpsId}");
                Console.WriteLine($"ARL ID: {empleado.ArlId}");
                Console.WriteLine("\nIngrese los nuevos valores (deje en blanco para mantener el valor actual):");

                Console.Write($"TerceroId ({empleado.TerceroId}): ");
                var nuevoTerceroId = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoTerceroId))
                    empleado.TerceroId = nuevoTerceroId;

                Console.Write($"Nombre ({empleado.Nombre}): ");
                var nuevoNombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoNombre))
                    empleado.Nombre = nuevoNombre;

                Console.Write($"Apellidos ({empleado.Apellidos}): ");
                var nuevosApellidos = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevosApellidos))
                    empleado.Apellidos = nuevosApellidos;

                Console.Write($"Email ({empleado.Email}): ");
                var nuevoEmail = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoEmail))
                    empleado.Email = nuevoEmail;

                Console.Write($"Salario Base (${empleado.SalarioBase:N2}): ");
                var nuevoSalario = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoSalario) && double.TryParse(nuevoSalario, out double salarioBase))
                    empleado.SalarioBase = salarioBase;

                Console.Write($"Fecha de Ingreso ({empleado.FechaIngreso:dd/MM/yyyy}): ");
                var nuevaFecha = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevaFecha) && DateTime.TryParse(nuevaFecha, out DateTime fechaIngreso))
                    empleado.FechaIngreso = fechaIngreso;

                Console.Write($"EPS ID ({empleado.EpsId}): ");
                var nuevoEpsId = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoEpsId) && int.TryParse(nuevoEpsId, out int epsId))
                    empleado.EpsId = epsId;

                Console.Write($"ARL ID ({empleado.ArlId}): ");
                var nuevoArlId = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nuevoArlId) && int.TryParse(nuevoArlId, out int arlId))
                    empleado.ArlId = arlId;

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
            Console.Write("Ingrese el TerceroId del empleado a eliminar: ");

            var id = Console.ReadLine();
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nTerceroId inválido.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var empleado = _empleadoRepository.ObtenerPorId(id);
            if (empleado != null)
            {
                Console.WriteLine("\n=== INFORMACIÓN DEL EMPLEADO A ELIMINAR ===");
                Console.WriteLine($"TerceroId: {empleado.TerceroId}");
                Console.WriteLine($"Nombre Completo: {empleado.NombreCompleto}");
                Console.WriteLine($"Email: {empleado.Email}");
                Console.WriteLine($"Salario Base: ${empleado.SalarioBase:N2}");
                Console.WriteLine($"Fecha de Ingreso: {empleado.FechaIngreso:dd/MM/yyyy}");
                if (empleado.EpsId.HasValue)
                    Console.WriteLine($"EPS ID: {empleado.EpsId}");
                if (empleado.ArlId.HasValue)
                    Console.WriteLine($"ARL ID: {empleado.ArlId}");
                Console.WriteLine("\n¿Está seguro que desea eliminar este empleado? (S/N)");
                
                var confirmacion = Console.ReadLine()?.ToUpper();
                while (confirmacion != "S" && confirmacion != "N")
                {
                    Console.WriteLine("\nPor favor, ingrese 'S' para confirmar o 'N' para cancelar:");
                    confirmacion = Console.ReadLine()?.ToUpper();
                }

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
                Console.WriteLine("\nNo se encontró el empleado con el TerceroId especificado.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 