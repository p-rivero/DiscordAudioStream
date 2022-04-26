using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
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
			// TODO: If an exception is thrown, fallback to another method
			currentSource = CaptureSourceFactory.Build(captureState);
			// Dispose after switching to avoid data races
			oldSource?.Dispose();
		}

		private void EnqueueFrame()
		{
			// Try to enqueue with a timeout of 0.5 seconds
			var task = Task.Run(() => {
				frameQueue.Enqueue(currentSource.CaptureFrame());
			});
			if (!task.Wait(TimeSpan.FromMilliseconds(500)))
            {
				throw new TimeoutException("CaptureFrame() timed out");
            }

			// Limit the size of frameQueue to LIMIT_QUEUE_SZ
			if (frameQueue.Count > LIMIT_QUEUE_SZ)
			{
				frameQueue.TryDequeue(out Bitmap b);
				b.Dispose();
			}
		}
	}
}
