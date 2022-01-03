using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CustomComponents
{
    public class DarkThemeGroupBox : GroupBox
    {
        private bool darkMode;

        public DarkThemeGroupBox()
        {
            base.Paint += PaintDarkGroupBox;
        }

        public void SetDarkMode(bool dark)
        {
            darkMode = dark;
        }

        public void PaintDarkGroupBox(object sender, PaintEventArgs p)
        {
            if (darkMode)
            {
                GroupBox groupBox = (GroupBox)sender;
                Pen pen = new Pen(new SolidBrush(DarkThemeManager.DarkSecondColor), 1f);
                p.Graphics.Clear(DarkThemeManager.DarkBackColor);
                p.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
                p.Graphics.DrawString(groupBox.Text, groupBox.Font, Brushes.White, -2f, -3f);
                p.Graphics.DrawLine(pen, 0, 20, 0, groupBox.Height - 2);
                p.Graphics.DrawLine(pen, p.Graphics.MeasureString(groupBox.Text, groupBox.Font).Width + 6f, 8f, groupBox.Width - 1, 8f);
                p.Graphics.DrawLine(pen, groupBox.Width - 1, 8, groupBox.Width - 1, groupBox.Height - 2);
                p.Graphics.DrawLine(pen, 0, groupBox.Height - 2, groupBox.Width - 1, groupBox.Height - 2);
            }
        }
    }
}