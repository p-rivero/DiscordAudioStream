using Microsoft.Win32;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class CursorPainter : ICaptureSource
	{
		// Get the cursor size only once at the start
		private int cursorSize = -1;
		private readonly ICaptureSource source;


		public delegate Rectangle CaptureAreaRectDelegate();
		public CaptureAreaRectDelegate CaptureAreaRect;

		public CursorPainter(ICaptureSource source)
		{
			this.source = source;
		}

		public Bitmap CaptureFrame()
		{
			if (CaptureAreaRect == null)
			{
				throw new ArgumentNullException("Attempting to paint cursor without setting CaptureAreaRect");
			}
			Bitmap bmp = source.CaptureFrame();
			return PaintCursor(bmp, CaptureAreaRect().Location);
		}

		public void Dispose()
		{
			source.Dispose();
		}


		private Bitmap PaintCursor(Bitmap src, Point originPos)
		{
			User32.CURSORINFO pci = User32.CURSORINFO.Init();

			if (User32.GetCursorInfo(ref pci) && pci.flags == User32.CURSOR_SHOWING)
			{
				// Get the cursor hotspot
				User32.GetIconInfo(pci.hCursor, out User32.ICONINFO iconInfo);
				// Screen coordinates where the cursor has to be drawn (compensate for hotspot)
				Point cursorPos = new Point(pci.ptScreenPos.x - iconInfo.xHotspot, pci.ptScreenPos.y - iconInfo.yHotspot);
				// Transform from screen coordinates (relative to main screen) to window coordinates (relative to captured area)
				cursorPos.X -= originPos.X;
				cursorPos.Y -= originPos.Y;

				// Draw the cursor
				using (Graphics g = Graphics.FromImage(src))
				{
					int cursorSz = GetCursorSize();
					User32.DrawIconEx(g.GetHdc(), cursorPos.X, cursorPos.Y, pci.hCursor, cursorSz, cursorSz, 0, IntPtr.Zero, User32.DI_NORMAL);
				}

				// Clean up
				GDI32.DeleteObject(iconInfo.hbmMask);
				GDI32.DeleteObject(iconInfo.hbmColor);
			}
			return src;
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
