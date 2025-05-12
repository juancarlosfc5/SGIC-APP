using System;
using System.Collections.Generic;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Application.UI
{
    public class UIPaisDto
    {
        private readonly IDtoPais<Pais> _paisRepository;

        public UIPaisDto(IDtoPais<Pais> paisRepository)
        {
            _paisRepository = paisRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE PAÍSES ===\n");
                Console.WriteLine("1. Listar Países");
                Console.WriteLine("2. Agregar País");
                Console.WriteLine("3. Actualizar País");
                Console.WriteLine("4. Eliminar País");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        ListarPaises();
                        break;
                    case "2":
                        CrearPais();
                        break;
                    case "3":
                        ActualizarPais();
                        break;
                    case "4":
                        EliminarPais();
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
        }

        private void ListarPaises()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE PAÍSES ===\n");

            var paises = _paisRepository.ObtenerTodos();
            if (!paises.Any())
            {
                Console.WriteLine("No hay países registrados.");
            }
            else
            {
                foreach (var pais in paises)
                {
                    Console.WriteLine($"ID: {pais.Id}");
                    Console.WriteLine($"Nombre: {pais.Nombre}");
                    Console.WriteLine(new string('-', 30));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearPais()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR PAÍS ===\n");

            try
            {
                var pais = new Pais();

                Console.Write("Nombre del país: ");
                pais.Nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido.");

                _paisRepository.Crear(pais);
                Console.WriteLine("\nPaís creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarPais()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PAÍS ===\n");

            // Listar los países existentes para que el usuario pueda ver sus ID y nombres
            var paisesExistentes = _paisRepository.ObtenerTodos().ToList();
            if (!paisesExistentes.Any())
            {
                Console.WriteLine("No hay países registrados.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Países existentes:");
            foreach (var p in paisesExistentes)
            {
                Console.WriteLine($"ID: {p.Id} - Nombre: {p.Nombre}");
            }
            Console.WriteLine(new string('-', 50));
            Console.Write("Ingrese el ID del país a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var pais = _paisRepository.ObtenerPorId(id);
            if (pais == null)
            {
                Console.WriteLine("\nPaís no encontrado.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine("\nDeje en blanco el campo si no desea modificarlo.\n");

                Console.Write($"Nombre actual ({pais.Nombre}): ");
                var nombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nombre))
                    pais.Nombre = nombre;

                _paisRepository.Actualizar(pais);
                Console.WriteLine("\nPaís actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarPais()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PAÍS ===\n");
            // Listar los países existentes para que el usuario pueda ver sus ID y nombres
            var paisesExistentes = _paisRepository.ObtenerTodos().ToList();
            if (!paisesExistentes.Any())
            {
                Console.WriteLine("No hay países registrados.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Países existentes:");
            foreach (var p in paisesExistentes)
            {
                Console.WriteLine($"ID: {p.Id} - Nombre: {p.Nombre}");
            }
            Console.WriteLine(new string('-', 50));
            Console.Write("Ingrese el ID del país a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                _paisRepository.Eliminar(id);
                Console.WriteLine("\nPaís eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("regiones asociadas"))
                {
                    Console.WriteLine("\nAdvertencia: No se puede eliminar el país porque tiene regiones asociadas.\nElimine o reasigne las regiones primero.");
                }
                else
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 