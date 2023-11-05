using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents;

public class ComboBoxItemWithSeparator
{
    private readonly string text;

    public ComboBoxItemWithSeparator(string text)
    {
        this.text = text;
    }

    public override string ToString()
    {
        return text;
    }
}

public class DarkThemeComboBox : ComboBox
{
    private bool darkMode;
    private bool hovered;

    private int numSeparators;
    private readonly object separatorSentinel = new();

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
        DrawItem += CustomDrawItem;
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

    public void RefreshSeparators()
    {
        RemoveSentinels();
        numSeparators = CountItemsWithSeparators();
        AddSentinels(numSeparators);
    }

    public override int SelectedIndex
    {
        get => base.SelectedIndex;
        set => base.SelectedIndex = Math.Min(value, Items.Count - numSeparators - 1);
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
        if (SelectedIndex >= Items.Count - numSeparators)
        {
            SelectedIndex = Items.Count - numSeparators - 1;
        }
        else
        {
            base.OnSelectedIndexChanged(e);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Enter or Keys.Space)
        {
            DroppedDown = !DroppedDown;
            e.Handled = true;
        }
        base.OnKeyDown(e);
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        if (e.KeyChar == '\r')
        {
            e.Handled = true;
        }
        base.OnKeyPress(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Color hoverColor = darkMode ? DarkThemeManager.DarkHoverColor : DarkThemeManager.LightHoverColor;
        Color rectangleColor = hovered ? hoverColor : BackColor;
        using (SolidBrush brush = new(rectangleColor))
        {
            e.Graphics.FillRectangle(brush, 0, 0, Width, Height - 2);
        }

        e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
        using (SolidBrush foreground = new(ForeColor))
        using (SolidBrush background = new(BackColor))
        {
            e.Graphics.DrawString(Text, Font, foreground, 3f, 3f);
            e.Graphics.FillRectangle(background, Width - 18, 0, 18, Height);
            e.Graphics.FillPolygon(foreground, new PointF[3]
            {
                new(Width - 13, 10f),
                new(Width - 9, 14f),
                new(Width - 5, 10f)
            });
        }

        Rectangle bounds = new(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height - 1);
        ControlPaint.DrawBorder(e.Graphics, bounds, DarkThemeManager.BorderColor, ButtonBorderStyle.Solid);
        using Pen pen = new(DarkThemeManager.BorderColor);
        e.Graphics.DrawLine(pen, Width - 18, 0, Width - 18, Height - 2);
    }

    private void CustomDrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index == -1)
        {
            e.DrawBackground();
            return;
        }
        object item = Items[e.Index];

        if (item == separatorSentinel)
        {
            return;
        }

        if (item is ComboBoxItemWithSeparator && e.Bounds.Height > ItemHeight)
        {
            Rectangle bounds = e.Bounds;
            bounds.Height -= ItemHeight;
            using (SolidBrush brush = new(e.BackColor))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            float y = e.Bounds.Bottom - ItemHeight / 2;
            using Pen pen = new(DarkThemeManager.BorderColor);
            e.Graphics.DrawLine(pen, e.Bounds.Left + 5, y, e.Bounds.Right - 5, y);
        }
        else
        {
            e.DrawBackground();
        }

        bool isSelected = (e.State & DrawItemState.Selected) != 0;
        bool forceWhite = !darkMode && isSelected;
        Color fgColor = forceWhite ? Color.White : ForeColor;
        using SolidBrush fgBrush = new(fgColor);
        e.Graphics.DrawString(item.ToString(), Font, fgBrush, e.Bounds.X, e.Bounds.Y);
    }

    protected override void OnMeasureItem(MeasureItemEventArgs e)
    {
        if (Items[e.Index] is ComboBoxItemWithSeparator)
        {
            e.ItemHeight += ItemHeight;
        }
        else if (Items[e.Index] == separatorSentinel)
        {
            e.ItemHeight = 0;
        }
        base.OnMeasureItem(e);
    }

    private void RemoveSentinels()
    {
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            if (Items[i] == separatorSentinel)
            {
                Items.RemoveAt(i);
            }
        }
    }

    private void AddSentinels(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _ = Items.Add(separatorSentinel);
        }
    }

    private int CountItemsWithSeparators()
    {
        return Items.Cast<object>().Count(item => item is ComboBoxItemWithSeparator);
    }
}
