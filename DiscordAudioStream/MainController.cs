using System.Drawing;
using System.Windows.Forms;

using DiscordAudioStream.AudioCapture;
using DiscordAudioStream.VideoCapture;
using DiscordAudioStream.VideoCapture.CaptureStrategy;

using Windows.Win32.Foundation;

namespace DiscordAudioStream;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static")]
public class MainController : IDisposable
{
    internal Action? OnAudioMeterClosed { get; set; }

    public bool IsStreaming { get; private set; }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            videoCapture?.Dispose();
            audioPlayback?.Dispose();
            currentMeterForm?.Dispose();
        }
    }

    internal void Init()
    {
        videoSources.UpdateCaptureState(captureState, form.VideoIndex);

        videoCapture = new(captureState);
        videoCapture.CaptureAborted += AbortCapture;

        DrawThread drawThread = new(videoCapture);
        drawThread.PaintFrame += frame =>
        {
            SetPreviewSize(frame.Size);
            form.UpdatePreview(frame, forceRefresh && IsStreaming);
        };
        drawThread.Start();

        // Prefetch preset slots
        InvokeOnUI.RunAsync(() => _ = GetPopulatedPresets());
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
            videoCapture?.Dispose();
        }
        return cancel;
    }

    private void SetPreviewSize(Size size)
    {
        if (size == lastCapturedFrameSize)
        {
            return;
        }
        lastCapturedFrameSize = size;
        if (IsStreaming)
        {
            Size scaledSize = captureResizer.GetScaledSize(size);
            form.SetPreviewUISize(scaledSize);
        }
    }

    private void ForcePreviewResize()
    {
        lastCapturedFrameSize = Size.Empty;
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

        ForcePreviewResize();
    }

    private void StartStreamWithoutAudio(bool skipAudioWarning)
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
        StartAudioPlayback(deviceIndex);
    }

    private void EndStream()
    {
        Logger.EmptyLine();
        Logger.Log("END STREAM");
        StopAudioPlayback();
        form.EnableStreamingUI(false);
        IsStreaming = false;
    }

    private void StartAudioPlayback(int deviceIndex)
    {
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

    private void StopAudioPlayback()
    {
        if (audioPlayback != null)
        {
            Logger.Log("Stopping audio playback");
            audioPlayback.Stop();
            audioPlayback.Dispose();
            audioPlayback = null;
        }
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
        using SettingsForm settingsBox = new(darkMode, captureState) { Owner = form, TopMost = form.TopMost };
        settingsBox.CaptureMethodChanged += () => videoSources.UpdateCaptureState(captureState, form.VideoIndex);
        settingsBox.FramerateChanged += () => videoCapture?.RefreshFramerate();
        settingsBox.ShowAudioInputsChanged += RefreshAudioDevices;
        settingsBox.ShowDialog();
    }

    internal void ShowAboutForm(bool darkMode)
    {
        using AboutForm aboutBox = new(darkMode) { Owner = form, TopMost = form.TopMost };
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
    }

    internal void SetHideTaskbar(bool hideTaskbar)
    {
        captureState.HideTaskbar = hideTaskbar;
        Properties.Settings.Default.HideTaskbar = hideTaskbar;
    }

    internal void SetCapturingCursor(bool capturing)
    {
        captureState.CapturingCursor = capturing;
        Properties.Settings.Default.CaptureCursor = capturing;
    }

    internal void MoveWindow(Point newPosition)
    {
        form.Location = newPosition;
    }

    internal void UpdateAudioIndex()
    {
        if (form.HasSomeAudioSource)
        {
            AudioPlayback.StoreLastDeviceIndex(form.AudioSourceIndex);
            if (IsStreaming && audioPlayback?.GetIndex() != form.AudioSourceIndex)
            {
                StopAudioPlayback();
                StartAudioPlayback(form.AudioSourceIndex);
            }
        }
        else
        {
            AudioPlayback.ClearLastDeviceIndex();
            if (IsStreaming && audioPlayback != null)
            {
                StopAudioPlayback();
            }
        }
    }

    internal void LoadCapturePreset(int slotNumber)
    {
        Logger.Log($"Loading capture preset {slotNumber}");
        CapturePreset? preset = CapturePreset.LoadSlot(slotNumber);
        if (preset == null)
        {
            MessageBox.Show(
                $"The capture preset slot {slotNumber} is empty.\nFirst, save your current settings using [Ctrl+Shift+{slotNumber}].",
                "Preset not found",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }
        captureState.StateChangeEventEnabled = false;
        preset.ApplyToSettings();
        form.RefreshCaptureUI();
        CustomAreaCapture.RestoreCaptureArea(hideForm: true);
        ForcePreviewResize();
        captureState.StateChangeEventEnabled = true;

        if (!IsStreaming)
        {
            MessageBox.Show(
                $"The capture preset {slotNumber} has been applied.",
                "Preset loaded",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }

    internal void SaveCapturePreset(int slotNumber)
    {
        Logger.Log($"Storing capture preset {slotNumber}");
        CustomAreaCapture.SaveCaptureArea();
        CapturePreset.FromCurrentSettings().SaveToSlot(slotNumber);
        MessageBox.Show(
            $"Your settings have been stored as the capture preset {slotNumber}.",
            "Preset saved",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }

    internal IList<bool> GetPopulatedPresets()
    {
        return CapturePreset.GetPopulatedPresets();
    }
}
