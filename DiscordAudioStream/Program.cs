using CustomComponents;
using System;
using System.Windows.Forms;

namespace DiscordAudioStream
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				User32.SetProcessDPIAware();
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool darkMode;
			int theme = Properties.Settings.Default.Theme;
			if (theme == 0)
			{
				darkMode = DarkThemeManager.IsDarkTheme();
			}
			else if (theme == 1)
			{
				darkMode = false;
			}
			else
			{
				darkMode = true;
			}

			if (darkMode)
			{
				Uxtheme.AllowDarkModeForApp(true);
			}

			try
			{
				MainForm mainForm = new MainForm(darkMode);
				Application.Run(mainForm);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
