using System.Drawing;
using System.Windows.Forms;

using Microsoft.Win32;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

namespace CustomComponents;

internal static class DarkThemeManager
{
    public static readonly Color DarkMainColor = Color.Black;
    public static readonly Color DarkBackColor = Color.FromArgb(32, 32, 32);
    public static readonly Color DarkSecondColor = Color.FromArgb(56, 56, 56);
    public static readonly Color DarkPaleColor = Color.FromArgb(0, 58, 105);
    public static readonly Color DarkHoverColor = Color.FromArgb(67, 67, 67);
    public static readonly Color LightMainColor = Color.White;
    public static readonly Color LightBackColor = SystemColors.Control;
    public static readonly Color LightSecondColor = SystemColors.ControlLight;
    public static readonly Color LightPaleColor = Color.FromArgb(192, 216, 235);
    public static readonly Color LightHoverColor = Color.FromArgb(204, 204, 204);
    public static readonly Color BorderColor = Color.FromArgb(100, 100, 100);
    public static readonly Color AccentColor = Color.FromArgb(0, 120, 215);
    public static readonly Color PressedColor = Color.FromArgb(140, 140, 140);

    public static void FormHandleCreated(object? sender, EventArgs e)
    {
        if (sender is Form form)
        {
            EnableDarkTitlebar((HWND)form.Handle, dark: true);
        }
    }

    public static bool IsDarkTheme()
    {
        if (!IsWindows10OrGreater())
        {
            return false;
        }

        try
        {
            string key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            object? light = Registry.GetValue(key, "AppsUseLightTheme", null);
            return light?.ToString() == "0";
        }
        catch
        {
            return false;
        }
    }

    private static void EnableDarkTitlebar(HWND handle, bool dark)
    {
        if (!IsWindows10OrGreater(18985))
        {
            return;
        }
        PInvoke.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, dark)
            .AssertSuccess($"Failed to enable dark titlebar");
    }

    private static bool IsWindows10OrGreater(int build = 0)
    {
        return Environment.OSVersion.Version >= new Version(10, 0, build);
    }
}
