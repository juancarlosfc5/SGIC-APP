using System;
using System.Collections.Generic;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Application.UI
{
    public class UICiudadDto
    {
        private readonly IDtoCiudad<Ciudad> _ciudadRepository;
        private readonly IDtoRegion<Region> _regionRepository;

        public UICiudadDto(IDtoCiudad<Ciudad> ciudadRepository, IDtoRegion<Region> regionRepository)
        {
            _ciudadRepository = ciudadRepository;
            _regionRepository = regionRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE CIUDADES ===\n");
                Console.WriteLine("1. Listar Ciudades");
                Console.WriteLine("2. Agregar Ciudad");
                Console.WriteLine("3. Actualizar Ciudad");
                Console.WriteLine("4. Eliminar Ciudad");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        ListarCiudades();
                        break;
                    case "2":
                        CrearCiudad();
                        break;
                    case "3":
                        ActualizarCiudad();
                        break;
                    case "4":
                        EliminarCiudad();
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

        private void ListarCiudades()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE CIUDADES ===\n");

            var ciudades = _ciudadRepository.ObtenerTodos();
            if (!ciudades.Any())
            {
                Console.WriteLine("No hay ciudades registradas.");
            }
            else
            {
                foreach (var ciudad in ciudades)
                {
                    var region = _regionRepository.ObtenerPorId(ciudad.RegionId);
                    Console.WriteLine($"ID: {ciudad.Id}");
                    Console.WriteLine($"Nombre: {ciudad.Nombre}");
                    Console.WriteLine($"Región: {(region != null ? region.Nombre : "Desconocida")}");
                    Console.WriteLine(new string('-', 30));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearCiudad()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR CIUDAD ===\n");

            try
            {
                var ciudad = new Ciudad();

                Console.Write("Nombre de la ciudad: ");
                ciudad.Nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido.");

                Console.WriteLine("Seleccione la región a la que pertenece la ciudad:");
                var regiones = _regionRepository.ObtenerTodos().ToList();
                if (!regiones.Any())
                {
                    Console.WriteLine("No hay regiones registradas. Debe crear una región primero.");
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                for (int i = 0; i < regiones.Count; i++)
                {
                    Console.WriteLine($"{regiones[i].Id}. {regiones[i].Nombre}");
                }
                Console.Write("Ingrese el ID de la región: ");
                if (!int.TryParse(Console.ReadLine(), out int regionId) || !regiones.Any(r => r.Id == regionId))
                {
                    Console.WriteLine("ID de región inválido. Operación cancelada.");
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                ciudad.RegionId = regionId;

                _ciudadRepository.Crear(ciudad);
                Console.WriteLine("\nCiudad creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarCiudad()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR CIUDAD ===\n");

            // Listar las ciudades existentes para que el usuario pueda ver los ID y nombres
            var ciudadesExistentes = _ciudadRepository.ObtenerTodos().ToList();
            if (!ciudadesExistentes.Any())
            {
                Console.WriteLine("No hay ciudades registradas.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Ciudades existentes:");
            foreach (var ciudadItem in ciudadesExistentes)
            {
                Console.WriteLine($"ID: {ciudadItem.Id} - Nombre: {ciudadItem.Nombre}");
            }
            Console.WriteLine();
            Console.Write("Ingrese el ID de la ciudad a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var ciudad = _ciudadRepository.ObtenerPorId(id);
            if (ciudad == null)
            {
                Console.WriteLine("\nCiudad no encontrada.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine("\nDeje en blanco el campo si no desea modificarlo.\n");

                Console.Write($"Nombre actual ({ciudad.Nombre}): ");
                var nombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nombre))
                    ciudad.Nombre = nombre;

                Console.WriteLine($"Región actual (ID: {ciudad.RegionId}):");
                var regiones = _regionRepository.ObtenerTodos().ToList();
                for (int i = 0; i < regiones.Count; i++)
                {
                    Console.WriteLine($"{regiones[i].Id}. {regiones[i].Nombre}");
                }
                Console.Write("Ingrese el ID de la nueva región (deje en blanco para no cambiar): ");
                var regionIdStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(regionIdStr) && int.TryParse(regionIdStr, out int regionId) && regiones.Any(r => r.Id == regionId))
                {
                    ciudad.RegionId = regionId;
                }

                _ciudadRepository.Actualizar(ciudad);
                Console.WriteLine("\nCiudad actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarCiudad()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR CIUDAD ===\n");

            Console.Write("Ingrese el ID de la ciudad a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                _ciudadRepository.Eliminar(id);
                Console.WriteLine("\nCiudad eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("datos asociados"))
                {
                    Console.WriteLine("\nAdvertencia: No se puede eliminar la ciudad porque tiene datos asociados.\nElimine o reasigne los datos primero.");
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