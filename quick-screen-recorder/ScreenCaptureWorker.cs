using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace quick_screen_recorder
{
	internal class ScreenCaptureWorker
	{
		private static ConcurrentQueue<Bitmap> frameQueue = new ConcurrentQueue<Bitmap>();
		private const int LIMIT_QUEUE_SZ = 5;

		private static ScreenCaptureWorker[] workers;

		private static NumericUpDown widthNumeric = null;
		private static NumericUpDown heightNumeric = null;
		private static NumericUpDown xNumeric = null;
		private static NumericUpDown yNumeric = null;
		private static CheckBox captureCursorCheckBox = null;

		private static int INTERVAL_MS;
		private Thread captureThread;


		public static void Initialize(NumericUpDown w, NumericUpDown h, NumericUpDown x, NumericUpDown y, CheckBox cursor)
		{
			widthNumeric = w;
			heightNumeric = h;
			xNumeric = x;
			yNumeric = y;
			captureCursorCheckBox = cursor;
		}

		public static void StartWorkers(double targetFramerate, int numThreads)
		{
			double waitTimeMs = (1000.0 / targetFramerate);
			INTERVAL_MS = (int) (waitTimeMs * numThreads);
			int offset = (int) waitTimeMs;

			workers = new ScreenCaptureWorker[numThreads];

			for (int i = 0; i < numThreads; i++)
			{
				workers[i] = new ScreenCaptureWorker();
				Thread.Sleep(offset);
			}
		}

		public static void StopWorkers()
		{
			foreach (ScreenCaptureWorker worker in workers)
			{
				worker.Stop();
			}
		}

		// Return the next frame, if it exists (null otherwise)
		public static Bitmap GetNextFrame()
		{
			Bitmap frame;
			bool success = frameQueue.TryDequeue(out frame);

			if (success) return frame;
			return null;
		}



		private ScreenCaptureWorker()
		{
			if (widthNumeric == null || heightNumeric == null || xNumeric == null || yNumeric == null)
			{
				throw new InvalidOperationException("Must call initialize() before creating workers");
			}

			captureThread = new Thread(() =>
			{
				Stopwatch stopwatch = new Stopwatch();

				while (true)
				{
					stopwatch.Restart();
					CaptureFrame();
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


		private void CaptureFrame()
		{
			try
			{
				int width = (int)widthNumeric.Value;
				int height = (int)heightNumeric.Value;
				int x = (int)xNumeric.Value;
				int y = (int)yNumeric.Value;

				Bitmap BMP = new Bitmap(width, height);
				using (var g = Graphics.FromImage(BMP))
				{
					g.CopyFromScreen(new Point(x, y), Point.Empty, new Size(width, height), CopyPixelOperation.SourceCopy);

					if (captureCursorCheckBox.Checked)
					{
						Recorder.CURSORINFO pci;
						pci.cbSize = Marshal.SizeOf(typeof(Recorder.CURSORINFO));

						if (Recorder.GetCursorInfo(out pci))
						{
							if (pci.flags == Recorder.CURSOR_SHOWING)
							{
								Recorder.DrawIcon(g.GetHdc(), pci.ptScreenPos.x - (int)xNumeric.Value, pci.ptScreenPos.y - (int)yNumeric.Value, pci.hCursor);
								g.ReleaseHdc();
							}
						}
					}

					g.Flush();
				}

				frameQueue.Enqueue(BMP);

				// Limit the size of frameQueue to LIMIT_QUEUE_SZ
				if (frameQueue.Count > LIMIT_QUEUE_SZ)
				{
					frameQueue.TryDequeue(out BMP);
					BMP.Dispose(); // Prevent memory leak
				}
			}
			catch
			{
				// TODO: Remove try-catch
			}
		}

		private void Stop()
		{
			captureThread.Abort();
		}
	}
}
