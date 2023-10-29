using System.Diagnostics;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

namespace DiscordAudioStream.ScreenCapture;

public class ProcessHandleList
{
    private record ProcessHandleItem(HWND handle, string name, string fileName);

    private readonly List<ProcessHandleItem> processes;

    private ProcessHandleList(List<ProcessHandleItem> processes)
    {
        this.processes = processes;
    }

    public static ProcessHandleList Empty()
    {
        return new(new());
    }

    public static ProcessHandleList Refresh()
    {
        HWND shellWindow = PInvoke.GetShellWindow().AssertNotNull("No shell process found");
        HWND discordAudioStreamWindow = (HWND)Process.GetCurrentProcess().MainWindowHandle;
        List<ProcessHandleItem> processes = new();

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

                PInvoke.GetWindowThreadProcessId(hWnd, out uint processId).AssertNotZero("GetWindowThreadProcessId failed");
                string fileName = Process.GetProcessById((int)processId).MainModule.FileName;

                processes.Add(new(hWnd, name, fileName));
                return true;
            },
            IntPtr.Zero
        ).AssertSuccess("EnumWindows failed");

        return new ProcessHandleList(processes);
    }

    public IEnumerable<string> Names => processes.Select(p => p.name);

    public HWND getHandle(int index)
    {
        if (processes == null)
        {
            throw new InvalidOperationException("Call RefreshHandles() before attempting to get a handle");
        }
        if (index < 0 || index >= processes.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        return processes[index].handle;
    }

    public int IndexOf(HWND handle)
    {
        return processes.FindIndex(p => p.handle == handle);
    }
}
