using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using DiscordAudioStream.ScreenCapture.CaptureStrategy;
using Mono.Options;

namespace DiscordAudioStream
{
	internal class CommandArguments
	{
		private Point? position = null;
		private bool startStream = false;
		private bool printFps = false;

		public CommandArguments(string[] args)
		{
			bool showHelp = false;

			OptionSet options = new OptionSet()
			{
				{
					"position=",
					"Move the window to the given {X,Y} coordinates (can be negative).\n"
						+ "--position=0,0 moves the window to the top-left edge of the MAIN screen.",
					v => position = ParsePoint(v)
				},
				{
					"start",
					"Start streaming immediately, using the previous settings. All warnings are ignored.",
					v => startStream = v != null
				},
				{
					"fps",
					"[Debug] Measure the time required to capture each frame and print it to console.",
					v => printFps = v != null
				},
				{
					"h|help|?", 
					"Show this message and exit",
					v => showHelp = v != null
				},
			};

			try
			{
				IEnumerable<string> extra = options.Parse(args);
				foreach (string arg in extra)
				{
					Console.WriteLine($"Warning: Unrecognized argument '{arg}'");
				}
			}
			catch (OptionException e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `DiscordAudioStream.exe --help' for more information.");
				ExitImmediately = true;
				return;
			}
			
			if (showHelp)
			{
				PrintHelp(options);
				ExitImmediately = true;
			}
		}

		public bool ExitImmediately { get; private set; } = false;
		

		public void ProcessArgsBeforeMainForm()
		{
			if (printFps)
			{
				Logger.Log("-fps flag used, printing FPS to console");
				CaptureSourceFactory.PrintFrameTime = true;
			}
		}

		public void ProcessArgsAfterMainForm(MainController controller)
		{
			if (startStream)
			{
				Logger.Log("-start flag used, starting stream");
				controller.StartStream(true);
			}
			if (position.HasValue)
			{
				Logger.Log($"-position flag used, moving window to {position.Value}");
				controller.MoveWindow(position.Value);
			}
		}


		private void PrintHelp(OptionSet options)
		{
			Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} [options]");
			Console.WriteLine();
			Console.WriteLine("Options:");
			options.WriteOptionDescriptions(Console.Out);
			Console.WriteLine("All options support Unix and Windows styles (-option, --option or /option)");
		}

		static private Point ParsePoint(string str)
		{
			string[] parts = str.Split(',');
			if (parts.Length != 2) throw new FormatException("Invalid point format. Expected \"X,Y\".");
			if (!int.TryParse(parts[0], out int x)) throw new FormatException("Invalid X coordinate.");
			if (!int.TryParse(parts[1], out int y)) throw new FormatException("Invalid Y coordinate.");

			return new Point(x, y);
		}
	}
}
