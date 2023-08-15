using System;
using System.Drawing;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
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
		private const int DXGI_ERROR_ACCESS_LOST   = unchecked((int)0x887A0026);
		private const int DXGI_ERROR_WAIT_TIMEOUT  = unchecked((int)0x887A0027);

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
			var adapter = Adapter;
			// Iterate adapter.Outputs and create OutputDuplication on each
			var result = new OutputDuplication[adapter.Outputs.Length];
			for (int i = 0; i < result.Length; i++)
			{
				Output1 o = adapter.Outputs[i].QueryInterface<Output1>();
				result[i] = o.DuplicateOutput(d3dDevice);
			}
			return result;
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
					Logger.Log("\nScreen was disposed, attempting to refresh OutputDuplication...");
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
					Logger.Log("\nAccess lost, attempting to refresh OutputDuplication...");
					RefreshScreen(index);
					return CloneThumbnail();
				}
				else
				{
					Logger.Log("\nSharpDXException while capturing frame.");
					Logger.Log("HResult = {0}", e.HResult);
					Logger.Log(e);
					throw;
				}
			}
			catch (Exception e)
			{
				Logger.Log("\n{0} while capturing frame.", e.GetType().Name);
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
				Logger.Log("\nAcquireNextFrame: Failed to get the first frame!");
				return null;
			}

			try
			{
				return (Bitmap)cachedThumbnails[index].Clone();
			}
			catch (ArgumentException)
			{
				Logger.Log("\nArgument exception while cloning cached thumbnail!");
				Logger.Log("The thumbnail has probably been disposed.");
				LogBitmapParams();
			}
			catch (Exception e)
			{
				Logger.Log("\n{0} while cloning cached thumbnail.", e.GetType().Name);
				LogBitmapParams();
			}
			return null;
		}

		private void LogBitmapParams()
		{
			Logger.Log("Attempting to log bitmap params...");
			try
			{
				Logger.Log("Format: {0}", cachedThumbnails[index].PixelFormat);
				Logger.Log("Size: {0}", cachedThumbnails[index].Size);
				Logger.Log("Flags: {0}", cachedThumbnails[index].Flags);
			}
			catch (Exception e)
			{
				Logger.Log("Could not read bitmap ({0}).", e.GetType().Name);
			}
		}
	}
}