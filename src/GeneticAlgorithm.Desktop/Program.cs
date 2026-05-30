using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticAlgorithm.Desktop
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            System.Windows.Forms.Application.ThreadException += (_, e) =>
                ShowStartupError("Unhandled error", e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
                ShowStartupError("Fatal error", e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject?.ToString()));

            try
            {
                System.Windows.Forms.Application.Run(new OptimizationView());
            }
            catch (Exception ex)
            {
                ShowStartupError("Could not start Truck Fleet Problem", ex);
            }
        }

        private static void ShowStartupError(string title, Exception ex)
        {
            MessageBox.Show(
                ex?.ToString() ?? "Unknown error.",
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
