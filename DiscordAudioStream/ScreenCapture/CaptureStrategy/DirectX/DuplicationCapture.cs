using System;
using System.Drawing;
using System.Linq;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using D3D11Device = SharpDX.Direct3D11.Device;
using DXGIResource = SharpDX.DXGI.Resource;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
    // DirectX capture using the Desktop Duplication API

    public class DuplicationCapture : CaptureSource
    {
        // https://docs.microsoft.com/en-us/windows/win32/seccrypto/common-hresult-values
        private const int E_ACCESSDENIED = unchecked((int)0x80070005);

        // https://docs.microsoft.com/en-us/windows/win32/direct3ddxgi/dxgi-error
        private const int DXGI_ERROR_ACCESS_LOST = unchecked((int)0x887A0026);
        private const int DXGI_ERROR_WAIT_TIMEOUT = unchecked((int)0x887A0027);

        private static OutputDuplication[] screens = null;
        private static Bitmap[] cachedThumbnails = null;
        private static readonly D3D11Device d3dDevice = new D3D11Device(Adapter);

        // Index of the selected screen to capture
        private readonly int index;


        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3010", Justification =
            "We cannot control when static initializers are called, and we need InitScreens to be called here. See commit b5eb879")]
        public DuplicationCapture(int index)
        {
            this.index = index;
            // Initialize static field manually, since InitScreens throws exceptions and we
            // can't control when the static initializer is called
            if (screens == null)
            {
                screens = InitScreens();
                cachedThumbnails = new Bitmap[screens.Length];
            }
        }

        private static OutputDuplication[] InitScreens()
        {
            return Adapter.Outputs
                .Select(o => o.QueryInterface<Output1>().DuplicateOutput(d3dDevice))
                .ToArray();
        }

        private static void RefreshScreen(int screenIndex)
        {
            try
            {
                // Release the old screen
                screens[screenIndex]?.Dispose();
                // Get the new output
                Output1 o = Adapter.Outputs[screenIndex].QueryInterface<Output1>();
                screens[screenIndex] = o.DuplicateOutput(d3dDevice);
            }
            catch (SharpDXException e)
            {
                if (e.HResult == E_ACCESSDENIED)
                {
                    // Could not read resource, do nothing and attempt to refresh on the next frame
                    Logger.Log("Access denied! Screen was not refreshed.");
                    return;
                }
                // Otherwise, don't catch the exception
                throw;
            }
        }

        internal static Adapter Adapter
        {
            get
            {
                // Get adapter for GPU 0
                return new Factory1().GetAdapter(0);
            }
        }

        public override Bitmap CaptureFrame()
        {
            try
            {
                // If RefreshScreen failed, the screen may be disposed after switching capture methods
                if (screens[index].IsDisposed)
                {
                    Logger.EmptyLine();
                    Logger.Log("Screen was disposed, attempting to refresh OutputDuplication...");
                    RefreshScreen(index);
                    return null;
                }

                // Try to get duplicated frame in 100 ms
                screens[index].AcquireNextFrame(100, out _, out DXGIResource screenResource);
                // Success: convert captured frame to Bitmap
                using (Texture2D texture = screenResource.QueryInterface<Texture2D>())
                {
                    Bitmap bmp = BitmapHelper.CreateFromTexture2D(texture, d3dDevice);
                    screenResource.Dispose();
                    // Done processing this frame
                    screens[index].ReleaseFrame();

                    // Delete old thumbnail and store new captured frame
                    cachedThumbnails[index]?.Dispose();
                    cachedThumbnails[index] = (Bitmap)bmp.Clone();
                    return bmp;
                }
            }
            catch (SharpDXException e)
            {
                if (e.HResult == DXGI_ERROR_WAIT_TIMEOUT)
                {
                    // The screen does not have new content. Return cached thumbnail
                    return CloneThumbnail();
                }
                else if (e.HResult == DXGI_ERROR_ACCESS_LOST)
                {
                    // The desktop duplication interface is invalid. Release the OutputDuplication and create a new one
                    Logger.EmptyLine();
                    Logger.Log("Access lost, attempting to refresh OutputDuplication...");
                    RefreshScreen(index);
                    return CloneThumbnail();
                }
                else
                {
                    Logger.EmptyLine();
                    Logger.Log("SharpDXException while capturing frame.");
                    Logger.Log("HResult = " + e.HResult);
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

        private Bitmap CloneThumbnail()
        {
            if (cachedThumbnails[index] == null)
            {
                // AcquireNextFrame failed on the very first frame and we don't have a cache yet.
                // This should never happen, but return null just to be safe
                Logger.EmptyLine();
                Logger.Log("AcquireNextFrame: Failed to get the first frame!");
                return null;
            }

            try
            {
                return (Bitmap)cachedThumbnails[index].Clone();
            }
            catch (ArgumentException)
            {
                Logger.EmptyLine();
                Logger.Log("Argument exception while cloning cached thumbnail! The thumbnail has probably been disposed.");
                LogBitmapParams();
            }
            catch (Exception e)
            {
                Logger.EmptyLine();
                Logger.Log($"{e.GetType().Name} while cloning cached thumbnail.");
                LogBitmapParams();
            }
            return null;
        }

        private void LogBitmapParams()
        {
            Logger.Log("Attempting to log bitmap params...");
            try
            {
                Logger.Log("Format: " + cachedThumbnails[index].PixelFormat);
                Logger.Log("Size: " + cachedThumbnails[index].Size);
                Logger.Log("Flags: " + cachedThumbnails[index].Flags);
            }
            catch (Exception e)
            {
                Logger.Log($"Could not read bitmap ({e.GetType().Name})");
            }
        }
    }
}
