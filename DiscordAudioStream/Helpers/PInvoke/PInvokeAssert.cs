using System.Runtime.InteropServices;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace Windows.Win32;

public static class PInvokeAssert
{
    public static void AssertSuccess(this HRESULT result, string message = "Win32 call failed")
    {
        if (result.Failed)
        {
            throw new ExternalException(message, result.Value);
        }
    }

    public static void AssertSuccess(this BOOL success, string message = "Win32 call failed")
    {
        ThrowIf(!success, message);
    }

    public static HGDIOBJ AssertSuccess(this HGDIOBJ handle, string message = "Win32 call failed")
    {
        ThrowIf(handle.IsNull || handle == (nint)PInvoke.GDI_ERROR, message);
        return handle;
    }

    public static void AssertNotZero(this uint number, string message = "Win32 call failed")
    {
        ThrowIf(number == 0, message);
    }

    public static void AssertNotZero(this int number, string message = "Win32 call failed")
    {
        ThrowIf(number == 0, message);
    }

    public static HDC AssertNotNull(this HDC handle, string message = "Win32 call failed")
    {
        ThrowIf(handle == IntPtr.Zero, message);
        return handle;
    }

    public static HWND AssertNotNull(this HWND handle, string message = "Win32 call failed")
    {
        ThrowIf(handle.IsNull, message);
        return handle;
    }

    public static HBITMAP AssertNotNull(this HBITMAP handle, string message = "Win32 call failed")
    {
        ThrowIf(handle.IsNull, message);
        return handle;
    }

    private static void ThrowIf(bool condition, string message)
    {
        if (condition)
        {
            throw new ExternalException(message, Marshal.GetLastWin32Error());
        }
    }
}
