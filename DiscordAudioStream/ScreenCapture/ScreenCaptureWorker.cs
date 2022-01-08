using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Microsoft.Win32;

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
		private int cursorSize = -1;


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
			Size size;
			Point position;
			bool captureCursor = master.IsCapturingCursor();
			if (ProcessHandleManager.CapturingWindow)
			{
				IntPtr proc = ProcessHandleManager.GetHandle();
				GetWindowArea(proc, out size, out position);

				if (size != oldSize)
				{
					oldSize = size;
					master.CapturedWindowSizeChanged(size);
				}
			}
			else
			{
				master.GetCaptureArea(out size, out position);
			}

			Bitmap BMP;

			if (size.IsEmpty)
			{
				// Minimized windows store app. Display a black square.
				BMP = new Bitmap(1, 1);
			}
			else if (ProcessHandleManager.CapturingWindow && Properties.Settings.Default.UseExperimentalCapture)
			{
				BMP = CaptureWindow(ProcessHandleManager.GetHandle(), size);
			}
			else
			{
				BMP = CaptureScreen(position, size);
			}

			if (captureCursor)
			{
				User32.CURSORINFO pci = User32.CURSORINFO.Init();

				if (User32.GetCursorInfo(ref pci) && pci.flags == User32.CURSOR_SHOWING)
				{
					// Get the cursor hotspot
					User32.GetIconInfo(pci.hCursor, out User32.ICONINFO iconInfo);
					// Screen coordinates where the cursor has to be drawn (compensate for hotspot)
					Point cursorPos = new Point(pci.ptScreenPos.x - iconInfo.xHotspot, pci.ptScreenPos.y - iconInfo.yHotspot);
					// Transform from screen coordinates (relative to main screen) to window coordinates (relative to captured area)
					cursorPos.X -= position.X;
					cursorPos.Y -= position.Y;

					// Draw the cursor
					using (Graphics g = Graphics.FromImage(BMP))
					{
						int cursorSz = GetCursorSize();
						User32.DrawIconEx(g.GetHdc(), cursorPos.X, cursorPos.Y, pci.hCursor, cursorSz, cursorSz, 0, IntPtr.Zero, User32.DI_NORMAL);
					}

					// Clean up
					GDI32.DeleteObject(iconInfo.hbmMask);
					GDI32.DeleteObject(iconInfo.hbmColor);
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
			bool success = User32.GetClientRect(hwnd, out User32.RECT clientRect);
			if (!success)
			{
				throw new Exception("Window was closed");
			}

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
			IntPtr hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow());
			IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, size.Width, size.Height);
			GDI32.SelectObject(hdcDest, hBitmap);
			GDI32.BitBlt(hdcDest, 0, 0, size.Width, size.Height, hdcSrc, startPos.X, startPos.Y, GDI32.RasterOps.SRCCOPY);

			Bitmap result = Image.FromHbitmap(hBitmap);

			// Cleanup
			User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
			GDI32.DeleteDC(hdcDest);
			GDI32.DeleteObject(hBitmap);

			return result;
		}

		public Bitmap CaptureWindow(IntPtr hwnd, Size winSize)
		{
			Bitmap result = new Bitmap(winSize.Width, winSize.Height);

			using (Graphics g = Graphics.FromImage(result))
			{
				User32.PrintWindow(hwnd, g.GetHdc(), User32.PW_CLIENTONLY | User32.PW_RENDERFULLCONTENT);
			}
			return result;
		}

		private int GetCursorSize()
		{
			if (cursorSize == -1)
			{
				string scale = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Accessibility", "CursorSize", 1).ToString();
				cursorSize = 32 + (int.Parse(scale) - 1) * 16;
			}
			return cursorSize;
		}
	}
}
