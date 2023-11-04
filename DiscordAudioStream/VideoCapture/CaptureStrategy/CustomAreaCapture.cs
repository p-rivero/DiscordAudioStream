using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public abstract class CustomAreaCapture : CaptureSource
{
    private static readonly CustomAreaForm customAreaForm = new();
    private static int instanceCount;
    private static readonly object instanceCountLock = new();

    private readonly Rectangle bounds = SystemInformation.VirtualScreen;

    protected CustomAreaCapture()
    {
        ShowAreaForm();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            HideAreaForm();
        }
    }

    public static void SaveCaptureArea()
    {
        Properties.Settings.Default.AreaForm_Position = customAreaForm.Location;
        Properties.Settings.Default.AreaForm_Size = customAreaForm.Size;
    }

    public static void RestoreCaptureArea(bool hideForm = false)
    {
        if (Properties.Settings.Default.AreaForm_Size != Size.Empty)
        {
            customAreaForm.Location = Properties.Settings.Default.AreaForm_Position;
            customAreaForm.Size = Properties.Settings.Default.AreaForm_Size;
        }
        customAreaForm.FormHidden = hideForm;
    }

    private static void ShowAreaForm()
    {
        lock (instanceCountLock)
        {
            if (instanceCount == 0)
            {
                InvokeOnUI.RunSync(() =>
                {
                    customAreaForm.Show();
                    RestoreCaptureArea();
                    _ = InvokeOnUI.FocusMainForm();
                });
            }
            instanceCount++;
        }
    }

    private static void HideAreaForm()
    {
        SaveCaptureArea();
        lock (instanceCountLock)
        {
            instanceCount--;
            if (instanceCount == 0)
            {
                InvokeOnUI.RunSync(customAreaForm.Hide);
            }
        }
    }

    protected Rectangle GetCustomArea(bool relativeToVirtualScreen)
    {
        // Omit pixels of the red border
        const int BORDER = CustomAreaForm.BORDER_WIDTH_PX;
        int left = customAreaForm.Left + BORDER;
        int top = customAreaForm.Top + BORDER;
        int width = customAreaForm.Width - 2 * BORDER;
        int height = customAreaForm.Height - 2 * BORDER;

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
