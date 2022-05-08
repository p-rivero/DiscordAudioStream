using DiscordAudioStream.ScreenCapture.CaptureStrategy;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Drawing;

namespace DiscordAudioStream
{
	// DirectX capture using Windows.Graphics.Capture

	internal class DuplicationCapture : CaptureSource
	{
		private static readonly SharpDX.Direct3D11.Device d3dDevice = new SharpDX.Direct3D11.Device(Adapter);
		private static readonly OutputDuplication[] screens = InitScreens();

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
			// Try to get duplicated frame
			screens[index].AcquireNextFrame(-1, out _, out SharpDX.DXGI.Resource screenResource);
			using (Texture2D texture = screenResource.QueryInterface<Texture2D>())
			{
				Bitmap bmp = BitmapHelper.CreateFromTexture2D(texture, d3dDevice);
				screens[index].ReleaseFrame();
				screenResource.Dispose();
				return bmp;
			}
		}
	}
}