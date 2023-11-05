using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents;

public class DarkThemeTextBox : TextBox
{
    public void SetDarkMode(bool dark)
    {
        // Enable auto-complete to get Ctrl+Backspace to work.
        AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        AutoCompleteSource = AutoCompleteSource.CustomSource;
        if (dark)
        {
            BackColor = DarkThemeManager.DarkSecondColor;
            ForeColor = Color.White;
        }
    }
}
