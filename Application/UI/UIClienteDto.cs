using System;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;
using SGIC_APP.Domain.Entities;
using System.Collections.Generic;

namespace SGIC_APP.Application.UI
{
    public class UIClienteDto
    {
        private readonly IDtoCliente<ClienteDto> _clienteRepository;
        private static readonly Dictionary<int, string> TiposDocumento = new Dictionary<int, string>
        {
            { 1, "Cédula de ciudadanía" },
            { 2, "NIT" },
            { 3, "Pasaporte" }
        };

        public UIClienteDto(IDtoCliente<ClienteDto> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE CLIENTES ===");
                Console.WriteLine("1. Ver todos los clientes");
                Console.WriteLine("2. Crear nuevo cliente");
                Console.WriteLine("3. Actualizar cliente");
                Console.WriteLine("4. Eliminar cliente");
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
                            MostrarTodosLosClientes();
                            break;
                        case 2:
                            CrearCliente();
                            break;
                        case 3:
                            ActualizarCliente();
                            break;
                        case 4:
                            EliminarCliente();
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

        private void MostrarTodosLosClientes()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE CLIENTES ===\n");
            
            var clientes = _clienteRepository.ObtenerTodos();
            if (!clientes.Any())
            {
                Console.WriteLine("No hay clientes registrados.");
            }
            else
            {
                foreach (var cliente in clientes)
                {
                    MostrarCliente(cliente);
                    Console.WriteLine(new string('-', 80));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearCliente()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO CLIENTE ===\n");

            Console.Write("TERCERO_ID: ");
            var terceroId = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(terceroId))
                throw new Exception("El TERCERO_ID es requerido.");
            if (_clienteRepository.ObtenerPorId(terceroId) != null)
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

            var cliente = new ClienteDto
            {
                TerceroId = terceroId,
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                TipoDocId = tipoDocId,
                TipoTerceroId = 1 // Tipo Cliente
            };

            Console.Write("Ciudad (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
                throw new Exception("La ciudad debe ser un número entero.");
            cliente.CiudadId = ciudadId;

            Console.Write("Fecha de Nacimiento (yyyy-MM-dd): ");
            var fechaNacStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fechaNacStr))
            {
                if (!DateTime.TryParse(fechaNacStr, out DateTime fechaNac))
                    throw new Exception("El formato de fecha debe ser yyyy-MM-dd.");
                cliente.FechaNacimiento = fechaNac;
            }

            Console.Write("Fecha de Última Compra (yyyy-MM-dd): ");
            var fechaUltimaCompraStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fechaUltimaCompraStr))
            {
                if (!DateTime.TryParse(fechaUltimaCompraStr, out DateTime fechaUltimaCompra))
                    throw new Exception("El formato de fecha debe ser yyyy-MM-dd.");
                cliente.FechaUltimaCompra = fechaUltimaCompra;
            }

            _clienteRepository.Crear(cliente);
            Console.WriteLine("\nCliente creado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR CLIENTE ===\n");

            var clientes = _clienteRepository.ObtenerTodos();
            if (!clientes.Any())
            {
                Console.WriteLine("No hay clientes disponibles.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Listado de clientes disponibles:");
            foreach(var cli in clientes)
            {
                Console.WriteLine($"Tercero_ID: {cli.TerceroId}  Nombre: {cli.NombreCompleto}");
            }
            Console.WriteLine(new string('-', 50));

            Console.Write("Ingrese el TERCERO_ID del cliente a actualizar: ");
            var id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nEl TERCERO_ID no puede estar vacío. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var cliente = _clienteRepository.ObtenerPorId(id);
            if (cliente == null)
            {
                Console.WriteLine("\nNo se encontró el cliente.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nDeje en blanco los campos que no desee modificar.\n");

            Console.Write($"Nombre ({cliente.Nombre}): ");
            var nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre))
                cliente.Nombre = nombre;

            Console.Write($"Apellidos ({cliente.Apellidos}): ");
            var apellidos = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(apellidos))
                cliente.Apellidos = apellidos;

            Console.Write($"Email ({cliente.Email}): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                cliente.Email = email;

            Console.Write($"Ciudad ({cliente.CiudadId}): ");
            var ciudadIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ciudadIdStr))
            {
                if (!int.TryParse(ciudadIdStr, out int ciudadId))
                    throw new Exception("La ciudad debe ser un número entero.");
                cliente.CiudadId = ciudadId;
            }

            Console.WriteLine("Tipos de documento disponibles:");
            foreach (var kvp in TiposDocumento)
            {
                Console.WriteLine($"{kvp.Key} - {kvp.Value}");
            }
            Console.Write($"Tipo de Documento (ID) actual: {cliente.TipoDocId}: ");
            var tipoDocIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoDocIdStr))
            {
                if (!int.TryParse(tipoDocIdStr, out int tipoDocId))
                    throw new Exception("El tipo de documento debe ser un número entero.");
                cliente.TipoDocId = tipoDocId;
            }

            Console.Write($"Fecha de Nacimiento ({cliente.FechaNacimiento?.ToShortDateString() ?? "No especificada"}): ");
            var fechaNacStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fechaNacStr))
            {
                if (!DateTime.TryParse(fechaNacStr, out DateTime fechaNac))
                    throw new Exception("El formato de fecha debe ser yyyy-MM-dd.");
                cliente.FechaNacimiento = fechaNac;
            }

            _clienteRepository.Actualizar(cliente);
            Console.WriteLine("\nCliente actualizado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR CLIENTE ===\n");

            var clientes = _clienteRepository.ObtenerTodos();
            if (!clientes.Any())
            {
                Console.WriteLine("No hay clientes disponibles.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Listado de clientes disponibles:");
            foreach(var cli in clientes)
            {
                Console.WriteLine($"Tercero_ID: {cli.TerceroId}  Nombre: {cli.NombreCompleto}");
            }
            Console.WriteLine(new string('-', 50));

            Console.Write("Ingrese el TERCERO_ID del cliente a eliminar: ");
            var id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nEl TERCERO_ID no puede estar vacío. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var cliente = _clienteRepository.ObtenerPorId(id);
            if (cliente == null)
            {
                Console.WriteLine("\nNo se encontró el cliente.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n¿Está seguro que desea eliminar el siguiente cliente?");
            MostrarCliente(cliente);
            Console.Write("\nIngrese 'SI' para confirmar: ");
            
            if (Console.ReadLine()?.ToUpper() != "SI")
            {
                Console.WriteLine("\nOperación cancelada.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            _clienteRepository.Eliminar(id);
            Console.WriteLine("\nCliente eliminado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void MostrarCliente(ClienteDto cliente)
        {
            string tipoDocDesc = TiposDocumento.ContainsKey(cliente.TipoDocId)
                ? TiposDocumento[cliente.TipoDocId]
                : "Desconocido";
            Console.WriteLine($"TERCERO_ID: {cliente.TerceroId}");
            Console.WriteLine($"Nombre: {cliente.NombreCompleto}");
            Console.WriteLine($"Email: {cliente.Email}");
            Console.WriteLine($"Ciudad: {cliente.CiudadId}");
            Console.WriteLine($"Tipo de Documento: {tipoDocDesc}");
            Console.WriteLine($"Fecha de Nacimiento: {cliente.FechaNacimiento?.ToShortDateString() ?? "No especificada"}");
            Console.WriteLine($"Última Compra: {cliente.FechaUltimaCompra?.ToShortDateString() ?? "No registrada"}");
        }
    }
}
