using DLLs;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class CursorPainter : CaptureSource
	{
		public delegate Rectangle CaptureAreaRectDelegate();

		private readonly CaptureSource source;

		public CursorPainter(CaptureSource source)
		{
			this.source = source;
			Logger.Log("Instantiating CursorPainter source (wrapping {0})", source.GetType().Name);
		}

		public CaptureAreaRectDelegate CaptureAreaRect { get; set; }

		public override Bitmap CaptureFrame()
		{
			if (CaptureAreaRect == null)
			{
				throw new ArgumentException("Attempting to paint cursor without setting CaptureAreaRect");
			}
			Bitmap bmp = source.CaptureFrame();
			if (bmp == null) return null;
			return PaintCursor(bmp, CaptureAreaRect().Location);
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

			// Get the cursor hotspot and icon
			if (!User32.GetIconInfo(pci.hCursor, out User32.IconInfo iconInfo) || iconInfo.hbmColor == IntPtr.Zero)
			{
				// GetIconInfo failed, or the cursor has no color bitmap. Do not paint the cursor
				return src;
			}

			// Screen coordinates where the cursor has to be drawn (compensate for hotspot)
			Point cursorPos = new Point(pci.ptScreenPos.x - iconInfo.xHotspot, pci.ptScreenPos.y - iconInfo.yHotspot);
			// Transform from screen coordinates (relative to main screen) to window coordinates (relative to captured area)
			cursorPos.X -= originPos.X;
			cursorPos.Y -= originPos.Y;

			// Draw the cursor only if it's inside the bounds
			if (cursorPos.X >= 0 && cursorPos.Y >= 0 && cursorPos.X <= src.Width && cursorPos.Y <= src.Height)
			{
				Bitmap curBmp = BitmapFromCursor(iconInfo);
				using (Graphics g = Graphics.FromImage(src))
				{
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.DrawImage(curBmp, cursorPos);
				}
			}

			// Clean up
			Gdi32.DeleteObject(iconInfo.hbmMask);
			Gdi32.DeleteObject(iconInfo.hbmColor);

			return src;
		}

		private Bitmap BitmapFromCursor(in User32.IconInfo iconInfo)
		{
			Bitmap bmp = Image.FromHbitmap(iconInfo.hbmColor);
			// Move data pointer (bmData.Scan0) from bmp to dstBitmap
			BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
			Bitmap dstBitmap = new Bitmap(bmData.Width, bmData.Height, bmData.Stride, PixelFormat.Format32bppArgb, bmData.Scan0);
			bmp.UnlockBits(bmData);

			return new Bitmap(dstBitmap);
		}
	}
}
