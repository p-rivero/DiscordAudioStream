using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents;

public class DarkThemeToolStrip : ToolStrip
{
    public void SetDarkMode(bool dark, bool titlebar)
    {
        if (dark)
        {
            BackColor = titlebar ? DarkThemeManager.DarkMainColor : DarkThemeManager.DarkBackColor;
        }

        Renderer = new CustomToolStripSystemRenderer(dark);
    }
}

internal class CustomToolStripSystemRenderer : ToolStripSystemRenderer
{
    private readonly bool darkMode;

    public CustomToolStripSystemRenderer(bool darkMode)
    {
        this.darkMode = darkMode;
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // Do not draw border
    }

    protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
    {
        ToolStripButton button = (ToolStripButton)e.Item;
        if (!button.Checked)
        {
            return;
        }

        int xOffset = button.Pressed ? 1 : 0;
        Rectangle buttonArea = new(xOffset, 0, e.Item.Width - 2, e.Item.Height - 3);
        Color backColor = darkMode ? DarkThemeManager.DarkPaleColor : DarkThemeManager.LightPaleColor;
        using Brush backBrush = new SolidBrush(backColor);
        using Pen edgePen = new(DarkThemeManager.AccentColor);
        e.Graphics.FillRectangle(backBrush, buttonArea);
        e.Graphics.DrawRectangle(edgePen, buttonArea);
    }
}
