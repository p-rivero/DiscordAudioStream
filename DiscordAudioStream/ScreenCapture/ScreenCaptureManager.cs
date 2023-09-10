using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DiscordAudioStream.ScreenCapture.CaptureStrategy;

namespace DiscordAudioStream.ScreenCapture
{
	public class ScreenCaptureManager
	{
		public delegate void CaptureAbortDelegate();
		public event CaptureAbortDelegate CaptureAborted;

		// Control framerate limit
		public int CaptureIntervalMs { get; private set; } = 0;

		// Number of frames that are stored in the queue
		private const int LIMIT_QUEUE_SZ = 3;

		private readonly Thread captureThread;
		private readonly CaptureState captureState;
		private static readonly ConcurrentQueue<Bitmap> frameQueue = new ConcurrentQueue<Bitmap>();
		private CaptureSource currentSource = null;

		public ScreenCaptureManager(CaptureState captureState)
		{
			this.captureState = captureState;
			captureState.StateChanged += UpdateState;

			// Start capture
			RefreshFramerate();
			UpdateState();
			captureThread = CreateCaptureThread();
			captureThread.Start();
		}

		// Return the next frame, if it exists (null otherwise)
		public static Bitmap GetNextFrame()
		{
			bool success = frameQueue.TryDequeue(out Bitmap frame);

			if (success) return frame;
			return null;
		}

		public void RefreshFramerate()
		{
			CaptureIntervalMs = 1000 / Properties.Settings.Default.CaptureFramerate;
		}

		public void Stop()
		{
			captureThread.Abort();
			currentSource?.Dispose();
		}



		private Thread CreateCaptureThread()
		{
			return new Thread(() =>
			{
				int fps = Properties.Settings.Default.CaptureFramerate;
				Logger.EmptyLine();
				Logger.Log($"Creating Capture thread. Target framerate: {fps} FPS ({CaptureIntervalMs} ms)");

				Stopwatch stopwatch = new Stopwatch();

				while (true)
				{
					stopwatch.Restart();
					try
					{
						EnqueueFrame(currentSource.CaptureFrame());
					}
					catch (ThreadAbortException)
					{
						Logger.EmptyLine();
						Logger.Log("Aborting capture due to ThreadAbortException.");
						break;
					}
					catch (Exception e)
					{
						Logger.EmptyLine();
						Logger.Log("Aborting capture due to exception.");
						Logger.Log(e);
						CaptureAborted?.Invoke();
					}
					stopwatch.Stop();

					int wait = CaptureIntervalMs - (int)stopwatch.ElapsedMilliseconds;
					if (wait > 0)
					{
						Thread.Sleep(wait);
					}
				}
			})
			{
				IsBackground = true,
				Name = "Capture Thread"
			};
		}

		private void UpdateState()
		{
			try
			{
				CaptureSource oldSource = currentSource;
				currentSource = CaptureSourceFactory.Build(captureState);
				// Dispose after switching to avoid data races
				oldSource?.Dispose();

				Logger.Log("Changed current source to " + currentSource.GetType().Name);
			}
			catch (Exception e)
			{
				if (currentSource != null)
				{
					Logger.EmptyLine();
					Logger.Log($"CANNOT INSTANTIATE NEW SOURCE. Returning to old source ({currentSource.GetType().Name}).");
					Logger.Log(e);

					// We already have a valid source, do not call Dispose and just show warning
					string msg = "Unable to display this item.\n" +
						"If the problem persists, consider changing the capture method in Settings > Capture";
					MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Logger.EmptyLine();
				Logger.Log("CANNOT INSTANTIATE FIRST SOURCE. Changing capture methods to BitBlt.");
				Logger.Log(e);
				// We do not have a valid source, fallback to the safest methods
				captureState.ScreenMethod = CaptureState.ScreenCaptureMethod.BitBlt;
				captureState.WindowMethod = CaptureState.WindowCaptureMethod.BitBlt;
			}
		}

		private void EnqueueFrame(Bitmap frame)
		{
			frameQueue.Enqueue(frame);

			// Limit the size of frameQueue to LIMIT_QUEUE_SZ
			if (frameQueue.Count > LIMIT_QUEUE_SZ)
			{
				frameQueue.TryDequeue(out Bitmap b);
				b?.Dispose();
			}
		}
	}
}
