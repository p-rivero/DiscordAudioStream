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

		private readonly CaptureState captureState;

		public SettingsForm(bool darkMode, CaptureState state)
		{
			this.captureState = state;

			if (darkMode)
			{
				this.HandleCreated += new EventHandler(DarkThemeManager.formHandleCreated);
			}

			InitializeComponent();

			if (darkMode)
			{
				this.BackColor = DarkThemeManager.DarkBackColor;
				this.ForeColor = Color.White;

				settingsTabs.BackTabColor = DarkThemeManager.DarkBackColor;
				settingsTabs.BorderColor = DarkThemeManager.DarkSecondColor;
				settingsTabs.HeaderColor = DarkThemeManager.DarkSecondColor;
				settingsTabs.TextColor = Color.White;
				settingsTabs.HorizontalLineColor = Color.Transparent;

				mixerBtn.BackColor = DarkThemeManager.DarkSecondColor;
				mixerBtn.Image = Properties.Resources.white_mixer;
				winSoundBtn.BackColor = DarkThemeManager.DarkSecondColor;
				winSoundBtn.Image = Properties.Resources.white_speaker;
			}

			settingsTabs.ActiveColor = DarkThemeManager.AccentColor;

			systemThemeRadio.SetDarkMode(darkMode);
			darkThemeRadio.SetDarkMode(darkMode);
			lightThemeRadio.SetDarkMode(darkMode);
			captureMethodGroup.SetDarkMode(darkMode);
			windowMethodComboBox.SetDarkMode(darkMode);
			fullscreenMethodComboBox.SetDarkMode(darkMode);

			int theme = Properties.Settings.Default.Theme;
			if (theme == 0)
			{
				systemThemeRadio.Checked = true;
			}
			else if (theme == 1)
			{
				lightThemeRadio.Checked = true;
			}
			else
			{
				darkThemeRadio.Checked = true;
			}

			autoExitCheckbox.Checked = Properties.Settings.Default.AutoExit;

			windowMethodComboBox.SelectedIndex = (int) state.WindowMethod;
			fullscreenMethodComboBox.SelectedIndex = (int) state.ScreenMethod;
		}

		private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void systemThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (systemThemeRadio.Checked)
			{
				Properties.Settings.Default.Theme = 0;
				Properties.Settings.Default.Save();
			}
		}

		private void lightThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (lightThemeRadio.Checked)
			{
				Properties.Settings.Default.Theme = 1;
				Properties.Settings.Default.Save();
			}
		}

		private void darkThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (darkThemeRadio.Checked)
			{
				Properties.Settings.Default.Theme = 2;
				Properties.Settings.Default.Save();
			}
		}

		private void mixerBtn_Click(object sender, EventArgs e)
		{
			var cplPath = System.IO.Path.Combine(Environment.SystemDirectory, "sndvol.exe");
			System.Diagnostics.Process.Start(cplPath);
		}

		private void winSoundBtn_Click(object sender, EventArgs e)
		{
			var cplPath = System.IO.Path.Combine(Environment.SystemDirectory, "control.exe");
			System.Diagnostics.Process.Start(cplPath, "/name Microsoft.Sound");
		}

		private void autoExitCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.AutoExit = autoExitCheckbox.Checked;
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
