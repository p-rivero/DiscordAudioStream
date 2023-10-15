using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents;

public class DarkThemeTextBox : TextBox
{
    public void SetDarkMode(bool dark)
    {
        if (dark)
        {
            BackColor = DarkThemeManager.DarkSecondColor;
            ForeColor = Color.White;
        }
    }
}
