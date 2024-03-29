﻿using System.Diagnostics;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.System.Threading;

namespace DiscordAudioStream.VideoCapture;

public class WindowList
{
    private const char HASH_SEPARATOR = '|';

    private sealed record WindowListItem(HWND handle, string title, string filename);
    private readonly List<WindowListItem> windowList;

    private WindowList(List<WindowListItem> windowList)
    {
        this.windowList = windowList;
    }

    public static WindowList Empty()
    {
        return new(new());
    }

    public static WindowList Refresh()
    {
        HWND shellWindow = PInvoke.GetShellWindow().AssertNotNull("No shell process found");
        HWND discordAudioStreamWindow = (HWND)Process.GetCurrentProcess().MainWindowHandle;
        List<WindowListItem> processes = new();

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
                string filename = GetProcessFilename(processId, name);

                processes.Add(new(hWnd, name, filename));
                return true;
            },
            IntPtr.Zero
        ).AssertSuccess("EnumWindows failed");

        return new WindowList(processes);
    }

    public IEnumerable<string> Names => windowList.Select(p => p.title);

    public int Count => windowList.Count;

    public HWND GetHandle(int index)
    {
        return windowList[index].handle;
    }

    public string GetWindowHash(int index)
    {
        return $"{windowList[index].handle}{HASH_SEPARATOR}{windowList[index].filename}{HASH_SEPARATOR}{windowList[index].title}";
    }

    public int IndexOfHandle(HWND handle)
    {
        return windowList.FindIndex(p => p.handle == handle);
    }

    public int IndexOfWindowHash(string hash)
    {
        string[] hashParts = hash.Split(new char[] { HASH_SEPARATOR }, 3);
        if (hashParts.Length != 3)
        {
            throw new ArgumentException("Invalid hash");
        }
        string handle = hashParts[0];
        string filename = hashParts[1];
        string title = hashParts[2];

        int exactHandleMatch = windowList.FindIndex(p => p.filename == filename && p.handle.ToString() == handle);
        if (exactHandleMatch != -1)
        {
            return exactHandleMatch;
        }

        int exactTitleMatch = windowList.FindIndex(p => p.filename == filename && p.title == title);
        if (exactTitleMatch != -1)
        {
            return exactTitleMatch;
        }

        int filenameMatch = windowList.FindIndex(p => p.filename == filename);
        if (filenameMatch != -1)
        {
            return filenameMatch;
        }

        int titleMatch = windowList.FindIndex(p => p.title == title);
        if (titleMatch != -1)
        {
            return titleMatch;
        }

        throw new InvalidOperationException("No window matches hash");
    }

    private static string GetProcessFilename(uint processId, string backupName)
    {
        HANDLE processHandle = PInvoke.OpenProcess(PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_LIMITED_INFORMATION, false, processId);
        try
        {
            return PInvoke.QueryFullProcessImageName(processHandle, PROCESS_NAME_FORMAT.PROCESS_NAME_WIN32, 512);
        }
        catch (ExternalException)
        {
            Logger.Log($"Failed to get filename for '{backupName}'. Process may be suspended.");
            return backupName;
        }
    }
}
