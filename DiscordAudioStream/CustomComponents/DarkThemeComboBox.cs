using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents
{
    public class DarkThemeComboBox : ComboBox
    {
        private bool darkMode;
        private bool hovered;

        public class ItemWithSeparator
        {
            private readonly string text;

            public ItemWithSeparator(string text)
            {
                this.text = text;
            }

            public override string ToString()
            {
                return text;
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S2094", Justification =
            "This class cannot be an interface because it needs to be instantiated as a sentinel")]
        public class Dummy { }

        public DarkThemeComboBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            DrawMode = DrawMode.OwnerDrawVariable;
            MouseEnter += (s, e) =>
            {
                hovered = true;
                Refresh();
            };
            MouseLeave += (s, e) =>
            {
                hovered = false;
                Refresh();
            };
            DrawItem += new DrawItemEventHandler(CustomDrawItem);
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

        public override int SelectedIndex
        {
            get => base.SelectedIndex;
            set
            {
                if (value >= Items.Count)
                {
                    base.SelectedIndex = 0;
                }
                else
                {
                    base.SelectedIndex = value;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color hoverColor = darkMode ? DarkThemeManager.DarkHoverColor : DarkThemeManager.LightHoverColor;
            Color rectangleColor = hovered ? hoverColor : BackColor;
            using (Brush brush = new SolidBrush(rectangleColor))
            {
                e.Graphics.FillRectangle(brush, 0, 0, Width, Height - 2);
            }

            e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            using (Brush foreground = new SolidBrush(ForeColor))
            using (Brush background = new SolidBrush(BackColor))
            {
                e.Graphics.DrawString(Text, Font, foreground, 3f, 3f);
                e.Graphics.FillRectangle(background, Width - 18, 0, 18, Height);
                e.Graphics.FillPolygon(foreground, new PointF[3]
                {
                    new PointF(Width - 13, 10f),
                    new PointF(Width - 9, 14f),
                    new PointF(Width - 5, 10f)
                });
            }

            Rectangle bounds = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height - 1);
            ControlPaint.DrawBorder(e.Graphics, bounds, DarkThemeManager.BorderColor, ButtonBorderStyle.Solid);
            using (Pen pen = new Pen(DarkThemeManager.BorderColor))
            {
                e.Graphics.DrawLine(pen, Width - 18, 0, Width - 18, Height - 2);
            }
        }

        private void CustomDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                e.DrawBackground();
                return;
            }
            object item = Items[e.Index];

            if (item is ItemWithSeparator && e.Bounds.Height > ItemHeight)
            {
                Rectangle bounds = e.Bounds;
                bounds.Height -= ItemHeight;
                using (Brush brush = new SolidBrush(e.BackColor))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                }

                float y = e.Bounds.Bottom - ItemHeight / 2;
                using (Pen pen = new Pen(DarkThemeManager.BorderColor))
                {
                    e.Graphics.DrawLine(pen, e.Bounds.Left + 5, y, e.Bounds.Right - 5, y);
                }
            }
            else
            {
                e.DrawBackground();
            }

            bool isSelected = (e.State & DrawItemState.Selected) != 0;
            bool useWhite = !darkMode && isSelected;
            Color fgColor = useWhite ? Color.White : ForeColor;
            using (Brush fgBrush = new SolidBrush(fgColor))
            {
                e.Graphics.DrawString(item.ToString(), Font, fgBrush, e.Bounds.X, e.Bounds.Y);
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (Items[e.Index] is ItemWithSeparator)
            {
                e.ItemHeight += ItemHeight;
            }
            else if (Items[e.Index] is Dummy)
            {
                e.ItemHeight = 0;
            }
            base.OnMeasureItem(e);
        }
    }
}
