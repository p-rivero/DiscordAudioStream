﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

namespace Windows.Win32;

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

    [DllImport("uxtheme.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "#133", ExactSpelling = true, SetLastError = true)]
    [SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible")]
    public static extern BOOL AllowDarkModeForWindow(HWND hWnd, BOOL allow);

    [DllImport("uxtheme.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "#135", ExactSpelling = true, SetLastError = true)]
    [SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible")]
    public static extern BOOL AllowDarkModeForApp(BOOL allow);
}

public struct DWMWINDOWATTRIBUTE_EXT
{
    public static readonly DWMWINDOWATTRIBUTE_EXT DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = new() { Value = 19 };
    public static readonly DWMWINDOWATTRIBUTE_EXT WCA_USEDARKMODECOLORS = new() { Value = 26 };
    
    private int Value { get; init; }
    public static implicit operator DWMWINDOWATTRIBUTE(DWMWINDOWATTRIBUTE_EXT dwmwindowattribute) => (DWMWINDOWATTRIBUTE)dwmwindowattribute.Value;
}