using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents
{
	public class DarkThemeRadioButton : RadioButton
	{
		private bool darkMode;

		private string darkText;

		public DarkThemeRadioButton()
		{
			if (darkMode)
			{
				SetStyle(ControlStyles.UserPaint, value: true);
				SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			}
		}

		public void SetDarkMode(bool dark)
		{
			darkMode = dark;
			if (dark)
			{
				darkText = Text;
				Text = " ";
			}
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			if (!darkMode)
			{
				base.OnPaint(pevent);
				return;
			}

			pevent.Graphics.Clear(DarkThemeManager.DarkBackColor);
			pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			pevent.Graphics.FillEllipse(new SolidBrush(DarkThemeManager.DarkSecondColor), new Rectangle(0, 2, 13, 13));
			if (Focused)
			{
				pevent.Graphics.DrawEllipse(new Pen(DarkThemeManager.BorderColor, 2f), new Rectangle(1, 3, 11, 11));
			}
			else
			{
				pevent.Graphics.DrawEllipse(new Pen(DarkThemeManager.BorderColor), new Rectangle(0, 2, 13, 13));
			}


			if (base.Checked)
			{
				if (base.Enabled)
				{
					pevent.Graphics.FillEllipse(new SolidBrush(ForeColor), new Rectangle(3, 5, 7, 7));
				}
				else
				{
					pevent.Graphics.FillEllipse(new SolidBrush(DarkThemeManager.BorderColor), new Rectangle(3, 5, 7, 7));
				}
			}

			pevent.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
			if (base.Enabled)
			{
				pevent.Graphics.DrawString(darkText, Font, new SolidBrush(ForeColor), 17f, 0f);
			}
			else
			{
				pevent.Graphics.DrawString(darkText, Font, new SolidBrush(DarkThemeManager.BorderColor), 17f, 0f);
			}
		}
	}
}