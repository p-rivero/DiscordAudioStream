using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;


namespace DiscordAudioStream
{
	internal class ScreenCaptureWorker
	{
		private IScreenCaptureMaster master;

		private static ConcurrentQueue<Bitmap> frameQueue = new ConcurrentQueue<Bitmap>();
		private const int LIMIT_QUEUE_SZ = 3;

		private int INTERVAL_MS;
		private Thread captureThread;
		private Size oldSize = new Size(-1, -1);


		// Return the next frame, if it exists (null otherwise)
		public static Bitmap GetNextFrame()
		{
			Bitmap frame;
			bool success = frameQueue.TryDequeue(out frame);

			if (success) return frame;
			return null;
		}



		public ScreenCaptureWorker(double targetFramerate, IScreenCaptureMaster captureMaster)
		{
			master = captureMaster;

			if (targetFramerate <= 0)
			{
				throw new ArgumentOutOfRangeException("The target framerate must be greater than 0");
			}

			INTERVAL_MS = (int) (1000.0 / targetFramerate);

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
						master.AbortCapture();
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

		public void Stop()
		{
			captureThread.Abort();
		}


		private void EnqueueFrame()
		{
			Size windowSize;
			Point position;
			bool captureCursor = master.IsCapturingCursor();
			if (ProcessHandleManager.CapturingWindow)
			{
				IntPtr proc = ProcessHandleManager.GetHandle();
				GetWindowArea(proc, out windowSize, out position);

				if (windowSize != oldSize)
				{
					oldSize = windowSize;
					master.CapturedWindowSizeChanged(windowSize);
				}
			}
			else
			{
				master.GetCaptureArea(out windowSize, out position);
			}

			if (windowSize.Width == 0 && windowSize.Height == 0)
				return;

			Bitmap BMP;
			if (ProcessHandleManager.CapturingWindow && Properties.Settings.Default.UseExperimentalCapture)
			{
				BMP = CaptureWindow(ProcessHandleManager.GetHandle(), windowSize);
			} else
			{
				BMP = CaptureScreen(position, windowSize);
			}

			if (captureCursor)
			{
				User32.CURSORINFO pci;
				pci.cbSize = Marshal.SizeOf(typeof(User32.CURSORINFO));

				if (User32.GetCursorInfo(out pci) && pci.flags == User32.CURSOR_SHOWING)
				{
					Graphics g = Graphics.FromImage(BMP);
					User32.DrawIcon(g.GetHdc(), pci.ptScreenPos.x - position.X, pci.ptScreenPos.y - position.Y, pci.hCursor);
					g.ReleaseHdc();
					g.Dispose();
				}
			}

			frameQueue.Enqueue(BMP);

			// Limit the size of frameQueue to LIMIT_QUEUE_SZ
			if (frameQueue.Count > LIMIT_QUEUE_SZ)
			{
				frameQueue.TryDequeue(out Bitmap b);
				b.Dispose();
			}
		}

		private static void GetWindowArea(IntPtr hwnd, out Size windowSize, out Point position)
		{
			// Get size of client area (don't use X and Y, these are relative to the WINDOW rect)
			User32.GetClientRect(hwnd, out User32.RECT clientRect);
			// Get frame size and position (generally more accurate than GetWindowRect)
			User32.RECT frame = Dwmapi.GetRectAttr(hwnd, Dwmapi.DwmWindowAttribute.EXTENDED_FRAME_BOUNDS);

			// Trim the black bar at the top when the window is maximized,
			// as well as the title bar for applications with a defined client area
			int yOffset = frame.Height - clientRect.Height;

			windowSize = new Size(clientRect.Width, clientRect.Height);
			position = new Point(frame.X + 1, frame.Y + yOffset);
		}

		private static Bitmap CaptureScreen(Point startPos, Size size)
		{
			int hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow());
			int hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			int hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, size.Width, size.Height);
			GDI32.SelectObject(hdcDest, hBitmap);
			GDI32.BitBlt(hdcDest, 0, 0, size.Width, size.Height, hdcSrc, startPos.X, startPos.Y, 0x00CC0020);

			Bitmap result = Image.FromHbitmap(new IntPtr(hBitmap));

			// Cleanup
			User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
			GDI32.DeleteDC(hdcDest);
			GDI32.DeleteObject(hBitmap);

			return result;
		}

		public Bitmap CaptureWindow(IntPtr hwnd, Size winSize)
		{
			Bitmap bmp = new Bitmap(winSize.Width, winSize.Height);
			Graphics gfxBmp = Graphics.FromImage(bmp);
			IntPtr hdcBitmap = gfxBmp.GetHdc();
			User32.PrintWindow(hwnd, hdcBitmap, User32.PW_CLIENTONLY|User32.PW_RENDERFULLCONTENT);
			gfxBmp.ReleaseHdc(hdcBitmap);
			gfxBmp.Dispose();
			return bmp;
		}

	}
}
