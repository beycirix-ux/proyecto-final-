using VeterinariaApp.Datos;
using VeterinariaApp.Formularios;

namespace VeterinariaApp;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Conexion.Inicializar();          // crea la BD y las tablas si no existen
        Application.Run(new FrmPrincipal());
    }
}
