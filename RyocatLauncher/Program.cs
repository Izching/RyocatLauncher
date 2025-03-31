using RyocatLauncher;

namespace RyocatLauncher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;
            ApplicationConfiguration.Initialize();
            var form = new AccountForm();
            form.Show();
            Application.Run();
        }
    }
}