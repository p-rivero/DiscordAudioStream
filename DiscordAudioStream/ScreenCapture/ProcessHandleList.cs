using DLLs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DiscordAudioStream.ScreenCapture
{
	public class ProcessHandleList
	{

		private readonly IntPtr[] handles = null;
		private readonly string[] processNames = null;

		// Cannot instantiate directly, must call ProcessHandleList.Refresh()
		private ProcessHandleList(Dictionary<IntPtr, string> processes)
		{
			handles = processes.Keys.ToArray();
			processNames = processes.Values.ToArray();
		}

		public static ProcessHandleList Refresh()
		{
			IntPtr shellWindow = User32.GetShellWindow();
			Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

			User32.EnumWindows((hWnd, lParam) =>
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
				try
				{
					if (Dwmapi.GetBoolAttr(hWnd, Dwmapi.DwmWindowAttribute.CLOAKED))
					{
						return true;
					}
				} catch (InvalidOperationException)
				{
					Logger.Log($"Cannot get property CLOAKED of window {hWnd}. This is normal on Windows 7.");
				}

				StringBuilder builder = new StringBuilder(length);
				User32.GetWindowText(hWnd, builder, length + 1);
				string name = builder.ToString();

				// Ignore the "Custom area" window
				if (name == "Recording area - Discord Audio Stream") return true;

				windows[hWnd] = name;
				return true;

			}, IntPtr.Zero);

			return new ProcessHandleList(windows);
		}

		public string[] Names
		{
			get { return processNames; }
		}

		public IntPtr this[int index]
		{
			get
			{
				if (handles == null)
				{
					throw new InvalidOperationException("Call RefreshHandles() before attempting to get a handle");
				}
				if (index < 0 || index >= handles.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return handles[index];
			}
		}

		// Get the index of a given handle
		public int IndexOf(IntPtr handle)
		{
			for (int i = 0; i < handles.Length; i++)
			{
				if (handles[i] == handle)
					return i;
			}
			return -1;
		}
	}
}
