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
					result = MonitorSource(state);
					break;
				case CaptureState.CaptureTarget.AllScreens:
					result = MultiMonitorSource(state);
					break;
				case CaptureState.CaptureTarget.CustomArea:
					result = CustomAreaSource(state);
					break;
				default:
					throw new ArgumentException("Invalid capture target");
			}

			if (PrintFrameTime)
			{
				result = new MeasureTime(result);
			}

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
		
		private static CaptureSource MonitorSource(CaptureState state)
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
		
		private static CaptureSource MultiMonitorSource(CaptureState state)
		{
			if (Win10MultiMonitorCapture.IsAvailable() && state.ScreenMethod != CaptureState.ScreenCaptureMethod.BitBlt)
			{
				return new Win10MultiMonitorCapture(state.CapturingCursor);
			}
			return new BitBltMultiMonitorCapture(state.CapturingCursor);
		}
		
		private static CaptureSource CustomAreaSource(CaptureState state)
		{
			return new BitBltCustomAreaCapture(state.CapturingCursor);
		}
	}
}
