
// Uncomment the following line to print the time it took to capture the frame
//#define PRINT_TIME

using System;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public static class CaptureSourceFactory
	{
		public static CaptureSource Build(CaptureState state)
		{
			CaptureSource result = null;

			if (state.Target == CaptureState.CaptureTarget.Window)
			{
				result = WindowSource(state);
			}
			else if (state.Target == CaptureState.CaptureTarget.Screen)
			{
				result = ScreenSource(state);
			}
			else if (state.Target == CaptureState.CaptureTarget.AllScreens)
			{
				// Capturing all screens can only be done using BitBlt for now
				// There may be faster methods using DirectX
				result = new BitBltMultimonitorCapture(state.CapturingCursor);
			}
			else if (state.Target == CaptureState.CaptureTarget.CustomArea)
			{
				// Capturing a custom area can only be done using BitBlt for now
				result = new BitBltCustomAreaCapture(state.CapturingCursor);
			}
			else
			{
				throw new ArgumentException("Invalid capture target");
			}


			// If PRINT_TIME is defined and the project is compiled in Debug mode, measure the time
			#if (PRINT_TIME && DEBUG)
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
