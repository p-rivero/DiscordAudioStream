﻿using System.Drawing;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class MeasureTime : CaptureSource
{
    private readonly CaptureSource capture;

    public MeasureTime(CaptureSource capture)
    {
        this.capture = capture;
        Logger.Log($"Instantiating MeasureTime source (wrapping {capture.GetType().Name})");
    }

    public override Bitmap? CaptureFrame()
    {
        System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        Bitmap? bmp = capture.CaptureFrame();

        long elapsed_ms = watch.ElapsedMilliseconds;
        if (elapsed_ms != 0)
        {
            float fps = 1000f / elapsed_ms;
            Console.WriteLine($"{capture.GetType().Name}: {elapsed_ms} ms ({fps:0.#} FPS)");
        }

        return bmp;
    }

    public override bool ScaleWithGPU => capture.ScaleWithGPU;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            capture.Dispose();
        }
    }
}
