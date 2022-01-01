using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace quick_screen_recorder
{
	public class ProcessHandleManager
	{
		private static IntPtr[] procs = null;
		private static int selectedIndex = -1;

		public static string[] RefreshHandles()
		{
			ClearSelectedIndex();
			IntPtr shellWindow = User32.GetShellWindow();
			Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

			User32.EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
			{
				// Called for each top-level window

				// Ignore shell
				if (hWnd == shellWindow) return true;

				// Ignore windows without WS_VISIBLE
				if (!User32.IsWindowVisible(hWnd)) return true;

				// Ignore windows with "" as title
				int length = User32.GetWindowTextLength(hWnd);
				if (length == 0) return true;

				// Ignore suspended Windows Store apps
				Dwmapi.DwmGetWindowAttribute(hWnd, (int)Dwmapi.DwmWindowAttribute.CLOAKED, out IntPtr isCloacked, Marshal.SizeOf(typeof(bool)));
				if (isCloacked != IntPtr.Zero) return true;

				StringBuilder builder = new StringBuilder(length);
				User32.GetWindowText(hWnd, builder, length + 1);

				windows[hWnd] = builder.ToString();
				return true;

			}, IntPtr.Zero);

			procs = windows.Keys.ToArray();
			return windows.Values.ToArray();
		}

		public static int SelectedIndex
		{
			get { return selectedIndex; }
			set
			{
				ClearSelectedIndex();

				if (procs == null)
				{
					throw new InvalidOperationException("Please call RefreshHandles() before attempting to set the index");
				}
				if (value < 0 || value >= procs.Length)
				{
					throw new InvalidOperationException("ProcessHandleManager: The provided index is out of bounds");
				}
				selectedIndex = value;

				// Set selected process as topmost
				User32.SetWindowPos(procs[selectedIndex], User32.HWND_TOPMOST, 0, 0, 0, 0, User32.SWP_NOMOVE | User32.SWP_NOSIZE);
			}
		}

		private static bool capturingWindow = false;
		public static bool CapturingWindow
		{
			get { return capturingWindow; }
			set
			{ 
				if (!value) ClearSelectedIndex();
				capturingWindow = value;
			}
		}

		public static IntPtr GetHandle()
		{
			if (selectedIndex < 0 || selectedIndex >= procs.Length)
			{
				throw new InvalidOperationException("Incorrect process index, make sure to set SelectedIndex first");
			}
			return procs[selectedIndex];
		}

		public static void ClearSelectedIndex()
		{
			if (selectedIndex == -1)
				return;

			User32.SetWindowPos(procs[selectedIndex], User32.HWND_NOTOPMOST, 0, 0, 0, 0, User32.SWP_NOMOVE | User32.SWP_NOSIZE);
			selectedIndex = -1;
		}
	}
}
