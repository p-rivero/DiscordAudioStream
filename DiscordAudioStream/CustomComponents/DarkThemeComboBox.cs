using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents
{
    public class DarkThemeComboBox : ComboBox
    {
        private bool darkMode;
        private bool hovered;

        public DarkThemeComboBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            base.DrawMode = DrawMode.OwnerDrawFixed;
            base.MouseEnter += CustomComboBox_MouseEnter;
            base.MouseLeave += CustomComboBox_MouseLeave;
        }

        private void CustomComboBox_MouseLeave(object sender, EventArgs e)
        {
            hovered = false;
            Refresh();
        }

        private void CustomComboBox_MouseEnter(object sender, EventArgs e)
        {
            hovered = true;
            Refresh();
        }

        public void SetDarkMode(bool dark)
        {
            darkMode = dark;
            if (dark)
            {
                BackColor = DarkThemeManager.DarkSecondColor;
                ForeColor = Color.White;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (hovered)
            {
                if (darkMode) e.Graphics.FillRectangle(new SolidBrush(DarkThemeManager.DarkHoverColor), 0, 0, base.Width, base.Height - 2);
                else e.Graphics.FillRectangle(new SolidBrush(DarkThemeManager.LightHoverColor), 0, 0, base.Width, base.Height - 2);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, base.Width, base.Height - 2);
            }

            e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), 3f, 3f);
            e.Graphics.FillRectangle(new SolidBrush(BackColor), base.Width - 18, 0, 18, base.Height);
            e.Graphics.FillPolygon(new SolidBrush(ForeColor), new PointF[3]
            {
                new PointF(base.Width - 13, 10f),
                new PointF(base.Width - 9, 14f),
                new PointF(base.Width - 5, 10f)
            });

            Rectangle bounds = new Rectangle(base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.Width, base.ClientRectangle.Height - 1);
            ControlPaint.DrawBorder(e.Graphics, bounds, DarkThemeManager.BorderColor, ButtonBorderStyle.Solid);
            e.Graphics.DrawLine(new Pen(DarkThemeManager.BorderColor), base.Width - 18, 0, base.Width - 18, base.Height - 2);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index != -1)
            {
                if (!darkMode && (e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.DrawString(base.Items[e.Index].ToString(), Font, Brushes.White, e.Bounds.X, e.Bounds.Y);
                }
                else
                {
                    e.Graphics.DrawString(base.Items[e.Index].ToString(), Font, new SolidBrush(ForeColor), e.Bounds.X, e.Bounds.Y);
                }
            }
        }
    }
}