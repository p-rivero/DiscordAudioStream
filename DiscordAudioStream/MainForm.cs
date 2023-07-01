using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using CustomComponents;
using DLLs;
using System.Collections.Generic;

namespace DiscordAudioStream
{
	public partial class MainForm : Form
	{
		private readonly MainController controller;

		private readonly bool darkMode;
		private readonly Size defaultWindowSize;
		private readonly Size defaultPreviewSize;
		private readonly Point defaultPreviewLocation;
		
		public MainForm(bool darkMode)
		{
			Logger.Log("\nInitializing MainForm. darkMode={0}", darkMode);

			controller = new MainController(this);

			this.darkMode = darkMode;
			if (darkMode) HandleCreated += new EventHandler(DarkThemeManager.FormHandleCreated);

			InitializeComponent();
			previewBox.Visible = true;

			defaultWindowSize = this.Size;
			defaultPreviewSize = previewBox.Size;
			defaultPreviewLocation = previewBox.Location;

			inputDeviceComboBox.SelectedIndex = 0;

			controller.RefreshScreens();
			controller.RefreshAudioDevices();
			try
			{
				VideoIndex = Properties.Settings.Default.AreaIndex;
			}
			catch (ArgumentOutOfRangeException)
			{
				// Number of screens may have changed
				Logger.Log("ArgumentOutOfRangeException caught, number of screens may have changed.");
				VideoIndex = 0;
			}

			previewBtn.Checked = Properties.Settings.Default.Preview;
			DisplayPreview(previewBtn.Checked);

			VideoIndex = Properties.Settings.Default.AreaIndex;
			scaleComboBox.SelectedIndex = Math.Min(Properties.Settings.Default.ScaleIndex, scaleComboBox.Items.Count - 1);

			ApplyDarkTheme(darkMode);

			toolTip.SetToolTip(captureCursorCheckBox, Properties.Resources.Tooltip_CaptureCursor);
			toolTip.SetToolTip(hideTaskbarCheckBox, Properties.Resources.Tooltip_HideTaskbar);
			toolTip.SetToolTip(areaComboBox, Properties.Resources.Tooltip_CaptureArea);
			toolTip.SetToolTip(areaLabel, Properties.Resources.Tooltip_CaptureArea);
			toolTip.SetToolTip(scaleComboBox, Properties.Resources.Tooltip_VideoScale);
			toolTip.SetToolTip(scaleLabel, Properties.Resources.Tooltip_VideoScale);
			toolTip.SetToolTip(inputDeviceComboBox, Properties.Resources.Tooltip_AudioSource);
			toolTip.SetToolTip(inputDeviceLabel, Properties.Resources.Tooltip_AudioSource);
			toolTip.SetToolTip(startButton, Properties.Resources.Tooltip_StartStream);
		}

		public MainController Controller => controller;

		// INTERNAL METHODS (called from controller)

		internal int VideoIndex
		{
			get { return areaComboBox.SelectedIndex; }
			set { areaComboBox.SelectedIndex = value; }
		}

		internal void SetVideoItems(IEnumerable<string> items)
		{
			areaComboBox.Items.Clear();
			bool addSeparator = false;
			int numSeparators = 0;
			foreach (string item in items)
			{
				// Use null to indicate that the next element has a separator
				if (item == null)
				{
					addSeparator = true;
				}
				else if (addSeparator)
				{
					areaComboBox.Items.Add(new DarkThemeComboBox.ItemWithSeparator("Custom area"));
					numSeparators++;
					addSeparator = false;
				}
				else
				{
					areaComboBox.Items.Add(item);
				}
			}
			// For each separator, we need to add a dummy element at the end
			for (int i = 0; i < numSeparators; i++)
			{
				areaComboBox.Items.Add(new DarkThemeComboBox.Dummy());
			}
		}

		internal bool HasSomeAudioSource
		{
			get => inputDeviceComboBox.SelectedIndex > 0;
		}
		internal int AudioSourceIndex
		{
			get => inputDeviceComboBox.SelectedIndex - 1;
		}

		internal void SetAudioElements(IEnumerable<string> elements, int defaultIndex)
		{
			inputDeviceComboBox.Items.Clear();

			foreach (string element in elements)
			{
				inputDeviceComboBox.Items.Add(element);
			}

			inputDeviceComboBox.SelectedIndex = defaultIndex;
		}

		internal bool HideTaskbar
		{
			get { return hideTaskbarCheckBox.Checked; }
		}
		internal bool HideTaskbarEnabled
		{
			get { return hideTaskbarCheckBox.Enabled; }
			set
			{
				hideTaskbarCheckBox.Enabled = value;
				// When disabling the checkbox, also set its state to unchecked
				if (!value) hideTaskbarCheckBox.Checked = false;
			}
		}


		internal void SetPreviewUISize(Size newSize)
		{
			BeginInvoke(new Action(() => previewBox.Size = newSize));
		}

