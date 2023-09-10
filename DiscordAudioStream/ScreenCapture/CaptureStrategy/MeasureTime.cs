using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class MeasureTime : CaptureSource
	{
		private readonly CaptureSource capture;

		public MeasureTime(CaptureSource capture)
		{
			this.capture = capture;
			Logger.Log($"Instantiating MeasureTime source (wrapping {capture.GetType().Name})");
		}

		public override Bitmap CaptureFrame()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			Bitmap bmp = capture.CaptureFrame();
			long elapsed_ms = watch.ElapsedMilliseconds;
			Console.WriteLine($"Capture using {capture.GetType().Name}: {elapsed_ms} ms");
			return bmp;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			capture.Dispose();
		}
	}
}
