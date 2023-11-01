using System.Windows.Forms;

using DiscordAudioStream.Properties;

using Windows.Win32.Foundation;

namespace DiscordAudioStream.VideoCapture;

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
        None,
        Screen,
        Window,
        AllScreens,
        CustomArea
    }

    public event Action? StateChanged;

    private HWND hWnd = HWND.Null;
    private Screen? screen;
    private CaptureTarget captureTarget = CaptureTarget.None;
    private bool capturingCursor;
    private bool hideTaskbar;

    public bool HideTaskbarSupported => Target == CaptureTarget.Screen;

    public CaptureTarget Target
    {
        get
        {
            if (captureTarget == CaptureTarget.None)
            {
                throw new InvalidOperationException("Must set either Target, WindowHandle or Screen before reading Target");
            }
            return captureTarget;
        }
        set
        {
            if (value is CaptureTarget.Window or CaptureTarget.Screen)
            {
                throw new ArgumentException("Don't set the Target to Window or Screen manually. Instead, set the WindowHandle or Screen");
            }
            if (value == CaptureTarget.None)
            {
                throw new ArgumentException("Don't set the capture target to None");
            }
            if (captureTarget == value)
            {
                return; // No changes
            }

            Logger.EmptyLine();
            Logger.Log($"Changing CaptureState... (Target = {value})");
            CaptureTarget oldValue = captureTarget;
            captureTarget = value;
            if (oldValue != value)
            {
                StateChanged?.Invoke();
            }
            Logger.Log($"Done changing CaptureState. Target = {value}");
        }
    }

    public HWND WindowHandle
    {
        get
        {
            if (hWnd.IsNull)
            {
                throw new InvalidOperationException("Trying to get WindowHandle without setting it first");
            }
            return hWnd;
        }
        set
        {
            if (value.IsNull)
            {
                throw new ArgumentException("Trying to set WindowHandle to null value");
            }
            if (hWnd == value && captureTarget == CaptureTarget.Window)
            {
                return; // No changes
            }

            Logger.EmptyLine();
            Logger.Log($"Changing CaptureState... (WindowHandle = {value})");
            hWnd = value;
            captureTarget = CaptureTarget.Window;
            screen = null; // Remove screen (if any)
            StateChanged?.Invoke();
            Logger.Log($"Done changing CaptureState. WindowHandle = {value}");
        }
    }

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
                throw new ArgumentNullException(nameof(value));
            }
            if (screen == value && captureTarget == CaptureTarget.Screen)
            {
                return; // No changes
            }

            Logger.EmptyLine();
            Logger.Log($"Changing CaptureState... (Screen = {value})");
            screen = value;
            captureTarget = CaptureTarget.Screen;
            hWnd = HWND.Null; // Remove window handle (if any)
            StateChanged?.Invoke();
            Logger.Log($"Done changing CaptureState. Screen = {value.DeviceName}");
        }
    }

    public bool CapturingCursor
    {
        get => capturingCursor;
        set
        {
            if (CapturingCursor != value)
            {
                Logger.EmptyLine();
                Logger.Log($"Changing CaptureState... (CapturingCursor = {value})");
                capturingCursor = value;
                StateChanged?.Invoke();
                Logger.Log($"Done changing CaptureState. CapturingCursor = {value}");
            }
        }
    }

    public bool HideTaskbar
    {
        get => hideTaskbar;
        set
        {
            if (HideTaskbar != value)
            {
                Logger.EmptyLine();
                Logger.Log($"Changing CaptureState... (HideTaskbar = {value})");
                hideTaskbar = value;
                StateChanged?.Invoke();
                Logger.Log($"Done changing CaptureState. HideTaskbar = {value}");
            }
        }
    }

    public WindowCaptureMethod WindowMethod
    {
        get => (WindowCaptureMethod)Settings.Default.CaptureWindowMethod;
        set
        {
            if (WindowMethod != value)
            {
                Logger.EmptyLine();
                Logger.Log($"Changing CaptureState... (WindowMethod = {value})");
                Settings.Default.CaptureWindowMethod = (int)value;
                StateChanged?.Invoke();
                Logger.Log($"Done changing CaptureState. WindowMethod = {value}");
            }
        }
    }

    public ScreenCaptureMethod ScreenMethod
    {
        get => (ScreenCaptureMethod)Settings.Default.CaptureScreenMethod;
        set
        {
            if (ScreenMethod != value)
            {
                Logger.EmptyLine();
                Logger.Log($"Changing CaptureState... ScreenMethod = {value}");
                Settings.Default.CaptureScreenMethod = (int)value;
                StateChanged?.Invoke();
                Logger.Log($"Done changing CaptureState. ScreenMethod = {value}");
            }
        }
    }
}
