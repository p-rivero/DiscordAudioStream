using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture
{
    // The order of the enum values must match the dropdown in the UI
    public enum ScaleMode
    {
        Fixed720p,
        Fixed1080p,
        Scale100,
        Scale50,
        Scale25,
        Scale20,
        Scale15,
        Scale10,
    }
    public class CaptureResizer
    {
        private const double DISCORD_ASPECT_RATIO = 16.0 / 9.0;
        private double constantScaleFactor = 0;
        private uint fixedHeight = 0;
        private uint fixedWidth = 0;

        public CaptureResizer()
        {
            SetScaleMode(ScaleMode.Scale100);
        }

        public void SetScaleMode(ScaleMode mode)
        {
            switch (mode)
            {
                case ScaleMode.Fixed720p:
                    constantScaleFactor = 0;
                    fixedHeight = 720;
                    fixedWidth = (uint)(fixedHeight * DISCORD_ASPECT_RATIO);
                    break;
                case ScaleMode.Fixed1080p:
                    constantScaleFactor = 0;
                    fixedHeight = 1080;
                    fixedWidth = (uint)(fixedHeight * DISCORD_ASPECT_RATIO);
                    break;
                case ScaleMode.Scale100:
                    constantScaleFactor = 1;
                    break;
                case ScaleMode.Scale50:
                    constantScaleFactor = Math.Sqrt(0.5);
                    break;
                case ScaleMode.Scale25:
                    constantScaleFactor = Math.Sqrt(0.25);
                    break;
                case ScaleMode.Scale20:
                    constantScaleFactor = Math.Sqrt(0.2);
                    break;
                case ScaleMode.Scale15:
                    constantScaleFactor = Math.Sqrt(0.15);
                    break;
                case ScaleMode.Scale10:
                    constantScaleFactor = Math.Sqrt(0.1);
                    break;
                default:
                    throw new ArgumentException("Unknown scale mode");
            }
        }

        public Size GetScaledSize(Size original)
        {
            double dynamicScaleFactor;
            if (constantScaleFactor != 0) dynamicScaleFactor = constantScaleFactor;
            else dynamicScaleFactor = ComputeDynamicScaleFactor(original);

            int newWidth = (int)(original.Width * dynamicScaleFactor);
            int newHeight = (int)(original.Height * dynamicScaleFactor);

            return new Size(newWidth, newHeight);
        }


        private double ComputeDynamicScaleFactor(Size original)
        {
            double widthScaleFactor = (double)fixedWidth / original.Width;
            double heightScaleFactor = (double)fixedHeight / original.Height;

            double scaleFactor = Math.Min(widthScaleFactor, heightScaleFactor);
            // Do not upscale
            return Math.Min(1, scaleFactor);
        }
    }
}
