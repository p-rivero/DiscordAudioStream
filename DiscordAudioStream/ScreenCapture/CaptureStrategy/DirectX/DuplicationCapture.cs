using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	// DirectX capture using the Desktop Duplication API

	public class DuplicationCapture : CaptureSource
	{
		private static readonly SharpDX.Direct3D11.Device d3dDevice = new SharpDX.Direct3D11.Device(Adapter);
		private static readonly OutputDuplication[] screens = InitScreens();
		private static readonly Bitmap[] cachedThumbnails = new Bitmap[screens.Length];

		// Index of the selected screen to capture
		private readonly int index;

		static OutputDuplication[] InitScreens()
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

		internal static Adapter Adapter
		{
			get
			{
				// Get adapter for GPU 0
				return new Factory1().GetAdapter(0);
			}
		}

		public DuplicationCapture(int index)
		{
			this.index = index;
		}


		public override Bitmap CaptureFrame()
		{
			try
			{
				// Try to get duplicated frame in 100 ms
				screens[index].AcquireNextFrame(100, out _, out SharpDX.DXGI.Resource screenResource);
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
			catch (SharpDX.SharpDXException)
			{
				if (cachedThumbnails[index] == null)
				{
					// AcquireNextFrame failed on the very first frame and we don't have a cache yet.
					// This should never happen, but return null just to be sure
					Logger.Log("\nAcquireNextFrame: Failed to get the first frame!");
					return null;
				}
				// Return cached thumbnail
				return CloneThumbnail();
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