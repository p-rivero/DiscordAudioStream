using CustomComponents;
using System;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using DLLs;

namespace DiscordAudioStream
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			ExceptionHandler.Register();
			EmbeddedAssemblyResolver.Register();
			
			Logger.Log("Started Main method. Arguments: [{0}] (size={1})", string.Join(",", args), args.Length);
			Logger.Log("OS Version: {0}", Environment.OSVersion);
			Logger.Log("Log ID: {0}", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());
			Logger.Log("Build ID: " + BuildId.Id);

			CommandArguments consoleArgs = new CommandArguments(args);

			if (consoleArgs.ExitImmediately)
			{
				Logger.Log("Exiting as requested by command line arguments.");
				return;
			}

			bool darkMode = IsDarkTheme();
			EnableNativeStyles(darkMode);

			MainForm mainForm = new MainForm(darkMode);
			mainForm.Load += (sender, e) => consoleArgs.ProcessArgs(mainForm.Controller);
			Application.Run(mainForm);
		}

		private static bool IsDarkTheme()
		{
			switch (Properties.Settings.Default.Theme)
			{
				case 0:
					return DarkThemeManager.IsDarkTheme();
				case 1:
					return true;
				default:
					return false;
			}
		}

		private static void EnableNativeStyles(bool darkMode)
		{
			User32.SetProcessDpiAwarenessContext(User32.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (darkMode) Uxtheme.AllowDarkModeForApp(true);
		}
	}
}
