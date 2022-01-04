using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using CustomComponents;

namespace DiscordAudioStream
{
	public partial class MainForm : Form, IScreenCaptureMaster
	{
		private AreaForm areaForm;

		private bool darkMode;
		private bool streamEnabled = false;
		private Size defaultWindowSize;
		private Size defaultPreviewSize;
		private Point defaultPreviewLocation;

		private ScreenCaptureWorker screenCaptureWorker;
		private const int TARGET_FRAMERATE = 60;
		private double scaleMultiplier = 1;
		private int numberOfScreens = -1;

		private AudioPlayback audioPlayback = null;
		
		public MainForm(bool darkMode)
		{
			int a = Keys.R.GetHashCode();
			if (darkMode)
			{
				this.HandleCreated += new EventHandler(DarkThemeManager.formHandleCreated);
			}

			this.darkMode = darkMode;

			InitializeComponent();
			previewBox.Visible = true;

			defaultWindowSize = this.Size;
			defaultPreviewSize = previewBox.Size;
			defaultPreviewLocation = previewBox.Location;

			areaForm = new AreaForm();
			areaForm.Owner = this;

			inputDeviceComboBox.SelectedIndex = 0;

			RefreshScreens();
			RefreshAudioDevices();

			previewBtn.Checked = Properties.Settings.Default.Preview;
			enablePreview(previewBtn.Checked);

			areaComboBox.SelectedIndex = Properties.Settings.Default.AreaIndex;
			scaleComboBox.SelectedIndex = Properties.Settings.Default.ScaleIndex;

			xNumeric.Minimum = SystemInformation.VirtualScreen.Left;
			yNumeric.Minimum = SystemInformation.VirtualScreen.Top;

			applyDarkTheme(darkMode);
		}

		private void applyDarkTheme(bool darkMode)
		{
			if (darkMode)
			{
				this.ForeColor = Color.White;
				this.BackColor = DarkThemeManager.DarkBackColor;

				aboutBtn.Image = Properties.Resources.white_about;
				onTopBtn.Image = Properties.Resources.white_ontop;
				volumeMixerButton.Image = Properties.Resources.white_mixer;
				soundDevicesButton.Image = Properties.Resources.white_speaker;
				settingsBtn.Image = Properties.Resources.white_settings;
				previewBtn.Image = Properties.Resources.white_preview;

				refreshAudioBtn.BackColor = DarkThemeManager.DarkSecondColor;
				refreshAudioBtn.Image = Properties.Resources.white_refresh;

				refreshScreensBtn.BackColor = DarkThemeManager.DarkSecondColor;
				refreshScreensBtn.Image = Properties.Resources.white_refresh;
			}

			videoGroup.SetDarkMode(darkMode);
			audioGroup.SetDarkMode(darkMode);
			toolStrip.SetDarkMode(darkMode, false);
			inputDeviceComboBox.SetDarkMode(darkMode);
			areaComboBox.SetDarkMode(darkMode);
			scaleComboBox.SetDarkMode(darkMode);
			widthNumeric.SetDarkMode(darkMode);
			heightNumeric.SetDarkMode(darkMode);
			xNumeric.SetDarkMode(darkMode);
			yNumeric.SetDarkMode(darkMode);
			captureCursorCheckBox.SetDarkMode(darkMode);
			hideTaskbarCheckBox.SetDarkMode(darkMode);
		}

		public void SetAreaWidth(int w)
		{
			widthNumeric.Value = w;
		}

		public void SetAreaHeight(int h)
		{
			heightNumeric.Value = h;
		}

		public void SetAreaX(int x)
		{
			xNumeric.Value = x;
		}

		public void SetAreaY(int y)
		{
			yNumeric.Value = y;
		}

		public void SetMaximumX(int maxX)
		{
			xNumeric.Maximum = maxX + SystemInformation.VirtualScreen.Left;
		}

		public void SetMaximumY(int maxY)
		{
			yNumeric.Maximum = maxY + SystemInformation.VirtualScreen.Top;
		}

		private void aboutBtn_Click(object sender, EventArgs e)
		{
			AboutForm aboutBox = new AboutForm(darkMode);
			aboutBox.Owner = this;
			if (this.TopMost)
			{
				aboutBox.TopMost = true;
			}
			aboutBox.ShowDialog();
		}

		private void areaComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (numberOfScreens == -1)
				return;

			RefreshAreaInfo();
			// Do not save settings for Custom Area or Window
			if (areaComboBox.SelectedIndex >= numberOfScreens)
				return;

