using System;
using System.Drawing;
using System.Windows.Forms;
using CustomComponents;
using DiscordAudioStream.ScreenCapture;

namespace DiscordAudioStream
{
	partial class SettingsForm : Form
	{
		public delegate void CaptureMethodChangedDelegate();
		public event CaptureMethodChangedDelegate CaptureMethodChanged;

		private enum Theme
		{
			SYSTEM_DEFAULT = 0,
			LIGHT = 1,
			DARK = 2
		}
		private readonly CaptureState captureState;

		public SettingsForm(bool darkMode, CaptureState captureState)
		{
			// Store capture state in order to change ScreenMethod or WindowMethod
			this.captureState = captureState;

			// Enable dark titlebar
			if (darkMode) HandleCreated += new EventHandler(DarkThemeManager.FormHandleCreated);

			InitializeComponent();

			ApplyDarkTheme(darkMode);

			Theme theme = (Theme) Properties.Settings.Default.Theme;
			systemThemeRadio.Checked = (theme == Theme.SYSTEM_DEFAULT);
			lightThemeRadio.Checked = (theme == Theme.LIGHT);
			darkThemeRadio.Checked = (theme == Theme.DARK);

			autoExitCheckbox.Checked = Properties.Settings.Default.AutoExit;

			outputLogCheckbox.Checked = Properties.Settings.Default.OutputLogFile;

			windowMethodComboBox.SelectedIndex = (int) captureState.WindowMethod;
			fullscreenMethodComboBox.SelectedIndex = (int) captureState.ScreenMethod;
		}

		private void ApplyDarkTheme(bool darkMode)
		{
			if (darkMode)
			{
				BackColor = DarkThemeManager.DarkBackColor;
				ForeColor = Color.White;

				settingsTabs.BackTabColor = DarkThemeManager.DarkBackColor;
				settingsTabs.BorderColor = DarkThemeManager.DarkSecondColor;
				settingsTabs.HeaderColor = DarkThemeManager.DarkSecondColor;
				settingsTabs.TextColor = Color.White;
				settingsTabs.HorizontalLineColor = Color.Transparent;
			}

			settingsTabs.ActiveColor = DarkThemeManager.AccentColor;

			systemThemeRadio.SetDarkMode(darkMode);
			darkThemeRadio.SetDarkMode(darkMode);
			lightThemeRadio.SetDarkMode(darkMode);
			captureMethodGroup.SetDarkMode(darkMode);
			windowMethodComboBox.SetDarkMode(darkMode);
			fullscreenMethodComboBox.SetDarkMode(darkMode);
			classicVolumeMixerLink.LinkColor = DarkThemeManager.AccentColor;
			audioDevicesLink.LinkColor = DarkThemeManager.AccentColor;
		}


		// Events


		private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) Close();
		}

		private void systemThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (systemThemeRadio.Checked)
			{
				Properties.Settings.Default.Theme = (int) Theme.SYSTEM_DEFAULT;
				Properties.Settings.Default.Save();
			}
		}

		private void lightThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (lightThemeRadio.Checked)
			{
				Properties.Settings.Default.Theme = (int) Theme.LIGHT;
				Properties.Settings.Default.Save();
			}
		}

		private void darkThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (darkThemeRadio.Checked)
			{
				Properties.Settings.Default.Theme = (int) Theme.DARK;
				Properties.Settings.Default.Save();
			}
		}

		private void classicVolumeMixerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var cplPath = System.IO.Path.Combine(Environment.SystemDirectory, "sndvol.exe");
			System.Diagnostics.Process.Start(cplPath);
		}

		private void audioDevicesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var cplPath = System.IO.Path.Combine(Environment.SystemDirectory, "control.exe");
			System.Diagnostics.Process.Start(cplPath, "/name Microsoft.Sound");
		}

		private void autoExitCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.AutoExit = autoExitCheckbox.Checked;
			Properties.Settings.Default.Save();
		}

		private void outputLogCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.OutputLogFile = outputLogCheckbox.Checked;
			Properties.Settings.Default.Save();
		}

		private void fullscreenMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			captureState.ScreenMethod = (CaptureState.ScreenCaptureMethod) fullscreenMethodComboBox.SelectedIndex;
			CaptureMethodChanged?.Invoke();
		}

		private void windowMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			captureState.WindowMethod = (CaptureState.WindowCaptureMethod) windowMethodComboBox.SelectedIndex;
			CaptureMethodChanged?.Invoke();
		}
	}
}
