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

		// Framerate limit
		public const int TARGET_FRAMERATE = 60;

		// Number of frames that are stored in the queue
		private const int LIMIT_QUEUE_SZ = 3;
		// Wait time between frames
		private const int INTERVAL_MS = 1000/ TARGET_FRAMERATE;

		private readonly Thread captureThread;
		private readonly CaptureState captureState;
		private static readonly ConcurrentQueue<Bitmap> frameQueue = new ConcurrentQueue<Bitmap>();
		private ICaptureSource currentSource = null;

		public ScreenCaptureManager(CaptureState captureState)
		{
			this.captureState = captureState;
			captureState.StateChanged += UpdateState;

			// Start capture
			UpdateState();
			captureThread = new Thread(() =>
			{
				Stopwatch stopwatch = new Stopwatch();

				while (true)
				{
					stopwatch.Restart();
					try
					{
						EnqueueFrame();
					}
					catch (ThreadAbortException)
					{
						break;
					}
					catch (Exception)
					{
						CaptureAborted?.Invoke();
					}
					stopwatch.Stop();

					int wait = INTERVAL_MS - (int)stopwatch.ElapsedMilliseconds;
					if (wait > 0)
					{
						Thread.Sleep(wait);
					}
				}
			});
			captureThread.IsBackground = true;
			captureThread.Start();
		}

		// Return the next frame, if it exists (null otherwise)
		public static Bitmap GetNextFrame()
		{
			bool success = frameQueue.TryDequeue(out Bitmap frame);

			if (success) return frame;
			return null;
		}

		public void Stop()
		{
			captureThread.Abort();
			currentSource?.Dispose();
		}



		private void UpdateState()
		{
			ICaptureSource oldSource = currentSource;
			try
			{
				currentSource = CaptureSourceFactory.Build(captureState);
				// Dispose after switching to avoid data races
				oldSource?.Dispose();
			}
			catch
			{
				if (oldSource != null)
				{
					// We already have a valid source, do not call Dispose and just show warning
					string msg = "Unable to display this item.\n";
					msg += "If the problem persists, consider changing the capture method in Settings > Capture";
					MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				// We do not have a valid source, fallback to the safest methods
				captureState.ScreenMethod = CaptureState.ScreenCaptureMethod.BitBlt;
				captureState.WindowMethod = CaptureState.WindowCaptureMethod.BitBlt;
			}
		}

		private void EnqueueFrame()
		{
			frameQueue.Enqueue(currentSource.CaptureFrame());

			// Limit the size of frameQueue to LIMIT_QUEUE_SZ
			if (frameQueue.Count > LIMIT_QUEUE_SZ)
			{
				frameQueue.TryDequeue(out Bitmap b);
				b.Dispose();
			}
		}
	}
}
