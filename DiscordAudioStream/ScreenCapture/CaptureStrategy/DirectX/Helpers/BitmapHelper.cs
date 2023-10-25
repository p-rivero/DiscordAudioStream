using System.Drawing;
using System.Drawing.Imaging;

using SharpDX;
using SharpDX.Direct3D11;

namespace DiscordAudioStream.ScreenCapture;

public static class BitmapHelper
{
    public static Bitmap CreateFromTexture2D(Texture2D texture, Device d3dDevice)
    {
        Rectangle copiedArea = new(0, 0, texture.Description.Width, texture.Description.Height);
        return CreateFromTexture2D(texture, d3dDevice, copiedArea);
    }

    public static Bitmap CreateFromTexture2D(Texture2D texture, Device d3dDevice, Rectangle copiedArea)
    {
        int width = copiedArea.Width;
        int height = copiedArea.Height;

        using Texture2D screenTexture = CreateReadableTexture(texture.Description.Width, texture.Description.Height, d3dDevice);
        // Copy resource into memory that can be accessed by the CPU
        d3dDevice.ImmediateContext.CopyResource(texture, screenTexture);
        DataBox sourceData = d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

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
        d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

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
}
