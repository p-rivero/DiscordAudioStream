using System.Diagnostics;
using System.Drawing;

using DiscordAudioStream.AudioCapture;
using DiscordAudioStream.VideoCapture;
using DiscordAudioStream.VideoCapture.CaptureStrategy;

using Windows.Win32;

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
    private readonly VideoSourcesList videoSources = new();

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
            if (frame.Size != lastCapturedFrameSize)
            {
                lastCapturedFrameSize = frame.Size;
                SetPreviewSize(frame.Size);
            }
            form.UpdatePreview(frame, forceRefresh && IsStreaming);
        };
        drawThread.GetCurrentlyDisplayedFrame += () => form.OutputImage;
        drawThread.GetWaitText += () => captureState.Target switch
        {
            CaptureState.CaptureTarget.Window => "Minimized window",
            CaptureState.CaptureTarget.Webcam => "Camera is unavailable or already in use",
            _ => "Video not available",
        };
        videoCapture.ShouldClearStaleFrame += () => drawThread.Paused;
        drawThread.Start();

        RefreshAudioDevices();

        new Thread(BackgroundInit) { IsBackground = true }.Start();
    }

    private async void BackgroundInit()
    {
        // Prefetch preset slots
        InvokeOnUI.RunAsync(() => _ = GetPopulatedPresets());

        GitHubRelease release = await GitHubRelease.GetLatest().ConfigureAwait(false);
        if (release.Version > BuildInfo.Version && !Properties.Settings.Default.SeenAvailableUpdateDialog)
        {
            ShowMessage.Question()
                .Title("New version available")
                .Text($"A new version of DiscordAudioStream is available: {release.Version}")
                .Text("Do you want to open the download page?")
                .IfYes(() => Process.Start(release.DownloadUrl.AbsoluteUri))
                .AcceptByDefault()
                .IfDontShowAgain(() => Properties.Settings.Default.SeenAvailableUpdateDialog = true)
                .Show();
        }
    }

    private void RefreshAudioDevices()
    {
        IEnumerable<string> elements = new string[] { "(None)" }
            .Concat(AudioPlayback.RefreshDevices());

        int defaultIndex = AudioPlayback.GetLastDeviceIndex() + 1; // Add 1 for "None" element
        form.SetAudioElements(elements, defaultIndex);
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
        if (IsStreaming)
        {
            double scaleFactor = CaptureResizer.GetCPUScaleFactor(size);
            Size scaledSize = size.Scale(scaleFactor);
            form.SetPreviewUISize(scaledSize);
        }
    }

    private void RefreshPreviewSize()
    {
        Size size = InvokeOnUI.RunSync(() => form.OutputImage?.Size ?? Size.Empty);
        SetPreviewSize(size);
    }

    // INTERNAL METHODS (called from MainForm)

    internal void RefreshScreens(bool restoreSavedItem = false)
    {
        form.SetVideoItems(videoSources.Refresh());
        if (restoreSavedItem)
        {
            form.VideoIndex = videoSources.GetLastStoredItemIndex();
        }
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

        RefreshPreviewSize();
    }

    private void StartStreamWithoutAudio(bool skipAudioWarning)
    {
        if (!skipAudioWarning)
        {
            ShowMessage.Warning()
                .Text("No audio source selected, continue anyways?")
                .IfNo(() => throw new OperationCanceledException())
                .Show();
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
                ShowMessage.Warning()
                    .Text("The captured audio device is the same as the output device of DiscordAudioStream.")
                    .Text("This will cause an audio loop, which may result in echo or very loud sounds. Continue anyways?")
                    .IfCancel(() => throw new OperationCanceledException())
                    .Show();
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
            ShowMessage.Error()
                .Title("Unable to capture the audio device")
                .Text(e.Message)
                .Show();
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
        form.VideoIndex = 0;
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
        _ = settingsBox.ShowDialog();
    }

    internal void ShowAboutForm(bool darkMode)
    {
        using AboutForm aboutBox = new(darkMode) { Owner = form, TopMost = form.TopMost };
        _ = aboutBox.ShowDialog();
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
        form.Focus().AssertSuccess("Failed to re-focus main form");
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
        CaptureResizer.SetScaleMode((ScaleMode)index);
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
            ShowMessage.Warning()
                .Title("Preset not found")
                .Text($"The capture preset slot {slotNumber} is empty.")
                .Text($"First, save your current settings using [Ctrl+Shift+{slotNumber}].")
                .Show();
            return;
        }
        captureState.TriggerChangeEvents = false;
        preset.ApplyToSettings();
        form.RefreshCaptureUI();
        CustomAreaCapture.RestoreCaptureArea(hideForm: true);
        RefreshPreviewSize();
        RefreshAudioDevices();
        captureState.TriggerChangeEvents = true;

        if (IsStreaming)
        {
            form.Text = Properties.Settings.Default.StreamTitle;
        }
        else
        {
            ShowMessage.Information()
                .Title("Preset loaded")
                .Text($"The capture preset {slotNumber} has been applied.")
                .Show();
        }
    }

    internal void SaveCapturePreset(int slotNumber)
    {
        Logger.Log($"Storing capture preset {slotNumber}");
        CustomAreaCapture.SaveCaptureArea();
        CapturePreset.FromCurrentSettings().SaveToSlot(slotNumber);
        ShowMessage.Information()
            .Title("Preset saved")
            .Text($"Your settings have been stored as the capture preset {slotNumber}.")
            .Show();
    }

    internal IList<bool> GetPopulatedPresets()
    {
        return CapturePreset.GetPopulatedPresets();
    }
}
