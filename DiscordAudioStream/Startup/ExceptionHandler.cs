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
        try
        {
            HandleExceptionUnsafe(exception);
        }
        catch (Exception e)
        {
            Logger.Log("ERROR WHILE HANDLING EXCEPTION:");
            Logger.Log(e);
            Logger.Log("ORIGINAL EXCEPTION BEING HANDLED:");
            Logger.Log(exception);
        }
        Environment.Exit(1);
    }

    private static void HandleExceptionUnsafe(Exception exception)
    {
        Logger.Log("Unhandled exception caught, outputting stack trace...");

        string tracePath = Path.GetFullPath(TRACE_FILE_NAME);
        WriteStackTrace(exception, tracePath);

        ShowMessage.Error()
            .Title("DiscordAudioStream Error")
            .Text($"Unhandled exception: {exception.Message}")
            .Text("Click OK to open the generated trace file:")
            .Text(tracePath)
            .Show();
        OpenFile(tracePath);

        if (crashDuringStartup && !Properties.Settings.Default.OutputLogFile)
        {
            Thread.Sleep(500);
            ShowMessage.Question()
                .Title("DiscordAudioStream Error")
                .Text("The application crashed before it could finish starting up.")
                .Text("Do you want to enable debug logging?")
                .IfYes(() => Properties.Settings.Default.OutputLogFile = true)
                .Show();
        }
    }

    private static void WriteStackTrace(Exception exception, string tracePath)
    {
        Logger.Log("  Trace path is: " + tracePath);

        string contents = $"{exception}\n\n{BuildInfo.FullInfo}";
        File.WriteAllText(tracePath, contents);

        Logger.Log("  Stack trace written successfully.");
    }

    private static void OpenFile(string path)
    {
        _ = System.Diagnostics.Process.Start(path);
    }
}
