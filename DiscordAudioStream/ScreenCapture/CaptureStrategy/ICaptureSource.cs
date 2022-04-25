using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public interface ICaptureSource : IDisposable
	{
		Bitmap CaptureFrame();
	}
}
