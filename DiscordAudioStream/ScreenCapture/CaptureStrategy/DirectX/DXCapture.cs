using Composition.WindowsRuntimeHelpers;
using System;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using DiscordAudioStream.ScreenCapture.CaptureStrategy;
using System.Drawing;
using SharpDX.DXGI;
using SharpDX;
using SharpDX.Direct3D11;
using System.Drawing.Imaging;
using Windows.Foundation.Metadata;

namespace DiscordAudioStream
{
	// DirectX capture using Windows.Graphics.Capture

	internal class DXCapture : CaptureSource
	{
		private readonly Direct3D11CaptureFramePool framePool;
		private readonly GraphicsCaptureSession session;
		private SizeInt32 lastSize;

		private static readonly IDirect3DDevice device = Direct3D11Helper.CreateDevice();
		private static readonly SharpDX.Direct3D11.Device d3dDevice = Direct3D11Helper.CreateSharpDXDevice(device);

		public DXCapture(GraphicsCaptureItem item, bool captureCursor)
		{
			framePool = Direct3D11CaptureFramePool.Create(
				device,
				DirectXPixelFormat.B8G8R8A8UIntNormalized,
				2,
				item.Size);
			session = framePool.CreateCaptureSession(item);

			// Attempt to disable yellow capture border. This method is only avaiable from Windows 10, version 2104
			if (ApiInformation.IsPropertyPresent("Windows.Graphics.Capture.GraphicsCaptureSession", "IsBorderRequired"))
			{
				// This must be done in a separate method, otherwise a MethodNotFound will be thrown before any code can be executed
				DisableBorder(session);
			}

			// Control whether the cursor is enabled
			session.IsCursorCaptureEnabled = captureCursor;

			session.StartCapture();
		}

		private static void DisableBorder(GraphicsCaptureSession session)
		{
			session.IsBorderRequired = false;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			session?.Dispose();
			framePool?.Dispose();
		}

		public override Bitmap CaptureFrame()
		{
			// Poll until we get a frame
			Direct3D11CaptureFrame frame = null;
			while (frame == null) frame = framePool.TryGetNextFrame();
			int width = frame.ContentSize.Width;
			int height = frame.ContentSize.Height;

			if (width != lastSize.Width || height != lastSize.Height)
			{
				// The thing we have been capturing has changed size.
				lastSize = frame.ContentSize;

				// Need to recreate the framePool on the UI thread
				InvokeOnUI(new Action(() =>
					framePool.Recreate(device, DirectXPixelFormat.B8G8R8A8UIntNormalized, 2, frame.ContentSize)
				));
			}

			using (Texture2D screenTexture = CreateReadableTexture(width, height))
			{
				// copy resource into memory that can be accessed by the CPU
				using (Texture2D frameSurfaceTexture = Direct3D11Helper.CreateSharpDXTexture2D(frame.Surface))
				{
					d3dDevice.ImmediateContext.CopyResource(frameSurfaceTexture, screenTexture);
				}
				frame.Dispose();

				var mapSource = d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
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

		private Texture2D CreateReadableTexture(int width, int height)
		{
			var texture2DDescription = new Texture2DDescription
			{
				CpuAccessFlags = CpuAccessFlags.Read,
				BindFlags = BindFlags.None,
				Format = Format.B8G8R8A8_UNorm,
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

		private static void InvokeOnUI(Action action)
		{
			if (System.Windows.Forms.Application.OpenForms.Count == 0)
			{
				// No open form, execute on this thread
				action.Invoke();
			}
			// Execute on the UI thread
			System.Windows.Forms.Application.OpenForms[0].Invoke(action);
		}
	}
}