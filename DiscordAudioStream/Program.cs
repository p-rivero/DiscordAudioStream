using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using CustomComponents;

using Windows.Win32;
using Windows.Win32.UI.HiDpi;

namespace DiscordAudioStream;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        Logger.Log($"Started Main method. Arguments: [{args.CommaSeparated()}]");
        ExceptionHandler.Register();
        EmbeddedAssemblyResolver.Register();
        LogStartupInfo();
        RedirectConsoleOutput();
        CheckFrameworkVersion();

        CommandArguments consoleArgs = new(args);
        if (consoleArgs.ExitImmediately)
        {
            Logger.EmptyLine();
            Logger.Log("Exiting as requested by command line arguments.");
            return;
        }

        bool darkMode = IsDarkTheme();
        EnableNativeStyles(darkMode);

        consoleArgs.ProcessArgsBeforeMainForm();
        MainForm mainForm = new(darkMode);
        mainForm.Load += (sender, e) => consoleArgs.ProcessArgsAfterMainForm(mainForm.Controller);
        mainForm.Shown += (sender, e) => ExceptionHandler.StartupDone();
        Application.Run(mainForm);
    }

    private static void LogStartupInfo()
    {
        Logger.Log($"OS Version: {Environment.OSVersion}");
        Logger.Log($"Installed framework: {FrameworkInfo.VersionName}");
        Logger.Log($"Build ID: {BuildId.Id}");
        Logger.Log($"Log ID: {new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}");
    }

    private static bool IsDarkTheme()
    {
        return Properties.Settings.Default.Theme switch
        {
            0 => DarkThemeManager.IsDarkTheme(),
            1 => false,
            _ => true,
        };
    }

    private static void EnableNativeStyles(bool darkMode)
    {
        EnableDpiAwareness();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        if (darkMode)
        {
            PInvoke.AllowDarkModeForApp(true).AssertSuccess("Failed to enable dark mode.");
        }
    }

    private static void EnableDpiAwareness()
    {
        try
        {
            PInvoke.SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2)
                .AssertSuccess("Failed to set DPI awareness context");
        }
        catch (EntryPointNotFoundException)
        {
            Logger.Log("SetProcessDpiAwarenessContext not found. This is normal on Windows 7.");
        }
    }

    private static void RedirectConsoleOutput()
    {
        if (!PInvoke.AttachConsole(PInvoke.ATTACH_PARENT_PROCESS))
        {
            return;
        }

        // Skip shell prompt
        Console.WriteLine();
        Console.WriteLine();

        // Since we skipped the prompt, we need to trigger it manually
        AppDomain.CurrentDomain.ProcessExit += (sender, e) => SendKeys.SendWait("{ENTER}");
    }

    private static void CheckFrameworkVersion()
    {
        if (FrameworkInfo.VersionIsSupported)
        {
            return;
        }
        DialogResult result = MessageBox.Show(
            "This application requires .NET Framework 4.7.2 or newer to work properly.\n"
                + "Do you want to open the download page?",
            ".NET Framework runtime not found",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );
        if (result == DialogResult.Yes)
        {
            Process.Start(FrameworkInfo.DOWNLOAD_URL);
            Environment.Exit(0);
        }
    }

    private static string CommaSeparated(this string[] list)
    {
        return string.Join(", ", list.Select(a => $"'{a}'")); // "'a', 'b', 'c'"
    }
}
