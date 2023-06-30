using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordAudioStream
{
	internal class ConsoleArguments
	{
		public ConsoleArguments(string[] args)
		{
			// TODO
		}

		public bool ExitImmediately { get; private set; }

		public void ProcessArgs(MainController controller)
		{

		}


		private void PrintHelp()
		{
			Console.WriteLine("Usage: DiscordAudioStream.exe [options]");
			
		}
	}
}
