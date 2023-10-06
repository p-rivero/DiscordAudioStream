using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace DiscordAudioStream.AudioCapture
{
	class AudioPlayback
	{
		public event Action<float, float> AudioLevelChanged;

		private const int DESIRED_LATENCY_MS = 50;

		private readonly IWaveIn audioSource;
		private readonly DirectSoundOut output;
		private readonly BufferedWaveProvider outputProvider;
		private readonly CancellationTokenSource audioMeterCancel;

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
			if (device.DataFlow == DataFlow.Render)
			{
				// Input from programs outputting to selected device
				audioSource = new WasapiLoopbackCapture(device);
			}
			else
			{
				// Input from microphone
				audioSource = new WasapiCapture(device);
			}
			audioSource.DataAvailable += AudioSource_DataAvailable;

			Logger.Log("Started audio device: " + device);
			
			Logger.Log("Saving audio device ID: " + device.ID);
			Properties.Settings.Default.AudioDeviceID = device.ID;
			Properties.Settings.Default.Save();

			// Output (to default audio device)
			output = new DirectSoundOut(DESIRED_LATENCY_MS);
			WaveFormat format = audioSource.WaveFormat;
			outputProvider = new BufferedWaveProvider(format);
			outputProvider.DiscardOnBufferOverflow = true;
			outputProvider.BufferDuration = TimeSpan.FromSeconds(2);

			output.Init(outputProvider);

			// Start a periodic timer to update the audio meter, discard the result
			audioMeterCancel = new CancellationTokenSource();
			_ = UpdateAudioMeter(TimeSpan.FromMilliseconds(10), audioMeterCancel.Token, device);
		}

		public static string[] RefreshDevices()
		{
			MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
			DataFlow flow = Properties.Settings.Default.ShowAudioInputs ? DataFlow.All : DataFlow.Render;
			audioDevices = enumerator.EnumerateAudioEndPoints(flow, DeviceState.Active);

			string[] names = new string[audioDevices.Count];

			for (int i = 0; i < audioDevices.Count; i++)
			{
				names[i] = audioDevices[i].FriendlyName;
				// Add [IN] prefix to input devices (microphones and capture cards)
				if (audioDevices[i].DataFlow == DataFlow.Capture) names[i] = "[IN] " + names[i];
			}
			return names;
		}

		// Returns the index of the default audio output device
		public static int GetDefaultDeviceIndex()
		{
			if (audioDevices == null)
			{
				throw new InvalidOperationException("Must call RefreshDevices() before trying to get the default index");
			}
			MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
			if (!enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
				return -1;
			
			string defaultDeviceId = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).ID;
			for (int i = 0; i < audioDevices.Count; i++)
			{
				if (audioDevices[i].ID == defaultDeviceId)
					return i;
			}
			return -1;
		}

		// Returns the index of the last device that was used, or -1 if none
		public static int GetLastDeviceIndex()
		{
			if (audioDevices == null)
			{
				throw new InvalidOperationException("Must call RefreshDevices() before trying to get the last device index");
			}
			string lastDeviceId = Properties.Settings.Default.AudioDeviceID;
			for (int i = 0; i < audioDevices.Count; i++)
			{
				if (audioDevices[i].ID == lastDeviceId)
					return i;
			}
			return -1;
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
				Logger.Log("COMException while starting audio device:\n" + e);
				if ((uint)e.ErrorCode == 0x8889000A)
				{
					throw new InvalidOperationException("The selected audio device is already in use by another application. Please select a different device.");
				}
				else throw;
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

		private void AudioSource_DataAvailable(object sender, WaveInEventArgs e)
		{
			// New audio data available, append to output audio buffer
			outputProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
		}

		private void Output_StoppedHandler(object sender, StoppedEventArgs e)
		{
			// In some cases, streaming to Discord will cause DirectSoundOut to throw an
			// exception and stop. If that happens, just resume playback
			output.Play();
		}

		
		private async Task UpdateAudioMeter(TimeSpan interval, CancellationToken token, MMDevice device)
		{
			while (!token.IsCancellationRequested)
			{
				int numChannels = device.AudioMeterInformation.PeakValues.Count;
				float left = (numChannels >= 2) ?
					device.AudioMeterInformation.PeakValues[0] : 
					device.AudioMeterInformation.MasterPeakValue;
				float right = (numChannels >= 2) ?
					device.AudioMeterInformation.PeakValues[1] :
					device.AudioMeterInformation.MasterPeakValue;
				AudioLevelChanged?.Invoke(left, right);
				await Task.Delay(interval, token);
			}
		}
	}
}
