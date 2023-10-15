using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public abstract class CaptureSource : IDisposable
{
    public abstract Bitmap CaptureFrame();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Override this method if the subclass needs to dispose of resources
    }

    protected static void InvokeOnUI(Action action)
    {
        if (System.Windows.Forms.Application.OpenForms.Count == 0)
        {
            throw new InvalidOperationException("Cannot invoke on UI thread: no open forms");
        }
        System.Windows.Forms.Application.OpenForms[0].Invoke(action);
    }
}
