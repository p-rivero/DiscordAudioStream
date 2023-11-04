using System.Windows.Forms;

using Windows.Win32.Foundation;

namespace Windows.Win32;

public static class FormExtensions
{
    public static HWND HWnd(this Form form)
    {
        return (HWND)form.Handle;
    }

    public static void AssertSuccess(this bool success, string message = "Expected TRUE value")
    {
        ((BOOL)success).AssertSuccess(message);
    }
}
