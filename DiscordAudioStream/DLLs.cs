
using System;
using System.Runtime.InteropServices;
using System.Security;

class GDI32
{
	[DllImport("GDI32.dll")]
	public static extern bool BitBlt(int hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, int hdcSrc, int nXSrc, int nYSrc, int dwRop);
	[DllImport("GDI32.dll")]
	public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);
	[DllImport("GDI32.dll")]
	public static extern int CreateCompatibleDC(int hdc);
	[DllImport("GDI32.dll")]
	public static extern bool DeleteDC(int hdc);
	[DllImport("GDI32.dll")]
	public static extern bool DeleteObject(int hObject);
	[DllImport("GDI32.dll")]
	public static extern int GetDeviceCaps(int hdc, int nIndex);
	[DllImport("GDI32.dll")]
	public static extern int SelectObject(int hdc, int hgdiobj);
}

class User32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct POINTAPI
	{
		public int x;
		public int y;
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct CURSORINFO
	{
		public Int32 cbSize;
		public Int32 flags;
		public IntPtr hCursor;
		public POINTAPI ptScreenPos;

		public static CURSORINFO Init()
		{
			return new CURSORINFO { cbSize = Marshal.SizeOf(typeof(CURSORINFO)) };
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		private int left;
		private int top;
		private int right;
		private int bottom;

		public int X
		{
			get { return left; }
			set { left = value; }
		}
		public int Y
		{
			get { return top; }
			set { top = value; }
		}
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
		public RECT(int x, int y, int width, int height)
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


	public const Int32 CURSOR_SHOWING = 0x00000001;
	public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
	public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
	public const int SWP_NOSIZE = 0x0001;
	public const int SWP_NOMOVE = 0x0002;
	public const int PW_RENDERFULLCONTENT = 0x00000002;
	public const int PW_CLIENTONLY = 0x1;
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
	public static extern int GetDesktopWindow();
	[DllImport("user32.dll")]
	public static extern int GetWindowDC(int hWnd);
	[DllImport("user32.dll")]
	public static extern int ReleaseDC(int hWnd, int hDC);
	[DllImport("user32.dll")]
	public static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);
	[DllImport("user32.dll")]
	public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, uint istepIfAniCur, IntPtr hbrFlickerFreeDraw, uint diFlags);
	[DllImport("user32.dll")]
	public static extern bool GetCursorInfo(out CURSORINFO pci);
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
	public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
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
}

class Ntdll
{
	[SecurityCritical]
	[DllImport("ntdll.dll", SetLastError = true)]
	internal static extern bool RtlGetVersion(ref OSVERSIONINFOEX versionInfo);
	[StructLayout(LayoutKind.Sequential)]
	internal struct OSVERSIONINFOEX
	{
		// The OSVersionInfoSize field must be set to Marshal.SizeOf(typeof(OSVERSIONINFOEX))
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

		public static OSVERSIONINFOEX Init()
		{
			return new OSVERSIONINFOEX { OSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
		}
	}
}

class Dwmapi
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
		LAST
	}

	[DllImport("dwmapi.dll")]
	public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out IntPtr pvAttribute, int cbAttribute);
}

class Uxtheme
{
	[DllImport("uxtheme.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "#133", ExactSpelling = true, SetLastError = true)]
	public static extern bool AllowDarkModeForWindow(IntPtr hWnd, bool allow);
	[DllImport("uxtheme.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "#135", ExactSpelling = true, SetLastError = true)]
	public static extern bool AllowDarkModeForApp(bool allow);
}
