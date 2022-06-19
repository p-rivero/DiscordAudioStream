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
			Logger.Log("\nInitializing SettingsForm. darkMode={0}", darkMode);

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

			// Set tooltips
			toolTip.SetToolTip(autoExitCheckbox, Properties.Resources.AutoExitTooltip);
			toolTip.SetToolTip(captureFramerateLabel, Properties.Resources.CaptureFramerateTooltip);
			toolTip.SetToolTip(captureFramerateComboBox, Properties.Resources.CaptureFramerateTooltip);
			toolTip.SetToolTip(fullscreenMethodLabel, Properties.Resources.FullscreenMethodTooltip);
			toolTip.SetToolTip(fullscreenMethodComboBox, Properties.Resources.FullscreenMethodTooltip);
			toolTip.SetToolTip(windowMethodLabel, Properties.Resources.WindowMethodTooltip);
			toolTip.SetToolTip(windowMethodComboBox, Properties.Resources.WindowMethodTooltip);
			toolTip.SetToolTip(outputLogCheckbox, Properties.Resources.OutputLogTooltip);
			toolTip.SetToolTip(offscreenDrawCheckbox, Properties.Resources.OffscreenDrawTooltip);
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
				ChangeTheme(Theme.SYSTEM_DEFAULT);
			}
		}

		private void lightThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (lightThemeRadio.Checked)
			{
				ChangeTheme(Theme.LIGHT);
			} 
		}

		private void darkThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (darkThemeRadio.Checked)
			{
				ChangeTheme(Theme.DARK);
			}
		}

		private void ChangeTheme(Theme theme)
		{
			// Nothing changed
			if (Properties.Settings.Default.Theme == (int)theme) return;

			Properties.Settings.Default.Theme = (int)theme;
			Properties.Settings.Default.Save();
			Logger.Log("\nChange settings: Theme={0}", Properties.Settings.Default.Theme);
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
			// Nothing changed
			if (Properties.Settings.Default.AutoExit == autoExitCheckbox.Checked) return;

			Properties.Settings.Default.AutoExit = autoExitCheckbox.Checked;
			Properties.Settings.Default.Save();
			Logger.Log("\nChange settings: AutoExit={0}", Properties.Settings.Default.AutoExit);
		}

		private void outputLogCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			// Nothing changed
			if (Properties.Settings.Default.OutputLogFile == outputLogCheckbox.Checked) return;

			Properties.Settings.Default.OutputLogFile = outputLogCheckbox.Checked;
			Properties.Settings.Default.Save();
			Logger.Log("\nChange settings: OutputLogFile={0}", Properties.Settings.Default.OutputLogFile);
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
