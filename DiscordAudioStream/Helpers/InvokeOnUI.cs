using System.Windows.Forms;

namespace DiscordAudioStream;

public static class InvokeOnUI
{
    public static T RunSync<T>(Func<T> func)
    {
        return (T)RunImpl(func, false);
    }

    public static void RunSync(Action action)
    {
        RunImpl(action, false);
    }

    public static void RunAsync(Action action)
    {
        RunImpl(action, true);
    }

    private static object RunImpl(Delegate func, bool runAsync)
    {
        if (Application.OpenForms.Count == 0)
        {
            throw new InvalidOperationException("Cannot invoke on UI thread: no open forms");
        }
        Control form = Application.OpenForms[0];
        return runAsync ? form.BeginInvoke(func) : form.Invoke(func);
    }
}
