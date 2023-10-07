namespace DiscordAudioStream
{
    internal static class BuildId
    {
        // At compile-time, replace "__BUILD_ID__" with the current Unix time
        public readonly static string Id = "__BUILD_ID__";
    }
}
