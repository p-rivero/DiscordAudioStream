﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public abstract class CustomAreaCapture : CaptureSource
	{
		private readonly static AreaForm areaForm = new AreaForm();
		private static int customAreaCaptureCount = 0;
		
		private readonly Rectangle bounds = SystemInformation.VirtualScreen;

		protected CustomAreaCapture()
		{
			if (customAreaCaptureCount == 0)
			{
				InvokeOnUI(areaForm.Show);
			}
			customAreaCaptureCount++;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			
			customAreaCaptureCount--;
			if (customAreaCaptureCount == 0)
			{
				InvokeOnUI(areaForm.Hide);
			}
		}

		protected Rectangle GetCustomArea(bool relativeToVirtualScreen)
		{
			// Omit pixels of the red border
			const int BORDER = AreaForm.BORDER_WIDTH_PX;
			int left = areaForm.Left + BORDER;
			int top = areaForm.Top + BORDER;
			int width = areaForm.Width - 2 * BORDER;
			int height = areaForm.Height - 2 * BORDER;
			
			left = Math.Max(left, bounds.X);
			top = Math.Max(top, bounds.Y);
			left = Math.Min(left, bounds.Right - width);
			top = Math.Min(top, bounds.Bottom - height);

			// If relativeToVirtualScreen, the origin is the top-left corner of the multi-monitor desktop,
			// Otherwise, the origin is the top-left corner of the primary monitor
			if (relativeToVirtualScreen)
			{
				left -= bounds.X;
				top -= bounds.Y;
			}

			return new Rectangle(left, top, width, height);
		}
	}

}
