using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	internal class PrintWindowCore : WindowCapture
	{
		private readonly IntPtr windowHandle;

		public PrintWindowCore(IntPtr hWnd)
		{
			windowHandle = hWnd;
		}

		public override Bitmap CaptureFrame()
		{
			Rectangle winArea = WindowCapture.GetWindowArea(windowHandle);

			Bitmap result = new Bitmap(winArea.Width, winArea.Height);

			using (Graphics g = Graphics.FromImage(result))
			{
				User32.PrintWindow(windowHandle, g.GetHdc(), User32.PW_CLIENTONLY | User32.PW_RENDERFULLCONTENT);
			}
			return result;
		}

		public override void Dispose()
		{
			// Nothing to clean up
		}
	}
}
