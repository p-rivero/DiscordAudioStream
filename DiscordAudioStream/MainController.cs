using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using DiscordAudioStream.AudioCapture;
using DiscordAudioStream.VideoCapture;

using Windows.Win32.Foundation;

namespace DiscordAudioStream;

public class MainController
{
    internal Action? OnAudioMeterClosed { get; set; }

    private readonly MainForm form;
    private bool forceRefresh;
    private Size lastCapturedFrameSize = Size.Empty;

    private VideoCaptureManager? videoCapture;
    private readonly CaptureState captureState = new();
    private readonly CaptureResizer captureResizer = new();
    private readonly ScreenAndWindowList videoSources = new();

    private AudioPlayback? audioPlayback;
    private AudioMeterForm? currentMeterForm;

    public MainController(MainForm owner)
    {
        form = owner;
        captureState.StateChanged += () => form.HideTaskbarEnabled = captureState.HideTaskbarSupported;
    }

    public bool IsStreaming { get; private set; }

    internal void Init()
    {
        videoSources.UpdateCaptureState(captureState, form.VideoIndex);

        captureState.HideTaskbar = Properties.Settings.Default.HideTaskbar;
        captureState.CapturingCursor = Properties.Settings.Default.CaptureCursor;

        videoCapture = new(captureState);
        videoCapture.CaptureAborted += AbortCapture;

        Thread drawThread = CreateDrawThread();
        drawThread.Start();
    }

    // Called when the X button is pressed
    internal bool Stop()
    {
        bool cancel = false;
        if (IsStreaming)
        {
            cancel = true; // Do not close form, return to settings instead
            EndStream();
        }
        else
        {
            Logger.EmptyLine();
            Logger.Log("Close button pressed, stopping program.");
            videoCapture?.Stop();
        }
        return cancel;
    }

    private void SetPreviewSize(Size size)
    {
        lastCapturedFrameSize = size;
        if (IsStreaming)
        {
            size = captureResizer.GetScaledSize(size);
            form.SetPreviewUISize(size);
        }
    }

    private Thread CreateDrawThread()
    {
        // Get the handle now, since we cannot get it from inside the thread
        HWND formHandle = (HWND)form.Handle;
        return new Thread(() => DrawThreadRun(formHandle)) { IsBackground = true, Name = "Draw Thread" };
    }

    private void DrawThreadRun(HWND formHandle)
    {
        if (videoCapture == null)
        {
            throw new InvalidOperationException("Must call Init() before creating draw thread");
        }

        int fps = Properties.Settings.Default.CaptureFramerate;
        Logger.EmptyLine();
        Logger.Log($"Creating Draw thread. Target framerate: {fps} FPS ({videoCapture.CaptureIntervalMs} ms)");

        Stopwatch stopwatch = new();
        Size oldSize = Size.Empty;

        while (true)
        {
            stopwatch.Restart();
            try
            {
                Bitmap? next = VideoCaptureManager.GetNextFrame();

                // No new data, keep displaying last frame
                if (next == null)
                {
                    continue;
                }

                // Detect size changes
                if (next.Size != oldSize)
                {
                    oldSize = next.Size;
                    SetPreviewSize(next.Size);
                }

                // Display captured frame
                // Refresh if the stream has started and "Force screen redraw" is enabled
                form.UpdatePreview(next, IsStreaming && forceRefresh, formHandle);
            }
            catch (InvalidOperationException)
            {
                Logger.Log("Form is closing, stop Draw thread.");
                return;
            }
            stopwatch.Stop();

            int wait = videoCapture.CaptureIntervalMs - (int)stopwatch.ElapsedMilliseconds;
            if (wait > 0)
            {
                Thread.Sleep(wait);
            }
        }
    }

    // INTERNAL METHODS (called from MainForm)

    internal void RefreshScreens(bool restoreSavedItem = true)
    {
        form.SetVideoItems(videoSources.Refresh());
        if (restoreSavedItem)
        {
            form.VideoIndex = videoSources.GetLastStoredItemIndex();
        }
    }

    internal void UpdateAreaComboBox(int oldIndex)
    {
        HWND capturedWindow = videoSources.GetWindowAtIndex(oldIndex);
        RefreshScreens(false);
        form.VideoIndex = capturedWindow.IsNull ? oldIndex : videoSources.GetIndexOfWindow(capturedWindow);
    }

    internal void RefreshAudioDevices()
    {
        IEnumerable<string> elements = new string[] { "(None)" }
            .Concat(AudioPlayback.RefreshDevices());

        int defaultIndex = AudioPlayback.GetLastDeviceIndex() + 1; // Add 1 for "None" element
        form.SetAudioElements(elements, defaultIndex);
    }

