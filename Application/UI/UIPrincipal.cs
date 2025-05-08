using SGICAPP.Application.UI.Clientes;
using SGICAPP.Domain.Factory;
namespace SGICAPP.Application.UI.Principal;

public class UIPrincipal{
    private readonly UICliente UICliente;
    public UIPrincipal(IDbFactory factory)
    {
        UICliente = new UICliente(factory);
    }
    public void MostrarMenu(){
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- MENÚ PRINCIPAL ---");
            Console.WriteLine("1. Clientes");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
            var opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    UICliente.MenuCliente();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("❌ Opción inválida.");
                    Console.WriteLine("Presione cualquier tecla para volver al menú...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }
}