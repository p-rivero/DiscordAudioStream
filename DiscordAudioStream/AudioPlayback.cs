using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace DiscordAudioStream
{
    class AudioPlayback
    {
        private IWaveIn audioSource;
        private DirectSoundOut output;
        private BufferedWaveProvider waveProvider;
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
                throw new ArgumentOutOfRangeException("Invalid index");
            }

            MMDevice device = audioDevices[deviceIndex];
            audioSource = new WasapiLoopbackCapture(device);
            output = new DirectSoundOut(DESIRED_LATENCY_MS);
            audioSource.DataAvailable += audioSource_DataAvailable;


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
            audioSource.StartRecording();
            output.Play();
        }

        public void Stop()
        {
            audioSource.StopRecording();
            output.Stop();
        }

        private void audioSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
