using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace quick_screen_recorder
{
    class AudioPlayback
    {
        private IWaveIn audioSource;
        private DirectSoundOut output;
        private BufferedWaveProvider waveProvider;
        private const int DESIRED_LATENCY_MS = 50;

        public AudioPlayback(MMDevice device)
        {
            audioSource = new WasapiLoopbackCapture(device);
            output = new DirectSoundOut(DESIRED_LATENCY_MS);
            audioSource.DataAvailable += audioSource_DataAvailable;


            WaveFormat format = audioSource.WaveFormat;
            waveProvider = new BufferedWaveProvider(format);
            waveProvider.DiscardOnBufferOverflow = true;
            waveProvider.BufferDuration = TimeSpan.FromSeconds(2);

            output.Init(waveProvider);
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
