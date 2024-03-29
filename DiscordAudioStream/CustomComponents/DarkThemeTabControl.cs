﻿using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents;

public class DarkThemeTabControl : TabControl
{
    private readonly StringFormat CenterSringFormat = new()
    {
        Alignment = StringAlignment.Near,
        LineAlignment = StringAlignment.Center
    };

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the background of the tab")]
    public Color BackTabColor { get; set; } = Color.FromArgb(28, 28, 28);

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the border of the control")]
    public Color BorderColor { get; set; } = Color.FromArgb(30, 30, 30);

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the header.")]
    public Color HeaderColor { get; set; } = Color.FromArgb(45, 45, 48);

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the title of the page")]
    public Color TextColor { get; set; } = Color.FromArgb(255, 255, 255);

    public DarkThemeTabControl()
    {
        SetStyle(
            ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer,
            value: true
        );
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Normal;
        ItemSize = new(240, 16);
        AllowDrop = true;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            CenterSringFormat.Dispose();
        }
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Top;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics graphics = e.Graphics;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        graphics.Clear(HeaderColor);
        SelectedTab.BackColor = BackTabColor;
        SelectedTab.BorderStyle = BorderStyle.None;

        for (int i = 0; i <= TabCount - 1; i++)
        {
            Rectangle tabRect = GetTabRect(i);
            Rectangle rectangle = new(tabRect.Location.X + 2, tabRect.Location.Y, tabRect.Width, tabRect.Height);

            if (i == SelectedIndex)
            {
                using SolidBrush brush = new(BackTabColor);
                graphics.FillRectangle(brush, rectangle.X - 3, rectangle.Y - 3, rectangle.Width, rectangle.Height + 5);

                if (Focused)
                {
                    using Pen pen = new(TextColor) { DashPattern = new float[2] { 2f, 2f } };
                    graphics.DrawRectangle(pen, tabRect);
                }
            }

            using SolidBrush textBrush = new(TextColor);
            graphics.DrawString(TabPages[i].Text, Font, textBrush, rectangle, CenterSringFormat);
        }

        using SolidBrush backTabBrush = new(BackTabColor);
        using Pen borderPen = new(BorderColor, 2f);
        graphics.FillRectangle(backTabBrush, new Rectangle(0, 20, Width, Height - 20));
        graphics.DrawRectangle(borderPen, new Rectangle(0, 0, Width, Height));
    }
}
