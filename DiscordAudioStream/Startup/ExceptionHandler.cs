using System.Windows.Forms;

namespace DiscordAudioStream;

internal static class ExceptionHandler
{
    private const string TRACE_FILE_NAME = "DiscordAudioStream_stack_trace.txt";

    private static bool crashDuringStartup = true;

    public static void Register()
    {
        Application.ThreadException += (sender, e) => HandleException(e.Exception);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
    }

    public static void StartupDone()
    {
        crashDuringStartup = false;
    }

    private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            HandleException(ex);
        }
    }

    private static void HandleException(Exception exception)
    {
        Logger.Log("Unhandled exception caught, outputting stack trace...");

        string tracePath = Path.GetFullPath(TRACE_FILE_NAME);
        WriteStackTrace(exception, tracePath);

        MessageBox.Show(
            $"Unhandled exception: {exception.Message}\nClick OK to open the generated trace file:\n{tracePath}",
            "DiscordAudioStream Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
        OpenFile(tracePath);

        if (crashDuringStartup && !Properties.Settings.Default.OutputLogFile)
        {
            Thread.Sleep(500);
            DialogResult result = MessageBox.Show(
                "The application crashed before it could finish starting up.\n"
                    + "Do you want to enable debug logging?",
                "DiscordAudioStream Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.OutputLogFile = true;
            }
        }

        Environment.Exit(1);
    }

    private static void WriteStackTrace(Exception exception, string tracePath)
    {
        Logger.Log("  Trace path is: " + tracePath);

        File.WriteAllText(tracePath, exception.ToString());

        Logger.Log("  Stack trace written successfully.");
    }

    private static void OpenFile(string path)
    {
        System.Diagnostics.Process.Start(path);
    }
}
