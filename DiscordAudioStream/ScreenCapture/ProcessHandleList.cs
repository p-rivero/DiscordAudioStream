using System.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

namespace DiscordAudioStream.ScreenCapture;

public class ProcessHandleList
{
    private readonly List<HWND> handles;
    private readonly List<string> processNames;

    // Cannot instantiate directly, must call ProcessHandleList.Refresh()
    private ProcessHandleList(Dictionary<HWND, string> processes)
    {
        handles = processes.Keys.ToList();
        processNames = processes.Values.ToList();
    }

    public static ProcessHandleList Empty()
    {
        return new(new());
    }

    public static ProcessHandleList Refresh()
    {
        HWND shellWindow = PInvoke.GetShellWindow().AssertNotNull("No shell process found");
        HWND discordAudioStreamWindow = (HWND)Process.GetCurrentProcess().MainWindowHandle;
        Dictionary<HWND, string> windows = new();

        PInvoke.EnumWindows(
            // Called for each top-level window
            (hWnd, lParam) =>
            {
                if (hWnd == shellWindow || hWnd == discordAudioStreamWindow)
                {
                    return true;
                }

                // Ignore windows without WS_VISIBLE
                if (!PInvoke.IsWindowVisible(hWnd))
                {
                    return true;
                }

                // Ignore windows with "" as title
                int windowTextLength = PInvoke.GetWindowTextLength(hWnd);
                if (windowTextLength == 0)
                {
                    return true;
                }

                // Ignore suspended Windows Store apps
                if (PInvoke.DwmGetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_CLOAKED, out BOOL cloaked).Failed)
                {
                    Logger.Log($"Cannot get property DWMWA_CLOAKED. This is normal on Windows 7.");
                }
                else if (cloaked)
                {
                    return true;
                }

                string name = PInvoke.GetWindowText(hWnd, windowTextLength + 1);
                if (name == CustomAreaForm.WINDOW_TITLE)
                {
                    return true;
                }

                windows[hWnd] = name;
                return true;
            },
            IntPtr.Zero
        ).AssertSuccess("EnumWindows failed");

        return new ProcessHandleList(windows);
    }

    public ICollection<string> Names => processNames;

    public HWND this[int index]
    {
        get
        {
            if (handles == null)
            {
                throw new InvalidOperationException("Call RefreshHandles() before attempting to get a handle");
            }
            if (index < 0 || index >= handles.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return handles[index];
        }
    }

    public int IndexOf(HWND handle)
    {
        return handles.IndexOf(handle);
    }
}
