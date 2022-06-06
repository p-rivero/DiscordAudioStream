using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using CustomComponents;

namespace DiscordAudioStream
{
	partial class AboutForm : Form
	{
		public AboutForm(bool darkMode)
		{
			if (darkMode) HandleCreated += new EventHandler(DarkThemeManager.FormHandleCreated);

			InitializeComponent();

			string fullVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			int lastDotIndex = fullVer.LastIndexOf('.');
			versionLabel.Text = string.Format("Version: {0} ({1} bit)", fullVer.Substring(0, lastDotIndex), 8*IntPtr.Size);

			ApplyDarkTheme(darkMode);
		}

		private void ApplyDarkTheme(bool darkMode)
		{
			if (darkMode)
			{
				BackColor = DarkThemeManager.DarkBackColor;
				ForeColor = Color.White;
			}

			updatesLink.LinkColor = DarkThemeManager.AccentColor;
			screenRecorderLink.LinkColor = DarkThemeManager.AccentColor;
			moduleArtLabel.LinkColor = DarkThemeManager.AccentColor;
			projectLink.LinkColor = DarkThemeManager.AccentColor;
			licenseLink.LinkColor = DarkThemeManager.AccentColor;
			issuesLink.LinkColor = DarkThemeManager.AccentColor;
			authorLink.LinkColor = DarkThemeManager.AccentColor;

			infoGroup.SetDarkMode(darkMode);
			pagesGroup.SetDarkMode(darkMode);
		}


		// Events


		private void projectLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Resources.ProjectLinkUrl);
		}

		private void issuesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Resources.IssuesLinkUrl);
		}

		private void updatesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Resources.UpdatesLinkUrl);
		}

		private void AboutForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) Close();
		}

		private void licenseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Resources.LicenseLinkUrl);
		}

		private void moduleArtLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Resources.ModuleArtLinkUrl);
		}

		private void screenRecorderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Resources.ScreenRecorderLinkUrl);
		}

		private void authorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Resources.AuthorLinkUrl);
		}
	}
}
