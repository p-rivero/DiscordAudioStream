using DiscordAudioStream.AudioCapture;
using DiscordAudioStream.ScreenCapture;
using DLLs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DiscordAudioStream
{
	internal class MainController
	{
		private readonly MainForm form;

		private bool streamEnabled = false;
		private bool forceRefresh = false;
		private double scaleMultiplier = 1;
		private int numberOfScreens = -1;

		private ScreenCaptureManager screenCapture;
		private ProcessHandleList processHandleList;
		private readonly CaptureState captureState = new CaptureState();

		private AudioPlayback audioPlayback = null;
		private AudioMeterForm currentMeterForm = null;


		public MainController(MainForm owner)
		{
			form = owner;
			processHandleList = ProcessHandleList.Refresh();
		}

		public bool IsStreaming { get => streamEnabled; }

		internal void Init()
		{
			RefreshAreaInfo();

			captureState.HideTaskbar = Properties.Settings.Default.HideTaskbar;
			captureState.CapturingCursor = Properties.Settings.Default.CaptureCursor;

			screenCapture = new ScreenCaptureManager(captureState);
			screenCapture.CaptureAborted += AbortCapture;

			Thread drawThread = CreateDrawThread();
			drawThread.Start();
		}

		// Called when the X button is pressed
		internal bool Stop()
		{
			bool cancel = false;
			if (streamEnabled)
			{
				cancel = true; // Do not close form
				// Instead, return to settings
				EndStream();
			}
			else
			{
				Logger.Log("\nClose button pressed, stopping program.");
				screenCapture?.Stop();
				User32.UnregisterHotKey(form.Handle, 0);
			}
			return cancel;
		}


		// PRIVATE METHODS

		private void SetPreviewSize(Size size)
		{
			if (streamEnabled)
			{
				size.Width = (int)(Math.Max(160, size.Width) * scaleMultiplier);
				size.Height = (int)(Math.Max(160, size.Height) * scaleMultiplier);
				form.SetPreviewUISize(size);
			}
		}

		private void RefreshAreaInfo()
		{
			if (!form.Created) return;

			int windowIndex = form.VideoIndex - numberOfScreens - 1;

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
				if (Screen.AllScreens.Length > 1 && form.VideoIndex == numberOfScreens - 1)
				{
					// All screens
					captureState.Target = CaptureState.CaptureTarget.AllScreens;
				}
				else
				{
					// Single screen
					captureState.Screen = Screen.AllScreens[form.VideoIndex];
				}
			}

			form.HideTaskbarEnabled = captureState.HideTaskbarSupported;
			if (captureState.HideTaskbarSupported)
			{
				// The selected method allows hiding taskbar, see if checkbox is checked
				captureState.HideTaskbar = form.HideTaskbar;
			}
		}

		private Thread CreateDrawThread()
		{
			// Get the handle now, since we cannot get it from inside the thread
			IntPtr formHandle = form.Handle;
			Thread newThread = new Thread(() =>
			{
				Logger.Log("\nCreating Draw thread. Target framerate: {0} FPS ({1} ms)", 
					Properties.Settings.Default.CaptureFramerate, screenCapture.CaptureIntervalMs);

				Stopwatch stopwatch = new Stopwatch();
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
						// Refresh if the stream has started and "Force screen redraw" is enabled
						form.UpdatePreview(next, streamEnabled && forceRefresh, formHandle);
					}
					catch (InvalidOperationException)
					{
						// Form is closing
						Logger.Log("Form is closing, stop Draw thread.");
						return;
					}
					stopwatch.Stop();

					int wait = screenCapture.CaptureIntervalMs - (int)stopwatch.ElapsedMilliseconds;
					if (wait > 0)
					{
						Thread.Sleep(wait);
					}
				}
			});
			newThread.IsBackground = true;
			newThread.Name = "Draw Thread";
			return newThread;
		}



		// INTERNAL METHODS (called from MainForm)

		internal void UpdateAreaComboBox(int oldIndex)
		{
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
					form.VideoIndex = Properties.Settings.Default.AreaIndex;
				}
				else
				{
					// Window still exists
					form.VideoIndex = windowIndex + numberOfScreens + 1;
				}
			}
			else
			{
				// We were capturing a screen
				form.VideoIndex = oldIndex;
			}
		}

		internal void RefreshScreens()
		{
			List<string> elements = new List<string>();

			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				Rectangle bounds = Screen.AllScreens[i].Bounds;
				if (Screen.AllScreens[i].Primary)
				{
					elements.Add("Primary screen (" + bounds.Width + "x" + bounds.Height + ")");
				}
				else
				{
					elements.Add("Screen " + (i + 1) + " (" + bounds.Width + "x" + bounds.Height + ")");
				}
			}
			if (Screen.AllScreens.Length > 1)
			{
				elements.Add("Everything");
			}

			numberOfScreens = elements.Count;

			// Item with separator
			elements.Add(null);
			elements.Add("Custom area");

			processHandleList = ProcessHandleList.Refresh();
			foreach (string window in processHandleList.Names)
			{
				elements.Add(window);
			}
			
			form.SetVideoItems(elements);
		}

		internal void RefreshAudioDevices()
		{
			List<string> elements = new List<string>();

			elements.Add("(None)");

			foreach (string device in AudioPlayback.RefreshDevices())
			{
				elements.Add(device);
			}

			int defaultIndex = AudioPlayback.GetLastDeviceIndex() + 1; // Add 1 for "None" element
			form.SetAudioElements(elements, defaultIndex);
		}


		internal void StartStream()
		{
			if (form.AudioIndex == 0)
			{
				// No audio device selected, show warning
				DialogResult r = MessageBox.Show(
					"No audio source selected, continue anyways?",
					"Warning",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,
					// The second button ("No") is the default option
					MessageBoxDefaultButton.Button2
				);

				if (r == DialogResult.No)
					return;

				Logger.Log("\nSTART STREAM (Without audio)");
				// Clear the stored last used audio device
				Properties.Settings.Default.AudioDeviceID = "";
				Properties.Settings.Default.Save();
			}
			else
			{
				int deviceIndex = form.AudioIndex - 1;
				if (deviceIndex == AudioPlayback.GetDefaultDeviceIndex())
				{
					// No audio device selected, show warning
					DialogResult r = MessageBox.Show(
						"The captured audio device is the same as the output device of DiscordAudioStream.\n" +
						"This will cause an audio loop, which may result in echo or very loud sounds. Continue anyways?",
						"Warning",
						MessageBoxButtons.OKCancel,
						MessageBoxIcon.Warning,
						// The second button ("Cancel") is the default option
						MessageBoxDefaultButton.Button2
					);

					if (r == DialogResult.Cancel)
						return;

					Logger.Log("\nDEFAULT DEVICE CAPTURED (Audio loop)");
				}
				
				Logger.Log("\nSTART STREAM (With audio)");
				// Skip "None"
				audioPlayback = new AudioPlayback(deviceIndex);
				try
				{
					audioPlayback.AudioLevelChanged += AudioPlayback_AudioLevelChanged;
					audioPlayback.Start();
				}
				catch (InvalidOperationException e)
				{
					MessageBox.Show(
						e.Message,
						"Unable to capture the audio device",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error,
						MessageBoxDefaultButton.Button1
					);
					return;
				}
			}

			form.EnableStreamingUI(true);
			// Reading Properties.Settings can be slow, set flag once at the start of the stream
			forceRefresh = Properties.Settings.Default.OffscreenDraw;
			Logger.Log("Force screen redraw: " + forceRefresh);
			streamEnabled = true;

			// Show preview at full size
			SetPreviewSize(form.TruePreviewSize);
		}

		private void EndStream()
		{
			Logger.Log("\nEND STREAM");
			form.EnableStreamingUI(false);
			streamEnabled = false;
			if (audioPlayback != null)
			{
				audioPlayback.Stop();
			}
		}


		private void AbortCapture()
		{
			form.Invoke(new Action(() =>
			{
				RefreshScreens();
				form.VideoIndex = Properties.Settings.Default.AreaIndex;
				if (!streamEnabled) return;

				EndStream();
				if (Properties.Settings.Default.AutoExit)
				{
					Logger.Log("\nAutoExit was enabled, closing form.");
					form.Close();
				}
			}));
		}

		internal void ShowSettingsForm(bool darkMode)
		{
			SettingsForm settingsBox = new SettingsForm(darkMode, captureState);
			settingsBox.Owner = form;
			settingsBox.CaptureMethodChanged += RefreshAreaInfo;
			settingsBox.FramerateChanged += screenCapture.RefreshFramerate;
			settingsBox.ShowAudioInputsChanged += RefreshAudioDevices;
			if (form.TopMost) settingsBox.TopMost = true;
			settingsBox.ShowDialog();
		}

		internal void ShowAboutForm(bool darkMode)
		{
			AboutForm aboutBox = new AboutForm(darkMode);
			aboutBox.Owner = form;
			if (form.TopMost) aboutBox.TopMost = true;
			aboutBox.ShowDialog();
		}
		
		internal void ShowAudioMeterForm(bool darkMode)
		{
			if (currentMeterForm == null || currentMeterForm.IsDisposed)
			{
				currentMeterForm = new AudioMeterForm(darkMode);
				currentMeterForm.Owner = form;
			}
			if (form.TopMost) currentMeterForm.TopMost = true;
			currentMeterForm.Show();
			form.Focus();
		}
		internal void HideAudioMeterForm()
		{
			if (currentMeterForm != null && !currentMeterForm.IsDisposed)
			{
				currentMeterForm.Hide();
			}
		}
		private void AudioPlayback_AudioLevelChanged(float left, float right)
		{
			if (currentMeterForm != null && !currentMeterForm.IsDisposed)
			{
				currentMeterForm.SetLevels(left, right);
			}
		}

		internal void SetVideoIndex(int index)
		{
			if (numberOfScreens == -1) return;

			RefreshAreaInfo();

			// Do not save settings for Windows (only screen or Custom area)
			if (index <= numberOfScreens)
			{
				Properties.Settings.Default.AreaIndex = index;
				Properties.Settings.Default.Save();
			}
		}

		internal void SetScaleIndex(int index)
		{
			if (index == 0) scaleMultiplier = 1;              // Original resolution
			else if (index == 1) scaleMultiplier = Math.Sqrt(0.50); // 50% resolution
			else if (index == 2) scaleMultiplier = Math.Sqrt(0.25); // 25% resolution
			else throw new ArgumentException("Invalid index");

			Properties.Settings.Default.ScaleIndex = index;
			Properties.Settings.Default.Save();
		}

		internal void SetHideTaskbar(bool hideTaskbar)
		{
			captureState.HideTaskbar = hideTaskbar;
			Properties.Settings.Default.HideTaskbar = hideTaskbar;
			Properties.Settings.Default.Save();
		}

		internal void SetCapturingCursor(bool capturing)
		{
			captureState.CapturingCursor = capturing;
			Properties.Settings.Default.CaptureCursor = capturing;
			Properties.Settings.Default.Save();
		}
	}
}
