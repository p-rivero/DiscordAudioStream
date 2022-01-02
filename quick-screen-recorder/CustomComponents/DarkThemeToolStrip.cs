using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents
{
    public class DarkThemeToolStrip : ToolStrip
    {
        public void SetDarkMode(bool dark, bool titlebar)
        {
            if (dark)
            {
                if (titlebar)
                {
                    base.BackColor = DarkThemeManager.DarkMainColor;
                }
                else
                {
                    base.BackColor = DarkThemeManager.DarkBackColor;
                }
            }

            base.Renderer = new CustomToolStripSystemRenderer(dark);
        }
    }

    internal class CustomToolStripSystemRenderer : ToolStripSystemRenderer
    {
        private bool darkMode;

        public CustomToolStripSystemRenderer(bool darkMode)
        {
            this.darkMode = darkMode;
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!(e.Item as ToolStripButton).Checked) return;

            if ((e.Item as ToolStripButton).Pressed)
            {
                if (darkMode) e.Graphics.FillRectangle(new SolidBrush(DarkThemeManager.DarkPaleColor), new Rectangle(1, 0, e.Item.Width - 1, e.Item.Height - 3));
                else e.Graphics.FillRectangle(new SolidBrush(DarkThemeManager.LightPaleColor), new Rectangle(1, 0, e.Item.Width - 1, e.Item.Height - 3));
                e.Graphics.DrawRectangle(new Pen(DarkThemeManager.AccentColor), new Rectangle(1, 0, e.Item.Width - 2, e.Item.Height - 3));
            }
            else
            {
                if (darkMode) e.Graphics.FillRectangle(new SolidBrush(DarkThemeManager.DarkPaleColor), new Rectangle(0, 0, e.Item.Width - 2, e.Item.Height - 3));
                else e.Graphics.FillRectangle(new SolidBrush(DarkThemeManager.LightPaleColor), new Rectangle(0, 0, e.Item.Width - 2, e.Item.Height - 3));
                e.Graphics.DrawRectangle(new Pen(DarkThemeManager.AccentColor), new Rectangle(0, 0, e.Item.Width - 2, e.Item.Height - 3));
            }
        }
    }
}
