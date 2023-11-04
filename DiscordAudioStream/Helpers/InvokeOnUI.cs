using System.Windows.Forms;

namespace DiscordAudioStream;

public static class InvokeOnUI
{
    private static readonly Control mainForm = Application.OpenForms[0];

    public static T RunSync<T>(Func<T> func)
    {
        return (T)mainForm.Invoke(func);
    }

    public static void RunSync(Action action)
    {
        _ = mainForm.Invoke(action);
    }

    public static void RunAsync(Action action)
    {
        _ = mainForm.BeginInvoke(action);
    }

    public static bool FocusMainForm()
    {
        return mainForm.Focus();
    }
}
