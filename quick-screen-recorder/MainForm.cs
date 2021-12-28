using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using QuickLibrary;
using System.Diagnostics;
using NAudio.CoreAudioApi;

namespace quick_screen_recorder
{
	public partial class MainForm : Form
	{
		private AreaForm areaForm;
		private StopForm stopForm;

		private Recorder recorder = null;

		private bool darkMode;
		private bool streamEnabled = false;
		private const int TARGET_FRAMERATE = 60;
		private AudioPlayback audioPlayback = null;
		private MMDeviceCollection audioDevices;

		public MainForm(bool darkMode)
		{
			if (darkMode)
			{
				this.HandleCreated += new EventHandler(ThemeManager.formHandleCreated);
			}

			this.darkMode = darkMode;

			InitializeComponent();

			areaForm = new AreaForm();
			areaForm.Owner = this;

			inputDeviceComboBox.SelectedIndex = 0;

			RefreshScreens();
			RefreshAudioDevices();

			previewBtn.Checked = Properties.Settings.Default.Preview;
			enabledPreview(previewBtn.Checked);

			applyDarkTheme(darkMode);
		}

		private void applyDarkTheme(bool darkMode)
		{
			if (darkMode)
			{
				this.ForeColor = Color.White;
				this.BackColor = ThemeManager.DarkBackColor;

				recButton.BackColor = ThemeManager.DarkSecondColor;
				recButton.Image = Properties.Resources.white_record;

				aboutBtn.Image = Properties.Resources.white_about;
				onTopBtn.Image = Properties.Resources.white_ontop;
				settingsBtn.Image = Properties.Resources.white_settings;
				previewBtn.Image = Properties.Resources.white_preview;

				fileNameTextBox.BackColor = ThemeManager.DarkSecondColor;
				fileNameTextBox.ForeColor = Color.White;

				folderTextBox.BackColor = ThemeManager.DarkSecondColor;
				folderTextBox.ForeColor = Color.White;

				refreshAudioBtn.BackColor = ThemeManager.DarkSecondColor;
				refreshAudioBtn.Image = Properties.Resources.white_refresh;

				refreshScreensBtn.BackColor = ThemeManager.DarkSecondColor;
				refreshScreensBtn.Image = Properties.Resources.white_refresh;
			}

			generalGroup.SetDarkMode(darkMode);
			videoGroup.SetDarkMode(darkMode);
			audioGroup.SetDarkMode(darkMode);
			toolStrip.SetDarkMode(darkMode, false);
			browseFolderBtn.SetDarkMode(darkMode);
			qualityComboBox.SetDarkMode(darkMode);
			inputDeviceComboBox.SetDarkMode(darkMode);
			areaComboBox.SetDarkMode(darkMode);
			widthNumeric.SetDarkMode(darkMode);
			heightNumeric.SetDarkMode(darkMode);
			xNumeric.SetDarkMode(darkMode);
			yNumeric.SetDarkMode(darkMode);
			separateAudioCheckBox.SetDarkMode(darkMode);
			captureCursorCheckBox.SetDarkMode(darkMode);
			hideTaskbarCheckBox.SetDarkMode(darkMode);
		}

		public void SetAreaWidth(int w)
		{
			if (w > widthNumeric.Maximum)
			{
				widthNumeric.Value = widthNumeric.Maximum;
			}
			else
			{
				if (w < widthNumeric.Minimum)
				{
					widthNumeric.Value = widthNumeric.Minimum;
				}
				else
				{
					widthNumeric.Value = w;
				}
			}
		}

		public void SetAreaHeight(int h)
		{
			if (h > heightNumeric.Maximum)
			{
				heightNumeric.Value = heightNumeric.Maximum;
			}
			else
			{
				if (h < widthNumeric.Minimum)
				{
					heightNumeric.Value = heightNumeric.Minimum;
				}
				else
				{
					heightNumeric.Value = h;
				}
			}
		}

		public void SetAreaX(int x)
		{
			if (x > xNumeric.Maximum)
			{
				xNumeric.Value = xNumeric.Maximum;
			}
			else
			{
				if (x < xNumeric.Minimum)
				{
					xNumeric.Value = xNumeric.Minimum;
				}
				else
				{
					xNumeric.Value = x;
				}
			}
		}

		public void SetAreaY(int y)
		{
			if (y > yNumeric.Maximum)
			{
				yNumeric.Value = yNumeric.Maximum;
			}
			else
			{
				if (y < xNumeric.Minimum)
				{
					yNumeric.Value = yNumeric.Minimum;
				}
				else
				{
					yNumeric.Value = y;
				}
			}
		}

		public void SetMaximumX(int maxX)
		{
			xNumeric.Maximum = maxX;
		}

		public void SetMaximumY(int maxY)
		{
			yNumeric.Maximum = maxY;
		}

		private void recButton_Click(object sender, EventArgs e)
		{
			CheckStartRec();
		}

