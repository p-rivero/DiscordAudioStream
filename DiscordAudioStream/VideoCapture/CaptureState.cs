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
        PrintScreen,
    }

    public enum ScreenCaptureMethod
    {
        DXGIDuplication,
        Windows10,
        BitBlt,
    }

    public enum CaptureTarget
    {
        None,
        Screen,
        Window,
        Webcam,
        AllScreens,
        CustomArea,
    }

    public event Action? StateChanged;

    private HWND hWnd = HWND.Null;
    private Screen? screen;
    private string? webcamMonikerString;
    private CaptureTarget captureTarget = CaptureTarget.None;
    private bool capturingCursor;
    private bool hideTaskbar;

    private bool stateDirty;
    private bool stateChangeEventEnabled;

    public bool TriggerChangeEvents
    {
        get => stateChangeEventEnabled;
        set
        {
            stateChangeEventEnabled = value;
            if (stateChangeEventEnabled && stateDirty)
            {
                StateChanged?.Invoke();
                stateDirty = false;
            }
        }
    }

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
            if (captureTarget != value)
            {
                captureTarget = value;
                TriggerStateChange("Target", value);
            }
        }
    }

    public HWND WindowHandle
    {
        get
        {
            if (hWnd.IsNull || captureTarget != CaptureTarget.Window)
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
            if (hWnd != value || captureTarget != CaptureTarget.Window)
            {
                hWnd = value;
                captureTarget = CaptureTarget.Window;
                TriggerStateChange("WindowHandle", value);
            }
        }
    }

    public Screen Screen
    {
        get
        {
            if (screen == null || captureTarget != CaptureTarget.Screen)
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
            if (screen != value || captureTarget != CaptureTarget.Screen)
            {
                screen = value;
                captureTarget = CaptureTarget.Screen;
                TriggerStateChange("Screen", value.DeviceName);
            }
        }
    }

    public string WebcamMonikerString
    {
        get
        {
            if (webcamMonikerString == null || captureTarget != CaptureTarget.Webcam)
            {
                throw new InvalidOperationException("Trying to get WebcamMonikerString without setting it first");
            }
            return webcamMonikerString;
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (webcamMonikerString != value || captureTarget != CaptureTarget.Webcam)
            {
                webcamMonikerString = value;
                captureTarget = CaptureTarget.Webcam;
                TriggerStateChange("WebcamMonikerString", value);
            }
        }
    }

    public bool CapturingCursor
    {
        get => capturingCursor;
        set
        {
            if (CapturingCursor != value)
            {
                capturingCursor = value;
                TriggerStateChange("CapturingCursor", value);
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
                hideTaskbar = value;
                TriggerStateChange("HideTaskbar", value);
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
                Settings.Default.CaptureWindowMethod = (int)value;
                TriggerStateChange("WindowMethod", value);
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
                Settings.Default.CaptureScreenMethod = (int)value;
                TriggerStateChange("ScreenMethod", value);
            }
        }
    }

    private void TriggerStateChange(string property, object value)
    {
        Logger.Log($"Changing CaptureState ({property} = {value})");
        if (TriggerChangeEvents)
        {
            StateChanged?.Invoke();
        }
        else
        {
            stateDirty = true;
        }
    }
}
