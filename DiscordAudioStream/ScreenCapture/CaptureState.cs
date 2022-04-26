using System;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture
{
	public class CaptureState
	{
		public enum WindowCaptureMethod
		{
			DirectX,
			BitBlt,
			PrintScreen
		}
		public enum ScreenCaptureMethod
		{
			DirectX,
			BitBlt
		}

		public delegate void StateChangedDelegate();
		public event StateChangedDelegate StateChanged;


		private IntPtr hWnd = IntPtr.Zero;
		private Screen screen = null;
		private bool capturingCursor;
		private bool hideTaskbar;


		// True if the cursor should be captured
		public bool CapturingCursor
		{
			get { return capturingCursor; }
			set
			{
				bool oldValue = capturingCursor;
				capturingCursor = value;
				if (oldValue != value) StateChanged?.Invoke();
			}
		}
		
		// True if the taskbar should be hidden
		public bool HideTaskbar
		{
			get { return hideTaskbar; }
			set
			{
				bool oldValue = hideTaskbar;
				hideTaskbar = value;
				if (oldValue != value) StateChanged?.Invoke();
			}
		}

		// True if capturing a window, false if capturing a monitor
		public bool CapturingWindow {
			get
			{
				if (hWnd == IntPtr.Zero && screen == null)
				{
					throw new InvalidOperationException("Must set either WindowHandle or Screen before calling CapturingWindow");
				}
				return hWnd != IntPtr.Zero;
			}
		}

		// Handle to the captured window (if any)
		public IntPtr WindowHandle
		{
			get
			{
				if (hWnd == IntPtr.Zero)
				{
					throw new InvalidOperationException("Trying to get WindowHandle without setting it first");
				}
				return hWnd;
			}
			set
			{
				if (value == IntPtr.Zero)
				{
					throw new ArgumentException("Trying to set WindowHandle to IntPtr.Zero");
				}
				// Remove screen (if any)
				screen = null;
				// Set new handle
				IntPtr oldValue = hWnd;
				hWnd = value;
				if (oldValue != value) StateChanged?.Invoke();
			}
		}

		// Captured screen (if any)
		public Screen Screen
		{
			get
			{
				if (screen == null)
				{
					throw new InvalidOperationException("Trying to get Screen without setting it first");
				}
				return screen;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				// Remove window handle (if any)
				hWnd = IntPtr.Zero;
				// Set new screen
				Screen oldValue = screen;
				screen = value;
				if (oldValue != value) StateChanged?.Invoke();
			}
		}

		public WindowCaptureMethod WindowMethod
		{
			get
			{
				int method = Properties.Settings.Default.CaptureWindowMethod;
				return (WindowCaptureMethod) method;
			}
			set
			{
				WindowCaptureMethod oldValue = WindowMethod;
				if (oldValue != value)
				{
					Properties.Settings.Default.CaptureWindowMethod = (int)value;
					Properties.Settings.Default.Save();
					StateChanged?.Invoke();
				}
			}
		}

		public ScreenCaptureMethod ScreenMethod
		{
			get
			{
				int method = Properties.Settings.Default.CaptureScreenMethod;
				return (ScreenCaptureMethod)method;
			}
			set
			{
				ScreenCaptureMethod oldValue = ScreenMethod;
				if (oldValue != value)
				{
					Properties.Settings.Default.CaptureScreenMethod = (int)value;
					Properties.Settings.Default.Save();
					StateChanged?.Invoke();
				}
			}
		}

		public bool RequiresBringWindowToFront
		{
			get
			{
				// If we are capturing the screen, there is no need to bring windows to front
				if (!CapturingWindow) return false;

				switch (WindowMethod)
				{
					case WindowCaptureMethod.DirectX:
						return false;
					case WindowCaptureMethod.BitBlt:
						return true;
					case WindowCaptureMethod.PrintScreen:
						return false;
					default:
						throw new InvalidOperationException("Invalid Window capture method");
				}
			}
		}

	}
}
