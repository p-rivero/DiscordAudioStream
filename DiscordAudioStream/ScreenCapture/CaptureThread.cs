using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
