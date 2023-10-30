using System.Runtime.InteropServices;

namespace Windows.Win32.UI.WindowsAndMessaging;

[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types")]
public partial struct CURSORINFO
{
    public static CURSORINFO New()
    {
        return new() { cbSize = (uint)Marshal.SizeOf(typeof(CURSORINFO)) };
    }
}
