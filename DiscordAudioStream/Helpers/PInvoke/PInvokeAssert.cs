using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace Windows.Win32;

[SuppressMessage("Usage", "CA2201:Do not raise reserved exception types",
    Justification = "Throwing ExternalException in a P/Invoke helper seems correct")]
public static class PInvokeAssert
{
    private const string DEFAULT_MESSAGE = "Win32 call failed";

    public static void AssertSuccess(this HRESULT result, string message = DEFAULT_MESSAGE)
    {
        if (result.Failed)
        {
            throw new ExternalException(message, result.Value);
        }
    }

    public static void AssertSuccess(this BOOL success, string message = DEFAULT_MESSAGE)
    {
        ThrowIf(!success, message);
    }

    public static HGDIOBJ AssertSuccess(this HGDIOBJ handle, string message = DEFAULT_MESSAGE)
    {
        ThrowIf(handle.IsNull || handle == (nint)PInvoke.GDI_ERROR, message);
        return handle;
    }

    public static void AssertNotZero(this uint number, string message = DEFAULT_MESSAGE)
    {
        ThrowIf(number == 0, message);
    }

    public static void AssertNotZero(this int number, string message = DEFAULT_MESSAGE)
    {
        ThrowIf(number == 0, message);
    }

    public static HDC AssertNotNull(this HDC handle, string message = DEFAULT_MESSAGE)
    {
        ThrowIf(handle == IntPtr.Zero, message);
        return handle;
    }

    public static HWND AssertNotNull(this HWND handle, string message = DEFAULT_MESSAGE)
    {
        ThrowIf(handle.IsNull, message);
        return handle;
    }

    public static HBITMAP AssertNotNull(this HBITMAP handle, string message = DEFAULT_MESSAGE)
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
