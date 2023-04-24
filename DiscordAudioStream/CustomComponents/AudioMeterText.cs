using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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

		protected override void OnResize(EventArgs pe)
		{
			base.OnResize(pe);
			Refresh();
		}


		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.Clear(BackColor);
			
			DrawMeterLine(pe.Graphics, "0", 0f);
			DrawMeterLine(pe.Graphics, "-20", 0.3333f);
			DrawMeterLine(pe.Graphics, "-40", 0.6667f);
			DrawMeterLine(pe.Graphics, "-60", 1f);

			if (Height > 500)
			{
				DrawMeterLine(pe.Graphics, "-10", 0.1667f);
				DrawMeterLine(pe.Graphics, "-30", 0.5f);
				DrawMeterLine(pe.Graphics, "-50", 0.8333f);
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

			if (RightToLeft == RightToLeft.Yes)
			{
				SizeF strSz = g.MeasureString(text, Font);
				g.DrawString(text, Font, new SolidBrush(ForeColor), Width - strSz.Width - PADDING, y - strSz.Height/2);
				g.DrawLine(new Pen(ForeColor, 2f), Width - LINE_LENGTH, y, Width, y);
			}
			else
			{
				SizeF strSz = g.MeasureString(text, Font);
				g.DrawString(text, Font, new SolidBrush(ForeColor), PADDING, y - strSz.Height / 2);
				g.DrawLine(new Pen(ForeColor, 2f), 0, y, LINE_LENGTH, y);
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
