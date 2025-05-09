using System;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;

namespace SGIC_APP.Application.UI
{
    public class UIClienteDto
    {
        private readonly IDtoCliente<ClienteDto> _clienteRepository;

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
                Console.WriteLine("2. Buscar cliente por ID");
                Console.WriteLine("3. Crear nuevo cliente");
                Console.WriteLine("4. Actualizar cliente");
                Console.WriteLine("5. Eliminar cliente");
                Console.WriteLine("6. Volver al menú principal");
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
                            BuscarClientePorId();
                            break;
                        case 3:
                            CrearCliente();
                            break;
                        case 4:
                            ActualizarCliente();
                            break;
                        case 5:
                            EliminarCliente();
                            break;
                        case 6:
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

        private void BuscarClientePorId()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR CLIENTE ===\n");
            
            Console.Write("Ingrese el ID del cliente: ");
            var id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nEl ID no puede estar vacío. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var cliente = _clienteRepository.ObtenerPorId(id);
            if (cliente == null)
            {
                Console.WriteLine("\nNo se encontró el cliente.");
            }
            else
            {
                Console.WriteLine("\n=== DETALLES DEL CLIENTE ===");
                MostrarCliente(cliente);
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearCliente()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO CLIENTE ===\n");

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

            var cliente = new ClienteDto
            {
                TerceroId = DateTime.Now.ToString("yyyyMMddHHmmss"),
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                TipoTerceroId = 1, // Tipo Cliente
                Activo = true
            };

            Console.Write("Teléfono: ");
            cliente.Telefono = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                throw new Exception("El teléfono es requerido.");

            Console.Write("Tipo de Teléfono (Celular/Fijo): ");
            cliente.TipoTelefono = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cliente.TipoTelefono))
                throw new Exception("El tipo de teléfono es requerido.");

            Console.Write("Tipo de Documento (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int tipoDocId))
                throw new Exception("El tipo de documento debe ser un número entero.");
            cliente.TipoDocId = tipoDocId;

            Console.Write("Ciudad (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int ciudadId))
                throw new Exception("La ciudad debe ser un número entero.");
            cliente.CiudadId = ciudadId;

            Console.Write("Fecha de Nacimiento (dd/MM/yyyy): ");
            var fechaNacStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fechaNacStr))
            {
                if (!DateTime.TryParse(fechaNacStr, out DateTime fechaNac))
                    throw new Exception("El formato de fecha debe ser dd/MM/yyyy.");
                cliente.FechaNacimiento = fechaNac;
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

            Console.Write("Ingrese el ID del cliente a actualizar: ");
            var id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nEl ID no puede estar vacío. Presione cualquier tecla para continuar...");
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

            Console.Write($"Teléfono ({cliente.Telefono}): ");
            var telefono = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(telefono))
                cliente.Telefono = telefono;

            Console.Write($"Tipo de Teléfono ({cliente.TipoTelefono}): ");
            var tipoTelefono = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoTelefono))
                cliente.TipoTelefono = tipoTelefono;

            Console.Write($"Tipo de Documento ({cliente.TipoDocId}): ");
            var tipoDocIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoDocIdStr))
            {
                if (!int.TryParse(tipoDocIdStr, out int tipoDocId))
                    throw new Exception("El tipo de documento debe ser un número entero.");
                cliente.TipoDocId = tipoDocId;
            }

            Console.Write($"Ciudad ({cliente.CiudadId}): ");
            var ciudadIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ciudadIdStr))
            {
                if (!int.TryParse(ciudadIdStr, out int ciudadId))
                    throw new Exception("La ciudad debe ser un número entero.");
                cliente.CiudadId = ciudadId;
            }

            Console.Write($"Fecha de Nacimiento ({cliente.FechaNacimiento?.ToShortDateString() ?? "No especificada"}): ");
            var fechaNacStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fechaNacStr))
            {
                if (!DateTime.TryParse(fechaNacStr, out DateTime fechaNac))
                    throw new Exception("El formato de fecha debe ser dd/MM/yyyy.");
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

            Console.Write("Ingrese el ID del cliente a eliminar: ");
            var id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nEl ID no puede estar vacío. Presione cualquier tecla para continuar...");
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
            Console.WriteLine($"ID: {cliente.TerceroId}");
            Console.WriteLine($"Nombre: {cliente.Nombre} {cliente.Apellidos}");
            Console.WriteLine($"Email: {cliente.Email}");
            Console.WriteLine($"Tipo de Documento ID: {cliente.TipoDocId}");
            Console.WriteLine($"Tipo de Tercero ID: {cliente.TipoTerceroId}");
            Console.WriteLine($"Ciudad ID: {cliente.CiudadId}");
            Console.WriteLine($"Fecha de Nacimiento: {cliente.FechaNacimiento?.ToShortDateString() ?? "No especificada"}");
            Console.WriteLine($"Última Compra: {cliente.FechaUltimaCompra?.ToShortDateString() ?? "No registrada"}");
            Console.WriteLine($"Estado: {(cliente.Activo ? "Activo" : "Inactivo")}");
            Console.WriteLine($"Fecha de Registro: {cliente.FechaRegistro.ToShortDateString()}");
        }
    }
}
