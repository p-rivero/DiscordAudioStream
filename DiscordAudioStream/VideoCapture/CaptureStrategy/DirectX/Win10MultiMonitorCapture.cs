﻿using System.Drawing;

using Composition.WindowsRuntimeHelpers;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class Win10MultiMonitorCapture : CaptureSource
{
    private readonly Win10Capture winCapture;

    public Win10MultiMonitorCapture(bool captureCursor)
    {
        winCapture = new(CaptureHelper.CreateItemForMonitor(CaptureHelper.ALL_SCREENS), captureCursor);
    }

    public override Bitmap? CaptureFrame()
    {
        return winCapture.CaptureFrame();
    }

    public override bool ScaleWithGPU => winCapture.ScaleWithGPU;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            winCapture.Dispose();
        }
    }
}
