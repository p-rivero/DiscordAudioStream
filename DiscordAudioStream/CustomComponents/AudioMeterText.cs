using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents;

public class AudioMeterText : Control
{
    private int windowWidth = -1;

    public void SetWindowWidth(int newWidth)
    {
        if (windowWidth != newWidth)
        {
            windowWidth = newWidth;
            Refresh();
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        Refresh();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);

        DrawMeterLine(e.Graphics, "0", 0f);
        DrawMeterLine(e.Graphics, "-20", 0.3333f);
        DrawMeterLine(e.Graphics, "-40", 0.6667f);
        DrawMeterLine(e.Graphics, "-60", 1f);

        if (Height > 500)
        {
            DrawMeterLine(e.Graphics, "-10", 0.1667f);
            DrawMeterLine(e.Graphics, "-30", 0.5f);
            DrawMeterLine(e.Graphics, "-50", 0.8333f);
        }
    }

    private void DrawMeterLine(Graphics g, string text, float percent)
    {
        const int PADDING = 10;
        const int LINE_LENGTH = 3;
        const int WIDTH_SHOW_TEXT = 130;
        const int WIDTH_SHOW_DB = 170;

        text = windowWidth switch
        {
            < WIDTH_SHOW_TEXT => "",
            < WIDTH_SHOW_DB => text,
            _ => $"{text} dB"
        };

        SizeF strSz = g.MeasureString(text, Font);
        float lineY = GetYPos(percent);
        float textY = lineY - strSz.Height / 2;

        using SolidBrush fgBrush = new(ForeColor);
        using Pen fgPen = new(ForeColor);
        if (RightToLeft == RightToLeft.Yes)
        {
            g.DrawString(text, Font, fgBrush, Width - strSz.Width - PADDING, textY);
            g.DrawLine(fgPen, Width - LINE_LENGTH, lineY, Width, lineY);
        }
        else
        {
            g.DrawString(text, Font, fgBrush, PADDING, textY);
            g.DrawLine(fgPen, 0, lineY, LINE_LENGTH, lineY);
        }
    }

    private int GetYPos(float percent)
    {
        const int PADDING_TOP = 11;
        const int PADDING_BOTTOM = 28;
        int bottom = Height - PADDING_BOTTOM;
        return PADDING_TOP + (int)((bottom - PADDING_TOP) * percent);
    }
}
