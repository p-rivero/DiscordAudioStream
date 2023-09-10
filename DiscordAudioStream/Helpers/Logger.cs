using System;
using System.Diagnostics;
using System.IO;

namespace DiscordAudioStream
{
	internal static class Logger
	{
		const string LOG_FILE_PATH = "DiscordAudioStream_log.txt";
		
		const int GROUP_TIME_DELTA_MS = 50;

		private static readonly int startTime = Environment.TickCount;

		private static string groupCallerName = null;
		private static int groupLastLogTime = 0;

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
			ForceStartNewGroup();
			Trace.WriteLine("");
		}
		
		public static void Log(string text)
		{
			int timestamp = GetTimestamp();
			string callerName = GetCallerName();
			if (!GroupWithPreviousLogs(timestamp, callerName))
			{
				Trace.WriteLine($"\n{timestamp}ms [{callerName}]");
			}
			Trace.WriteLine("    " + text);
			groupCallerName = callerName;
			groupLastLogTime = timestamp;
		}

		public static void Log(Exception e)
		{
			string separator = new string('-', 40);
			Log($"Exception:\n{separator}\n{e}\n{separator}");
		}

		private static string GetCallerName()
		{
			StackTrace stackTrace = new StackTrace();
			// Go down the stack until a method that isn't in this class is found
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				System.Reflection.MethodBase method = frame.GetMethod();
				if (method.DeclaringType != typeof(Logger))
				{
					return $"{method.DeclaringType.Name}.{method.Name}";
				}
			}
			return "Unknown";
		}

		private static int GetTimestamp()
		{
			return Environment.TickCount - startTime;
		}

		private static bool GroupWithPreviousLogs(int timestamp, string callerName)
		{
			bool longTimeSinceLastLog = timestamp - groupLastLogTime > GROUP_TIME_DELTA_MS;
			bool sameCaller = callerName == groupCallerName;
			return !longTimeSinceLastLog && sameCaller;
		}

		private static void ForceStartNewGroup()
		{
			groupCallerName = null;
		}
	}
}
