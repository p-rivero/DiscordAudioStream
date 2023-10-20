namespace DiscordAudioStream.ScreenCapture;
internal class CaptureThread
{
    private readonly Thread thread;

    public CaptureThread()
    {
        thread = new(Execute);
    }

    private void Execute()
    {
        throw new NotImplementedException();
    }
}
