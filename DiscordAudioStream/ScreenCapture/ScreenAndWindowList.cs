using System.Drawing;
using System.Windows.Forms;

using Windows.Win32.Foundation;

namespace DiscordAudioStream.ScreenCapture;
public class ScreenAndWindowList
{
    private int numberOfScreens = -1;
    private WindowList windowList = WindowList.Empty();

    private static bool MultiMonitor => Screen.AllScreens.Length > 1;

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
        numberOfScreens = screens.Count;

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
        int windowIndex = windowList.IndexOf(hWnd);
        if (windowIndex == -1)
        {
            // Window no longer exists, return to first screen
            return 0;
        }
        return ToGlobalIndex(windowIndex);
    }

    public void UpdateCaptureState(CaptureState captureState, int selectedGlobalIndex)
    {
        int customAreaIndex = numberOfScreens;
        int allScreensIndex = customAreaIndex - 1;
        int firstWindowIndex = customAreaIndex + 1;

        if (selectedGlobalIndex == customAreaIndex)
        {
            captureState.Target = CaptureState.CaptureTarget.CustomArea;
        }
        else if (MultiMonitor && selectedGlobalIndex == allScreensIndex)
        {
            captureState.Target = CaptureState.CaptureTarget.AllScreens;
        }
        else if (selectedGlobalIndex >= firstWindowIndex)
        {
            int windowIndex = ToWindowIndex(selectedGlobalIndex);
            captureState.WindowHandle = windowList.getHandle(windowIndex);
        }
        else
        {
            int screenIndex = selectedGlobalIndex;
            captureState.Screen = Screen.AllScreens[screenIndex];
        }

        // TODO: Store always
        if (selectedGlobalIndex <= numberOfScreens)
        {
            Properties.Settings.Default.AreaIndex = selectedGlobalIndex;
            Properties.Settings.Default.Save();
        }
    }

    private int ToWindowIndex(int videoIndex)
    {
        int firstWindowIndex = numberOfScreens + 1;
        return videoIndex - firstWindowIndex;
    }

    private int ToGlobalIndex(int windowIndex)
    {
        int firstWindowIndex = numberOfScreens + 1;
        return windowIndex + firstWindowIndex;
    }
}
