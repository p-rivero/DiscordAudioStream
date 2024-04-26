using System.Diagnostics;
using System.Windows.Forms;

using CustomComponents;

using DiscordAudioStream.Properties;

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
        Logger.Log(BuildInfo.FullInfo);
        RedirectConsoleOutput();
        CheckFrameworkVersion();
        InitializeSettings();

        CommandArguments consoleArgs = new(args);
        if (consoleArgs.ExitImmediately)
        {
            Logger.EmptyLine();
            Logger.Log("Exiting as requested by command line arguments.");
            return;
        }

        EnableNativeStyles();

        consoleArgs.ProcessArgsBeforeMainForm();
        using MainForm mainForm = new(IsDarkTheme);
        mainForm.Load += (sender, e) => consoleArgs.ProcessArgsAfterMainForm(mainForm.Controller);
        mainForm.Shown += (sender, e) => ExceptionHandler.StartupDone();
        Application.Run(mainForm);
    }

    private static bool IsDarkTheme => Settings.Default.Theme switch
    {
        0 => DarkThemeManager.IsDarkTheme(),
        1 => false,
        _ => true,
    };

    private static void EnableNativeStyles()
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

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
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
        ShowMessage.Warning()
            .Title(".NET Framework runtime not found")
            .Text("This application requires .NET Framework 4.7.2 or newer to work properly.")
            .Text("Do you want to open the download page?")
            .IfYes(() =>
            {
                _ = Process.Start(Resources.URL_NETFrameworkDownloadLink);
                Environment.Exit(0);
            })
            .AcceptByDefault()
            .Show(native: true);
    }

    private static void InitializeSettings()
    {
        Settings.Default.PropertyChanged += (sender, e) => Settings.Default.Save();
        if (Settings.Default.NeedsSettingsUpgrade)
        {
            try
            {
                Settings.Default.Upgrade();
            }
            catch (ArgumentNullException)
            {
                Logger.Log("Settings upgrade failed. Attempting to reset settings.");
                ResetSettings();
            }
            Settings.Default.NeedsSettingsUpgrade = false;
        }
    }

    private static void ResetSettings()
    {
        try
        {
            Settings.Default.Reset();
            Settings.Default.Save();
        }
        catch (Exception ex)
        {
            Logger.Log("Failed to reset settings:");
            Logger.Log(ex);
        }
    }

    private static string CommaSeparated(this string[] list)
    {
        return string.Join(", ", list.Select(a => $"'{a}'")); // "'a', 'b', 'c'"
    }
}
