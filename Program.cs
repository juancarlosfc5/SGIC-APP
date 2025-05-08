using SGICAPP.Application.Services;
using SGICAPP.Application.UI.Clientes;
using SGICAPP.Application.UI.Principal;
using SGICAPP.Domain.Entities;
using SGICAPP.Domain.Factory;
using SGICAPP.Infrastructure.Mysql;

internal class Program
{
    private static void Main(string[] args)
    {
        string connStr = "server=localhost;database=NJSIGC;user=campus2023;password=campus2023;";
        IDbFactory factory = new MySqlDbFactory(connStr);
        UIPrincipal UIPrincipal = new UIPrincipal(factory);
        UIPrincipal.MostrarMenu();
    }
}