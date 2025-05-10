using System;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;
using System.Collections.Generic;

namespace SGIC_APP.Application.UI
{
    public class UIEmpleadoDto
    {
        private readonly IDtoEmpleado<EmpleadoDto> _empleadoRepository;
        private static readonly Dictionary<int, string> TiposDocumento = new Dictionary<int, string>
        {
            { 1, "Cédula de ciudadanía" },
            { 2, "NIT" },
            { 3, "Pasaporte" }
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
                Console.WriteLine("=== MENÚ DE EMPLEADOS ===");
                Console.WriteLine("1. Ver todos los empleados");
                Console.WriteLine("2. Crear nuevo empleado");
                Console.WriteLine("3. Actualizar empleado");
                Console.WriteLine("4. Eliminar empleado");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out int opcion))
                {
                    Console.WriteLine("\nOpción inválida. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            MostrarTodosLosEmpleados();
                            break;
                        case 2:
                            CrearEmpleado();
                            break;
                        case 3:
                            ActualizarEmpleado();
                            break;
                        case 4:
                            EliminarEmpleado();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("\nOpción inválida. Presione cualquier tecla para continuar...");
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

        private void MostrarTodosLosEmpleados()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE EMPLEADOS ===\n");

            var empleados = _empleadoRepository.ObtenerTodos();
            if (!empleados.Any())
            {
                Console.WriteLine("No hay empleados registrados.");
            }
            else
            {
                foreach (var emp in empleados)
                {
                    MostrarEmpleado(emp);
                    Console.WriteLine(new string('-', 80));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO EMPLEADO ===\n");

            Console.Write("TERCERO_ID: ");
            var terceroId = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(terceroId))
                throw new Exception("El TERCERO_ID es requerido.");
            if (_empleadoRepository.ObtenerPorId(terceroId) != null)
            {
                Console.WriteLine("\nEl TERCERO_ID ingresado ya está en uso (cliente, empleado o proveedor).");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Nombre: ");
            var nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es requerido.");

            Console.Write("Apellidos: ");
            var apellidos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(apellidos))
                throw new Exception("Los apellidos son requeridos.");

            Console.Write("Email: ");
            var email = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("El email es requerido.");

            Console.WriteLine("Tipos de documento disponibles:");
            foreach (var kvp in TiposDocumento)
            {
                Console.WriteLine($"{kvp.Key} - {kvp.Value}");
            }
            Console.Write("Tipo de Documento (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
                throw new Exception("El tipo de documento debe ser un número entero.");

            var empleado = new EmpleadoDto
            {
                TerceroId = terceroId,
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                TipoDocId = tipoDocId,
                TipoTerceroId = 2 // Tipo Empleado
            };

            Console.Write("Ciudad (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
                throw new Exception("La ciudad debe ser un número entero.");
            empleado.CiudadId = ciudadId;

            Console.Write("Fecha de Contratación (yyyy-MM-dd): ");
            var fechaContratacionStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fechaContratacionStr))
            {
                if (!DateTime.TryParse(fechaContratacionStr, out DateTime fechaContratacion))
                    throw new Exception("El formato de fecha debe ser yyyy-MM-dd.");
                empleado.FechaContratacion = fechaContratacion;
            }

            _empleadoRepository.Crear(empleado);
            Console.WriteLine("\nEmpleado creado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR EMPLEADO ===\n");

            var empleados = _empleadoRepository.ObtenerTodos();
            if (!empleados.Any())
            {
                Console.WriteLine("No hay empleados disponibles.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Listado de empleados:");
            foreach (var emp in empleados)
                Console.WriteLine($"Tercero_ID: {emp.TerceroId}  Nombre: {emp.Nombre} {emp.Apellidos}");

            Console.Write("\nIngrese el TERCERO_ID del empleado a actualizar: ");
            var id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id))
                return;

            var empleado = _empleadoRepository.ObtenerPorId(id);
            if (empleado == null)
            {
                Console.WriteLine("Empleado no encontrado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nDeje los campos en blanco si no desea modificar.");

            Console.Write($"Nombre ({empleado.Nombre}): ");
            var nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre)) empleado.Nombre = nombre;

            Console.Write($"Apellidos ({empleado.Apellidos}): ");
            var apellidos = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(apellidos)) empleado.Apellidos = apellidos;

            Console.Write($"Email ({empleado.Email}): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) empleado.Email = email;

            Console.Write($"Ciudad ({empleado.CiudadId}): ");
            var ciudadStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ciudadStr) && int.TryParse(ciudadStr, out int ciudadId))
                empleado.CiudadId = ciudadId;

            Console.Write($"Fecha de Contratación ({empleado.FechaContratacion?.ToShortDateString()}): ");
            var fechaStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fechaStr) && DateTime.TryParse(fechaStr, out DateTime fecha))
                empleado.FechaContratacion = fecha;

            _empleadoRepository.Actualizar(empleado);
            Console.WriteLine("\nEmpleado actualizado exitosamente.");
            Console.ReadKey();
        }

        private void EliminarEmpleado()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR EMPLEADO ===\n");

            var empleados = _empleadoRepository.ObtenerTodos();
            if (!empleados.Any())
            {
                Console.WriteLine("No hay empleados disponibles.");
                Console.ReadKey();
                return;
            }

            foreach (var emp in empleados)
                Console.WriteLine($"Tercero_ID: {emp.TerceroId} - {emp.Nombre} {emp.Apellidos}");

            Console.Write("\nIngrese el TERCERO_ID del empleado a eliminar: ");
            var id = Console.ReadLine();
            var empleado = _empleadoRepository.ObtenerPorId(id);
            if (empleado == null)
            {
                Console.WriteLine("Empleado no encontrado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n¿Está seguro que desea eliminar el empleado {empleado.Nombre} {empleado.Apellidos}? (Escriba 'SI' para confirmar)");
            if (Console.ReadLine()?.ToUpper() != "SI")
            {
                Console.WriteLine("Operación cancelada.");
                Console.ReadKey();
                return;
            }

            _empleadoRepository.Eliminar(id);
            Console.WriteLine("Empleado eliminado exitosamente.");
            Console.ReadKey();
        }

        private void MostrarEmpleado(EmpleadoDto emp)
        {
            string tipoDoc = TiposDocumento.ContainsKey(emp.TipoDocId) ? TiposDocumento[emp.TipoDocId] : "Desconocido";
            Console.WriteLine($"TERCERO_ID: {emp.TerceroId}");
            Console.WriteLine($"Nombre Completo: {emp.Nombre} {emp.Apellidos}");
            Console.WriteLine($"Email: {emp.Email}");
            Console.WriteLine($"Ciudad: {emp.CiudadId}");
            Console.WriteLine($"Tipo de Documento: {tipoDoc}");
            Console.WriteLine($"Fecha de Contratación: {emp.FechaContratacion?.ToShortDateString() ?? "No especificada"}");
        }
    }
}
