using DiscordAudioStream.ScreenCapture;
using DLLs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DiscordAudioStream
{
    public class ProcessHandleManager
	{
		private IntPtr[] procs = null;
		private int selectedIndex = -1;
		private readonly CaptureState state;


		public ProcessHandleManager(CaptureState state)
        {
			this.state = state ?? throw new ArgumentNullException("state");
		}

		public string[] RefreshHandles()
		{
			ClearTopmostWindow();
			IntPtr shellWindow = User32.GetShellWindow();
			Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

			User32.EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
			{
				// Called for each top-level window

				// Ignore shell
				if (hWnd == shellWindow) return true;

				// Ignore this window
				if (hWnd == Process.GetCurrentProcess().MainWindowHandle) return true;

				// Ignore windows without WS_VISIBLE
				if (!User32.IsWindowVisible(hWnd)) return true;

				// Ignore windows with "" as title
				int length = User32.GetWindowTextLength(hWnd);
				if (length == 0) return true;

				// Ignore suspended Windows Store apps
				if (Dwmapi.GetBoolAttr(hWnd, Dwmapi.DwmWindowAttribute.CLOAKED))
					return true;

				StringBuilder builder = new StringBuilder(length);
				User32.GetWindowText(hWnd, builder, length + 1);
				string name = builder.ToString();

				// Ignore the "Custom area" window
				if (name == "Recording area - Discord Audio Stream") return true;

				windows[hWnd] = name;
				return true;

			}, IntPtr.Zero);

			procs = windows.Keys.ToArray();
			return windows.Values.ToArray();
		}

		public int SelectedIndex
		{
			get { return selectedIndex; }
			set
			{
				ClearTopmostWindow();

				if (procs == null)
				{
					throw new InvalidOperationException("Please call RefreshHandles() before attempting to set the index");
				}
				if (value < 0 || value >= procs.Length)
				{
					throw new InvalidOperationException("ProcessHandleManager: The provided index is out of bounds");
				}
				selectedIndex = value;

				if (state != null && state.RequiresBringWindowToFront)
				{
					// Set selected process as topmost
					User32.SetWindowPos(procs[selectedIndex], User32.HWND_TOPMOST, 0, 0, 0, 0, User32.SWP_NOMOVE | User32.SWP_NOSIZE);
				}
			}
		}

		public IntPtr GetHandle()
		{
			if (selectedIndex < 0 || selectedIndex >= procs.Length)
			{
				return IntPtr.Zero;
			}
			return procs[selectedIndex];
		}

		public void ClearSelectedIndex()
		{
			ClearTopmostWindow();
			selectedIndex = -1;
		}

		// Get the index of a given handle
		public int Lookup(IntPtr handle)
		{
			for (int i = 0; i < procs.Length; i++)
			{
				if (procs[i] == handle)
					return i;
			}
			return -1;
		}


		private void ClearTopmostWindow()
		{
			if (selectedIndex != -1 && state != null && state.RequiresBringWindowToFront)
			{
				User32.SetWindowPos(procs[selectedIndex], User32.HWND_NOTOPMOST, 0, 0, 0, 0, User32.SWP_NOMOVE | User32.SWP_NOSIZE);
			}
		}
	}
}
