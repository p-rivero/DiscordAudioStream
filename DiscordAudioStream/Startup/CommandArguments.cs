using System;

namespace DiscordAudioStream
{
	internal class CommandArguments
	{
		public CommandArguments(string[] args)
		{
			// TODO
		}

		public bool ExitImmediately { get; private set; }

		public void ProcessArgs(MainController controller)
		{
			// TODO
		}


		private void PrintHelp()
		{
			Console.WriteLine("Usage: DiscordAudioStream.exe [options]");
			
		}
	}
}
