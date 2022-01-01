using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

namespace quick_screen_recorder
{
	internal class ScreenCaptureWorker
	{
		private IScreenCaptureMaster master;

		private static ConcurrentQueue<Bitmap> frameQueue = new ConcurrentQueue<Bitmap>();
		private const int LIMIT_QUEUE_SZ = 3;

		private int INTERVAL_MS;
		private Thread captureThread;


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
					EnqueueFrame();
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
			Bitmap BMP = null;
			if (master.isCapturingWindow())
			{
				IntPtr proc = ProcessHandleManager.GetHandle();
				BMP = CaptureWindow(proc);
			}
			else
			{
				master.getCaptureArea(out int width, out int height, out int x, out int y, out bool captureCursor);
				BMP = CaptureScreen(x, y, width, height);
				if (captureCursor)
				{
					User32.CURSORINFO pci;
					pci.cbSize = Marshal.SizeOf(typeof(User32.CURSORINFO));

					if (User32.GetCursorInfo(out pci) && pci.flags == User32.CURSOR_SHOWING)
					{
						Graphics g = Graphics.FromImage(BMP);
						User32.DrawIcon(g.GetHdc(), pci.ptScreenPos.x - x, pci.ptScreenPos.y - y, pci.hCursor);
						g.ReleaseHdc();
						g.Dispose();
					}
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

		private static Bitmap CaptureScreen(int startX, int startY, int width, int height)
		{
			int hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow());
			int hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			int hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
			GDI32.SelectObject(hdcDest, hBitmap);
			GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, startX, startY, 0x00CC0020);

			Bitmap result = Image.FromHbitmap(new IntPtr(hBitmap));

			// Cleanup
			User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
			GDI32.DeleteDC(hdcDest);
			GDI32.DeleteObject(hBitmap);

			return result;
		}

		public static Bitmap CaptureWindow(IntPtr hwnd)
		{
			User32.GetWindowRect(hwnd, out User32.RECT rc);
			return CaptureScreen(rc.X, rc.Y, rc.Width, rc.Height);

			//Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
			//Graphics gfxBmp = Graphics.FromImage(bmp);
			//IntPtr hdcBitmap = gfxBmp.GetHdc();

			//User32.PrintWindow(hwnd, hdcBitmap, 0);

			//gfxBmp.ReleaseHdc(hdcBitmap);
			//gfxBmp.Dispose();

			//return bmp;
		}
	}
}
