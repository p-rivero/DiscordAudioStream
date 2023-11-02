using System.Drawing;
using System.Text.Json;

using DiscordAudioStream.Properties;

namespace DiscordAudioStream;

public record CapturePreset
(
    string lastVideoCaptureType,
    string lastVideoCaptureValue,
    Point areaFormPosition,
    Size areaFormSize,
    bool hideTaskbar,
    bool captureCursor,
    int scaleIndex,
    string audioDeviceID
)
{
    private const int MIN_SLOT = 1;
    private const int MAX_SLOT = 5;

    private static readonly List<CapturePreset?> presetList = DeserializeStoredPresets();

    public static CapturePreset FromCurrentSettings()
    {
        return new(
            Settings.Default.LastVideoCaptureType,
            Settings.Default.LastVideoCaptureValue,
            Settings.Default.AreaForm_Position,
            Settings.Default.AreaForm_Size,
            Settings.Default.HideTaskbar,
            Settings.Default.CaptureCursor,
            Settings.Default.ScaleIndex,
            Settings.Default.AudioDeviceID
        );
    }

    public void ApplyToSettings()
    {
        Settings.Default.LastVideoCaptureType = lastVideoCaptureType;
        Settings.Default.LastVideoCaptureValue = lastVideoCaptureValue;
        Settings.Default.AreaForm_Position = areaFormPosition;
        Settings.Default.AreaForm_Size = areaFormSize;
        Settings.Default.HideTaskbar = hideTaskbar;
        Settings.Default.CaptureCursor = captureCursor;
        Settings.Default.ScaleIndex = scaleIndex;
        Settings.Default.AudioDeviceID = audioDeviceID;
    }

    public static CapturePreset? LoadSlot(int slotNumber)
    {
        int index = SlotNumberToIndex(slotNumber);
        return presetList[index];
    }

    public void SaveToSlot(int slotNumber)
    {
        int index = SlotNumberToIndex(slotNumber);
        presetList[index] = this;
        SerializePresets(presetList);
    }

    public static IList<bool> PopulatedPresets => presetList.Select(p => p != null).ToList();

    private static List<CapturePreset?> DeserializeStoredPresets()
    {
        const int LIST_REQUIRED_SIZE = MAX_SLOT - MIN_SLOT + 1;
        string json = Settings.Default.StoredPresets;
        List<CapturePreset?> list = JsonSerializer.Deserialize<List<CapturePreset?>>(json) ?? new();
        while (list.Count < LIST_REQUIRED_SIZE)
        {
            list.Add(null);
        }
        return list;
    }

    private static void SerializePresets(List<CapturePreset?> presets)
    {
        Settings.Default.StoredPresets = JsonSerializer.Serialize(presets);
    }

    private static int SlotNumberToIndex(int slotNumber)
    {
        if (slotNumber is < MIN_SLOT or > MAX_SLOT)
        {
            throw new ArgumentOutOfRangeException(nameof(slotNumber), slotNumber, $"Slot number must be between {MIN_SLOT} and {MAX_SLOT}.");
        }
        return slotNumber - MIN_SLOT;
    }
}
