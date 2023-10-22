using System.Drawing;

using Composition.WindowsRuntimeHelpers;

using SharpDX.Direct3D11;

using Windows.Foundation.Metadata;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class Win10Capture : CaptureSource
{
    public Func<Rectangle>? CustomAreaCrop { get; set; }

    private readonly Direct3D11CaptureFramePool framePool;
    private readonly GraphicsCaptureSession session;
    private SizeInt32 lastSize;

    private static readonly IDirect3DDevice device = Direct3D11Helper.CreateDevice();
    private static readonly Device d3dDevice = Direct3D11Helper.CreateSharpDXDevice(device);

    public Win10Capture(GraphicsCaptureItem item, bool captureCursor)
    {
        framePool = Direct3D11CaptureFramePool.Create(device, DirectXPixelFormat.B8G8R8A8UIntNormalized, 1, item.Size);
        session = framePool.CreateCaptureSession(item);

        // Attempt to disable yellow capture border. This method is only avaiable from Windows 11
        if (ApiInformation.IsPropertyPresent("Windows.Graphics.Capture.GraphicsCaptureSession", "IsBorderRequired"))
        {
            Logger.Log("Attempting to disable yellow border...");
            DisableBorder(session);
        }

        // Control whether the cursor is enabled
        session.IsCursorCaptureEnabled = captureCursor;

        session.StartCapture();
    }

    private static void DisableBorder(GraphicsCaptureSession session)
    {
        // This must be done in a separate method, otherwise a MethodNotFound will be thrown before any code can be executed
        session.IsBorderRequired = false;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        try
        {
            session?.Dispose();
        }
        catch (Exception e)
        {
            Logger.Log("Failed to dispose capture session due to exception:");
            Logger.Log(e);
        }
        framePool?.Dispose();
    }

    public override Bitmap? CaptureFrame()
    {
        using Direct3D11CaptureFrame frame = framePool.TryGetNextFrame();
        if (frame == null)
        {
            // No new content
            return null;
        }

        int width = frame.ContentSize.Width;
        int height = frame.ContentSize.Height;

        if (width != lastSize.Width || height != lastSize.Height)
        {
            lastSize = frame.ContentSize;
            framePool.Recreate(device, DirectXPixelFormat.B8G8R8A8UIntNormalized, 1, frame.ContentSize);
        }

        using Texture2D texture = Direct3D11Helper.CreateSharpDXTexture2D(frame.Surface);
        if (CustomAreaCrop != null)
        {
            return BitmapHelper.CreateFromTexture2D(texture, d3dDevice, CustomAreaCrop());
        }
        return BitmapHelper.CreateFromTexture2D(texture, d3dDevice);
    }
}
