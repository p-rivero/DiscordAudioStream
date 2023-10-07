using System;
using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents
{
    public class AudioMeterText : Control
    {
        private int windowWidth = 0;
        public void SetWindowWidth(int width)
        {
            bool refresh = (width != windowWidth);
            windowWidth = width;
            if (refresh) Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            DrawMeterLine(e.Graphics, "0", 0f);
            DrawMeterLine(e.Graphics, "-20", 0.3333f);
            DrawMeterLine(e.Graphics, "-40", 0.6667f);
            DrawMeterLine(e.Graphics, "-60", 1f);

            if (Height > 500)
            {
                DrawMeterLine(e.Graphics, "-10", 0.1667f);
                DrawMeterLine(e.Graphics, "-30", 0.5f);
                DrawMeterLine(e.Graphics, "-50", 0.8333f);
            }
        }

        private void DrawMeterLine(Graphics g, string text, float percent)
        {
            const int PADDING = 10;
            const int LINE_LENGTH = 3;
            const int WIDTH_SHOW_TEXT = 130;
            const int WIDTH_SHOW_DB = 170;
            int y = GetYPos(percent);

            if (windowWidth < WIDTH_SHOW_TEXT) text = "";
            else if (windowWidth > WIDTH_SHOW_DB) text += " dB";

            SizeF strSz = g.MeasureString(text, Font);
            using (Brush fgBrush = new SolidBrush(ForeColor))
            using (Pen fgPen = new Pen(ForeColor))
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    g.DrawString(text, Font, fgBrush, Width - strSz.Width - PADDING, y - strSz.Height / 2);
                    g.DrawLine(fgPen, Width - LINE_LENGTH, y, Width, y);
                }
                else
                {
                    g.DrawString(text, Font, fgBrush, PADDING, y - strSz.Height / 2);
                    g.DrawLine(fgPen, 0, y, LINE_LENGTH, y);
                }
            }
        }

        private int GetYPos(float percent)
        {
            const int PADDING_TOP = 11;
            const int PADDING_BOTTOM = 28;
            int bottom = Height - PADDING_BOTTOM;
            return PADDING_TOP + (int)((bottom - PADDING_TOP) * percent);
        }
    }
}
