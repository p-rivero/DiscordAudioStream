
using System.Runtime.InteropServices;

using Microsoft.Win32;

namespace DiscordAudioStream;

public static class FrameworkInfo
{
    // https://learn.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed#minimum-version
    private const int MINIMUM_VERSION_472 = 461808;

    private const string FRAMEWORK_VERSION_SUBKEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full";

    private static readonly int releaseNumber = GetReleaseNumber();

    public static bool VersionIsSupported => releaseNumber >= MINIMUM_VERSION_472;

    public static string VersionName => VersionIsSupported ? FrameworkDescription : $"{releaseNumber} (UNSUPPORTED)";

    private static int GetReleaseNumber()
    {
        object? release = Registry.GetValue(FRAMEWORK_VERSION_SUBKEY, "Release", null);
        if (release is null)
        {
            return -1;
        }
        return int.Parse(release.ToString());
    }

    // Encapsulate on separate property to avoid TypeLoadException
    private static string FrameworkDescription => RuntimeInformation.FrameworkDescription;
}
