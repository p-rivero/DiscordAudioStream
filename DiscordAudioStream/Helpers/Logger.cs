using System;
using System.Diagnostics;
using System.IO;

namespace DiscordAudioStream
{
	internal static class Logger
	{
		const string LOG_FILE_PATH = "DiscordAudioStream_log.txt";

		private static readonly int START_TIME = Environment.TickCount;

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
					Console.WriteLine($"Warning: The log file ({LOG_FILE_PATH}) is already in use.");
					Console.WriteLine("Logging will be disabled until the next launch.");
				}
			}
		}
		
		public static void EmptyLine()
		{
			Trace.WriteLine("");
		}
		
		public static void Log(string text)
		{
			LogImpl(text);
		}

		public static void Log(Exception e)
		{
			string separator = new string('-', 40);
			LogImpl($"Exception:\n{separator}\n{e}\n{separator}");
		}


		private static void LogImpl(string text)
		{
			Trace.WriteLine($"{GetTimestamp()}ms [{GetCallerName(3)}]\n    {text}");
		}

		private static string GetCallerName(int stackDepth)
		{
			StackTrace stackTrace = new StackTrace();
			StackFrame frame = stackTrace.GetFrame(stackDepth);
			System.Reflection.MethodBase method = frame.GetMethod();
			return $"{method.DeclaringType.Name}.{method.Name}";
		}

		private static int GetTimestamp()
		{
			return Environment.TickCount - START_TIME;
		}
	}
}
