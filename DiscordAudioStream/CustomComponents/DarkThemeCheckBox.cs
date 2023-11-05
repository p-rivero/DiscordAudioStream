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
            MouseEnter += (s, e) => UpdateHovered(true);
            MouseLeave += (s, e) => UpdateHovered(false);
            MouseDown += (s, e) => UpdatePressed(true);
            MouseUp += (s, e) => UpdatePressed(false);
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

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            Checked = !Checked;
            e.Handled = true;
        }
        base.OnKeyDown(e);
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
        Color bgColor = (pressed, hovered) switch
        {
            (true, _) => DarkThemeManager.PressedColor,
            (_, true) => DarkThemeManager.DarkHoverColor,
            _ => DarkThemeManager.DarkSecondColor,
        };
        Color edgeColor = Focused ? DarkThemeManager.PressedColor : DarkThemeManager.BorderColor;
        int extraThicknessPx = Focused ? 1 : 0;
        int x = extraThicknessPx;
        int y = 2 + extraThicknessPx;
        int size = 12 - extraThicknessPx;
        using SolidBrush bgBrush = new(bgColor);
        using Pen edgePen = new(edgeColor, 1 + extraThicknessPx);
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
