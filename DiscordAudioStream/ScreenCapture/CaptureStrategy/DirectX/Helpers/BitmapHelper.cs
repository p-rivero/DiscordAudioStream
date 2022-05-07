using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DiscordAudioStream
{
	public static class BitmapHelper
	{
		public static Bitmap CreateFromTexture2D(Texture2D texture, Device d3dDevice)
		{
			int width = texture.Description.Width;
			int height = texture.Description.Height;

			using (Texture2D screenTexture = CreateReadableTexture(width, height, d3dDevice))
			{
				// copy resource into memory that can be accessed by the CPU
				d3dDevice.ImmediateContext.CopyResource(texture, screenTexture);

				var mapSource = d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);
				var boundsRect = new Rectangle(0, 0, width, height);

				// Create new Bitmap
				Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

				// Copy pixels from screen capture Texture to GDI bitmap
				BitmapData bitmapData = bmp.LockBits(boundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
				IntPtr sourcePtr = mapSource.DataPointer;
				IntPtr destinationPtr = bitmapData.Scan0;
				for (int y = 0; y < height; y++)
				{
					// Copy a single line 
					Utilities.CopyMemory(destinationPtr, sourcePtr, width * 4);

					// Advance pointers
					sourcePtr += mapSource.RowPitch;
					destinationPtr += bitmapData.Stride;
				}

				// Release source and dest locks
				bmp.UnlockBits(bitmapData);
				d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

				return bmp;
			}
		}

		private static Texture2D CreateReadableTexture(int width, int height, Device d3dDevice)
		{
			var texture2DDescription = new Texture2DDescription
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
}
