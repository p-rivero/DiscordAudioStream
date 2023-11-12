
using System.Reflection;
using System.Text;

namespace DiscordAudioStream;

public static class BuildInfo
{
    // At compile-time, replace "__BUILD_ID__" with the current Unix time
    public const string BUILD_ID = "__BUILD_ID__";

    public static string FullInfo => GetFullInfo();

    public static Version Version => GetVersionWithoutRevision();

    public static string Bitness => Environment.Is64BitProcess ? "64 bit" : "32 bit";

    private static string GetFullInfo()
    {
        try
        {
            return new StringBuilder()
                .AppendLine($"OS Version: {Environment.OSVersion} ({Bitness})")
                .AppendLine($"Installed framework: {FrameworkInfo.VersionName}")
                .AppendLine($"Build ID: {BUILD_ID} ({Version})")
                .AppendLine($"Log ID: {new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}")
                .ToString();
        }
        catch (Exception ex)
        {
            return $"Failed to get startup info: {ex}";
        }
    }

    private static Version GetVersionWithoutRevision()
    {
        Version v = Assembly.GetExecutingAssembly().GetName().Version;
        return new Version(v.Major, v.Minor, v.Build);
    }
}
