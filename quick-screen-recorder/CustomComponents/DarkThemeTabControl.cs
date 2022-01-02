﻿using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents
{
    public class DarkThemeTabControl : TabControl
    {
        private readonly StringFormat CenterSringFormat = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        private Color activeColor = Color.FromArgb(0, 122, 204);
        private Color backTabColor = Color.FromArgb(28, 28, 28);
        private Color borderColor = Color.FromArgb(30, 30, 30);
        private Color closingButtonColor = Color.WhiteSmoke;
        private Color headerColor = Color.FromArgb(45, 45, 48);
        private Color horizLineColor = Color.FromArgb(0, 122, 204);
        private Color textColor = Color.FromArgb(255, 255, 255);
        public Color selectedTextColor = Color.FromArgb(255, 255, 255);

        private string closingMessage;
        private TabPage predraggedTab;

        public bool ShowClosingButton
        {
            get;
            set;
        }
        
        public bool DraggableTabs
        {
            get;
            set;
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the selected page")]
        public Color ActiveColor
        {
            get
            {
                return activeColor;
            }
            set
            {
                activeColor = value;
            }
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the background of the tab")]
        public Color BackTabColor
        {
            get
            {
                return backTabColor;
            }
            set
            {
                backTabColor = value;
            }
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the border of the control")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
            }
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the closing button")]
        public Color ClosingButtonColor
        {
            get
            {
                return closingButtonColor;
            }
            set
            {
                closingButtonColor = value;
            }
        }

        [Category("Options")]
        [Browsable(true)]
        [Description("The message that will be shown before closing.")]
        public string ClosingMessage
        {
            get
            {
                return closingMessage;
            }
            set
            {
                closingMessage = value;
            }
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the header.")]
        public Color HeaderColor
        {
            get
            {
                return headerColor;
            }
            set
            {
                headerColor = value;
            }
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the horizontal line which is located under the headers of the pages.")]
        public Color HorizontalLineColor
        {
            get
            {
                return horizLineColor;
            }
            set
            {
                horizLineColor = value;
            }
        }

        [Category("Options")]
        [Browsable(true)]
        [Description("Show a Yes/No message before closing?")]
        public bool ShowClosingMessage
        {
            get;
            set;
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the title of the page")]
        public Color SelectedTextColor
        {
            get
            {
                return selectedTextColor;
            }
            set
            {
                selectedTextColor = value;
            }
        }

        [Category("Colors")]
        [Browsable(true)]
        [Description("The color of the title of the page")]
        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
            }
        }

        public DarkThemeTabControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
            DoubleBuffered = true;
            base.SizeMode = TabSizeMode.Normal;
            base.ItemSize = new Size(240, 16);
            AllowDrop = true;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            base.Alignment = TabAlignment.Top;
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            TabPage tabPage = (TabPage)drgevent.Data.GetData(typeof(TabPage));
            TabPage pointedTab = getPointedTab();
            if (tabPage == predraggedTab && pointedTab != null)
            {
                drgevent.Effect = DragDropEffects.Move;
                if (pointedTab != tabPage)
                {
                    ReplaceTabPages(tabPage, pointedTab);
                }
            }

            base.OnDragOver(drgevent);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DraggableTabs) predraggedTab = getPointedTab();
            Point location = e.Location;
            if (ShowClosingButton)
            {
                for (int i = 0; i < base.TabCount; i++)
                {
                    Rectangle tabRect = GetTabRect(i);
                    tabRect.Offset(tabRect.Width - 15, 2);
                    tabRect.Width = 10;
                    tabRect.Height = 10;
                    if (!tabRect.Contains(location))
                    {
                        continue;
                    }

                    if (ShowClosingMessage)
                    {
                        if (DialogResult.Yes == MessageBox.Show(ClosingMessage, "Close", MessageBoxButtons.YesNo))
                        {
                            base.TabPages.RemoveAt(i);
                        }
                    }
                    else
                    {
                        base.TabPages.RemoveAt(i);
                    }
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && predraggedTab != null)
            {
                DoDragDrop(predraggedTab, DragDropEffects.Move);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            predraggedTab = null;
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.Clear(headerColor);
            try
            {
                base.SelectedTab.BackColor = backTabColor;
            }
            catch
            {
            }

            try
            {
                base.SelectedTab.BorderStyle = BorderStyle.None;
            }
            catch
            {
            }

            for (int i = 0; i <= base.TabCount - 1; i++)
            {
                Rectangle rectangle = new Rectangle(new Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y), new Size(GetTabRect(i).Width, GetTabRect(i).Height));
                Rectangle rectangle2 = new Rectangle(rectangle.Location, new Size(rectangle.Width, rectangle.Height));
                Brush brush = new SolidBrush(closingButtonColor);
                if (i == base.SelectedIndex)
                {
                    graphics.FillRectangle(new SolidBrush(headerColor), rectangle2);
                    graphics.FillRectangle(new SolidBrush(activeColor), new Rectangle(rectangle.X - 5, rectangle.Y - 3, rectangle.Width, rectangle.Height + 5));
                    graphics.DrawString(base.TabPages[i].Text, Font, new SolidBrush(selectedTextColor), rectangle2, CenterSringFormat);
                    if (ShowClosingButton)
                    {
                        e.Graphics.DrawString("X", Font, brush, rectangle2.Right - 17, 3f);
                    }
                }
                else
                {
                    graphics.DrawString(base.TabPages[i].Text, Font, new SolidBrush(textColor), rectangle2, CenterSringFormat);
                }
            }

            graphics.DrawLine(new Pen(horizLineColor, 5f), new Point(0, 19), new Point(base.Width, 19));
            graphics.FillRectangle(new SolidBrush(backTabColor), new Rectangle(0, 20, base.Width, base.Height - 20));
            graphics.DrawRectangle(new Pen(borderColor, 2f), new Rectangle(0, 0, base.Width, base.Height));
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        private TabPage getPointedTab()
        {
            for (int i = 0; i <= base.TabPages.Count - 1; i++)
            {
                if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
                {
                    return base.TabPages[i];
                }
            }

            return null;
        }

        private void ReplaceTabPages(TabPage Source, TabPage Destination)
        {
            int num = base.TabPages.IndexOf(Source);
            int num2 = base.TabPages.IndexOf(Destination);
            base.TabPages[num2] = Source;
            base.TabPages[num] = Destination;
            if (base.SelectedIndex == num)
            {
                base.SelectedIndex = num2;
            }
            else if (base.SelectedIndex == num2)
            {
                base.SelectedIndex = num;
            }

            Refresh();
        }
    }
}