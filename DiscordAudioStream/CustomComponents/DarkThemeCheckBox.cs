using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents;

public class DarkThemeCheckBox : CheckBox
{
    private bool darkMode;

    private string darkText = "";

    private bool hovered;

    private bool pressed;

    public DarkThemeCheckBox()
    {
        if (darkMode)
        {
            SetStyle(ControlStyles.UserPaint, value: true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
            base.MouseEnter += (s, e) => UpdateHovered(true);
            base.MouseLeave += (s, e) => UpdateHovered(false);
            base.MouseDown += (s, e) => UpdatePressed(true);
            base.MouseUp += (s, e) => UpdatePressed(false);
        }
    }

    private void UpdateHovered(bool value)
    {
        hovered = value;
        Refresh();
    }

    private void UpdatePressed(bool value)
    {
        pressed = value;
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
        PaintSquare(pevent.Graphics);
        PaintText(pevent.Graphics);

        if (Checked)
        {
            PaintCheck(pevent.Graphics);
        }
    }

    private void PaintSquare(Graphics g)
    {
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

        int extraThickness = Focused ? 1 : 0;
        int x = extraThickness;
        int y = 2 + extraThickness;
        int size = 12 - extraThickness;
        using SolidBrush bgBrush = new(bgColor);
        using Pen edgePen = new(DarkThemeManager.BorderColor, 1 + extraThickness);
        g.FillRectangle(bgBrush, x, y, size, size);
        g.DrawRectangle(edgePen, x, y, size, size);
    }

    private void PaintText(Graphics g)
    {
        g.TextRenderingHint = TextRenderingHint.SystemDefault;
        Color textColor = Enabled ? ForeColor : DarkThemeManager.BorderColor;
        using SolidBrush textBrush = new(textColor);
        g.DrawString(darkText, Font, textBrush, 17f, 0f);
    }

    private void PaintCheck(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Color checkColor = Enabled ? ForeColor : DarkThemeManager.BorderColor;
        using Pen checkPen = new(checkColor, 2f);
        g.DrawLine(checkPen, 2, 7, 5, 10);
        g.DrawLine(checkPen, 5, 11, 12, 4);
    }
}
