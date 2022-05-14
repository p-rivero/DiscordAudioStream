using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using CustomComponents;
using DiscordAudioStream.ScreenCapture;
using DLLs;

namespace DiscordAudioStream
{
	public partial class MainForm : Form
	{
		private readonly bool darkMode;
		private readonly Size defaultWindowSize;
		private readonly Size defaultPreviewSize;
		private readonly Point defaultPreviewLocation;
		private bool streamEnabled = false;

		private double scaleMultiplier = 1;
		private int numberOfScreens = -1;

		private ScreenCaptureManager screenCapture;
		private ProcessHandleList processHandleList;
		private readonly CaptureState captureState = new CaptureState();

		private AudioPlayback audioPlayback = null;
		
		public MainForm(bool darkMode)
		{
			if (darkMode)
			{
				this.HandleCreated += new EventHandler(DarkThemeManager.FormHandleCreated);
			}

			this.darkMode = darkMode;
			processHandleList = ProcessHandleList.Refresh();

			InitializeComponent();
			previewBox.Visible = true;

			defaultWindowSize = this.Size;
			defaultPreviewSize = previewBox.Size;
			defaultPreviewLocation = previewBox.Location;

			inputDeviceComboBox.SelectedIndex = 0;

			RefreshScreens();
			RefreshAudioDevices();
			try
			{
				areaComboBox.SelectedIndex = Properties.Settings.Default.AreaIndex;
			}
			catch (ArgumentOutOfRangeException)
			{
				// Number of screen may have changed
				areaComboBox.SelectedIndex = 0;
			}

			previewBtn.Checked = Properties.Settings.Default.Preview;
			EnablePreview(previewBtn.Checked);

			areaComboBox.SelectedIndex = Properties.Settings.Default.AreaIndex;
			scaleComboBox.SelectedIndex = Properties.Settings.Default.ScaleIndex;

			ApplyDarkTheme(darkMode);
		}


		// PUBLIC METHODS

		public void SetPreviewSize(Size size)
		{
			if (streamEnabled)
			{
				size.Width = (int)(Math.Max(160, size.Width) * scaleMultiplier);
				size.Height = (int)(Math.Max(160, size.Height) * scaleMultiplier);
				BeginInvoke(new Action(() => previewBox.Size = size));
			}
		}

		public void UpdateAreaComboBox()
		{
			int oldIndex = areaComboBox.SelectedIndex;
			IntPtr oldHandle = IntPtr.Zero;
			if (oldIndex > numberOfScreens)
			{
				// We were capturing a window, store its handle
				int windowIndex = oldIndex - numberOfScreens - 1;
				oldHandle = processHandleList[windowIndex];
			}

			// Refresh list of windows
			RefreshScreens();

			if (oldIndex > numberOfScreens)
			{
				// We were capturing a window, see if it still exists
				int windowIndex = processHandleList.IndexOf(oldHandle);
				if (windowIndex == -1)
				{
					// Window has been closed, return to last saved screen
					areaComboBox.SelectedIndex = Properties.Settings.Default.AreaIndex;
				}
				else
				{
					// Window still exists
					areaComboBox.SelectedIndex = windowIndex + numberOfScreens + 1;
				}
			}
			else
			{
				// We were capturing a screen
				areaComboBox.SelectedIndex = oldIndex;
			}
		}



		// PRIVATE METHODS

		private void ApplyDarkTheme(bool darkMode)
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
			}

