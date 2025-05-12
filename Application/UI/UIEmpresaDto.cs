using System;
using System.Collections.Generic;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Application.UI
{
    public class UIEmpresaDto
    {
        private readonly IDtoEmpresa<Empresa> _empresaRepository;
        private readonly IDtoCiudad<Ciudad> _ciudadRepository;

        public UIEmpresaDto(IDtoEmpresa<Empresa> empresaRepository, IDtoCiudad<Ciudad> ciudadRepository)
        {
            _empresaRepository = empresaRepository;
            _ciudadRepository = ciudadRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE EMPRESAS ===\n");
                Console.WriteLine("1. Listar Empresas");
                Console.WriteLine("2. Agregar Empresa");
                Console.WriteLine("3. Actualizar Empresa");
                Console.WriteLine("4. Eliminar Empresa");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        ListarEmpresas();
                        break;
                    case "2":
                        CrearEmpresa();
                        break;
                    case "3":
                        ActualizarEmpresa();
                        break;
                    case "4":
                        EliminarEmpresa();
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

        private void ListarEmpresas()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE EMPRESAS ===\n");

            var empresas = _empresaRepository.ObtenerTodos();
            if (!empresas.Any())
            {
                Console.WriteLine("No hay empresas registradas.");
            }
            else
            {
                foreach (var empresa in empresas)
                {
                    var ciudad = _ciudadRepository.ObtenerPorId(empresa.CiudadId);
                    Console.WriteLine($"ID: {empresa.Id}");
                    Console.WriteLine($"Nombre: {empresa.Nombre}");
                    Console.WriteLine($"Ciudad: {(ciudad != null ? ciudad.Nombre : "Desconocida")}");
                    Console.WriteLine($"Fecha de Registro: {empresa.FechaReg:dd/MM/yyyy HH:mm:ss}");
                    Console.WriteLine(new string('-', 30));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearEmpresa()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR EMPRESA ===\n");

            try
            {
                var empresa = new Empresa();

                Console.Write("Nombre de la empresa: ");
                empresa.Nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido.");

                Console.WriteLine("Seleccione la ciudad a la que pertenece la empresa:");
                var ciudades = _ciudadRepository.ObtenerTodos().ToList();
                if (!ciudades.Any())
                {
                    Console.WriteLine("No hay ciudades registradas. Debe crear una ciudad primero.");
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                for (int i = 0; i < ciudades.Count; i++)
                {
                    Console.WriteLine($"{ciudades[i].Id}. {ciudades[i].Nombre}");
                }
                Console.Write("Ingrese el ID de la ciudad: ");
                if (!int.TryParse(Console.ReadLine(), out int ciudadId) || !ciudades.Any(c => c.Id == ciudadId))
                {
                    Console.WriteLine("ID de ciudad inválido. Operación cancelada.");
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                empresa.CiudadId = ciudadId;

                Console.Write("ID de la empresa: ");
                empresa.Id = Console.ReadLine() ?? throw new ArgumentException("El ID es requerido.");

                empresa.FechaReg = DateTime.Now;

                _empresaRepository.Crear(empresa);
                Console.WriteLine("\nEmpresa creada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarEmpresa()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR EMPRESA ===\n");

            // Listar las empresas existentes para que el usuario pueda ver los ID y nombres
            var empresasExistentes = _empresaRepository.ObtenerTodos().ToList();
            if (!empresasExistentes.Any())
            {
                Console.WriteLine("No hay empresas registradas.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Empresas existentes:");
            foreach (var emp in empresasExistentes)
            {
                Console.WriteLine($"ID: {emp.Id} - Nombre: {emp.Nombre}");
            }
            Console.WriteLine(new string('-', 50));
            Console.Write("Ingrese el ID de la empresa a actualizar: ");
            var id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var empresa = _empresaRepository.ObtenerPorId(id);
            if (empresa == null)
            {
                Console.WriteLine("\nEmpresa no encontrada.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine("\nDeje en blanco el campo si no desea modificarlo.\n");

                Console.Write($"Nombre actual ({empresa.Nombre}): ");
                var nombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nombre))
                    empresa.Nombre = nombre;

                Console.WriteLine($"Ciudad actual (ID: {empresa.CiudadId}):");
                var ciudades = _ciudadRepository.ObtenerTodos().ToList();
                for (int i = 0; i < ciudades.Count; i++)
                {
                    Console.WriteLine($"{ciudades[i].Id}. {ciudades[i].Nombre}");
                }
                Console.Write("Ingrese el ID de la nueva ciudad (deje en blanco para no cambiar): ");
                var ciudadIdStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ciudadIdStr) && int.TryParse(ciudadIdStr, out int ciudadId) && ciudades.Any(c => c.Id == ciudadId))
                {
                    empresa.CiudadId = ciudadId;
                }

                Console.Write($"Fecha de registro actual ({empresa.FechaReg:dd/MM/yyyy HH:mm:ss}): ");
                var fechaRegStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(fechaRegStr) && DateTime.TryParse(fechaRegStr, out DateTime fechaReg))
                {
                    empresa.FechaReg = fechaReg;
                }

                _empresaRepository.Actualizar(empresa);
                Console.WriteLine("\nEmpresa actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarEmpresa()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR EMPRESA ===\n");
            // Listar las empresas existentes para que el usuario pueda ver los ID y nombres
            var empresasExistentes = _empresaRepository.ObtenerTodos().ToList();
            if (!empresasExistentes.Any())
            {
                Console.WriteLine("No hay empresas registradas.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Empresas existentes:");
            foreach (var emp in empresasExistentes)
            {
                Console.WriteLine($"ID: {emp.Id} - Nombre: {emp.Nombre}");
            }
            Console.WriteLine(new string('-', 50));
            Console.Write("Ingrese el ID de la empresa a eliminar: ");
            var id = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                _empresaRepository.Eliminar(id);
                Console.WriteLine("\nEmpresa eliminada exitosamente.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("datos asociados"))
                {
                    Console.WriteLine("\nAdvertencia: No se puede eliminar la empresa porque tiene datos asociados.\nElimine o reasigne los datos primero.");
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