using System.Drawing;
using System.Drawing.Imaging;

using SharpDX;
using SharpDX.Direct3D11;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public abstract class DirectXCapture : CaptureSource
{
    private readonly Rescaler rescaler;
    protected Device D3DDevice { get; }

    protected DirectXCapture(Device d3dDevice)
    {
        D3DDevice = d3dDevice;
        rescaler = new(d3dDevice);
    }

    public Func<Rectangle>? CustomAreaCrop { get; set; }

    public override bool ScaleWithGPU => true;

    protected Bitmap ScaledTextureToBitmap(Texture2D originalTexture)
    {
        Rectangle area = CustomAreaCrop?.Invoke() ?? new(0, 0, originalTexture.Description.Width, originalTexture.Description.Height);
        double scaleFactor = CaptureResizer.GetScaleFactor(area.Size);
        if (scaleFactor.Is1())
        {
            return TextureToBitmap(originalTexture, area);
        }
        using Texture2D scaledTexture = rescaler.ScaleTexture(originalTexture, scaleFactor);
        return TextureToBitmap(scaledTexture, area.Scale(scaleFactor));
    }

    private Bitmap TextureToBitmap(Texture2D texture, Rectangle copiedArea)
    {
        int width = copiedArea.Width;
        int height = copiedArea.Height;

        using Texture2D screenTexture = CreateReadableTexture(texture.Description.Width, texture.Description.Height, D3DDevice);
        // Copy resource into memory that can be accessed by the CPU
        D3DDevice.ImmediateContext.CopyResource(texture, screenTexture);
        DataBox sourceData = D3DDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

        // Create new Bitmap
        Bitmap bmp = new(width, height, PixelFormat.Format32bppArgb);
        Rectangle boundsRect = new(0, 0, width, height);
        BitmapData destinationData = bmp.LockBits(boundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

        nint sourcePtr =
            sourceData.DataPointer
            + copiedArea.Top * sourceData.RowPitch
            + copiedArea.Left * 4;
        nint destinationPtr = destinationData.Scan0;
        for (int y = 0; y < height; y++)
        {
            // Copy a single line
            Utilities.CopyMemory(destinationPtr, sourcePtr, width * 4);

            // Advance pointers
            sourcePtr += sourceData.RowPitch;
            destinationPtr += destinationData.Stride;
        }

        // Release source and dest locks
        bmp.UnlockBits(destinationData);
        D3DDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

        return bmp;
    }

    private static Texture2D CreateReadableTexture(int width, int height, Device d3dDevice)
    {
        Texture2DDescription texture2DDescription = new()
        {
            CpuAccessFlags = CpuAccessFlags.Read,
            BindFlags = BindFlags.None,
            Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
            Width = width,
            Height = height,
            OptionFlags = ResourceOptionFlags.None,
            MipLevels = 1,
            ArraySize = 1,
            SampleDescription = { Count = 1, Quality = 0 },
            Usage = ResourceUsage.Staging
        };

        return new Texture2D(d3dDevice, texture2DDescription);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            rescaler.Dispose();
        }
    }
}
