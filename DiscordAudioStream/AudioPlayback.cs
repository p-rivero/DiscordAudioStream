using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace DiscordAudioStream
{
    class AudioPlayback
    {
        private readonly IWaveIn audioSource;
        private readonly DirectSoundOut output;
        private readonly BufferedWaveProvider waveProvider;
        private const int DESIRED_LATENCY_MS = 50;

        private static MMDeviceCollection audioDevices = null;

        public AudioPlayback(int deviceIndex)
        {
            if (audioDevices == null)
            {
                throw new InvalidOperationException("Must call RefreshDevices() before initializing an AudioPlayback");
            }
            if (deviceIndex < 0 || deviceIndex > audioDevices.Count)
            {
                throw new ArgumentOutOfRangeException("deviceIndex");
            }

            MMDevice device = audioDevices[deviceIndex];
            audioSource = new WasapiLoopbackCapture(device);
            output = new DirectSoundOut(DESIRED_LATENCY_MS);
            audioSource.DataAvailable += AudioSource_DataAvailable;


            WaveFormat format = audioSource.WaveFormat;
            waveProvider = new BufferedWaveProvider(format);
            waveProvider.DiscardOnBufferOverflow = true;
            waveProvider.BufferDuration = TimeSpan.FromSeconds(2);

            output.Init(waveProvider);
        }

        public static string[] RefreshDevices()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            audioDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            string[] names = new string[audioDevices.Count];

            for (int i = 0; i < audioDevices.Count; i++)
            {
                names[i] = audioDevices[i].DeviceFriendlyName;
            }
            return names;
        }

        public void Start()
        {
            output.PlaybackStopped += StoppedHandler;
            audioSource.StartRecording();
            output.Play();
        }

        public void Stop()
        {
            // Remove the handler before stopping manually
            output.PlaybackStopped -= StoppedHandler;

            audioSource.StopRecording();
            output.Stop();
        }

        private void AudioSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void StoppedHandler(object sender, StoppedEventArgs e)
        {
            // In some cases, streaming to Discord will cause DirectSoundOut to throw an
            // exception and stop. If that happens, just resume playback
            output.Play();
        }
    }
}
