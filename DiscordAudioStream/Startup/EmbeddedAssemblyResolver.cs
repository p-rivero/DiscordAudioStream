using System;
using System.Reflection;
using System.Resources;

namespace DiscordAudioStream
{
	internal static class EmbeddedAssemblyResolver
	{
		public static void Register()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
			dllName = dllName.Replace(".", "_");

			if (dllName.EndsWith("_resources")) return null;

			Logger.Log("Loading assembly: " + dllName);

			ResourceManager rm = new ResourceManager(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".Properties.Resources", Assembly.GetExecutingAssembly());

			byte[] bytes = (byte[])rm.GetObject(dllName);

			return Assembly.Load(bytes);
		}
	}
}
