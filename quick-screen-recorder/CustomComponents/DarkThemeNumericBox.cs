using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents
{
	public class DarkThemeNumericBox : NumericUpDown
	{
        public DarkThemeNumericBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            base.Controls[0].Visible = false;
        }

        public void SetDarkMode(bool dark)
        {
            if (dark)
            {
                BackColor = DarkThemeManager.DarkSecondColor;
                ForeColor = Color.White;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            ControlPaint.DrawBorder(e.Graphics, base.ClientRectangle, DarkThemeManager.BorderColor, ButtonBorderStyle.Solid);
            e.Graphics.DrawLine(new Pen(DarkThemeManager.BorderColor), base.Width - 18, 0, base.Width - 18, base.Height);
            e.Graphics.DrawLine(new Pen(DarkThemeManager.BorderColor), base.Width - 18, base.Height / 2, base.Width, base.Height / 2);
            Brush brush = new SolidBrush(ForeColor);
            if (!base.Enabled)
            {
                brush = new SolidBrush(DarkThemeManager.BorderColor);
            }

            e.Graphics.FillPolygon(brush, new PointF[3]
            {
                new PointF(base.Width - 13, 15f),
                new PointF(base.Width - 9, 19f),
                new PointF(base.Width - 5, 15f)
            });
            e.Graphics.FillPolygon(brush, new PointF[3]
            {
                new PointF(base.Width - 14, 8f),
                new PointF(base.Width - 9, 3f),
                new PointF(base.Width - 5, 8f)
            });
        }

        public new decimal Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				if (value < base.Minimum) value = base.Minimum;
				else if (value > base.Maximum) value = base.Maximum;
				base.Value = value;

				base.Text = value.ToString();
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.X > base.Width - 18)
			{
				Focus();
				if (e.Y > base.Height / 2)
				{
					Value--;
				}
				else
				{
					Value++;
				}
			}
		}

		protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			//if (e.KeyChar == '\r' || e.KeyChar == '\n')
			//{
			//    Value = decimal.Parse(base.Text);
			//    e.Handled = true;
			//    return;
			//}

			base.OnTextBoxKeyPress(source, e);

			base.Text = Value.ToString();
		}
	}
}
