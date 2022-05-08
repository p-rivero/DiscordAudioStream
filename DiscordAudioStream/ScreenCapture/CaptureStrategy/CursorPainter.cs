using DLLs;
using Microsoft.Win32;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class CursorPainter : CaptureSource
	{
		public delegate Rectangle CaptureAreaRectDelegate();


		// Get the cursor size only once at the start
		private int cursorSize = -1;
		private readonly CaptureSource source;
		private CaptureAreaRectDelegate captureAreaRect;

		public CursorPainter(CaptureSource source)
		{
			this.source = source;
		}

		public CaptureAreaRectDelegate CaptureAreaRect
		{
			get
			{
				return captureAreaRect;
			}
			set
			{
				captureAreaRect = value ?? throw new ArgumentNullException("value");
			}
		}

		public override Bitmap CaptureFrame()
		{
			if (captureAreaRect == null)
			{
				throw new ArgumentException("Attempting to paint cursor without setting CaptureAreaRect");
			}
			Bitmap bmp = source.CaptureFrame();
			if (bmp == null) return null;
			return PaintCursor(bmp, captureAreaRect().Location);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			source.Dispose();
		}


		private Bitmap PaintCursor(Bitmap src, Point originPos)
		{
			User32.CursorInfo pci = User32.CursorInfo.Init();

			if (!User32.GetCursorInfo(ref pci) || pci.flags != User32.CURSOR_SHOWING)
			{
				// Could not get cursor info, or the cursor was hidden. Do not paint the cursor
				return src;
			}

			// Get the cursor hotspot
			User32.GetIconInfo(pci.hCursor, out User32.IconInfo iconInfo);

			// Screen coordinates where the cursor has to be drawn (compensate for hotspot)
			Point cursorPos = new Point(pci.ptScreenPos.x - iconInfo.xHotspot, pci.ptScreenPos.y - iconInfo.yHotspot);
			// Transform from screen coordinates (relative to main screen) to window coordinates (relative to captured area)
			cursorPos.X -= originPos.X;
			cursorPos.Y -= originPos.Y;

			if (cursorPos.X < 0 || cursorPos.Y < 0 || cursorPos.X > src.Width || cursorPos.Y > src.Height)
			{
				// Invalid cursorpos. Do not paint the cursor
				return src;
			}

			// Draw the cursor
			using (Graphics g = Graphics.FromImage(src))
			{
				int cursorSz = GetCursorSize();
				User32.DrawIconEx(g.GetHdc(), cursorPos.X, cursorPos.Y, pci.hCursor, cursorSz, cursorSz, 0, IntPtr.Zero, User32.DI_NORMAL);
			}

			// Clean up
			Gdi32.DeleteObject(iconInfo.hbmMask);
			Gdi32.DeleteObject(iconInfo.hbmColor);
			
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
