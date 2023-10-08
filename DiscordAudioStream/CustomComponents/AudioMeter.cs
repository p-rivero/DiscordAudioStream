using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomComponents
{
    public class AudioMeter : NAudio.Gui.VolumeMeter
    {
        private class AudioMeterSegment
        {
            public double FromPercent { get; set; }
            public double ToPercent { get; set; }
            public Color InactiveColor { get; set; }
            public Color ActiveColor { get; set; }

            public double RelativeHeight => ToPercent - FromPercent;
            public bool CompletelyFilled(double audioValue) => audioValue >= ToPercent;
            public bool CompletelyEmpty(double audioValue) => audioValue <= FromPercent;

            public void Paint(Graphics g, double meterPercent, Size meterSize)
            {
                int PercentToPixels(double percent) => (int)Math.Round(percent * meterSize.Height) + 1;

                using (Brush colorActive = new SolidBrush(ActiveColor))
                using (Brush colorInactive = new SolidBrush(InactiveColor))
                {
                    int top = PercentToPixels(1 - ToPercent) - 2;

                    if (CompletelyFilled(meterPercent))
                    {
                        int height = PercentToPixels(RelativeHeight);
                        g.FillRectangle(colorActive, 1, top, meterSize.Width, height);
                    }
                    else if (CompletelyEmpty(meterPercent))
                    {
                        int height = PercentToPixels(RelativeHeight);
                        g.FillRectangle(colorInactive, 1, top, meterSize.Width, height);
                    }
                    else
                    {
                        int inactiveHeight = PercentToPixels(ToPercent - meterPercent);
                        int activeTop = top + inactiveHeight;
                        int activeHeight = PercentToPixels(meterPercent - FromPercent);
                        g.FillRectangle(colorInactive, 1, top, meterSize.Width, inactiveHeight);
                        g.FillRectangle(colorActive, 1, activeTop, meterSize.Width, activeHeight);
                    }
                }
            }
        }

        private static readonly AudioMeterSegment background = new AudioMeterSegment
        {
            FromPercent = 0,
            ToPercent = 0.1667,
            InactiveColor = Color.FromArgb(0x22, 0x55, 0x61),
            ActiveColor = Color.FromArgb(0x61, 0xc4, 0xdb)
        };

        private static readonly AudioMeterSegment speaking = new AudioMeterSegment
        {
            FromPercent = background.ToPercent,
            ToPercent = 0.8333,
            InactiveColor = Color.FromArgb(0x22, 0x63, 0x25),
            ActiveColor = Color.FromArgb(0x5f, 0xdd, 0x65)
        };

        private static readonly AudioMeterSegment loud = new AudioMeterSegment
        {
            FromPercent = speaking.ToPercent,
            ToPercent = 0.96,
            InactiveColor = Color.FromArgb(0x55, 0x61, 0x22),
            ActiveColor = Color.FromArgb(0xc4, 0xdb, 0x61)
        };

        private static readonly AudioMeterSegment clipping = new AudioMeterSegment
        {
            FromPercent = loud.ToPercent,
            ToPercent = 1,
            InactiveColor = Color.FromArgb(0x5c, 0x22, 0x22),
            ActiveColor = Color.FromArgb(0xce, 0x06, 0x06)
        };

        private static readonly List<AudioMeterSegment> paintedSegments = new List<AudioMeterSegment> {
            clipping,
            loud,
            speaking,
            background
        };

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Draw an audio meter, see https://github.com/p-rivero/DiscordAudioStream/issues/15

            double db = 20.0 * Math.Log10(Amplitude);
            db = Math.Min(db, MaxDb);
            db = Math.Max(db, MinDb);
            double percent = (db - (double)MinDb) / (double)(MaxDb - MinDb);

            Size meterSize = new Size(Width - 2, Height);
            paintedSegments.ForEach(segment => segment.Paint(pe.Graphics, percent, meterSize));
        }
    }
}