		public void StopRec()
		{
			try
			{
				HotkeyManager.RegisterHotKey(this.Handle, 0, (int)HotkeyManager.KeyModifier.Alt, Keys.R.GetHashCode());

				if (areaComboBox.SelectedIndex == areaComboBox.Items.Count - 1)
				{
					areaForm.Show();
				}

				recorder.Dispose();
				recorder.OnPeakVolumeChanged -= Recorder_OnPeakVolumeChanged;
				recorder = null;
			}
			catch
			{
				MessageBox.Show("Something went wrong!", "Error");
			}
		}

		private void CheckStartRec()
		{
			string path = folderTextBox.Text + "/" + fileNameTextBox.Text + ".avi";

			if (File.Exists(path))
			{
				DialogResult window = MessageBox.Show(
					fileNameTextBox.Text + ".avi already exists.\nDo you want to replace it and start recording?",
					"Warning",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);

				if (window == DialogResult.Yes)
				{
					StartRec(path);
				}
			}
			else
			{
				StartRec(path);
			}
		}

		private void StartRec(string path)
		{
			try
			{
				int quality = 0;
				int.TryParse(string.Concat(qualityComboBox.Text.Where(char.IsDigit)), out quality);
				int inputSourceIndex = inputDeviceComboBox.SelectedIndex - 2;

				int width = (int)widthNumeric.Value;
				int height = (int)heightNumeric.Value;
				int x = (int)xNumeric.Value;
				int y = (int)yNumeric.Value;

				recorder = new Recorder(path,
					quality, x, y, width, height, captureCursorCheckBox.Checked,
					inputSourceIndex, separateAudioCheckBox.Checked);
				recorder.OnPeakVolumeChanged += Recorder_OnPeakVolumeChanged;

				areaForm.Hide();
				this.Hide();

				HotkeyManager.UnregisterHotKey(this.Handle, 0);

				string videoStr = videoStr = width + "x" + height + " (";
				if (quality == 0)
				{
					videoStr += "Uncompressed";
				}
				else
				{
					videoStr += quality + "%";
				}
				if (captureCursorCheckBox.Checked)
				{
					videoStr += ", Cursor";
				}
				videoStr += ")";

				string audioStr = inputDeviceComboBox.Text;

				stopForm = new StopForm(DateTime.Now, darkMode, videoStr, audioStr);
				stopForm.Owner = this;
				stopForm.Show();
			}
			catch
			{
				MessageBox.Show("Something went wrong!", "Error");
			}
		}

		private void Recorder_OnPeakVolumeChanged(object sender, OnPeakVolumeChangedArgs e)
		{
			if (stopForm != null)
			{
				stopForm.UpdateVolumeBar(e.Volume);
			}
		}

