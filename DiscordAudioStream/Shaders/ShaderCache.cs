using System.Reflection;

namespace DiscordAudioStream.Shaders;
public static class ShaderCache
{
    private static readonly Dictionary<string, byte[]> ShaderCacheDictionary = new();

    public static byte[] GetShader(string shaderName)
    {
        if (ShaderCacheDictionary.TryGetValue(shaderName, out byte[]? value))
        {
            return value;
        }

        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourceName = $"DiscordAudioStream.Shaders.{shaderName}.cso";

        using Stream stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Shader {shaderName} not found as an embedded resource.");
        using MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        byte[] shaderBytes = memoryStream.ToArray();
        ShaderCacheDictionary[shaderName] = shaderBytes;
        return shaderBytes;
    }
}
