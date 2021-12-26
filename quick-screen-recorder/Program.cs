using QuickLibrary;
using System;
using System.Windows.Forms;

namespace quick_screen_recorder
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				SetProcessDPIAware();
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool darkMode;
			int theme = Properties.Settings.Default.Theme;
			if (theme == 0)
			{
				darkMode = ThemeManager.isDarkTheme();
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
				ThemeManager.allowDarkModeForApp(true);
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

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}
