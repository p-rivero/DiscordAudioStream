using System;
using System.Drawing;
using System.Windows.Forms;

using CustomComponents;

namespace DiscordAudioStream;

public partial class AudioMeterForm : Form
{
    public AudioMeterForm(bool darkMode)
    {
        Logger.EmptyLine();
        Logger.Log("Initializing AudioMeterForm. darkMode = " + darkMode);

        if (darkMode)
        {
            HandleCreated += DarkThemeManager.FormHandleCreated;
        }

        InitializeComponent();
        FormClosing += (s, e) => StorePosition();

        ApplyDarkTheme(darkMode);
    }

    private void ApplyDarkTheme(bool darkMode)
    {
        if (darkMode)
        {
            BackColor = DarkThemeManager.DarkBackColor;
            ForeColor = Color.White;
        }
    }

    public void SetLevels(float left, float right)
    {
        volumeMeterLeft.Amplitude = left;
        volumeMeterRight.Amplitude = right;
        volumeMeterRight.Refresh();
        volumeMeterLeft.Refresh();
    }

    public new void Show()
    {
        base.Show();
        RestoreSavedPosition();
    }

    public new void Hide()
    {
        StorePosition();
        base.Hide();
    }

    private void StorePosition()
    {
        Properties.Settings.Default.AudioMeterForm_Position = Location;
        Properties.Settings.Default.AudioMeterForm_Size = Size;
        Properties.Settings.Default.Save();
    }

    private void RestoreSavedPosition()
    {
        if (Properties.Settings.Default.AudioMeterForm_Size != Size.Empty)
        {
            Location = Properties.Settings.Default.AudioMeterForm_Position;
            Size = Properties.Settings.Default.AudioMeterForm_Size;
        }
        volumeMeterText.SetWindowWidth(Width);
        volumeMeterText2.SetWindowWidth(Width);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        volumeMeterText.SetWindowWidth(Width);
        volumeMeterText2.SetWindowWidth(Width);
    }
}
