using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using DiscordAudioStream.Properties;

using Windows.Win32.Foundation;

namespace DiscordAudioStream.VideoCapture;
public class ScreenAndWindowList
{
    private WindowList windowList = WindowList.Empty();

    private int CustomAreaIndex { get; set; } = -1;
    private static bool MultiMonitor => Screen.AllScreens.Length > 1;
    private int AllScreensIndex => MultiMonitor ? CustomAreaIndex - 1 : throw new InvalidOperationException("AllScreens not available");

    public IEnumerable<(string, bool)> Refresh()
    {
        List<string> screens = Screen.AllScreens
            .Select((screen, i) =>
            {
                Rectangle bounds = screen.Bounds;
                string screenName = screen.Primary ? "Primary screen" : $"Screen {i + 1}";
                return $"{screenName} ({bounds.Width} x {bounds.Height})";
            })
            .ToList();

        if (MultiMonitor)
        {
            screens.Add("Everything");
        }

        CustomAreaIndex = screens.Count;

        windowList = WindowList.Refresh();
        return screens
            .Select(screenName => (screenName, false))
            .Append(("Custom area", true))
            .Concat(windowList.Names.Select(windowName => (windowName, false)));
    }

    public HWND GetWindowAtIndex(int globalIndex)
    {
        int windowIndex = ToWindowIndex(globalIndex);
        bool capturingWindow = windowIndex >= 0;
        return capturingWindow ? windowList.getHandle(windowIndex) : HWND.Null;
    }

    public int GetIndexOfWindow(HWND hWnd)
    {
        int windowIndex = windowList.IndexOfHandle(hWnd);
        if (windowIndex == -1)
        {
            // Window no longer exists, return to first screen
            return 0;
        }
        return ToGlobalIndex(windowIndex);
    }

    public void UpdateCaptureState(CaptureState captureState, int selectedGlobalIndex)
    {
        int firstWindowIndex = CustomAreaIndex + 1;

        if (selectedGlobalIndex == CustomAreaIndex)
        {
            captureState.Target = CaptureState.CaptureTarget.CustomArea;
        }
        else if (MultiMonitor && selectedGlobalIndex == AllScreensIndex)
        {
            captureState.Target = CaptureState.CaptureTarget.AllScreens;
        }
        else if (selectedGlobalIndex >= firstWindowIndex)
        {
            int windowIndex = ToWindowIndex(selectedGlobalIndex);
            captureState.WindowHandle = windowList.getHandle(windowIndex);
            Settings.Default.LastVideoCaptureValue = windowList.getWindowHash(windowIndex);
        }
        else
        {
            int screenIndex = selectedGlobalIndex;
            captureState.Screen = Screen.AllScreens[screenIndex];
            Settings.Default.LastVideoCaptureValue = screenIndex.ToString(CultureInfo.InvariantCulture);
        }

        Settings.Default.LastVideoCaptureType = captureState.Target.ToString();
        Settings.Default.Save();
    }

    public int GetLastStoredItemIndex()
    {
        string value = Settings.Default.LastVideoCaptureValue;
        try
        {
            return Settings.Default.LastVideoCaptureType switch
            {
                "Screen" => int.Parse(value, CultureInfo.InvariantCulture),
                "Window" => ToGlobalIndex(windowList.IndexOfWindowHash(value)),
                "AllScreens" => MultiMonitor ? AllScreensIndex : throw new InvalidOperationException(),
                "CustomArea" => CustomAreaIndex,
                _ => throw new InvalidOperationException()
            };
        }
        catch
        {
            return 0;
        }
    }

    private int ToWindowIndex(int videoIndex)
    {
        int firstWindowIndex = CustomAreaIndex + 1;
        return videoIndex - firstWindowIndex;
    }

    private int ToGlobalIndex(int windowIndex)
    {
        int firstWindowIndex = CustomAreaIndex + 1;
        return windowIndex + firstWindowIndex;
    }
}