			Properties.Settings.Default.AreaIndex = areaComboBox.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		private void RefreshAreaInfo()
		{
			// Custom area
			if (areaComboBox.SelectedIndex == numberOfScreens)
			{
				areaForm.Show();

				// Omit pixels of the red border
				widthNumeric.Value = areaForm.Width - 2;
				heightNumeric.Value = areaForm.Height - 2;
				xNumeric.Value = areaForm.Left + 1;
				yNumeric.Value = areaForm.Top + 1;

				widthNumeric.Enabled = true;
				heightNumeric.Enabled = true;
				xNumeric.Enabled = true;
				yNumeric.Enabled = true;

				hideTaskbarCheckBox.Enabled = false;
				ProcessHandleManager.CapturingWindow = false;
			}
			// Window
			else if (areaComboBox.SelectedIndex > numberOfScreens)
			{
				areaForm.Hide();

				widthNumeric.Enabled = false;
				heightNumeric.Enabled = false;
				xNumeric.Enabled = false;
				yNumeric.Enabled = false;

				hideTaskbarCheckBox.Enabled = false;
				ProcessHandleManager.SelectedIndex = areaComboBox.SelectedIndex - numberOfScreens - 1;
				ProcessHandleManager.CapturingWindow = true;
			}
			// Screen
			else
			{
				areaForm.Hide();

				widthNumeric.Enabled = false;
				heightNumeric.Enabled = false;
				xNumeric.Enabled = false;
				yNumeric.Enabled = false;

				hideTaskbarCheckBox.Enabled = true;
				ProcessHandleManager.CapturingWindow = false;

				if (Screen.AllScreens.Length > 1)
				{
					// All screens
					if (areaComboBox.SelectedIndex == numberOfScreens - 1)
					{
						widthNumeric.Value = SystemInformation.VirtualScreen.Width;
						heightNumeric.Value = SystemInformation.VirtualScreen.Height;
						xNumeric.Value = SystemInformation.VirtualScreen.Left;
						yNumeric.Value = SystemInformation.VirtualScreen.Top;

						hideTaskbarCheckBox.Enabled = false;
					}
					else
					{
						hideTaskbarCheckBox.Enabled = true;

						if (hideTaskbarCheckBox.Checked)
						{
							widthNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.Width;
							heightNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.Height;
							xNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.X;
							yNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.Y;
						}
						else
						{
							widthNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.Width;
							heightNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.Height;
							xNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.X;
							yNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.Y;
						}
					}
				}
				else
				{
					if (hideTaskbarCheckBox.Checked)
					{
						widthNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.Width;
						heightNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.Height;
						xNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.X;
						yNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].WorkingArea.Y;
					}
					else
					{
						widthNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.Width;
						heightNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.Height;
						xNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.X;
						yNumeric.Value = Screen.AllScreens[areaComboBox.SelectedIndex].Bounds.Y;
					}

					hideTaskbarCheckBox.Enabled = true;
				}
			}
		}

