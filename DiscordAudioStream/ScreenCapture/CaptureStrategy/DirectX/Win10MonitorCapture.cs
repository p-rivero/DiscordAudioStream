using System;
using System.Drawing;
using System.Windows.Forms;

using Composition.WindowsRuntimeHelpers;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
    public class Win10MonitorCapture : CaptureSource
    {
        private readonly Win10Capture winCapture;

        public Win10MonitorCapture(Screen monitor, bool captureCursor)
        {
            IntPtr hMon = GetScreenHandle(monitor);
            winCapture = new Win10Capture(CaptureHelper.CreateItemForMonitor(hMon), captureCursor);
        }

        public override Bitmap CaptureFrame() => winCapture.CaptureFrame();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            winCapture.Dispose();
        }

        private IntPtr GetScreenHandle(Screen screen)
        {
            // Screen.GetHashCode() is implemented as "return (int)hmonitor"
            int hmonitor = screen.GetHashCode();
            return new IntPtr(hmonitor);
        }
    }
}
