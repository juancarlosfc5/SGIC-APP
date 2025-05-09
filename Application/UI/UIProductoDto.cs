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
                Console.WriteLine("=== GESTIÓN DE PRODUCTOS ===");
                Console.WriteLine("1. Listar Productos");
                Console.WriteLine("2. Buscar Producto");
                Console.WriteLine("3. Crear Producto");
                Console.WriteLine("4. Actualizar Producto");
                Console.WriteLine("5. Eliminar Producto");
                Console.WriteLine("6. Ver Productos por Categoría");
                Console.WriteLine("7. Ver Productos por Proveedor");
                Console.WriteLine("8. Ver Productos con Bajo Stock");
                Console.WriteLine("0. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            ListarProductos();
                            break;
                        case "2":
                            BuscarProducto();
                            break;
                        case "3":
                            CrearProducto();
                            break;
                        case "4":
                            ActualizarProducto();
                            break;
                        case "5":
                            EliminarProducto();
                            break;
                        case "6":
                            VerProductosPorCategoria();
                            break;
                        case "7":
                            VerProductosPorProveedor();
                            break;
                        case "8":
                            VerProductosBajoStock();
                            break;
                        case "0":
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
                    Console.WriteLine($"Precio de Compra: ${producto.PrecioCompra:F2}");
                    Console.WriteLine($"Precio de Venta: ${producto.PrecioVenta:F2}");
                    Console.WriteLine($"Código de Barras: {producto.Barcode}");
                    Console.WriteLine($"Categoría ID: {producto.CategoriaId}");
                    Console.WriteLine($"Proveedor ID: {producto.ProveedorId}");
                    Console.WriteLine($"Descripción: {producto.Descripcion ?? "Sin descripción"}");
                    Console.WriteLine($"Activo: {(producto.Activo ? "Sí" : "No")}");
                    Console.WriteLine($"Fecha de Creación: {producto.CreatedAt}");
                    Console.WriteLine($"Última Actualización: {producto.UpdatedAt}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void BuscarProducto()
        {
            Console.Clear();
            Console.WriteLine("=== BUSCAR PRODUCTO ===\n");

            Console.Write("Ingrese el ID del producto: ");
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
            }
            else
            {
                Console.WriteLine("\nInformación del Producto:");
                Console.WriteLine($"ID: {producto.Id}");
                Console.WriteLine($"Nombre: {producto.Nombre}");
                Console.WriteLine($"Stock: {producto.Stock}");
                Console.WriteLine($"Stock Mínimo: {producto.StockMin}");
                Console.WriteLine($"Stock Máximo: {producto.StockMax}");
                Console.WriteLine($"Precio de Compra: ${producto.PrecioCompra:F2}");
                Console.WriteLine($"Precio de Venta: ${producto.PrecioVenta:F2}");
                Console.WriteLine($"Código de Barras: {producto.Barcode}");
                Console.WriteLine($"Categoría ID: {producto.CategoriaId}");
                Console.WriteLine($"Proveedor ID: {producto.ProveedorId}");
                Console.WriteLine($"Descripción: {producto.Descripcion ?? "Sin descripción"}");
                Console.WriteLine($"Activo: {(producto.Activo ? "Sí" : "No")}");
                Console.WriteLine($"Fecha de Creación: {producto.CreatedAt}");
                Console.WriteLine($"Última Actualización: {producto.UpdatedAt}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void CrearProducto()
        {
            Console.Clear();
            Console.WriteLine("=== CREAR NUEVO PRODUCTO ===\n");

            var producto = new Producto();

            Console.Write("Nombre: ");
            producto.Nombre = Console.ReadLine() ?? throw new ArgumentException("El nombre es requerido.");

            Console.Write("Stock: ");
            if (!int.TryParse(Console.ReadLine(), out int stock))
                throw new ArgumentException("El stock debe ser un número válido.");
            producto.Stock = stock;

            Console.Write("Stock Mínimo: ");
            if (!int.TryParse(Console.ReadLine(), out int stockMin))
                throw new ArgumentException("El stock mínimo debe ser un número válido.");
            producto.StockMin = stockMin;

            Console.Write("Stock Máximo: ");
            if (!int.TryParse(Console.ReadLine(), out int stockMax))
                throw new ArgumentException("El stock máximo debe ser un número válido.");
            producto.StockMax = stockMax;

            Console.Write("Precio de Compra: ");
            if (!double.TryParse(Console.ReadLine(), out double precioCompra))
                throw new ArgumentException("El precio de compra debe ser un número válido.");
            producto.PrecioCompra = precioCompra;

            Console.Write("Precio de Venta: ");
            if (!double.TryParse(Console.ReadLine(), out double precioVenta))
                throw new ArgumentException("El precio de venta debe ser un número válido.");
            producto.PrecioVenta = precioVenta;

            Console.Write("Código de Barras: ");
            producto.Barcode = Console.ReadLine() ?? throw new ArgumentException("El código de barras es requerido.");

            Console.Write("ID de Categoría: ");
            if (!int.TryParse(Console.ReadLine(), out int categoriaId))
                throw new ArgumentException("El ID de categoría debe ser un número válido.");
            producto.CategoriaId = categoriaId;

            Console.Write("ID de Proveedor: ");
            if (!int.TryParse(Console.ReadLine(), out int proveedorId))
                throw new ArgumentException("El ID de proveedor debe ser un número válido.");
            producto.ProveedorId = proveedorId;

            Console.Write("Descripción (opcional): ");
            producto.Descripcion = Console.ReadLine();

            Console.Write("¿Activo? (S/N): ");
            producto.Activo = Console.ReadLine()?.ToUpper() == "S";

            _productoRepository.Crear(producto);
            Console.WriteLine("\nProducto creado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void ActualizarProducto()
        {
            Console.Clear();
            Console.WriteLine("=== ACTUALIZAR PRODUCTO ===\n");

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

            Console.WriteLine("\nDeje en blanco los campos que no desee modificar.\n");

            Console.Write($"Nombre ({producto.Nombre}): ");
            var nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre))
                producto.Nombre = nombre;

            Console.Write($"Stock ({producto.Stock}): ");
            var stockStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(stockStr) && int.TryParse(stockStr, out int stock))
                producto.Stock = stock;

            Console.Write($"Stock Mínimo ({producto.StockMin}): ");
            var stockMinStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(stockMinStr) && int.TryParse(stockMinStr, out int stockMin))
                producto.StockMin = stockMin;

            Console.Write($"Stock Máximo ({producto.StockMax}): ");
            var stockMaxStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(stockMaxStr) && int.TryParse(stockMaxStr, out int stockMax))
                producto.StockMax = stockMax;

            Console.Write($"Precio de Compra ({producto.PrecioCompra}): ");
            var precioCompraStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(precioCompraStr) && double.TryParse(precioCompraStr, out double precioCompra))
                producto.PrecioCompra = precioCompra;

            Console.Write($"Precio de Venta ({producto.PrecioVenta}): ");
            var precioVentaStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(precioVentaStr) && double.TryParse(precioVentaStr, out double precioVenta))
                producto.PrecioVenta = precioVenta;

            Console.Write($"Código de Barras ({producto.Barcode}): ");
            var barcode = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(barcode))
                producto.Barcode = barcode;

            Console.Write($"ID de Categoría ({producto.CategoriaId}): ");
            var categoriaIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(categoriaIdStr) && int.TryParse(categoriaIdStr, out int categoriaId))
                producto.CategoriaId = categoriaId;

            Console.Write($"ID de Proveedor ({producto.ProveedorId}): ");
            var proveedorIdStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(proveedorIdStr) && int.TryParse(proveedorIdStr, out int proveedorId))
                producto.ProveedorId = proveedorId;

            Console.Write($"Descripción ({producto.Descripcion ?? "Sin descripción"}): ");
            var descripcion = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(descripcion))
                producto.Descripcion = descripcion;

            Console.Write($"¿Activo? (S/N) ({(producto.Activo ? "S" : "N")}): ");
            var activoStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(activoStr))
                producto.Activo = activoStr.ToUpper() == "S";

            _productoRepository.Actualizar(producto);
            Console.WriteLine("\nProducto actualizado exitosamente.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void EliminarProducto()
        {
            Console.Clear();
            Console.WriteLine("=== ELIMINAR PRODUCTO ===\n");

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

            Console.WriteLine($"\n¿Está seguro que desea eliminar el producto '{producto.Nombre}'? (S/N)");
            var confirmacion = Console.ReadLine()?.ToUpper();
            if (confirmacion == "S")
            {
                _productoRepository.Eliminar(id);
                Console.WriteLine("\nProducto eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void VerProductosPorCategoria()
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCTOS POR CATEGORÍA ===\n");

            Console.Write("Ingrese el ID de la categoría: ");
            if (!int.TryParse(Console.ReadLine(), out int categoriaId))
            {
                Console.WriteLine("\nID de categoría inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var productos = _productoRepository.ObtenerPorCategoria(categoriaId);
            if (!productos.Any())
            {
                Console.WriteLine("\nNo hay productos en esta categoría.");
            }
            else
            {
                foreach (var producto in productos)
                {
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Stock: {producto.Stock}");
                    Console.WriteLine($"Precio de Venta: ${producto.PrecioVenta:F2}");
                    Console.WriteLine($"Código de Barras: {producto.Barcode}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void VerProductosPorProveedor()
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCTOS POR PROVEEDOR ===\n");

            Console.Write("Ingrese el ID del proveedor: ");
            if (!int.TryParse(Console.ReadLine(), out int proveedorId))
            {
                Console.WriteLine("\nID de proveedor inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var productos = _productoRepository.ObtenerPorProveedor(proveedorId);
            if (!productos.Any())
            {
                Console.WriteLine("\nNo hay productos de este proveedor.");
            }
            else
            {
                foreach (var producto in productos)
                {
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Stock: {producto.Stock}");
                    Console.WriteLine($"Precio de Compra: ${producto.PrecioCompra:F2}");
                    Console.WriteLine($"Precio de Venta: ${producto.PrecioVenta:F2}");
                    Console.WriteLine($"Código de Barras: {producto.Barcode}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void VerProductosBajoStock()
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCTOS CON BAJO STOCK ===\n");

            var productos = _productoRepository.ObtenerProductosBajoStock();
            if (!productos.Any())
            {
                Console.WriteLine("No hay productos con bajo stock.");
            }
            else
            {
                foreach (var producto in productos)
                {
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Stock Actual: {producto.Stock}");
                    Console.WriteLine($"Stock Mínimo: {producto.StockMin}");
                    Console.WriteLine($"Stock Máximo: {producto.StockMax}");
                    Console.WriteLine($"Código de Barras: {producto.Barcode}");
                    Console.WriteLine(new string('-', 50));
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
} 