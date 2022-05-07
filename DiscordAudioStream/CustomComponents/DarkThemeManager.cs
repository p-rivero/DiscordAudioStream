using DLLs;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CustomComponents
{
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


        public static void formHandleCreated(object sender, EventArgs e)
        {
            EnableDarkTitlebar((sender as Form).Handle, dark: true);
        }

        public static bool IsDarkTheme()
        {
            var osVersionInfo = Ntdll.OsVersionInfoEx.Init();
            Ntdll.RtlGetVersion(ref osVersionInfo);

            if (osVersionInfo.MajorVersion < 10)
                return false;

            try
            {
                return Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", null).ToString() == "0";
            }
            catch
            {
                return false;
            }
        }

        private static void EnableDarkTitlebar(IntPtr handle, bool dark)
        {
            Uxtheme.AllowDarkModeForWindow(handle, dark);
            int num = Marshal.SizeOf(dark);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            User32.WindowCompositionAttribData composition;
            composition.Attribute = User32.WindowCompositionAttribute.WCA_USEDARKMODECOLORS;
            composition.Data = intPtr;
            composition.SizeOfData = num;
            User32.SetWindowCompositionAttribute(handle, ref composition);
            Marshal.FreeHGlobal(intPtr);
        }
    }
}
