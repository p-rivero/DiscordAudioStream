using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents
{
    public class DarkThemeCheckBox : CheckBox
    {
        private bool darkMode;

        private string darkText;

        private bool hovered;

        private bool pressed;

        public DarkThemeCheckBox()
        {
            if (darkMode)
            {
                SetStyle(ControlStyles.UserPaint, value: true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
                base.MouseEnter += CustomCheckBox_MouseEnter;
                base.MouseLeave += CustomCheckBox_MouseLeave;
                base.MouseDown += CustomCheckBox_MouseDown;
                base.MouseUp += CustomCheckBox_MouseUp;
            }
        }

        private void CustomCheckBox_MouseUp(object sender, MouseEventArgs e)
        {
            pressed = false;
            Refresh();
        }

        private void CustomCheckBox_MouseDown(object sender, MouseEventArgs e)
        {
            pressed = true;
            Refresh();
        }

        private void CustomCheckBox_MouseLeave(object sender, EventArgs e)
        {
            hovered = false;
            Refresh();
        }

        private void CustomCheckBox_MouseEnter(object sender, EventArgs e)
        {
            hovered = true;
            Refresh();
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
            
            pevent.Graphics.Clear(BackColor);

            Color bgColor;
            if (pressed)
            {
                bgColor = DarkThemeManager.PressedColor;
            }
            else if (hovered)
            {
                bgColor = DarkThemeManager.DarkHoverColor;
            }
            else
            {
                bgColor = DarkThemeManager.DarkSecondColor;
            }
            pevent.Graphics.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 2, 13, 13));

            if (Focused)
            {
                pevent.Graphics.DrawRectangle(new Pen(DarkThemeManager.BorderColor, 2f), new Rectangle(1, 3, 11, 11));
            }
            else
            {
                ControlPaint.DrawBorder(pevent.Graphics, new Rectangle(0, 2, 13, 13), DarkThemeManager.BorderColor, ButtonBorderStyle.Solid);
            }

            if (base.Checked)
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (base.Enabled)
                {
                    pevent.Graphics.DrawLine(new Pen(ForeColor, 2f), 2, 7, 5, 10);
                    pevent.Graphics.DrawLine(new Pen(ForeColor, 2f), 5, 11, 12, 4);
                }
                else
                {
                    pevent.Graphics.DrawLine(new Pen(DarkThemeManager.BorderColor, 2f), 2, 7, 5, 10);
                    pevent.Graphics.DrawLine(new Pen(DarkThemeManager.BorderColor, 2f), 5, 11, 12, 4);
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