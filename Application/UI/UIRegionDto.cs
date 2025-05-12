using System;
using System.Collections.Generic;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Application.UI
{
    public class UIRegionDto
    {
        private readonly IDtoRegion<Region> _regionRepository;
        private readonly IDtoPais<Pais> _paisRepository;

        public UIRegionDto(IDtoRegion<Region> regionRepository, IDtoPais<Pais> paisRepository)
        {
            _regionRepository = regionRepository;
            _paisRepository = paisRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE REGIONES ===\n");
                Console.WriteLine("1. Listar Regiones");
                Console.WriteLine("2. Agregar Región");
                Console.WriteLine("3. Actualizar Región");
                Console.WriteLine("4. Eliminar Región");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        ListarRegiones();
                        break;
                    case "2":
                        CrearRegion();
                        break;
                    case "3":
                        ActualizarRegion();
                        break;
                    case "4":
                        EliminarRegion();
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

        private void ListarRegiones()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE REGIONES ===\n");

            var regiones = _regionRepository.ObtenerTodos();
            if (!regiones.Any())
            {
                Console.WriteLine("No hay regiones registradas.");
            }
            else
            {
                foreach (var region in regiones)
                {
                    var pais = _paisRepository.ObtenerPorId(region.PaisId);
                    Console.WriteLine($"ID: {region.Id}");
                    Console.WriteLine($"Nombre: {region.Nombre}");
                    Console.WriteLine($"País: {(pais != null ? pais.Nombre : "Desconocido")}");
                    Console.WriteLine(new string('-', 30));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearRegion()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR REGIÓN ===\n");

            try
            {
                var region = new Region();

                Console.Write("Nombre de la región: ");
                region.Nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido.");

                Console.WriteLine("Seleccione el país al que pertenece la región:");
                var paises = _paisRepository.ObtenerTodos().ToList();
                if (!paises.Any())
                {
                    Console.WriteLine("No hay países registrados. Debe crear un país primero.");
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                for (int i = 0; i < paises.Count; i++)
                {
                    Console.WriteLine($"{paises[i].Id}. {paises[i].Nombre}");
                }
                Console.Write("Ingrese el ID del país: ");
                if (!int.TryParse(Console.ReadLine(), out int paisId) || !paises.Any(p => p.Id == paisId))
                {
                    Console.WriteLine("ID de país inválido. Operación cancelada.");
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                region.PaisId = paisId;

                _regionRepository.Crear(region);
                Console.WriteLine("\nRegión creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarRegion()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR REGIÓN ===\n");

            // Listar las regiones existentes para que el usuario pueda ver los ID
            var regionesExistentes = _regionRepository.ObtenerTodos().ToList();
            if (!regionesExistentes.Any())
            {
                Console.WriteLine("No hay regiones registradas.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Regiones existentes:");
            foreach (var reg in regionesExistentes)
            {
                Console.WriteLine($"ID: {reg.Id} - Nombre: {reg.Nombre}");
            }
            Console.WriteLine(new string('-', 50));
            Console.Write("Ingrese el ID de la región a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var region = _regionRepository.ObtenerPorId(id);
            if (region == null)
            {
                Console.WriteLine("\nRegión no encontrada.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine("\nDeje en blanco el campo si no desea modificarlo.\n");

                Console.Write($"Nombre actual ({region.Nombre}): ");
                var nombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nombre))
                    region.Nombre = nombre;

                Console.WriteLine($"País actual (ID: {region.PaisId}):");
                var paises = _paisRepository.ObtenerTodos().ToList();
                for (int i = 0; i < paises.Count; i++)
                {
                    Console.WriteLine($"{paises[i].Id}. {paises[i].Nombre}");
                }
                Console.Write("Ingrese el ID del nuevo país (deje en blanco para no cambiar): ");
                var paisIdStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(paisIdStr) && int.TryParse(paisIdStr, out int paisId) && paises.Any(p => p.Id == paisId))
                {
                    region.PaisId = paisId;
                }

                _regionRepository.Actualizar(region);
                Console.WriteLine("\nRegión actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarRegion()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR REGIÓN ===\n");
            // Listar las regiones existentes para que el usuario pueda ver los ID
            var regionesExistentes = _regionRepository.ObtenerTodos().ToList();
            if (!regionesExistentes.Any())
            {
                Console.WriteLine("No hay regiones registradas.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Regiones existentes:");
            foreach (var reg in regionesExistentes)
            {
                Console.WriteLine($"ID: {reg.Id} - Nombre: {reg.Nombre}");
            }
            Console.WriteLine(new string('-', 50));
            Console.Write("Ingrese el ID de la región a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                _regionRepository.Eliminar(id);
                Console.WriteLine("\nRegión eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ciudades asociadas"))
                {
                    Console.WriteLine("\nAdvertencia: No se puede eliminar la región porque tiene ciudades asociadas.\nElimine o reasigne las ciudades primero.");
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