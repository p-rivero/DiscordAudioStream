
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DLLs
{
	internal static class Gdi32
	{
		public enum RasterOps : uint
		{
			SRCCOPY = 0x00CC0020,
			SRCPAINT = 0x00EE0086,
			SRCAND = 0x008800C6,
			SRCINVERT = 0x00660046,
			SRCERASE = 0x00440328,
			NOTSRCCOPY = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY = 0x00C000CA,
			MERGEPAINT = 0x00BB0226,
			PATCOPY = 0x00F00021,
			PATPAINT = 0x00FB0A09,
			PATINVERT = 0x005A0049,
			DSTINVERT = 0x00550009,
			BLACKNESS = 0x00000042,
			WHITENESS = 0x00FF0062,
			CAPTUREBLT = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
		}

		[DllImport("GDI32.dll")]
		public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOps dwRop);
		[DllImport("GDI32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
		[DllImport("GDI32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
		[DllImport("GDI32.dll")]
		public static extern bool DeleteDC(IntPtr hdc);
		[DllImport("GDI32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
		[DllImport("GDI32.dll")]
		public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
		[DllImport("GDI32.dll")]
		public static extern int SelectObject(IntPtr hdc, IntPtr hgdiobj);
	}

	internal static class User32
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct PointApi
		{
			public int x;
			public int y;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct CursorInfo
		{
			public Int32 cbSize;
			public Int32 flags;
			public IntPtr hCursor;
			public PointApi ptScreenPos;

			public static CursorInfo Init()
			{
				return new CursorInfo { cbSize = Marshal.SizeOf(typeof(CursorInfo)) };
			}
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct Rect
		{
			public readonly int left;
			public readonly int top;
			private int right;
			private int bottom;

			public int Height
			{
				get { return bottom - top; }
				set { bottom = value + top; }
			}
			public int Width
			{
				get { return right - left; }
				set { right = value + left; }
			}
			public Rect(int x, int y, int width, int height)
			{
				left = x;
				top = y;
				right = width + x;
				bottom = height + y;
			}
		}
		public enum WindowCompositionAttribute
		{
			WCA_UNDEFINED = 0,
			WCA_NCRENDERING_ENABLED,
			WCA_NCRENDERING_POLICY,
			WCA_TRANSITIONS_FORCEDISABLED,
			WCA_ALLOW_NCPAINT,
			WCA_CAPTION_BUTTON_BOUNDS,
			WCA_NONCLIENT_RTL_LAYOUT,
			WCA_FORCE_ICONIC_REPRESENTATION,
			WCA_EXTENDED_FRAME_BOUNDS,
			WCA_HAS_ICONIC_BITMAP,
			WCA_THEME_ATTRIBUTES,
			WCA_NCRENDERING_EXILED,
			WCA_NCADORNMENTINFO,
			WCA_EXCLUDED_FROM_LIVEPREVIEW,
			WCA_VIDEO_OVERLAY_ACTIVE,
			WCA_FORCE_ACTIVEWINDOW_APPEARANCE,
			WCA_DISALLOW_PEEK,
			WCA_CLOAK,
			WCA_CLOAKED,
			WCA_ACCENT_POLICY,
			WCA_FREEZE_REPRESENTATION,
			WCA_EVER_UNCLOAKED,
			WCA_VISUAL_OWNER,
			WCA_HOLOGRAPHIC,
			WCA_EXCLUDED_FROM_DDA,
			WCA_PASSIVEUPDATEMODE,
			WCA_USEDARKMODECOLORS,
			WCA_LAST
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct WindowCompositionAttribData
		{
			public WindowCompositionAttribute Attribute;
			public IntPtr Data;
			public int SizeOfData;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct IconInfo
		{
			public bool fIcon;
			public int xHotspot;
			public int yHotspot;
			public IntPtr hbmMask;
			public IntPtr hbmColor;
		}


		public const int CURSOR_SHOWING = 0x00000001;
		public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
		public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
		public const int SWP_NOSIZE = 0x0001;
		public const int SWP_NOMOVE = 0x0002;
		public const int PW_RENDERFULLCONTENT = 0x00000002;
		public const int PW_CLIENTONLY = 0x1;
		public const int DI_NORMAL = 0x0003;
		public const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4;
		public enum FsModifiers
		{
			NONE = 0x0000,
			ALT = 0x0001,
			CONTROL = 0x0002,
			SHIFT = 0x0004,
			WIN = 0x0008,
			NOREPEAT = 0x4000
		}
		// Delegate called for each window 
		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
		[DllImport("user32.dll")]
		public static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);
		[DllImport("user32.dll")]
		public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, uint istepIfAniCur, IntPtr hbrFlickerFreeDraw, uint diFlags);
		[DllImport("user32.dll")]
		public static extern bool GetCursorInfo(ref CursorInfo pci);
		[DllImport("user32.dll")]
		public static extern bool GetIconInfo(IntPtr hIcon, out IconInfo piconinfo);
		[DllImport("user32.dll")]
		public static extern bool SetProcessDPIAware();
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, FsModifiers fsModifiers, System.Windows.Forms.Keys vk);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
		[DllImport("user32.dll")]
		public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
		[DllImport("user32.dll")]
		public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
		[DllImport("user32.dll")]
		public static extern IntPtr GetShellWindow();
		[DllImport("user32.dll")]
		public static extern int GetWindowText(IntPtr IntPtr, System.Text.StringBuilder lpString, int nMaxCount);
		[DllImport("user32.dll")]
		public static extern int GetWindowTextLength(IntPtr IntPtr);
		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr IntPtr);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
		[DllImport("user32.dll")]
		public static extern bool SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttribData data);
		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetProcessDpiAwarenessContext(int value);
	}

	internal static class Kernel32
	{
		public const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AttachConsole(uint dwProcessId);
	}

	internal static class Ntdll
	{
		[SecurityCritical]
		[DllImport("ntdll.dll", SetLastError = true)]
		internal static extern bool RtlGetVersion(ref OsVersionInfoEx versionInfo);
		[StructLayout(LayoutKind.Sequential)]
		internal struct OsVersionInfoEx
		{
			// The OSVersionInfoSize field must be set to Marshal.SizeOf(typeof(OsVersionInfoEx))
			internal int OSVersionInfoSize;
			internal int MajorVersion;
			internal int MinorVersion;
			internal int BuildNumber;
			internal int PlatformId;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string CSDVersion;
			internal ushort ServicePackMajor;
			internal ushort ServicePackMinor;
			internal short SuiteMask;
			internal byte ProductType;
			internal byte Reserved;

			public static OsVersionInfoEx Init()
			{
				return new OsVersionInfoEx { OSVersionInfoSize = Marshal.SizeOf(typeof(OsVersionInfoEx)) };
			}
		}
	}

	internal static class Dwmapi
	{
		public enum DwmWindowAttribute
		{
			NCRENDERING_ENABLED = 1,
			NCRENDERING_POLICY,
			TRANSITIONS_FORCEDISABLED,
			ALLOW_NCPAINT,
			CAPTION_BUTTON_BOUNDS,
			NONCLIENT_RTL_LAYOUT,
			FORCE_ICONIC_REPRESENTATION,
			FLIP3D_POLICY,
			EXTENDED_FRAME_BOUNDS,
			HAS_ICONIC_BITMAP,
			DISALLOW_PEEK,
			EXCLUDED_FROM_PEEK,
			CLOAK,
			CLOAKED,
			FREEZE_REPRESENTATION,
			PASSIVE_UPDATE_MODE,
			USE_HOSTBACKDROPBRUSH,
			USE_IMMERSIVE_DARK_MODE_OLD = 19,
			USE_IMMERSIVE_DARK_MODE = 20,
			WINDOW_CORNER_PREFERENCE = 33,
			BORDER_COLOR,
			CAPTION_COLOR,
			TEXT_COLOR,
			VISIBLE_FRAME_BORDER_THICKNESS,
			SYSTEMBACKDROP_TYPE,
			LAST
		}

		[DllImport("dwmapi.dll")]
		private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out bool pvAttribute, int cbAttribute);
		[DllImport("dwmapi.dll")]
		private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out User32.Rect pvAttribute, int cbAttribute);
		[DllImport("dwmapi.dll")]
		private static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute attr, ref int attrValue, int attrSize);

		public static bool GetBoolAttr(IntPtr hwnd, DwmWindowAttribute attribute)
		{
			int result = DwmGetWindowAttribute(hwnd, (int)attribute, out bool pvAttribute, Marshal.SizeOf(typeof(bool)));
			if (result != 0)
			{
				throw new InvalidOperationException($"DwmGetWindowAttribute returned 0x{result:X}");
			}
			return pvAttribute;
		}
		public static User32.Rect GetRectAttr(IntPtr hwnd, DwmWindowAttribute attribute)
		{
			int result = DwmGetWindowAttribute(hwnd, (int)attribute, out User32.Rect pvAttribute, Marshal.SizeOf(typeof(User32.Rect)));
			if (result != 0)
			{
				throw new InvalidOperationException($"DwmGetWindowAttribute returned 0x{result:X}");
			}
			return pvAttribute;
		}
		public static void SetBoolAttr(IntPtr hwnd, DwmWindowAttribute attribute, bool value)
		{
			int intVal = value ? 1 : 0;
			int result = DwmSetWindowAttribute(hwnd, attribute, ref intVal, Marshal.SizeOf(typeof(int)));
			if (result != 0)
			{
				throw new InvalidOperationException($"DwmSetWindowAttribute returned 0x{result:X}");
			}
		}
	}

	internal static class Uxtheme
	{
		[DllImport("uxtheme.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "#133", ExactSpelling = true, SetLastError = true)]
		public static extern bool AllowDarkModeForWindow(IntPtr hWnd, bool allow);
		[DllImport("uxtheme.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "#135", ExactSpelling = true, SetLastError = true)]
		public static extern bool AllowDarkModeForApp(bool allow);
	}
}
