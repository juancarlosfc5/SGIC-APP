using System;
using System.Collections.Generic;
using System.Linq;
using SGIC_APP.Domain.Ports;
using SGIC_APP.Domain.Entities;

namespace SGIC_APP.Application.UI
{
    public class UIProductoDto
    {
        private readonly IDtoProducto<Producto> _productoRepository;

        public UIProductoDto(IDtoProducto<Producto> productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE PRODUCTOS ===\n");
                Console.WriteLine("1. Listar Productos");
                Console.WriteLine("2. Agregar Producto");
                Console.WriteLine("3. Actualizar Producto");
                Console.WriteLine("4. Eliminar Producto");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        ListarProductos();
                        break;
                    case "2":
                        CrearProducto();
                        break;
                    case "3":
                        ActualizarProducto();
                        break;
                    case "4":
                        EliminarProducto();
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

        private void ListarProductos()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE PRODUCTOS ===\n");

            var productos = _productoRepository.ObtenerTodos();
            if (!productos.Any())
            {
                Console.WriteLine("No hay productos registrados.");
            }
            else
            {
                foreach (var producto in productos)
                {
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Stock: {producto.Stock}");
                    Console.WriteLine($"Stock Mínimo: {producto.StockMin}");
                    Console.WriteLine($"Stock Máximo: {producto.StockMax}");
                    Console.WriteLine($"Fecha de Creación: {producto.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                    Console.WriteLine($"Última Actualización: {producto.UpdatedAt:yyyy-MM-dd HH:mm:ss}");
                    Console.WriteLine($"Código de Barras: {producto.Barcode}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearProducto()
        {
            Console.Clear();
            Console.WriteLine("=== AGREGAR PRODUCTO ===\n");

            try
            {
                var producto = new Producto();

                Console.Write("Nombre del producto: ");
                producto.Nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido.");

                Console.Write("Stock inicial: ");
                if (!int.TryParse(Console.ReadLine(), out int stock))
                    throw new ArgumentException("El stock debe ser un número válido.");
                producto.Stock = stock;

                Console.Write("Stock mínimo: ");
                if (!int.TryParse(Console.ReadLine(), out int stockMin))
                    throw new ArgumentException("El stock mínimo debe ser un número válido.");
                producto.StockMin = stockMin;

                Console.Write("Stock máximo: ");
                if (!int.TryParse(Console.ReadLine(), out int stockMax))
                    throw new ArgumentException("El stock máximo debe ser un número válido.");
                producto.StockMax = stockMax;

                // Asignar fechas automáticamente
                producto.CreatedAt = DateTime.Now;
                producto.UpdatedAt = DateTime.Now;

                // Generar código de barras automáticamente
                producto.Barcode = GenerarCodigoBarras();

                _productoRepository.Crear(producto);
                Console.WriteLine("\nProducto creado exitosamente.");
                Console.WriteLine($"Código de barras generado: {producto.Barcode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarProducto()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PRODUCTO ===\n");

            var productos = _productoRepository.ObtenerTodos();
            if (!productos.Any())
            {
                Console.WriteLine("No hay productos disponibles.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Listado de productos disponibles:");
            foreach(var prod in productos)
            {
                Console.WriteLine($"ID: {prod.Id}  Nombre: {prod.Nombre}");
            }
            Console.WriteLine(new string('-', 50));

            Console.Write("Ingrese el ID del producto a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var producto = _productoRepository.ObtenerPorId(id);
            if (producto == null)
            {
                Console.WriteLine("\nProducto no encontrado.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine("\nDeje en blanco los campos que no desee modificar.\n");

                Console.Write($"Nombre actual ({producto.Nombre}): ");
                var nombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nombre))
                    producto.Nombre = nombre;

                Console.Write($"Stock actual ({producto.Stock}): ");
                var stockStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(stockStr) && int.TryParse(stockStr, out int stock))
                    producto.Stock = stock;

                Console.Write($"Stock mínimo actual ({producto.StockMin}): ");
                var stockMinStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(stockMinStr) && int.TryParse(stockMinStr, out int stockMin))
                    producto.StockMin = stockMin;

                Console.Write($"Stock máximo actual ({producto.StockMax}): ");
                var stockMaxStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(stockMaxStr) && int.TryParse(stockMaxStr, out int stockMax))
                    producto.StockMax = stockMax;

                // Actualizar la fecha de actualización
                producto.UpdatedAt = DateTime.Now;

                _productoRepository.Actualizar(producto);
                Console.WriteLine("\nProducto actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarProducto()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PRODUCTO ===\n");

            var productos = _productoRepository.ObtenerTodos();
            if (!productos.Any())
            {
                Console.WriteLine("No hay productos disponibles.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Listado de productos disponibles:");
            foreach(var prod in productos)
            {
                Console.WriteLine($"ID: {prod.Id}  Nombre: {prod.Nombre}");
            }
            Console.WriteLine(new string('-', 50));

            Console.Write("Ingrese el ID del producto a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var producto = _productoRepository.ObtenerPorId(id);
            if (producto == null)
            {
                Console.WriteLine("\nProducto no encontrado.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine($"\n¿Está seguro que desea eliminar el producto '{producto.Nombre}'?");
                Console.Write("Escriba 'SI' para confirmar o cualquier otra tecla para cancelar: ");
                if (Console.ReadLine()?.ToUpper() == "SI")
                {
                    _productoRepository.Eliminar(id);
                    Console.WriteLine("\nProducto eliminado exitosamente.");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("detalle_compra"))
                {
                    Console.WriteLine("\nNo es posible eliminar este producto porque está relacionado con compras existentes.");
                    Console.WriteLine("Para eliminar el producto, primero debe eliminar todas las compras asociadas.");
                }
                else
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private string GenerarCodigoBarras()
        {
            // Generar un código de barras aleatorio de 13 dígitos
            var random = new Random();
            var codigo = "";
            for (int i = 0; i < 12; i++)
            {
                codigo += random.Next(0, 10).ToString();
            }

            // Calcular dígito verificador (algoritmo EAN-13)
            int suma = 0;
            for (int i = 0; i < 12; i++)
            {
                int digito = int.Parse(codigo[i].ToString());
                suma += (i % 2 == 0) ? digito : digito * 3;
            }
            int digitoVerificador = (10 - (suma % 10)) % 10;
            codigo += digitoVerificador.ToString();

            return codigo;
        }
    }
} 