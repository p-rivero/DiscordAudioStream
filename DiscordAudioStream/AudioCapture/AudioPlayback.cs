using System.Runtime.InteropServices;

using NAudio.CoreAudioApi;
using NAudio.Wave;

using Windows.Win32.Foundation;

namespace DiscordAudioStream.AudioCapture;

internal class AudioPlayback : IDisposable
{
    public event Action<float, float>? AudioLevelChanged;

    private readonly string deviceId;
    private readonly IWaveIn audioSource;
    private readonly IWavePlayer output;
    private readonly BufferedWaveProvider outputProvider;
    private readonly CancellationTokenSource audioMeterCancel;

    private static List<MMDevice>? audioDevices;

    public AudioPlayback(int deviceIndex)
    {
        MMDevice device = GetAudioDevice(deviceIndex);
        deviceId = device.ID;
        audioSource = CaptureDevice(device);
        audioSource.DataAvailable += AudioSource_DataAvailable;
        Logger.Log("Started audio device: " + device);

        output = new DirectSoundOut();
        outputProvider = new(audioSource.WaveFormat)
        {
            DiscardOnBufferOverflow = true,
            BufferDuration = TimeSpan.FromMilliseconds(500)
        };
        output.Init(LimitChannels(outputProvider));

        // Start a periodic timer to update the audio meter, discard the result
        audioMeterCancel = new();
        _ = UpdateAudioMeter(device, audioMeterCancel.Token);
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
            audioSource.Dispose();
            output.Dispose();
            audioMeterCancel.Dispose();
        }
    }

    public static string[] RefreshDevices()
    {
        using MMDeviceEnumerator enumerator = new();
        bool showInputs = Properties.Settings.Default.ShowAudioInputs;
        DataFlow flow = showInputs ? DataFlow.All : DataFlow.Render;
        audioDevices = enumerator.EnumerateAudioEndPoints(flow, DeviceState.Active).ToList();

        return audioDevices
            .Select(device => (showInputs && device.DataFlow == DataFlow.Capture ? "[IN] " : "") + device.FriendlyName)
            .ToArray();
    }

    public static int GetDefaultDeviceIndex()
    {
        using MMDevice? defaultDevice = GetDefaultDevice();
        return defaultDevice != null ? GetIndexOfDeviceWithId(defaultDevice.ID) : -1;
    }

    public static int GetLastDeviceIndex()
    {
        return GetIndexOfDeviceWithId(Properties.Settings.Default.AudioDeviceID);
    }

    public static void StoreLastDeviceIndex(int deviceIndex)
    {
        MMDevice device = GetAudioDevice(deviceIndex);
        if (Properties.Settings.Default.AudioDeviceID != device.ID)
        {
            Logger.Log($"Storing audio device ID: {device.ID}");
            Properties.Settings.Default.AudioDeviceID = device.ID;
        }
    }

    public static void ClearLastDeviceIndex()
    {
        Logger.Log("Clearing stored audio device");
        Properties.Settings.Default.AudioDeviceID = "";
    }

    public void Start()
    {
        output.PlaybackStopped += Output_StoppedHandler;
        try
        {
            audioSource.StartRecording();
        }
        catch (COMException e)
        {
            Logger.Log("COMException while starting audio device:");
            Logger.Log(e);
            if (e.ErrorCode == HRESULT.AUDCLNT_E_DEVICE_IN_USE)
            {
                throw new InvalidOperationException("The selected audio device is already in use by another application. Please select a different device.");
            }
            else
            {
                throw;
            }
        }
        catch (Exception)
        {
            output.PlaybackStopped -= Output_StoppedHandler;
            throw;
        }
        output.Play();
    }

    public void Stop()
    {
        audioMeterCancel.Cancel();
        audioSource.StopRecording();
        // Remove the handler before stopping manually
        output.PlaybackStopped -= Output_StoppedHandler;
        output.Stop();
    }

    public int GetIndex()
    {
        return GetIndexOfDeviceWithId(deviceId);
    }

    private void AudioSource_DataAvailable(object? sender, WaveInEventArgs e)
    {
        // New audio data available, append to output audio buffer
        outputProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
    }

    private void Output_StoppedHandler(object? sender, StoppedEventArgs e)
    {
        Logger.Log("Resuming playback after being interrupted due to exception:");
        Logger.Log(e.Exception);
        output.Play();
    }

    private async Task UpdateAudioMeter(MMDevice device, CancellationToken token)
    {
        TimeSpan updatePeriod = TimeSpan.FromMilliseconds(10);
        while (!token.IsCancellationRequested)
        {
            bool stereo = device.AudioMeterInformation.PeakValues.Count >= 2;
            float left = stereo
                ? device.AudioMeterInformation.PeakValues[0]
                : device.AudioMeterInformation.MasterPeakValue;
            float right = stereo
                ? device.AudioMeterInformation.PeakValues[1]
                : device.AudioMeterInformation.MasterPeakValue;
            AudioLevelChanged?.Invoke(left, right);
            await Task.Delay(updatePeriod, token).ConfigureAwait(true);
        }
    }

    private static MMDevice? GetDefaultDevice()
    {
        using MMDeviceEnumerator enumerator = new();
        if (!enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
        {
            return null;
        }
        return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    }

    private static int GetIndexOfDeviceWithId(string id)
    {
        if (audioDevices == null)
        {
            throw new InvalidOperationException("RefreshDevices() must be called before attempting to find index");
        }
        return audioDevices.FindIndex(device => device.ID == id);
    }

    private static MMDevice GetAudioDevice(int index)
    {
        if (audioDevices == null)
        {
            throw new InvalidOperationException("RefreshDevices() must be called before attempting to get device");
        }
        if (index < 0 || index > audioDevices.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        return audioDevices[index];
    }

    private static IWaveIn CaptureDevice(MMDevice device)
    {
        bool isOutputDevice = device.DataFlow == DataFlow.Render;
        return isOutputDevice ? new WasapiLoopbackCapture(device) : new WasapiCapture(device);
    }

    private static IWaveProvider LimitChannels(IWaveProvider provider)
    {
        const int maxChannels = 2;
        if (provider.WaveFormat.Channels <= maxChannels)
        {
            return provider;
        }

        Logger.Log($"Limiting audio channels to {maxChannels}, provider has {provider.WaveFormat.Channels}");
        IWaveProvider[] muxInputs = { provider };
        return new MultiplexingWaveProvider(muxInputs, maxChannels);
    }
}
