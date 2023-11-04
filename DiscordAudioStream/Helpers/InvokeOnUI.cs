using System.Windows.Forms;

namespace DiscordAudioStream;

public static class InvokeOnUI
{
    private static readonly Control mainForm = Application.OpenForms[0];

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
        return runAsync ? mainForm.BeginInvoke(func) : mainForm.Invoke(func);
    }
}
