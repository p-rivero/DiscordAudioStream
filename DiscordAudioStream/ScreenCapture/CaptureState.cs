using System;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture
{
	public class CaptureState
	{
		public enum WindowCaptureMethod
		{
			Windows10,
			BitBlt,
			PrintScreen
		}
		public enum ScreenCaptureMethod
		{
			DXGIDuplication,
			Windows10,
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
				if (capturingCursor == value) return; // No changes

				Logger.Log("\nChanging CaptureState... (CapturingCursor = {0})", value);
				capturingCursor = value;
				StateChanged?.Invoke();
				Logger.Log("Done changing CaptureState. CapturingCursor = {0}", value);
			}
		}
		
		// True if the taskbar should be hidden
		public bool HideTaskbar
		{
			get { return hideTaskbar; }
			set
			{
				if (hideTaskbar == value) return; // No changes

				Logger.Log("\nChanging CaptureState... (HideTaskbar = {0})", value);
				hideTaskbar = value;
				StateChanged?.Invoke();
				Logger.Log("Done changing CaptureState. HideTaskbar = {0}", value);
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
				if (captureTarget == value) return; // No changes

				Logger.Log("\nChanging CaptureState... (Target = {0})", value);
				CaptureTarget oldValue = captureTarget;
				captureTarget = value;
				if (oldValue != value) StateChanged?.Invoke();
				Logger.Log("Done changing CaptureState. Target = {0}", value);
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
				if (hWnd == value && captureTarget == CaptureTarget.Window) return; // No changes

				Logger.Log("\nChanging CaptureState... (WindowHandle = {0})", value);
				hWnd = value;
				captureTarget = CaptureTarget.Window;
				screen = null; // Remove screen (if any)
				StateChanged?.Invoke();
				Logger.Log("Done changing CaptureState. WindowHandle = {0}", value);
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
				if (screen == value && captureTarget == CaptureTarget.Screen) return; // No changes

				Logger.Log("\nChanging CaptureState... (Screen = {0})", value);
				screen = value;
				captureTarget = CaptureTarget.Screen;
				hWnd = IntPtr.Zero; // Remove window handle (if any)
				StateChanged?.Invoke();
				Logger.Log("Done changing CaptureState. Screen = {0}", value.DeviceName);
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
					Logger.Log("\nChanging CaptureState... (WindowMethod = {0})", value);
					Properties.Settings.Default.CaptureWindowMethod = (int)value;
					Properties.Settings.Default.Save();
					StateChanged?.Invoke();
					Logger.Log("Done changing CaptureState. WindowMethod = {0}", value);
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
					Logger.Log("\nChanging CaptureState... ScreenMethod = {0}", value);
					Properties.Settings.Default.CaptureScreenMethod = (int)value;
					Properties.Settings.Default.Save();
					StateChanged?.Invoke();
					Logger.Log("Done changing CaptureState. ScreenMethod = {0}", value);
				}
			}
		}
	}
}