		private void widthNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (widthNumeric.Enabled)
			{
				Invoke(new Action(() =>
				{
					areaForm.Width = (int)widthNumeric.Value + 2;
				}));
			}
		}

		private void heightNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (heightNumeric.Enabled)
			{
				Invoke(new Action(() =>
				{
					areaForm.Height = (int)heightNumeric.Value + 2;
				}));
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			onTopBtn.Checked = Properties.Settings.Default.AlwaysOnTop;
			captureCursorCheckBox.Checked = Properties.Settings.Default.CaptureCursor;
			hideTaskbarCheckBox.Checked = Properties.Settings.Default.HideTaskbar;

			screenCaptureWorker = new ScreenCaptureWorker(TARGET_FRAMERATE, this);

			Thread drawThread = new Thread(() =>
			{
				Stopwatch stopwatch = new Stopwatch();
				const int INTERVAL_MS = 1000 / TARGET_FRAMERATE;

				while (true)
				{
					stopwatch.Restart();
					try
					{
						Bitmap next = ScreenCaptureWorker.GetNextFrame();
						if (next == null) continue;
						Invoke(new Action(() =>
						{
							if (previewBox.Image != null)
							{
								previewBox.Image.Dispose();
							}
							previewBox.Image = next;
						}));
					}
					catch (InvalidOperationException)
					{
						// Form is closing
						return;
					}
					stopwatch.Stop();

					int wait = INTERVAL_MS - (int)stopwatch.ElapsedMilliseconds;
					if (wait > 0)
					{
						Thread.Sleep(wait);
					}
				}
			});
			drawThread.IsBackground = true;
			drawThread.Start();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (e.CloseReason == CloseReason.WindowsShutDown) return;

			if (streamEnabled)
			{
				e.Cancel = true; // Do not close form

				// Instead, return to settings
				EndStream();
			}
			else
			{
				screenCaptureWorker.Stop();
				User32.UnregisterHotKey(this.Handle, 0);
				ProcessHandleManager.ClearSelectedIndex();
			}
		}

		private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.TopMost = onTopBtn.Checked;
			areaForm.TopMost = onTopBtn.Checked;
			Properties.Settings.Default.AlwaysOnTop = onTopBtn.Checked;
			Properties.Settings.Default.Save();
		}

		private void refreshBtn_Click(object sender, EventArgs e)
		{
			RefreshAudioDevices();
		}

		private void RefreshAudioDevices()
		{
			inputDeviceComboBox.Items.Clear();

			string[] names = AudioPlayback.RefreshDevices();

			inputDeviceComboBox.Items.Add("(None)");
			foreach (string device in names)
			{
				inputDeviceComboBox.Items.Add(device);
			}
			inputDeviceComboBox.SelectedIndex = 0;
		}

		private void RefreshScreens()
		{
			areaComboBox.Items.Clear();

			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				if (Screen.AllScreens[i].Primary)
				{
					areaComboBox.Items.Add("Primary screen (" + Screen.AllScreens[i].Bounds.Width + "x" + Screen.AllScreens[i].Bounds.Height + ")");
				}
				else
				{
					areaComboBox.Items.Add("Screen " + (i + 1) + " (" + Screen.AllScreens[i].Bounds.Width + "x" + Screen.AllScreens[i].Bounds.Height + ")");
				}
			}

			if (Screen.AllScreens.Length > 1)
			{
				areaComboBox.Items.Add("Everything");
			}

			numberOfScreens = areaComboBox.Items.Count;

			try
			{
				areaComboBox.SelectedIndex = Properties.Settings.Default.AreaIndex;
			}
			catch (ArgumentOutOfRangeException)
			{
				// Number of screen may have changed
				areaComboBox.SelectedIndex = 0;
			}

			areaComboBox.Items.Add(new DarkThemeComboBox.ItemWithSeparator("Custom area"));

			foreach (string window in ProcessHandleManager.RefreshHandles())
			{
				areaComboBox.Items.Add(window);
			}
			areaComboBox.Items.Add(new DarkThemeComboBox.Dummy());

			widthNumeric.Maximum = SystemInformation.VirtualScreen.Width;
			heightNumeric.Maximum = SystemInformation.VirtualScreen.Height;

			areaForm.SetMaximumArea(SystemInformation.VirtualScreen);
		}

		private void settingsBtn_Click(object sender, EventArgs e)
		{
			SettingsForm settingsBox = new SettingsForm(darkMode);
			settingsBox.Owner = this;
			if (this.TopMost)
			{
				settingsBox.TopMost = true;
			}
			settingsBox.ShowDialog();
		}

		private void xNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (xNumeric.Enabled)
			{
				Invoke(new Action(() =>
				{
					// Omit 1 pixel for red border
					areaForm.Left = (int)xNumeric.Value - 1;
				}));
			}
		}

		private void yNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (yNumeric.Enabled)
			{
				Invoke(new Action(() =>
				{
					// Omit 1 pixel for red border
					areaForm.Top = (int)yNumeric.Value - 1;
				}));
			}
		}

		private void hideTaskbarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			RefreshAreaInfo();
			Properties.Settings.Default.HideTaskbar = hideTaskbarCheckBox.Checked;
			Properties.Settings.Default.Save();
		}

		private void refreshScreensBtn_Click(object sender, EventArgs e)
		{
			RefreshScreens();
		}

		private void captureCursorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.CaptureCursor = captureCursorCheckBox.Checked;
			Properties.Settings.Default.Save();
		}

		private void enablePreview(bool visible)
		{
			previewBox.Visible = visible;

			if (visible)
			{
				this.Size = defaultWindowSize;
			}
			else
			{
				Size newSize = this.Size;
				newSize.Width = defaultWindowSize.Width - (defaultPreviewSize.Width + 10);
				this.Size = newSize;

				if (previewBox.Image != null)
				{
					previewBox.Image.Dispose();
					previewBox.Image = null;
				}
			}
		}

		private void previewBtn_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Preview = previewBtn.Checked;
			Properties.Settings.Default.Save();

			enablePreview(previewBtn.Checked);
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				if (e.KeyCode == Keys.P)
				{
					previewBtn.PerformClick();
				}
				else if (e.KeyCode == Keys.T)
				{
					onTopBtn.PerformClick();
				}
				else if (e.KeyCode == Keys.Oemcomma)
				{
					settingsBtn.PerformClick();
				}
				else if (e.KeyCode == Keys.V)
				{
					volumeMixerButton.PerformClick();
				}
				else if (e.KeyCode == Keys.A)
				{
					soundDevicesButton.PerformClick();
				}
			}
			else
			{
				if (e.KeyCode == Keys.F1)
				{
					aboutBtn.PerformClick();
				}
			}
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			StartStream();
		}

		public void SetPreviewSize(Size size)
		{
			if (streamEnabled)
			{
				size.Width  = (int)(Math.Max(160, size.Width) * scaleMultiplier);
				size.Height = (int)(Math.Max(160, size.Height) * scaleMultiplier);
				BeginInvoke(new Action(() =>
				{
					previewBox.Size = size;
				}));
			}
		}

		public void CapturedWindowSizeChanged(Size newSize)
		{
			SetPreviewSize(newSize);
		}

		private void scaleComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = scaleComboBox.SelectedIndex;
			if (index == 0) scaleMultiplier = 1;              // Original resolution
			else if (index == 1) scaleMultiplier = Math.Sqrt(0.5); // 50% resolution
			else if (index == 2) scaleMultiplier = 0.5;            // 25% resolution
			else throw new ArgumentException("Invalid index");

			Properties.Settings.Default.ScaleIndex = index;
			Properties.Settings.Default.Save();
		}

		private void StartStream()
		{
			if (inputDeviceComboBox.SelectedIndex == 0)
			{
				DialogResult r = MessageBox.Show("No audio source selected, continue anyways?", "Warning",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (r == DialogResult.No)
					return;
			}
			else
			{
				// Skip "None"
				audioPlayback = new AudioPlayback(inputDeviceComboBox.SelectedIndex - 1);
				audioPlayback.Start();
			}

			enablePreview(true);
			previewBox.Location = new Point(0, 0);
			videoGroup.Visible = false;
			audioGroup.Visible = false;
			startButton.Visible = false;
			toolStrip.Visible = false;
			streamEnabled = true;
			this.AutoSize = true;

			SetPreviewSize(previewBox.Image.Size);
		}

		private void EndStream()
		{
			videoGroup.Visible = true;
			audioGroup.Visible = true;
			startButton.Visible = true;
			toolStrip.Visible = true;
			streamEnabled = false;

			previewBox.Size = defaultPreviewSize;
			previewBox.Location = defaultPreviewLocation;
			this.AutoSize = false;
			if (audioPlayback != null) audioPlayback.Stop();

			enablePreview(previewBtn.Checked);
			CenterToScreen();
		}

		public void GetCaptureArea(out Size size, out Point pos)
		{
			int tmp_width = 0, tmp_height = 0, tmp_x = 0, tmp_y = 0;
			Invoke(new Action(() =>
			{
				tmp_width = (int)widthNumeric.Value;
				tmp_height = (int)heightNumeric.Value;
				tmp_x = (int)xNumeric.Value;
				tmp_y = (int)yNumeric.Value;
			}));

			size = new Size(tmp_width, tmp_height);
			pos = new Point(tmp_x, tmp_y);
		}

		public bool IsCapturingCursor()
		{
			return captureCursorCheckBox.Checked;
		}

		private void volumeMixerButton_Click(object sender, EventArgs e)
		{
			var osVersionInfo = Ntdll.OSVERSIONINFOEX.Init();
			Ntdll.RtlGetVersion(ref osVersionInfo); 

			if (osVersionInfo.MajorVersion >= 10)
			{
				Process.Start("ms-settings:apps-volume");
			}
			else
			{
				// Use old volume mixer
				var cplPath = Path.Combine(Environment.SystemDirectory, "sndvol.exe");
				Process.Start(cplPath);
			}
		}

		private void soundDevicesButton_Click(object sender, EventArgs e)
		{
			var osVersionInfo = Ntdll.OSVERSIONINFOEX.Init();
			Ntdll.RtlGetVersion(ref osVersionInfo);

			if (osVersionInfo.MajorVersion >= 10 && osVersionInfo.BuildNumber >= 17063)
			{
				Process.Start("ms-settings:sound");
			}
			else
			{
				// Use old sound settings
				var cplPath = Path.Combine(Environment.SystemDirectory, "control.exe");
				Process.Start(cplPath, "/name Microsoft.Sound");
			}
		}

		public void AbortCapture()
		{
			Invoke(new Action(() =>
			{
				RefreshScreens();
				if (!streamEnabled) return;

				EndStream();
				if (Properties.Settings.Default.AutoExit) Close();
			}));
		}
	}
}
