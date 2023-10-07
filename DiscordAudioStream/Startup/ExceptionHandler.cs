using System;
using System.Windows.Forms;

namespace DiscordAudioStream
{
    internal static class ExceptionHandler
    {
        private const string TRACE_FILE_NAME = "DiscordAudioStream_stack_trace.txt";

        public static void Register()
        {
            Application.ThreadException += ApplicationThreadException;

            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }


        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void HandleException(Exception exception)
        {
            Logger.Log("Unhandled exception caught, outputting stack trace...");

            string tracePath = System.IO.Path.GetFullPath(TRACE_FILE_NAME);

            WriteStackTrace(exception, tracePath);

            string msg = $"Unhandled exception: {exception.Message}\nClick OK to open the generated trace file:\n{tracePath}";
            MessageBox.Show(msg, "DiscordAudioStream Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            OpenFile(tracePath);
            Application.Exit();
        }

        private static void WriteStackTrace(Exception exception, string tracePath)
        {
            Logger.Log("  Trace path is: " + tracePath);

            string[] lines = new string[] { exception.ToString() };
            System.IO.File.WriteAllLines(tracePath, lines);

            Logger.Log("  Stack trace written successfully.");
        }

        private static void OpenFile(string path)
        {
            System.Diagnostics.Process.Start(path);
        }
    }
}