    internal void StartStream(bool skipAudioWarning)
    {
        try
        {
            if (form.HasSomeAudioSource)
            {
                StartStreamAudioRecording(form.AudioSourceIndex, skipAudioWarning);
            }
            else
            {
                StartStreamWithoutAudio(skipAudioWarning);
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }

        form.EnableStreamingUI(true);
        // Reading Properties.Settings can be slow, set flag once at the start of the stream
        forceRefresh = Properties.Settings.Default.OffscreenDraw;
        Logger.Log("Force screen redraw: " + forceRefresh);
        IsStreaming = true;

        SetPreviewSize(lastCapturedFrameSize);
    }

    private static void StartStreamWithoutAudio(bool skipAudioWarning)
    {
        if (!skipAudioWarning)
        {
            DialogResult r = MessageBox.Show(
                "No audio source selected, continue anyways?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                // The second button ("No") is the default option
                MessageBoxDefaultButton.Button2
            );

            if (r == DialogResult.No)
            {
                throw new OperationCanceledException();
            }
        }

        Logger.EmptyLine();
        Logger.Log("START STREAM (Without audio)");
        // Clear the stored last used audio device
        Properties.Settings.Default.AudioDeviceID = "";
        Properties.Settings.Default.Save();
    }

    private void StartStreamAudioRecording(int deviceIndex, bool skipAudioWarning)
    {
        if (deviceIndex == AudioPlayback.GetDefaultDeviceIndex())
        {
            if (!skipAudioWarning)
            {
                DialogResult r = MessageBox.Show(
                    "The captured audio device is the same as the output device of DiscordAudioStream.\n"
                        + "This will cause an audio loop, which may result in echo or very loud sounds. Continue anyways?",
                    "Warning",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    // The second button ("Cancel") is the default option
                    MessageBoxDefaultButton.Button2
                );

                if (r == DialogResult.Cancel)
                {
                    throw new OperationCanceledException();
                }
            }

            Logger.EmptyLine();
            Logger.Log("DEFAULT DEVICE CAPTURED (Audio loop)");
        }

        Logger.EmptyLine();
        Logger.Log("START STREAM (With audio)");

        audioPlayback = new(deviceIndex);
        audioPlayback.AudioLevelChanged += (left, right) => currentMeterForm?.SetLevels(left, right);
        try
        {
            audioPlayback.Start();
        }
        catch (InvalidOperationException e)
        {
            MessageBox.Show(
                e.Message,
                "Unable to capture the audio device",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1
            );
            throw new OperationCanceledException();
        }
    }

    private void EndStream()
    {
        Logger.EmptyLine();
        Logger.Log("END STREAM");
        form.EnableStreamingUI(false);
        IsStreaming = false;
        audioPlayback?.Stop();
    }

    private void AbortCapture()
    {
        RefreshScreens();
        if (!IsStreaming)
        {
            return;
        }

        EndStream();
        if (Properties.Settings.Default.AutoExit)
        {
            Logger.EmptyLine();
            Logger.Log("AutoExit was enabled, closing form.");
            InvokeOnUI.RunAsync(form.Close);
        }
    }

    internal void ShowSettingsForm(bool darkMode)
    {
        SettingsForm settingsBox = new(darkMode, captureState) { Owner = form, TopMost = form.TopMost };
        settingsBox.CaptureMethodChanged += () => videoSources.UpdateCaptureState(captureState, form.VideoIndex);
        settingsBox.FramerateChanged += () => videoCapture?.RefreshFramerate();
        settingsBox.ShowAudioInputsChanged += RefreshAudioDevices;
        settingsBox.ShowDialog();
    }

    internal void ShowAboutForm(bool darkMode)
    {
        AboutForm aboutBox = new(darkMode) { Owner = form, TopMost = form.TopMost };
        aboutBox.ShowDialog();
    }

    internal void ShowAudioMeterForm(bool darkMode)
    {
        // Disabled by the user
        if (!Properties.Settings.Default.ShowAudioMeter)
        {
            return;
        }
        // No audio to display
        if (!form.HasSomeAudioSource)
        {
            return;
        }

        if (currentMeterForm == null)
        {
            currentMeterForm = new AudioMeterForm(darkMode) { Owner = form };
            currentMeterForm.FormClosed += (sender, e) =>
            {
                currentMeterForm = null;
                OnAudioMeterClosed?.Invoke();
            };
        }
        currentMeterForm.TopMost = form.TopMost;
        currentMeterForm.Show();
        form.Focus();
    }

    internal void HideAudioMeterForm()
    {
        currentMeterForm?.Hide();
    }

    internal void SetVideoIndex(int index)
    {
        videoSources.UpdateCaptureState(captureState, index);
    }

    internal void SetScaleIndex(int index)
    {
        captureResizer.SetScaleMode((ScaleMode)index);

        Properties.Settings.Default.ScaleIndex = index;
        Properties.Settings.Default.Save();
    }

    internal void SetHideTaskbar(bool hideTaskbar)
    {
        captureState.HideTaskbar = hideTaskbar;
        Properties.Settings.Default.HideTaskbar = hideTaskbar;
        Properties.Settings.Default.Save();
    }

    internal void SetCapturingCursor(bool capturing)
    {
        captureState.CapturingCursor = capturing;
        Properties.Settings.Default.CaptureCursor = capturing;
        Properties.Settings.Default.Save();
    }

    internal void MoveWindow(Point newPosition)
    {
        form.Location = newPosition;
    }
}
