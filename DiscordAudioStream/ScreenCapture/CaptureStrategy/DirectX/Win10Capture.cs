using Composition.WindowsRuntimeHelpers;
using DiscordAudioStream.ScreenCapture.CaptureStrategy;
using SharpDX.Direct3D11;
using System;
using System.Drawing;
using Windows.Foundation.Metadata;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;

namespace DiscordAudioStream
{
	// DirectX capture using Windows.Graphics.Capture

	internal class Win10Capture : CaptureSource
	{
		private readonly Direct3D11CaptureFramePool framePool;
		private readonly GraphicsCaptureSession session;
		private SizeInt32 lastSize;

		private static readonly IDirect3DDevice device = Direct3D11Helper.CreateDevice();
		private static readonly Device d3dDevice = Direct3D11Helper.CreateSharpDXDevice(device);

		public Win10Capture(GraphicsCaptureItem item, bool captureCursor)
		{
			framePool = Direct3D11CaptureFramePool.Create(
				device,
				DirectXPixelFormat.B8G8R8A8UIntNormalized,
				1,
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
					framePool.Recreate(device, DirectXPixelFormat.B8G8R8A8UIntNormalized, 1, frame.ContentSize)
				));
			}

			using (Texture2D texture = Direct3D11Helper.CreateSharpDXTexture2D(frame.Surface))
			{
				Bitmap bmp = BitmapHelper.CreateFromTexture2D(texture, d3dDevice);
				frame.Dispose();
				return bmp;
			}
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