
// Uncomment the following line to print the time it took to capture the frame
//#define PRINT_TIME

using System;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public static class CaptureSourceFactory
	{
		public static bool PrintFrameTime { get; set; }

		public static CaptureSource Build(CaptureState state)
		{
			CaptureSource result;
			switch (state.Target)
			{
				case CaptureState.CaptureTarget.Window:
					result = WindowSource(state);
					break;
				case CaptureState.CaptureTarget.Screen:
					result = ScreenSource(state);
					break;
				case CaptureState.CaptureTarget.AllScreens:
					result = new BitBltMultimonitorCapture(state.CapturingCursor);
					break;
				case CaptureState.CaptureTarget.CustomArea:
					result = new BitBltCustomAreaCapture(state.CapturingCursor);
					break;
				default:
					throw new ArgumentException("Invalid capture target");
			}


			#if (PRINT_TIME)
				result = new MeasureTime(result);
			#endif

			return result;
		}

		private static CaptureSource WindowSource(CaptureState state)
		{
			// Capturing a window
			switch (state.WindowMethod)
			{
				case CaptureState.WindowCaptureMethod.Windows10:
					return new Win10WindowCapture(state.WindowHandle, state.CapturingCursor);

				case CaptureState.WindowCaptureMethod.BitBlt:
					return new BitBltWindowCapture(state.WindowHandle, state.CapturingCursor);

				case CaptureState.WindowCaptureMethod.PrintScreen:
					return new PrintWindowCapture(state.WindowHandle, state.CapturingCursor);

				default:
					throw new ArgumentException("Invalid WindowCaptureMethod");
			}
		}
		
		private static CaptureSource ScreenSource(CaptureState state)
		{
			// Capturing a screen
			switch (state.ScreenMethod)
			{
				case CaptureState.ScreenCaptureMethod.DXGIDuplication:
					return new DuplicationMonitorCapture(state.Screen, state.CapturingCursor);

				case CaptureState.ScreenCaptureMethod.Windows10:
					return new Win10MonitorCapture(state.Screen, state.CapturingCursor);
			
				case CaptureState.ScreenCaptureMethod.BitBlt:
					return new BitBltMonitorCapture(state.Screen, state.CapturingCursor, state.HideTaskbar);

				default:
					throw new ArgumentException("Invalid ScreenCaptureMethod");
			}
		}
	}
}
