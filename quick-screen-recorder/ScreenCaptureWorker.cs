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

		private static NumericUpDown widthNumeric = null;
		private static NumericUpDown heightNumeric = null;
		private static NumericUpDown xNumeric = null;
		private static NumericUpDown yNumeric = null;
		private static CheckBox captureCursorCheckBox = null;

		private int INTERVAL_MS;
		private Thread captureThread;


		public static void Initialize(NumericUpDown w, NumericUpDown h, NumericUpDown x, NumericUpDown y, CheckBox cursor)
		{
			widthNumeric = w;
			heightNumeric = h;
			xNumeric = x;
			yNumeric = y;
			captureCursorCheckBox = cursor;
		}

		// Return the next frame, if it exists (null otherwise)
		public static Bitmap GetNextFrame()
		{
			Bitmap frame;
			bool success = frameQueue.TryDequeue(out frame);

			if (success) return frame;
			return null;
		}



		public ScreenCaptureWorker(double targetFramerate)
		{
			if (widthNumeric == null || heightNumeric == null || xNumeric == null || yNumeric == null)
			{
				throw new InvalidOperationException("Must call initialize() before creating workers");
			}
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
			try
			{
				int width = (int)widthNumeric.Value;
				int height = (int)heightNumeric.Value;
				int x = (int)xNumeric.Value;
				int y = (int)yNumeric.Value;

				Bitmap BMP = CaptureScreen(x, y, width, height);
				if (captureCursorCheckBox.Checked)
				{
					User32.CURSORINFO pci;
					pci.cbSize = Marshal.SizeOf(typeof(User32.CURSORINFO));

					if (User32.GetCursorInfo(out pci) && pci.flags == User32.CURSOR_SHOWING)
					{
						Graphics g = Graphics.FromImage(BMP);
						User32.DrawIcon(g.GetHdc(), pci.ptScreenPos.x - (int)xNumeric.Value, pci.ptScreenPos.y - (int)yNumeric.Value, pci.hCursor);
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
			catch
			{
				// TODO: Remove try-catch
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
	}
}



class GDI32
{
	[DllImport("GDI32.dll")]
	public static extern bool BitBlt(int hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, int hdcSrc, int nXSrc, int nYSrc, int dwRop);
	[DllImport("GDI32.dll")]
	public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);[DllImport("GDI32.dll")]
	public static extern int CreateCompatibleDC(int hdc);
	[DllImport("GDI32.dll")]
	public static extern bool DeleteDC(int hdc);
	[DllImport("GDI32.dll")]
	public static extern bool DeleteObject(int hObject);
	[DllImport("GDI32.dll")]
	public static extern int GetDeviceCaps(int hdc, int nIndex);
	[DllImport("GDI32.dll")]
	public static extern int SelectObject(int hdc, int hgdiobj);
}

class User32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct POINTAPI
	{
		public int x;
		public int y;
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct CURSORINFO
	{
		public Int32 cbSize;
		public Int32 flags;
		public IntPtr hCursor;
		public POINTAPI ptScreenPos;
	}
	public const Int32 CURSOR_SHOWING = 0x00000001;

	[DllImport("User32.dll")]
	public static extern int GetDesktopWindow();
	[DllImport("User32.dll")]
	public static extern int GetWindowDC(int hWnd);
	[DllImport("User32.dll")]
	public static extern int ReleaseDC(int hWnd, int hDC);
	[DllImport("user32.dll")]
	public static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);
	[DllImport("user32.dll")]
	public static extern bool GetCursorInfo(out CURSORINFO pci);
}

