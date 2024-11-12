﻿using System.Drawing;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public abstract class CaptureSource : IDisposable
{
    public abstract Bitmap? CaptureFrame();

    public abstract bool ScaleWithGPU { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Override this method if the subclass needs to dispose of resources
    }
}
