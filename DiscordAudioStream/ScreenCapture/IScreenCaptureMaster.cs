using System.Drawing;

namespace DiscordAudioStream
{
	public interface IScreenCaptureMaster
	{
		void GetCaptureArea(out Size windowSize, out Point position);
		bool IsCapturingCursor();
		void CapturedWindowSizeChanged(Size newSize);
		void AbortCapture();
	}
}
