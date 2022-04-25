using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class MeasureTime : ICaptureSource
	{
		private ICaptureSource capture;

		public MeasureTime(ICaptureSource capture)
		{
			this.capture = capture;
		}

		public Bitmap CaptureFrame()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			Bitmap bmp = capture.CaptureFrame();
			Console.WriteLine("Capture using " + capture.GetType().Name + ": " + watch.ElapsedMilliseconds + " ms");
			return bmp;
		}

		public void Dispose()
		{
			capture.Dispose();
		}
	}
}
