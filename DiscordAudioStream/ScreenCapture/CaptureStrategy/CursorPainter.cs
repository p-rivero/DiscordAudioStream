using System;
using System.Drawing;
using System.Drawing.Imaging;

using DLLs;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
    public class CursorPainter : CaptureSource
    {
        private readonly CaptureSource source;

        private IntPtr currentCursorHandle = IntPtr.Zero;
        private Bitmap cursorBitmap = null;
        private Point cursorHotspot = Point.Empty;

        public CursorPainter(CaptureSource source)
        {
            this.source = source;
            Logger.Log($"Instantiating CursorPainter source (wrapping {source.GetType().Name})");
        }

        public Func<Rectangle> CaptureAreaRect { get; set; }

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
            cursorBitmap?.Dispose();
        }


        private Bitmap PaintCursor(Bitmap src, Point originPos)
        {
            User32.CursorInfo pci = User32.CursorInfo.Init();

            if (!User32.GetCursorInfo(ref pci) || pci.flags != User32.CURSOR_SHOWING)
            {
                // Could not get cursor info, or the cursor was hidden. Do not paint the cursor
                return src;
            }

            if (currentCursorHandle != pci.hCursor)
            {
                UpdateCursorBitmap(pci.hCursor);
                currentCursorHandle = pci.hCursor;
            }
            if (cursorBitmap == null)
            {
                return src;
            }

            // Screen coordinates where the cursor has to be drawn (compensate for hotspot)
            Point cursorPos = new Point(pci.ptScreenPos.x - cursorHotspot.X, pci.ptScreenPos.y - cursorHotspot.Y);
            // Transform from screen coordinates (relative to main screen) to window coordinates (relative to captured area)
            cursorPos.X -= originPos.X;
            cursorPos.Y -= originPos.Y;

            // Draw the cursor only if it's inside the bounds
            if (cursorPos.X >= 0 && cursorPos.Y >= 0 && cursorPos.X <= src.Width && cursorPos.Y <= src.Height)
            {
                using (Graphics g = Graphics.FromImage(src))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.DrawImage(cursorBitmap, cursorPos);
                }
            }

            return src;
        }

        private void UpdateCursorBitmap(IntPtr hCursor)
        {
            if (!User32.GetIconInfo(hCursor, out User32.IconInfo iconInfo))
            {
                return;
            }
            cursorBitmap?.Dispose();
            cursorBitmap = BitmapFromCursor(iconInfo);
            cursorHotspot = new Point(iconInfo.xHotspot, iconInfo.yHotspot);
            CleanUpIconInfo(iconInfo);
        }

        private Bitmap BitmapFromCursor(in User32.IconInfo iconInfo)
        {
            if (iconInfo.hbmColor == IntPtr.Zero)
            {
                return null;
            }
            using (Bitmap bmp = Image.FromHbitmap(iconInfo.hbmColor))
            {
                // Move data pointer (bmData.Scan0) from bmp to dstBitmap
                BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                Bitmap dstBitmap = new Bitmap(bmData.Width, bmData.Height, bmData.Stride, PixelFormat.Format32bppArgb, bmData.Scan0);
                bmp.UnlockBits(bmData);

                return new Bitmap(dstBitmap);
            }
        }

        private void CleanUpIconInfo(User32.IconInfo iconInfo)
        {
            Gdi32.DeleteObject(iconInfo.hbmMask);
            Gdi32.DeleteObject(iconInfo.hbmColor);
        }

    }
}
