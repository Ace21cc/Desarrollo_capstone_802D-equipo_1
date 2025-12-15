using System;
using System.Windows.Forms;
using MiniMarket.Services;

namespace MiniMarket
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Inicializa la base de datos (usuario admin, etc.)
                Database.Initialize();
            }
            catch (Exception ex)
            {
                // Muestra el error pero NO cierra la app
                MessageBox.Show(
                    "Error al inicializar la base de datos:\n\n" + ex.Message,
                    "Error BD",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            // 👇 AQUÍ eliges qué formulario se abre primero

            // Opción 1: si tienes formulario de login:
            Application.Run(new LoginForm());

            // Opción 2 (para pruebas): si quieres entrar directo a ventas,
            // cambia la línea anterior por esta y COMENTA la de LoginForm:
            //
            // Application.Run(new VentasForm());
        }
    }
}
