using System.Windows.Forms;

namespace Windows.Win32.Foundation;

public static class FormExtensions
{
    public static HWND HWnd(this Form form)
    {
        return (HWND)form.Handle;
    }
}
