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

        int xOffset = button.Pressed ? 1 : 0;
        Rectangle buttonArea = new(xOffset, 0, e.Item.Width - 2, e.Item.Height - 3);

        if (button.Checked)
        {
            PaintBackground(e.Graphics, buttonArea);
            using Pen edgePen = new(DarkThemeManager.AccentColor);
            e.Graphics.DrawRectangle(edgePen, buttonArea);
        }
        if (e.Item.Selected)
        {
            using Pen edgePen = new(DarkThemeManager.BorderColor) { DashPattern = new float[2] { 2f, 2f } };
            e.Graphics.DrawRectangle(edgePen, buttonArea);
        }
    }

    private void PaintBackground(Graphics g, Rectangle buttonArea)
    {
        Color backColor = darkMode ? DarkThemeManager.DarkPaleColor : DarkThemeManager.LightPaleColor;
        using SolidBrush backBrush = new(backColor);
        g.FillRectangle(backBrush, buttonArea);
    }
}
