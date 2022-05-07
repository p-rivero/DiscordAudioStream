using DiscordAudioStream.ScreenCapture.CaptureStrategy;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Drawing;

namespace DiscordAudioStream
{
	// DirectX capture using Windows.Graphics.Capture

	internal class DuplicationCapture : CaptureSource
	{
		private static readonly OutputDuplication[] screens;
		private static readonly SharpDX.Direct3D11.Device d3dDevice;

		// Index of the selected screen to capture
		private readonly int index;

		static DuplicationCapture()
		{
			// Get adapter for GPU 0
			var adapter = new Factory1().GetAdapter(0);
			// Get device from adapter
			d3dDevice = new SharpDX.Direct3D11.Device(adapter);

			// Iterate adapter.Outputs and create OutputDuplication on each
			screens = new OutputDuplication[adapter.Outputs.Length];
			for (int i = 0; i < screens.Length; i++)
			{
				Output1 o = adapter.Outputs[i].QueryInterface<Output1>();
				screens[i] = o.DuplicateOutput(d3dDevice);
			}
		}

		public DuplicationCapture(int index)
		{
			this.index = index;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			try
			{
				screens[index].ReleaseFrame();
			}
			catch (SharpDX.SharpDXException)
			{
				int i = 5;
			}
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