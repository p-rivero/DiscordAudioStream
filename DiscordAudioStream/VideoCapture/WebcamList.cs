using AForge.Video.DirectShow;

namespace DiscordAudioStream.VideoCapture;

public class WebcamList
{
    private readonly List<FilterInfo> webcamList;

    private WebcamList(List<FilterInfo> webcamList)
    {
        this.webcamList = webcamList;
    }

    public static WebcamList Empty()
    {
        return new(new());
    }

    public static WebcamList Refresh()
    {
        FilterInfoCollection filterInfoCollection = new(FilterCategory.VideoInputDevice);
        List<FilterInfo> list = new(filterInfoCollection.Count);
        foreach (FilterInfo f in filterInfoCollection)
        {
            list.Add(f);
        }
        return new WebcamList(list);
    }

    public IEnumerable<string> Names => webcamList.Select(p => p.Name);

    public int Count => webcamList.Count;

    public string GetMonikerString(int index)
    {
        return webcamList[index].MonikerString;
    }

    public int IndexOfMonikerString(string monikerString)
    {
        int index = webcamList.FindIndex(p => p.MonikerString == monikerString);
        return index != -1 ? index : throw new InvalidOperationException("No webcam matches monikerString");
    }
}
