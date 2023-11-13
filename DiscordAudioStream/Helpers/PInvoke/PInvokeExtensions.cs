using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.System.Threading;
using Windows.Win32.UI.Controls;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Windows.Win32;

[SuppressMessage("SonarQube", "S6640", Justification = "Unsafe methods have been reviewed")]
public static partial class PInvoke
{
    public static unsafe string GetWindowText(HWND hWnd, int nMaxCount)
    {
        Span<char> buffer = stackalloc char[nMaxCount];
        fixed (char* bufferPtr = buffer)
        {
            int length = GetWindowText(hWnd, bufferPtr, nMaxCount);
            return buffer.Slice(0, length).ToString();
        }
    }

    public static unsafe string QueryFullProcessImageName(HANDLE hProcess, PROCESS_NAME_FORMAT dwFlags, int nMaxCount)
    {
        Span<char> buffer = stackalloc char[nMaxCount];
        uint dwSize = (uint)nMaxCount;
        fixed (char* bufferPtr = buffer)
        {
            QueryFullProcessImageName(hProcess, dwFlags, bufferPtr, ref dwSize).AssertSuccess("QueryFullProcessImageName failed");
            return buffer.Slice(0, (int)dwSize).ToString();
        }
    }

    public static unsafe uint GetWindowThreadProcessId(HWND hWnd, out uint processId)
    {
        fixed (uint* processIdPtr = &processId)
        {
            return GetWindowThreadProcessId(hWnd, processIdPtr);
        }
    }

    public static unsafe HRESULT LoadIconMetric(HINSTANCE hinst, PCWSTR pszName, _LI_METRIC lims, out HICON hico)
    {
        fixed (HICON* phico = &hico)
        {
            return LoadIconMetric(hinst, pszName, lims, phico);
        }
    }

    public static unsafe HRESULT DwmGetWindowAttribute<T>(HWND hwnd, DWMWINDOWATTRIBUTE dwAttribute, out T attr) where T : unmanaged
    {
        fixed (T* attrPtr = &attr)
        {
            return DwmGetWindowAttribute(hwnd, dwAttribute, attrPtr, SizeOf<T>());
        }
    }

    public static unsafe HRESULT DwmSetWindowAttribute<T>(HWND hWnd, DWMWINDOWATTRIBUTE attribute, in T value) where T : unmanaged
    {
        fixed (T* attrPtr = &value)
        {
            return DwmSetWindowAttribute(hWnd, attribute, attrPtr, SizeOf<T>());
        }
    }

    private static uint SizeOf<T>()
    {
        return (uint)Marshal.SizeOf(typeof(T));
    }
}

