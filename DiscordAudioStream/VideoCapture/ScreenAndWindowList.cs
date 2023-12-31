using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using DiscordAudioStream.Properties;

namespace DiscordAudioStream.VideoCapture;
public class ScreenAndWindowList
{
    private WindowList windowList = WindowList.Empty();
    private WebcamList webcamList = WebcamList.Empty();

    private int CustomAreaIndex { get; set; } = -1;
    private int LastWindowIndex { get; set; } = -1;
    private static bool MultiMonitor => Screen.AllScreens.Length > 1;
    private int AllScreensIndex => MultiMonitor ? CustomAreaIndex - 1 : throw new InvalidOperationException("AllScreens not available");

    public IEnumerable<(string, bool)> Refresh()
    {
        List<string> screenNames = Screen.AllScreens
            .Select((screen, i) =>
            {
                Rectangle bounds = screen.Bounds;
                string screenName = screen.Primary ? "Primary screen" : $"Screen {i + 1}";
                return $"{screenName} ({bounds.Width} x {bounds.Height})";
            })
            .ToList();

        if (MultiMonitor)
        {
            screenNames.Add("Everything");
        }

        windowList = WindowList.Refresh();
        webcamList = WebcamList.Refresh();
        CustomAreaIndex = screenNames.Count;
        LastWindowIndex = CustomAreaIndex + windowList.Count;

        IEnumerable<string> names = screenNames
            .Append("Custom area")
            .Concat(windowList.Names)
            .Concat(webcamList.Names);

        List<int> separators = new() { CustomAreaIndex };
        if (webcamList.Count > 0)
        {
            separators.Add(LastWindowIndex);
        }
        return AddSeparators(names, separators);
    }

    public void UpdateCaptureState(CaptureState captureState, int selectedGlobalIndex)
    {
        int firstWindowIndex = CustomAreaIndex + 1;
        int firstWebcamIndex = LastWindowIndex + 1;

        if (selectedGlobalIndex == CustomAreaIndex)
        {
            captureState.Target = CaptureState.CaptureTarget.CustomArea;
        }
        else if (MultiMonitor && selectedGlobalIndex == AllScreensIndex)
        {
            captureState.Target = CaptureState.CaptureTarget.AllScreens;
        }
        else if (selectedGlobalIndex >= firstWebcamIndex)
        {
            int webcamIndex = ToWebcamIndex(selectedGlobalIndex);
            captureState.WebcamMonikerString = webcamList.GetMonikerString(webcamIndex);
            Settings.Default.LastVideoCaptureValue = captureState.WebcamMonikerString;
        }
        else if (selectedGlobalIndex >= firstWindowIndex)
        {
            int windowIndex = ToWindowIndex(selectedGlobalIndex);
            captureState.WindowHandle = windowList.GetHandle(windowIndex);
            Settings.Default.LastVideoCaptureValue = windowList.GetWindowHash(windowIndex);
        }
        else
        {
            int screenIndex = selectedGlobalIndex;
            captureState.Screen = Screen.AllScreens[screenIndex];
            Settings.Default.LastVideoCaptureValue = screenIndex.ToString(CultureInfo.InvariantCulture);
        }

        Settings.Default.LastVideoCaptureType = captureState.Target.ToString();
    }

    public int GetLastStoredItemIndex()
    {
        string value = Settings.Default.LastVideoCaptureValue;
        try
        {
            return Settings.Default.LastVideoCaptureType switch
            {
                "Screen" => int.Parse(value, CultureInfo.InvariantCulture),
                "Window" => WindowIndexToGlobalIndex(windowList.IndexOfWindowHash(value)),
                "Webcam" => WebcamIndexToGlobalIndex(webcamList.IndexOfMonikerString(value)),
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

    private int ToWebcamIndex(int videoIndex)
    {
        int firstWebcamIndex = LastWindowIndex + 1;
        return videoIndex - firstWebcamIndex;
    }

    private int WindowIndexToGlobalIndex(int windowIndex)
    {
        int firstWindowIndex = CustomAreaIndex + 1;
        return windowIndex + firstWindowIndex;
    }

    private int WebcamIndexToGlobalIndex(int webcamIndex)
    {
        int firstWebcamIndex = LastWindowIndex + 1;
        return webcamIndex + firstWebcamIndex;
    }

    private static IEnumerable<(string, bool)> AddSeparators(IEnumerable<string> names, IEnumerable<int> separators)
    {
        return names.Select((name, index) => (name, separators.Contains(index)));
    }
}
