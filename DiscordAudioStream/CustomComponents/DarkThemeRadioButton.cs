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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (darkMode)
            {
                e.Graphics.Clear(DarkThemeManager.DarkBackColor);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(new SolidBrush(DarkThemeManager.DarkSecondColor), new Rectangle(0, 2, 13, 13));
                if (Focused)
                {
                    e.Graphics.DrawEllipse(new Pen(DarkThemeManager.BorderColor, 2f), new Rectangle(1, 3, 11, 11));
                }
                else
                {
                    e.Graphics.DrawEllipse(new Pen(DarkThemeManager.BorderColor), new Rectangle(0, 2, 13, 13));
                }

                if (base.Checked)
                {
                    if (base.Enabled)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(ForeColor), new Rectangle(3, 5, 7, 7));
                    }
                    else
                    {
                        e.Graphics.FillEllipse(new SolidBrush(DarkThemeManager.BorderColor), new Rectangle(3, 5, 7, 7));
                    }
                }

                e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
                if (base.Enabled)
                {
                    e.Graphics.DrawString(darkText, Font, new SolidBrush(ForeColor), 17f, 0f);
                }
                else
                {
                    e.Graphics.DrawString(darkText, Font, new SolidBrush(DarkThemeManager.BorderColor), 17f, 0f);
                }
            }
            else
            {
                base.OnPaint(e);
            }
        }
    }
}