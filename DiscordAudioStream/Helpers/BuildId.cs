﻿namespace DiscordAudioStream;

internal static class BuildId
{
    // At compile-time, replace "__BUILD_ID__" with the current Unix time
    public static readonly string Id = "__BUILD_ID__";
}
