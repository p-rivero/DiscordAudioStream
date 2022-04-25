using System;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class CaptureSourceFactory
	{
		private const bool DEBUG_TIME = false;

		public static ICaptureSource Build(CaptureState state)
		{
			ICaptureSource result = null;

			if (state.CapturingWindow)
			{
				// Capturing a window
				switch (state.WindowMethod)
				{
					case CaptureState.WindowCaptureMethod.DirectX:
						result = new DXWindowCapture(state.WindowHandle, state.CapturingCursor);
						break;
					case CaptureState.WindowCaptureMethod.BitBlt:
						result = new BitBltWindowCapture(state.WindowHandle, state.CapturingCursor);
						break;
					case CaptureState.WindowCaptureMethod.PrintScreen:
						result = new PrintWindowCapture(state.WindowHandle, state.CapturingCursor);
						break;
					default:
						throw new ArgumentException("Invalid WindowCaptureMethod");
				}
			}
			else
			{
				// Capturing a screen
				switch (state.ScreenMethod)
				{
					case CaptureState.ScreenCaptureMethod.DirectX:
						result = new DXMonitorCapture(state.Screen, state.CapturingCursor);
						break;
					case CaptureState.ScreenCaptureMethod.BitBlt:
						result = new BitBltMonitorCapture(state.Screen, state.CapturingCursor, state.HideTaskbar);
						break;
					default:
						throw new ArgumentException("Invalid ScreenCaptureMethod");
				}
			}

			if (DEBUG_TIME)
			{
				result = new MeasureTime(result);
			}

			return result;
		}
	}
}
