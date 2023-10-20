using System.Runtime.InteropServices;

namespace Windows.Win32.UI.WindowsAndMessaging;

public partial struct CURSORINFO
{
    public static CURSORINFO New()
    {
        return new CURSORINFO { cbSize = (uint)Marshal.SizeOf(typeof(CURSORINFO)) };
    }
}
