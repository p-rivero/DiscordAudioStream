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
		private bool darkMode;

		public AboutForm(bool darkMode)
		{
			if (darkMode)
			{
				this.HandleCreated += new EventHandler(DarkThemeManager.formHandleCreated);
			}

			this.darkMode = darkMode;

			InitializeComponent();

			string fullVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			int lastDotIndex = fullVer.LastIndexOf('.');
			versionLabel.Text = String.Format("Version: {0}", fullVer.Substring(0, lastDotIndex));

			if (IntPtr.Size == 4)
			{
				versionLabel.Text += " (x32)";
			}
			else if (IntPtr.Size == 8)
			{
				versionLabel.Text += " (x64)";
			}

			if (darkMode)
			{
				this.BackColor = DarkThemeManager.DarkBackColor;
				this.ForeColor = Color.White;
			}

			updatesLink.LinkColor = DarkThemeManager.AccentColor;
			developerLink.LinkColor = DarkThemeManager.AccentColor;
			projectLink.LinkColor = DarkThemeManager.AccentColor;
			licenseLink.LinkColor = DarkThemeManager.AccentColor;
			issuesLink.LinkColor = DarkThemeManager.AccentColor;

			infoGroup.SetDarkMode(darkMode);
			pagesGroup.SetDarkMode(darkMode);
		}

		private void developerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/ModuleArt/");
		}

		private void projectLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/ModuleArt/quick-picture-viewer/");
		}

		private void issuesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/ModuleArt/quick-picture-viewer/issues/");
		}

		private void updatesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/ModuleArt/quick-picture-viewer/issues/");
			this.Close();
		}

		private void AboutForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void licenseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://github.com/ModuleArt/quick-picture-viewer/blob/master/LICENSE.md/");
		}
	}
}