		internal void EnableStreamingUI(bool streaming)
		{
			// Autosize the form only while streaming
			this.AutoSize = streaming;

			// Disable UI elements while streaming
			videoGroup.Visible = !streaming;
			audioGroup.Visible = !streaming;
			startButton.Visible = !streaming;
			toolStrip.Visible = !streaming;

			if (streaming)
			{
				// If we start streaming, override previewBtn and enable the previewBox
				DisplayPreview(true);
				previewBox.Location = new Point(0, 0);
				controller.ShowAudioMeterForm(darkMode);
				this.Text = Properties.Settings.Default.StreamTitle;
				previewBox.ContextMenuStrip = streamContextMenu;
				
				showAudioMeterToolStripMenuItem.Enabled = HasSomeAudioSource;
				showAudioMeterToolStripMenuItem.Checked = HasSomeAudioSource && Properties.Settings.Default.ShowAudioMeter;
			}
			else
			{
				DisplayPreview(previewBtn.Checked);
				previewBox.Size = defaultPreviewSize;
				previewBox.Location = defaultPreviewLocation;
				CenterToScreen();
				controller.HideAudioMeterForm();
				this.Text = "Discord Audio Stream";
				previewBox.ContextMenuStrip = null;
			}
		}

		internal void UpdatePreview(Bitmap newImage, bool forceRefresh, IntPtr handle)
		{
			// This method is called in a worker thread, redraw the previewBox in the UI thread.
			// If needed, triger a full redraw of the form, but do it in the worker thread to reduce
			// the load on the UI thread.
			Invoke(new Action(() =>
			{
				if (IsDisposed)
				{
					Logger.Log("Attempting to update preview after disposing: ignore");
					return;
				}
				previewBox.Image?.Dispose();
				previewBox.Image = newImage;
			}));
			if (forceRefresh)
			{
				// Windows only refreshes the part of the window that is shown to the user. Therefore, if this
				// window is partially off-screen, it won't be streamed correctly in Discord.
				// Use PrintWindow to send a WM_PRINT to our own window handle, forcing a complete redraw.
				User32.PrintWindow(handle, IntPtr.Zero, 0);
			}
		}

		internal void AudioMeterClosed()
		{
			showAudioMeterToolStripMenuItem.Checked = false;
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

		private void DisplayPreview(bool visible)
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
			controller.Init();
			onTopBtn.Checked = Properties.Settings.Default.AlwaysOnTop;
			captureCursorCheckBox.Checked = Properties.Settings.Default.CaptureCursor;
			hideTaskbarCheckBox.Checked = Properties.Settings.Default.HideTaskbar;
			
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (e.CloseReason == CloseReason.WindowsShutDown) return;

			// MainController.Stop() returns false if the form has to be closed
			e.Cancel = controller.Stop();
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				switch (e.KeyCode)
				{
				case Keys.P:
					previewBtn.PerformClick();
					break;
				case Keys.T:
					onTopBtn.PerformClick();
					break;
				case Keys.Oemcomma:
					settingsBtn.PerformClick();
					break;
				case Keys.V:
					volumeMixerButton.PerformClick();
					break;
				case Keys.A:
					soundDevicesButton.PerformClick();
					break;
				case Keys.Enter:
					if (!controller.IsStreaming) controller.StartStream(false);
					break;
				}
			}
			else
			{
				switch (e.KeyCode)
				{
				case Keys.F1:
					aboutBtn.PerformClick();
					break;
				case Keys.Escape:
					if (controller.IsStreaming) controller.Stop();
					break;
				}
			}
		}


		private void previewBtn_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Preview = previewBtn.Checked;
			Properties.Settings.Default.Save();

			DisplayPreview(previewBtn.Checked);
		}

		private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.AlwaysOnTop = onTopBtn.Checked;
			Properties.Settings.Default.Save();

			TopMost = onTopBtn.Checked;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S4036", Justification = "This ms-settings URI is safe")]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S4036", Justification = "This ms-settings URI is safe")]
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
			controller.ShowSettingsForm(darkMode);
		}

		private void aboutBtn_Click(object sender, EventArgs e)
		{
			controller.ShowAboutForm(darkMode);
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			controller.StartStream(false);
		}

		private void areaComboBox_DropDown(object sender, EventArgs e)
		{
			// When the user expands the area combobox, update its elements
			controller.UpdateAreaComboBox(VideoIndex);
		}

		private void areaComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Do not select the dummy object at the end of the list
			if (VideoIndex == areaComboBox.Items.Count - 1)
			{
				VideoIndex = areaComboBox.Items.Count - 2;
			}
			controller.SetVideoIndex(VideoIndex);
		}

		private void scaleComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			controller.SetScaleIndex(scaleComboBox.SelectedIndex);
		}

		private void hideTaskbarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			controller.SetHideTaskbar(hideTaskbarCheckBox.Checked);
		}

		private void captureCursorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			controller.SetCapturingCursor(captureCursorCheckBox.Checked);
		}

		private void showAudioMeterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool show = !showAudioMeterToolStripMenuItem.Checked;

			showAudioMeterToolStripMenuItem.Checked = show;
			Properties.Settings.Default.ShowAudioMeter = show;
			Properties.Settings.Default.Save();

			if (show) controller.ShowAudioMeterForm(darkMode);
			else controller.HideAudioMeterForm();
		}

		private void stopStreamToolStripMenuItem_Click(object sender, EventArgs e)
		{
			controller.Stop();
		}
	}
}
