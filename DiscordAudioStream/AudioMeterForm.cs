using CustomComponents;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream
{
	public partial class AudioMeterForm : Form
	{
		public AudioMeterForm(bool darkMode)
		{
			Logger.Log("\nInitializing AudioMeterForm. darkMode={0}", darkMode);

			if (darkMode) HandleCreated += new EventHandler(DarkThemeManager.FormHandleCreated);

			InitializeComponent();
			FormClosing += AudioMeterForm_FormClosing;

			ApplyDarkTheme(darkMode);
		}

		private void ApplyDarkTheme(bool darkMode)
		{
			if (darkMode)
			{
				BackColor = DarkThemeManager.DarkBackColor;
				ForeColor = Color.White;
			}
			volumeMeterLeft.SetDarkMode(darkMode);
			volumeMeterRight.SetDarkMode(darkMode);
		}


		public void SetLevels(float left, float right)
		{
			volumeMeterLeft.Amplitude = left;
			volumeMeterRight.Amplitude = right;
		}


		
		// Store last position in settings
		
		new public void Show()
		{
			base.Show();
			// Restore saved position
			if (Properties.Settings.Default.AudioMeterForm_Size != Size.Empty)
			{
				Console.WriteLine("Restoring");
				Location = Properties.Settings.Default.AudioMeterForm_Position;
				Size = Properties.Settings.Default.AudioMeterForm_Size;
			}
		}
		
		new public void Hide()
		{
			StorePosition();
			base.Hide();
		}
		private void AudioMeterForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			StorePosition();
		}
		private void StorePosition()
		{
			Properties.Settings.Default.AudioMeterForm_Position = Location;
			Properties.Settings.Default.AudioMeterForm_Size = Size;
			Properties.Settings.Default.Save();
		}
	}
}
