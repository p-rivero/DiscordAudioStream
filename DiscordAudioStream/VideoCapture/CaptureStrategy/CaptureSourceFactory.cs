
using Windows.Foundation.Metadata;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public static class CaptureSourceFactory
{
    public static bool PrintFrameTime { get; set; }

    public static CaptureSource Build(CaptureState state)
    {
        CaptureSource result = state.Target switch
        {
            CaptureState.CaptureTarget.Window => WindowSource(state),
            CaptureState.CaptureTarget.Screen => MonitorSource(state),
            CaptureState.CaptureTarget.Webcam => WebcamSource(state),
            CaptureState.CaptureTarget.AllScreens => MultiMonitorSource(state),
            CaptureState.CaptureTarget.CustomArea => CustomAreaSource(state),
            _ => throw new ArgumentException("Invalid capture target"),
        };

        if (PrintFrameTime)
        {
            return new MeasureTime(result);
        }

        return result;
    }

    private static CaptureSource WindowSource(CaptureState state)
    {
        return state.WindowMethod switch
        {
            CaptureState.WindowCaptureMethod.Windows10 => new Win10WindowCapture(state.WindowHandle, state.CapturingCursor),
            CaptureState.WindowCaptureMethod.BitBlt => new BitBltWindowCapture(state.WindowHandle, state.CapturingCursor),
            CaptureState.WindowCaptureMethod.PrintScreen => new PrintWindowCapture(state.WindowHandle, state.CapturingCursor),
            _ => throw new ArgumentException("Invalid WindowCaptureMethod"),
        };
    }

    private static CaptureSource MonitorSource(CaptureState state)
    {
        return state.ScreenMethod switch
        {
            CaptureState.ScreenCaptureMethod.DXGIDuplication => new DuplicationMonitorCapture(state.Screen, state.CapturingCursor, state.HideTaskbar),
            CaptureState.ScreenCaptureMethod.Windows10 => new Win10MonitorCapture(state.Screen, state.CapturingCursor, state.HideTaskbar),
            CaptureState.ScreenCaptureMethod.BitBlt => new BitBltMonitorCapture(state.Screen, state.CapturingCursor, state.HideTaskbar),
            _ => throw new ArgumentException("Invalid ScreenCaptureMethod"),
        };
    }

    private static CaptureSource WebcamSource(CaptureState state)
    {
        return new WebcamCapture(state.WebcamMonikerString);
    }

    private static CaptureSource MultiMonitorSource(CaptureState state)
    {
        if (Win10CaptureAvailable() && state.ScreenMethod != CaptureState.ScreenCaptureMethod.BitBlt)
        {
            return new Win10MultiMonitorCapture(state.CapturingCursor);
        }
        return new BitBltMultiMonitorCapture(state.CapturingCursor);
    }

    private static CaptureSource CustomAreaSource(CaptureState state)
    {
        if (Win10CaptureAvailable() && state.ScreenMethod != CaptureState.ScreenCaptureMethod.BitBlt)
        {
            return new Win10CustomAreaCapture(state.CapturingCursor);
        }
        return new BitBltCustomAreaCapture(state.CapturingCursor);
    }

    private static bool Win10CaptureAvailable()
    {
        try
        {
            return HasUniversalApiContractv9();
        }
        catch (TypeLoadException)
        {
            return false;
        }
    }

    private static bool HasUniversalApiContractv9()
    {
        // This must be done in a separate method, otherwise a TypeLoadException will be thrown before any code can be executed
        return ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 9);
    }
}
