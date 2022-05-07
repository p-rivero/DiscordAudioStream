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
		public enum CaptureTarget
		{
			Invalid,
			Screen,
			Window,
			AllScreens,
			CustomArea
		}

		public delegate void StateChangedDelegate();
		public event StateChangedDelegate StateChanged;


		private IntPtr hWnd = IntPtr.Zero;
		private Screen screen = null;
		private CaptureTarget captureTarget = CaptureTarget.Invalid;
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

		// True if this method supports hiding the taskbar
		public bool HideTaskbarSupported
		{
			get
			{
				// Only BitBlt screen capture supports hiding taskbar
				if (Target != CaptureTarget.Screen) return false;
				return (ScreenMethod == ScreenCaptureMethod.BitBlt);
			}
		}

		// What kind of item are we capturing now?
		public CaptureTarget Target {
			get
			{
				// Default value for captureTarget
				if (captureTarget == CaptureTarget.Invalid)
				{
					throw new InvalidOperationException("Must set either Target, WindowHandle or Screen before reading Target");
				}
				return captureTarget;
			}
			set
			{
				if (value == CaptureTarget.Window || value == CaptureTarget.Screen)
				{
					throw new ArgumentException("Don't set the Target to Window or Screen manually. Instead, set the WindowHandle or Screen");
				}
				if (value == CaptureTarget.Invalid)
				{
					throw new ArgumentException("Don't set the capture target to Invalid");
				}
				CaptureTarget oldValue = captureTarget;
				captureTarget = value;
				if (oldValue != value) StateChanged?.Invoke();
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
				// Update Target
				CaptureTarget oldTarget = captureTarget;
				captureTarget = CaptureTarget.Window;
				if (oldValue != value || oldTarget != CaptureTarget.Window) StateChanged?.Invoke();
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
				// Update Target
				CaptureTarget oldTarget = captureTarget;
				captureTarget = CaptureTarget.Screen;
				if (oldValue != value || oldTarget != CaptureTarget.Screen) StateChanged?.Invoke();
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
	}
}
