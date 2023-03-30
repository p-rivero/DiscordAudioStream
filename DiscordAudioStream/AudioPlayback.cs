using System;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace DiscordAudioStream.AudioCapture
{
	class AudioPlayback
	{
		private readonly IWaveIn audioSource;
		private readonly DirectSoundOut output;
		private readonly BufferedWaveProvider outputProvider;
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

			// Input (from programs outputting to selected device)
			MMDevice device = audioDevices[deviceIndex];
			audioSource = new WasapiLoopbackCapture(device);
			audioSource.DataAvailable += AudioSource_DataAvailable;

			Logger.Log("Started audio device: {0}", device);
			
			Logger.Log("Saving audio device ID: {0}", device.ID);
			Properties.Settings.Default.AudioDeviceID = device.ID;
			Properties.Settings.Default.Save();

			// Output (to default audio device)
			output = new DirectSoundOut(DESIRED_LATENCY_MS);
			WaveFormat format = audioSource.WaveFormat;
			outputProvider = new BufferedWaveProvider(format);
			outputProvider.DiscardOnBufferOverflow = true;
			outputProvider.BufferDuration = TimeSpan.FromSeconds(2);

			output.Init(outputProvider);
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
			output.PlaybackStopped += StoppedHandler;
			try
			{
				audioSource.StartRecording();
			}
			catch (COMException e)
			{
				Logger.Log("COMException while starting audio device:\n{0}", e);
				if ((uint)e.ErrorCode == 0x8889000A)
				{
					throw new InvalidOperationException("The selected audio device is already in use by another application. Please select a different device.");
				}
				else throw;
			}
			catch (Exception)
			{
				output.PlaybackStopped -= StoppedHandler;
				throw;
			}
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
			// New audio data available, append to output audio buffer
			outputProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
		}

		private void StoppedHandler(object sender, StoppedEventArgs e)
		{
			// In some cases, streaming to Discord will cause DirectSoundOut to throw an
			// exception and stop. If that happens, just resume playback
			output.Play();
		}
	}
}
