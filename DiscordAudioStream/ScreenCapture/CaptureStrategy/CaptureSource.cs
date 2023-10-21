using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public abstract class CaptureSource : IDisposable
{
    public abstract Bitmap? CaptureFrame();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Override this method if the subclass needs to dispose of resources
    }

    protected static T InvokeOnUI<T>(Func<T> func)
    {
        return (T)InvokeOnUI((Delegate)func);
    }
    
    protected static void InvokeOnUI(Action action)
    {
        InvokeOnUI((Delegate)action);
    }

    private static object InvokeOnUI(Delegate func)
    {
        if (System.Windows.Forms.Application.OpenForms.Count == 0)
        {
            throw new InvalidOperationException("Cannot invoke on UI thread: no open forms");
        }
        return System.Windows.Forms.Application.OpenForms[0].Invoke(func);
    }
}
