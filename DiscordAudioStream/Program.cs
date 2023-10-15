using System;
using System.Linq;
using System.Windows.Forms;

using Windows.Win32;
using Windows.Win32.UI.HiDpi;

using CustomComponents;

using DLLs;

namespace DiscordAudioStream
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            ExceptionHandler.Register();
            EmbeddedAssemblyResolver.Register();
            RedirectConsoleOutput();

            string argumentList = string.Join(", ", args.Select(a => $"'{a}'"));
            Logger.Log($"Started Main method. Arguments: [{argumentList}]");
            Logger.Log("OS Version: " + Environment.OSVersion);
            Logger.Log("Log ID: " + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());
            Logger.Log("Build ID: " + BuildId.Id);

            CommandArguments consoleArgs = new CommandArguments(args);

            if (consoleArgs.ExitImmediately)
            {
                Logger.EmptyLine();
                Logger.Log("Exiting as requested by command line arguments.");
                return;
            }

            bool darkMode = IsDarkTheme();
            EnableNativeStyles(darkMode);

            consoleArgs.ProcessArgsBeforeMainForm();
            MainForm mainForm = new MainForm(darkMode);
            mainForm.Load += (sender, e) => consoleArgs.ProcessArgsAfterMainForm(mainForm.Controller);
            Application.Run(mainForm);
        }

        private static bool IsDarkTheme()
        {
            switch (Properties.Settings.Default.Theme)
            {
                case 0:
                    return DarkThemeManager.IsDarkTheme();
                case 1:
                    return false;
                default:
                    return true;
            }
        }

        private static void EnableNativeStyles(bool darkMode)
        {
            EnableDpiAwareness();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (darkMode)
            {
                Uxtheme.AllowDarkModeForApp(true);
            }
        }

        private static void EnableDpiAwareness()
        {
            try
            {
                if (!PInvoke.SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2))
                {
                    Logger.Log("Failed to set DPI awareness context.");
                }
            }
            catch (EntryPointNotFoundException)
            {
                Logger.Log("SetProcessDpiAwarenessContext not found. This is normal on Windows 7.");
            }
        }

        private static void RedirectConsoleOutput()
        {
            bool success = PInvoke.AttachConsole(PInvoke.ATTACH_PARENT_PROCESS);
            if (!success)
            {
                return;
            }

            // Skip shell prompt
            Console.WriteLine();
            Console.WriteLine();

            // Since we skipped the prompt, we need to trigger it manually
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => SendKeys.SendWait("{ENTER}");
        }
    }
}
