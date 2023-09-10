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
			Logger.EmptyLine();
			Logger.Log("Initializing AudioMeterForm. darkMode = " + darkMode);
			
			if (darkMode) HandleCreated += new EventHandler(DarkThemeManager.FormHandleCreated);

			InitializeComponent();
			FormClosing += (s, e) => StorePosition();

			ApplyDarkTheme(darkMode);
		}

		private void ApplyDarkTheme(bool darkMode)
		{
			if (darkMode)
			{
				BackColor = DarkThemeManager.DarkBackColor;
				ForeColor = Color.White;
			}
		}


		public void SetLevels(float left, float right)
		{
			volumeMeterLeft.Amplitude = left;
			volumeMeterRight.Amplitude = right;
			volumeMeterRight.Refresh();
			volumeMeterLeft.Refresh();
		}


		
		// Store last position in settings
		
		new public void Show()
		{
			base.Show();
			// Restore saved position
			if (Properties.Settings.Default.AudioMeterForm_Size != Size.Empty)
			{
				Location = Properties.Settings.Default.AudioMeterForm_Position;
				Size = Properties.Settings.Default.AudioMeterForm_Size;
			}
			volumeMeterText.SetWindowWidth(Width);
			volumeMeterText2.SetWindowWidth(Width);
		}
		
		new public void Hide()
		{
			StorePosition();
			base.Hide();
		}
		private void StorePosition()
		{
			Properties.Settings.Default.AudioMeterForm_Position = Location;
			Properties.Settings.Default.AudioMeterForm_Size = Size;
			Properties.Settings.Default.Save();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			volumeMeterText.SetWindowWidth(Width);
			volumeMeterText2.SetWindowWidth(Width);
		}
	}
}