		private void browseFolderBtn_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				folderTextBox.Text = folderBrowserDialog1.SelectedPath;
			}
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
			RefreshAreaInfo();
		}

		private void RefreshAreaInfo()
		{
			if (areaComboBox.SelectedIndex == areaComboBox.Items.Count - 1)
			{
				areaForm.Show();

				// Omit pixels of the red border
				widthNumeric.Value = areaForm.Width - 2;
				heightNumeric.Value = areaForm.Height - 2;
				xNumeric.Value = areaForm.Left + 1;
				yNumeric.Value = areaForm.Top + 1;

				// TODO: Make custom numericBox that doesn't throw an exception
				widthNumeric.Enabled = true;
				heightNumeric.Enabled = true;
				xNumeric.Enabled = true;
				yNumeric.Enabled = true;

				hideTaskbarCheckBox.Enabled = false;
			}
			else
			{
				areaForm.Hide();

				widthNumeric.Enabled = false;
				heightNumeric.Enabled = false;
				xNumeric.Enabled = false;
				yNumeric.Enabled = false;

				hideTaskbarCheckBox.Enabled = true;

				if (Screen.AllScreens.Length > 1)
				{
					if (areaComboBox.SelectedIndex == areaComboBox.Items.Count - 2)
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
				try
				{
					areaForm.Width = (int)widthNumeric.Value + 2;
				}
				catch (InvalidOperationException)
				{
					// TODO: Handle
				}
			}
		}

		private void heightNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (heightNumeric.Enabled)
			{
				try
				{
					areaForm.Height = (int)heightNumeric.Value + 2;
				}
				catch (InvalidOperationException)
				{
					// TODO: Handle
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			HotkeyManager.RegisterHotKey(this.Handle, 0, (int)HotkeyManager.KeyModifier.Alt, Keys.R.GetHashCode());

			onTopBtn.Checked = Properties.Settings.Default.AlwaysOnTop;
			qualityComboBox.SelectedIndex = Properties.Settings.Default.QualityIndex;
			captureCursorCheckBox.Checked = Properties.Settings.Default.CaptureCursor;
			hideTaskbarCheckBox.Checked = Properties.Settings.Default.HideTaskbar;

			if (Properties.Settings.Default.Folder == string.Empty)
			{
				folderTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
			else
			{
				folderTextBox.Text = Properties.Settings.Default.Folder;
			}

			ScreenCaptureWorker.Initialize(widthNumeric, heightNumeric, xNumeric, yNumeric, captureCursorCheckBox);
			ScreenCaptureWorker.StartWorkers(TARGET_FRAMERATE, 2);

			Thread drawThread = new Thread(() =>
			{
				Stopwatch stopwatch = new Stopwatch();
				const int INTERVAL_MS = 1000 / TARGET_FRAMERATE;

				while (true)
				{
					stopwatch.Restart();
					try
					{
						Invoke(new Action(() =>
						{
							if (previewBox.Image != null)
							{
								previewBox.Image.Dispose();
							}
							previewBox.Image = ScreenCaptureWorker.GetNextFrame();
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


			//if (Properties.Settings.Default.CheckForUpdates)
			//{
			//	UpdateManager.checkForUpdates(false, darkMode, this.TopMost, "ModuleArt", "quick-screen-recorder", "Quick Screen Recorder", "QuickScreenRecorder-Setup.msi");
			//}
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg == 0x0312)
			{
				Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
				HotkeyManager.KeyModifier modifier = (HotkeyManager.KeyModifier)((int)m.LParam & 0xFFFF);

				if (modifier == HotkeyManager.KeyModifier.Alt)
				{
					if (key == Keys.R)
					{
						CheckStartRec();
					}
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			ScreenCaptureWorker.StopWorkers();
			if (audioPlayback != null) audioPlayback.Stop();
			HotkeyManager.UnregisterHotKey(this.Handle, 0);
		}

		private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.TopMost = onTopBtn.Checked;
			areaForm.TopMost = onTopBtn.Checked;
			Properties.Settings.Default.AlwaysOnTop = onTopBtn.Checked;
			Properties.Settings.Default.Save();
		}

		public void MuteRecorder(bool b)
		{
			recorder.Mute = b;
		}

		private void refreshBtn_Click(object sender, EventArgs e)
		{
			RefreshAudioDevices();
		}

		private void RefreshAudioDevices()
		{
			inputDeviceComboBox.SelectedIndex = -1;
			inputDeviceComboBox.Items.Clear();

			MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
			audioDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
			
			foreach (MMDevice device in audioDevices)
			{
				inputDeviceComboBox.Items.Add(device.DeviceFriendlyName);
			}

			//while (inputDeviceComboBox.Items.Count > 2)
			//{
			//    inputDeviceComboBox.Items.RemoveAt(inputDeviceComboBox.Items.Count - 1);
			//}

			//for (int i = 0; i < WaveIn.DeviceCount; i++)
			//{
			//    inputDeviceComboBox.Items.Add(WaveIn.GetCapabilities(i).ProductName);
			//}
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

			areaComboBox.Items.Add("Custom area");

			areaComboBox.SelectedIndex = 0;

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

		private void inputDeviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			separateAudioCheckBox.Enabled = inputDeviceComboBox.SelectedIndex != 0;
		}

		private void xNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (xNumeric.Enabled)
			{
				try
				{
					// Omit 1 pixel for red border
					areaForm.Left = (int)xNumeric.Value - 1;
				}
				catch (InvalidOperationException)
				{
					// TODO: Handle
				}
			}
		}

		private void yNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (yNumeric.Enabled)
			{
				try
				{
					// Omit 1 pixel for red border
					areaForm.Top = (int)yNumeric.Value - 1;
				}
				catch (InvalidOperationException)
				{
					// TODO: Handle
				}
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

		private void qualityComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.QualityIndex = qualityComboBox.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		private void enabledPreview(bool b)
		{
			if (previewBtn.Checked)
			{
				this.Width = 820;
			}
			else
			{
				this.Width = 374;

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

			enabledPreview(previewBtn.Checked);
		}

		private void folderTextBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void folderTextBox_DragDrop(object sender, DragEventArgs e)
		{
			string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			FileAttributes attr = File.GetAttributes(FileList[0]);

			if (attr.HasFlag(FileAttributes.Directory))
			{
				folderTextBox.Text = FileList[0];
			}
			else
			{
				folderTextBox.Text = Path.GetDirectoryName(FileList[0]);
			}
		}

		private void folderTextBox_TextChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Folder = folderTextBox.Text;
			Properties.Settings.Default.Save();
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
			int width = (int)widthNumeric.Value;
			int height = (int)heightNumeric.Value;

			if (inputDeviceComboBox.SelectedIndex == -1)
			{
				DialogResult r = MessageBox.Show("No audio source selected, continue anyways?", "Warning",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (r == DialogResult.No)
					return;
			}
			else
			{
				MMDevice selectedDevice = audioDevices[inputDeviceComboBox.SelectedIndex];
				audioPlayback = new AudioPlayback(selectedDevice);
				audioPlayback.Start();
			}

			previewBox.Location = new Point(0, 0);
			videoGroup.Visible = false;
			audioGroup.Visible = false;
			startButton.Visible = false;
			toolStrip.Visible = false;
			streamEnabled = true;

			SetPreviewSize(new Size(width, height));
		}

		public void SetPreviewSize(Size size)
		{
			if (streamEnabled)
			{
				size.Width = Math.Max(160, size.Width);
				size.Height = Math.Max(160, size.Height);
				Size = size;
				BeginInvoke(new Action(() =>
				{
					previewBox.Size = size;
				}));
			}
		}
	}
}
