using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quick_screen_recorder
{
    class AudioPlayback
    {
        private IWaveIn audioSource;
        private WaveOut output;
        private BufferedWaveProvider waveProvider;

        public AudioPlayback()
        {
            audioSource = new WasapiLoopbackCapture();
            output = new WaveOut();
            audioSource.DataAvailable += audioSource_DataAvailable;


            WaveFormat format = audioSource.WaveFormat;
            waveProvider = new BufferedWaveProvider(format);
            waveProvider.DiscardOnBufferOverflow = true;

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