			videoGroup.SetDarkMode(darkMode);
			audioGroup.SetDarkMode(darkMode);
			toolStrip.SetDarkMode(darkMode, false);
			inputDeviceComboBox.SetDarkMode(darkMode);
			areaComboBox.SetDarkMode(darkMode);
			scaleComboBox.SetDarkMode(darkMode);
			captureCursorCheckBox.SetDarkMode(darkMode);
			hideTaskbarCheckBox.SetDarkMode(darkMode);
		}

		private void RefreshAreaInfo()
		{
			if (!Created) return;

			int windowIndex = areaComboBox.SelectedIndex - numberOfScreens - 1;

			if (windowIndex == -1)
			{
				// Index right before first Window: Custom area
				captureState.Target = CaptureState.CaptureTarget.CustomArea;
			}
			else if (windowIndex >= 0)
			{
				// Window
				captureState.WindowHandle = processHandleList[windowIndex];
			}
			else
			{
				// Screen
				if (Screen.AllScreens.Length > 1 && areaComboBox.SelectedIndex == numberOfScreens - 1)
				{
					// All screens
					captureState.Target = CaptureState.CaptureTarget.AllScreens;
				}
				else
				{
					// Single screen
					captureState.Screen = Screen.AllScreens[areaComboBox.SelectedIndex];
				}
			}

			hideTaskbarCheckBox.Enabled = captureState.HideTaskbarSupported;
			if (captureState.HideTaskbarSupported)
			{
				captureState.HideTaskbar = hideTaskbarCheckBox.Checked;
			}
		}

		private void RefreshAudioDevices()
		{
			inputDeviceComboBox.Items.Clear();
			inputDeviceComboBox.Items.Add("(None)");

			foreach (string device in AudioPlayback.RefreshDevices())
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
				Rectangle bounds = Screen.AllScreens[i].Bounds;
				if (Screen.AllScreens[i].Primary)
				{
					areaComboBox.Items.Add("Primary screen (" + bounds.Width + "x" + bounds.Height + ")");
				}
				else
				{
					areaComboBox.Items.Add("Screen " + (i + 1) + " (" + bounds.Width + "x" + bounds.Height + ")");
				}
			}
			if (Screen.AllScreens.Length > 1)
			{
				areaComboBox.Items.Add("Everything");
			}

			numberOfScreens = areaComboBox.Items.Count;

			areaComboBox.Items.Add(new DarkThemeComboBox.ItemWithSeparator("Custom area"));

			processHandleList = ProcessHandleList.Refresh();
			foreach (string window in processHandleList.Names)
			{
				areaComboBox.Items.Add(window);
			}
			areaComboBox.Items.Add(new DarkThemeComboBox.Dummy());
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

			EnablePreview(true);
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

			EnablePreview(previewBtn.Checked);
			CenterToScreen();
		}

		private void AbortCapture()
		{
			Invoke(new Action(() =>
			{
				RefreshScreens();
				areaComboBox.SelectedIndex = Properties.Settings.Default.AreaIndex;
				if (!streamEnabled) return;

				EndStream();
				if (Properties.Settings.Default.AutoExit) Close();
			}));
		}

		private void EnablePreview(bool visible)
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




		// EVENTS

		private void MainForm_Load(object sender, EventArgs e)
		{
			onTopBtn.Checked = Properties.Settings.Default.AlwaysOnTop;
			captureCursorCheckBox.Checked = Properties.Settings.Default.CaptureCursor;
			hideTaskbarCheckBox.Checked = Properties.Settings.Default.HideTaskbar;
			RefreshAreaInfo();

			captureState.HideTaskbar = hideTaskbarCheckBox.Checked;
			captureState.CapturingCursor = captureCursorCheckBox.Checked;

			screenCapture = new ScreenCaptureManager(captureState);
			screenCapture.CaptureAborted += AbortCapture;

			Thread drawThread = new Thread(() =>
			{
				Stopwatch stopwatch = new Stopwatch();
				const int INTERVAL_MS = 1000 / ScreenCaptureManager.TARGET_FRAMERATE;
				Size oldSize = new Size(0, 0);

				while (true)
				{
					stopwatch.Restart();
					try
					{
						Bitmap next = ScreenCaptureManager.GetNextFrame();

						// Continue iterating until GetNextFrame() doesn't return null
						if (next == null) continue;

						// Detect size changes
						if (next.Size != oldSize)
						{
							oldSize = next.Size;
							SetPreviewSize(next.Size);
						}

						// Display captured frame
						Invoke(new Action(() =>
						{
							previewBox.Image?.Dispose();
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
				screenCapture?.Stop();
				User32.UnregisterHotKey(this.Handle, 0);
			}
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


		private void previewBtn_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Preview = previewBtn.Checked;
			Properties.Settings.Default.Save();

			EnablePreview(previewBtn.Checked);
		}

		private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.AlwaysOnTop = onTopBtn.Checked;
			Properties.Settings.Default.Save();

			TopMost = onTopBtn.Checked;
		}

		private void volumeMixerButton_Click(object sender, EventArgs e)
		{
			var osVersionInfo = Ntdll.OsVersionInfoEx.Init();
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
			var osVersionInfo = Ntdll.OsVersionInfoEx.Init();
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

		private void settingsBtn_Click(object sender, EventArgs e)
		{
			SettingsForm settingsBox = new SettingsForm(darkMode, captureState);
			settingsBox.Owner = this;
			settingsBox.CaptureMethodChanged += RefreshAreaInfo;

			// If this window is topmost, settings is too
			if (TopMost) settingsBox.TopMost = true;
			
			settingsBox.ShowDialog();
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

		private void startButton_Click(object sender, EventArgs e)
		{
			StartStream();
		}



		private void areaComboBox_DropDown(object sender, EventArgs e)
		{
			UpdateAreaComboBox();
		}

		private void areaComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (numberOfScreens == -1)
				return;

			// Do not select the dummy object at the end of the list
			if (areaComboBox.SelectedIndex == areaComboBox.Items.Count - 1)
			{
				areaComboBox.SelectedIndex = areaComboBox.Items.Count - 2;
			}

			RefreshAreaInfo();
			// Do not save settings for Custom Area or Window
			if (areaComboBox.SelectedIndex >= numberOfScreens)
				return;

			Properties.Settings.Default.AreaIndex = areaComboBox.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		private void scaleComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = scaleComboBox.SelectedIndex;
			if (index == 0) scaleMultiplier = 1;              // Original resolution
			else if (index == 1) scaleMultiplier = Math.Sqrt(0.50); // 50% resolution
			else if (index == 2) scaleMultiplier = Math.Sqrt(0.25); // 25% resolution
			else throw new ArgumentException("Invalid index");

			Properties.Settings.Default.ScaleIndex = index;
			Properties.Settings.Default.Save();
		}

		private void hideTaskbarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			RefreshAreaInfo();
			Properties.Settings.Default.HideTaskbar = hideTaskbarCheckBox.Checked;
			Properties.Settings.Default.Save();
		}

		private void captureCursorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			captureState.CapturingCursor = captureCursorCheckBox.Checked;

			Properties.Settings.Default.CaptureCursor = captureCursorCheckBox.Checked;
			Properties.Settings.Default.Save();
		}

		private void refreshBtn_Click(object sender, EventArgs e)
		{
			RefreshAudioDevices();
		}
	}
}
