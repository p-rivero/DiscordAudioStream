using CustomComponents;
using System;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

namespace DiscordAudioStream
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
			User32.SetProcessDpiAwarenessContext(User32.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);

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


		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

			dllName = dllName.Replace(".", "_");

			if (dllName.EndsWith("_resources")) return null;

			ResourceManager rm = new ResourceManager(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".Properties.Resources", Assembly.GetExecutingAssembly());

			byte[] bytes = (byte[])rm.GetObject(dllName);

			return Assembly.Load(bytes);
		}
	}
}
