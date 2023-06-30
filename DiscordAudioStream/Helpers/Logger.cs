using System;
using System.Diagnostics;
using System.IO;

namespace DiscordAudioStream
{
	internal static class Logger
	{
		const string LOG_FILE_PATH = "DiscordAudioStream_log.txt";

		[System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3963", Justification = 
			"Cannot convert this static constructor into an inline initialization")]
		static Logger()
		{
			if (Properties.Settings.Default.OutputLogFile)
			{
				try
				{
					Stream file = File.Create(LOG_FILE_PATH);
					Trace.Listeners.Add(new TextWriterTraceListener(file));
					Trace.AutoFlush = true;
				}
				catch (IOException)
				{
					Console.WriteLine("Warning: The log file ({0}) is already in use.", LOG_FILE_PATH);
					Console.WriteLine("Logging will be disabled until the next launch.");
				}
			}
		}

		public static void Log(string text)
		{
			Trace.WriteLine(text);
		}
		
		public static void Log(string format, params object[] args)
		{
			Log(string.Format(format, args));
		}

		public static void Log(Exception e)
		{
			Log("Exception:\n{1}\n{0}\n{1}", e, "==================================");
		}
	}
}
