using System.Diagnostics;
using System.IO;

namespace DiscordAudioStream
{
	internal static class Logger
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3963", Justification = 
			"Cannot convert this static constructor into an inline initialization")]
		static Logger()
		{
			if (Properties.Settings.Default.OutputLogFile)
			{
				const string LOG_FILE_PATH = "DiscordAudioStream_log.txt";

				Stream file = File.Create(LOG_FILE_PATH);
				Trace.Listeners.Add(new TextWriterTraceListener(file));
				Trace.AutoFlush = true;
			}
		}

		public static void Log(string format, params object[] args)
		{
			Trace.WriteLine(string.Format(format, args));
		}
	}
}
