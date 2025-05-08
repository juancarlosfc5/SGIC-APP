using System;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.dto;
using SGIC_APP.Application.Services;

namespace SGIC_APP.Application.UI
{
    public class UIClienteDto
    {
        private readonly ClienteDtoService _clienteService;

        public UIClienteDto(IDtoCliente<ClienteDto> clienteRepository)
        {
            _clienteService = new ClienteDtoService(clienteRepository);
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GESTIÓN DE CLIENTES ===");
                Console.WriteLine("1. Mostrar todos los clientes");
                Console.WriteLine("2. Buscar cliente por ID");
                Console.WriteLine("3. Crear nuevo cliente");
                Console.WriteLine("4. Actualizar cliente");
                Console.WriteLine("5. Eliminar cliente");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarClientes();
                        break;
                    case "2":
                        BuscarCliente();
                        break;
                    case "3":
                        CrearCliente();
                        break;
                    case "4":
                        ActualizarCliente();
                        break;
                    case "5":
                        EliminarCliente();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void MostrarClientes()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE CLIENTES ===\n");
            _clienteService.MostrarClientes();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void BuscarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR CLIENTE ===\n");
            Console.Write("Ingrese el ID del cliente: ");
            var id = Console.ReadLine();

            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nEl ID no puede estar vacío. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var cliente = _clienteService.ObtenerClientePorId(id);
            if (cliente != null)
            {
                Console.WriteLine("\nCliente encontrado:");
                Console.WriteLine($"ID: {cliente.TerceroId}");
                Console.WriteLine($"Nombre: {cliente.NombreCompleto}");
                Console.WriteLine($"Email: {cliente.Email}");
                Console.WriteLine($"Teléfono: {cliente.Telefono}");
                Console.WriteLine($"Tipo de Teléfono: {cliente.TipoTelefono}");
                Console.WriteLine($"Tipo de Documento: {cliente.TipoDocId}");
                Console.WriteLine($"Ciudad: {cliente.CiudadId}");
                Console.WriteLine($"Fecha de Nacimiento: {cliente.FechaNacimiento?.ToShortDateString() ?? "No especificada"}");
                Console.WriteLine($"Última Compra: {cliente.FechaUltimaCompra?.ToShortDateString() ?? "No registrada"}");
            }
            else
            {
                Console.WriteLine("\nNo se encontró ningún cliente con ese ID.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearCliente()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO CLIENTE ===\n");

            var cliente = CrearClienteDtoDesdeConsola();
            if (cliente != null)
            {
                try
                {
                    _clienteService.CrearCliente(cliente);
                    Console.WriteLine("\nCliente creado exitosamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError al crear el cliente: {ex.Message}");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR CLIENTE ===\n");
            Console.Write("Ingrese el ID del cliente a actualizar: ");
            var id = Console.ReadLine();

            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nEl ID no puede estar vacío. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var clienteExistente = _clienteService.ObtenerClientePorId(id);
            if (clienteExistente == null)
            {
                Console.WriteLine("\nNo se encontró ningún cliente con ese ID.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var clienteActualizado = CrearClienteDtoDesdeConsola();
            if (clienteActualizado != null)
            {
                clienteActualizado.TerceroId = id;
                try
                {
                    _clienteService.ActualizarCliente(clienteActualizado);
                    Console.WriteLine("\nCliente actualizado exitosamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError al actualizar el cliente: {ex.Message}");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR CLIENTE ===\n");
            Console.Write("Ingrese el ID del cliente a eliminar: ");
            var id = Console.ReadLine();

            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nEl ID no puede estar vacío. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("\n¿Está seguro de que desea eliminar este cliente? (S/N): ");
            var confirmacion = Console.ReadLine()?.ToUpper();

            if (confirmacion == "S")
            {
                try
                {
                    _clienteService.EliminarCliente(id);
                    Console.WriteLine("\nCliente eliminado exitosamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError al eliminar el cliente: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private ClienteDto? CrearClienteDtoDesdeConsola()
        {
            try
            {
                var cliente = new ClienteDto();

                Console.Write("Nombre: ");
                cliente.Nombre = Console.ReadLine();

                Console.Write("Apellidos: ");
                cliente.Apellidos = Console.ReadLine();

                Console.Write("Email: ");
                cliente.Email = Console.ReadLine();

                Console.Write("Teléfono: ");
                cliente.Telefono = Console.ReadLine();

                Console.Write("Tipo de Teléfono (Celular/Fijo): ");
                cliente.TipoTelefono = Console.ReadLine();

                Console.Write("Tipo de Documento (ID): ");
                if (int.TryParse(Console.ReadLine(), out int tipoDocId))
                {
                    cliente.TipoDocId = tipoDocId;
                }
                else
                {
                    Console.WriteLine("Tipo de documento inválido.");
                    return null;
                }

                Console.Write("Ciudad (ID): ");
                if (int.TryParse(Console.ReadLine(), out int ciudadId))
                {
                    cliente.CiudadId = ciudadId;
                }
                else
                {
                    Console.WriteLine("Ciudad inválida.");
                    return null;
                }

                Console.Write("Fecha de Nacimiento (dd/MM/yyyy): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fechaNac))
                {
                    cliente.FechaNacimiento = fechaNac;
                }

                cliente.TipoTerceroId = 1; // Tipo Cliente

                return cliente;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al crear el cliente: {ex.Message}");
                return null;
            }
        }
    }
}
