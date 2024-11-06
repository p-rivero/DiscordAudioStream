﻿using System.Drawing;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Windows.Win32.Foundation;

using D3D11Device = SharpDX.Direct3D11.Device;
using DXGIResource = SharpDX.DXGI.Resource;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

// DirectX capture using the Desktop Duplication API

public class DuplicationCapture : DirectXCapture
{
    private const int FRAME_TIMEOUT_MS = 100;

    private static readonly D3D11Device d3dDevice = new(GPU0Adapter);
    private static readonly Lazy<OutputDuplication[]> screens = new(InitScreens);
    private static readonly Lazy<Bitmap?[]> cachedThumbnails = new(() => new Bitmap[screens.Value.Length]);

    private readonly int selectedIndex;

    private OutputDuplication Screen
    {
        get => screens.Value[selectedIndex];
        set
        {
            screens.Value[selectedIndex].Dispose();
            screens.Value[selectedIndex] = value;
        }
    }

    private Bitmap? CachedThumbnail
    {
        get => cachedThumbnails.Value[selectedIndex];
        set
        {
            cachedThumbnails.Value[selectedIndex]?.Dispose();
            cachedThumbnails.Value[selectedIndex] = value;
        }
    }

    public DuplicationCapture(int index)
    {
        selectedIndex = index;
        // Trigger InitScreens(), we want to throw during the constructor if there's an error
        _ = Screen.Description;
    }

    internal static Adapter GPU0Adapter
    {
        get
        {
            using Factory1 factory = new();
            return factory.GetAdapter(0);
        }
    }

    private static OutputDuplication[] InitScreens()
    {
        return GPU0Adapter.Outputs
            .Select(o => o.QueryInterface<Output1>().DuplicateOutput(d3dDevice))
            .ToArray();
    }

    private void RefreshScreen()
    {
        try
        {
            Screen = GPU0Adapter.Outputs[selectedIndex].QueryInterface<Output1>().DuplicateOutput(d3dDevice);
        }
        catch (SharpDXException e)
        {
            if (e.HResult == HRESULT.E_ACCESSDENIED)
            {
                // Could not read resource, do nothing and attempt to refresh on the next frame
                Logger.Log("Access denied! Screen was not refreshed.");
                return;
            }
            // Otherwise, don't catch the exception
            throw;
        }
    }

    public override Bitmap? CaptureFrame()
    {
        try
        {
            // If RefreshScreen failed, the screen may be disposed after switching capture methods
            if (Screen.IsDisposed)
            {
                Logger.EmptyLine();
                Logger.Log("Screen was disposed, attempting to refresh OutputDuplication...");
                RefreshScreen();
                return null;
            }

            Screen.AcquireNextFrame(FRAME_TIMEOUT_MS, out _, out DXGIResource screenResource);
            using (screenResource)
            {
                // Success: convert captured frame to Bitmap
                using Texture2D texture = screenResource.QueryInterface<Texture2D>();
                Bitmap bmp = TextureToBitmap(texture, d3dDevice);
                Screen.ReleaseFrame();

                CachedThumbnail = new Bitmap(bmp);
                return bmp;
            }
        }
        catch (SharpDXException e)
        {
            if (e.HResult == HRESULT.DXGI_ERROR_WAIT_TIMEOUT)
            {
                // The screen does not have new content
                return CloneThumbnail();
            }
            else if (e.HResult == HRESULT.DXGI_ERROR_ACCESS_LOST)
            {
                // The desktop duplication interface is invalid. Release the OutputDuplication and create a new one
                Logger.EmptyLine();
                Logger.Log("Access lost, attempting to refresh OutputDuplication...");
                RefreshScreen();
                return CloneThumbnail();
            }
            else
            {
                Logger.EmptyLine();
                Logger.Log($"SharpDXException while capturing frame: 0x{e.HResult:X}");
                Logger.Log(e);
                throw;
            }
        }
        catch (Exception e)
        {
            Logger.EmptyLine();
            Logger.Log($"{e.GetType().Name} while capturing frame.");
            Logger.Log(e);
            throw;
        }
    }

    private Bitmap? CloneThumbnail()
    {
        if (CachedThumbnail == null)
        {
            Logger.EmptyLine();
            Logger.Log("AcquireNextFrame: Failed to get the first frame, no cached thumbnail exists!");
            return null;
        }
        return new Bitmap(CachedThumbnail);
    }
}
