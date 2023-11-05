using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using CustomComponents;

using DiscordAudioStream.Properties;
using DiscordAudioStream.VideoCapture;

namespace DiscordAudioStream;

internal partial class SettingsForm : Form
{
    public event Action? CaptureMethodChanged;
    public event Action? FramerateChanged;
    public event Action? ShowAudioInputsChanged;

    private enum FrameRates
    {
        FPS_15 = 15,
        FPS_30 = 30,
        FPS_60 = 60
    }

    private readonly CaptureState captureState;

    public SettingsForm(bool darkMode, CaptureState captureState)
    {
        Logger.EmptyLine();
        Logger.Log("Initializing SettingsForm. darkMode=" + darkMode);

        // Store capture state in order to change ScreenMethod or WindowMethod
        this.captureState = captureState;

        // Enable dark titlebar
        if (darkMode)
        {
            HandleCreated += DarkThemeManager.FormHandleCreated;
        }

        InitializeComponent();

        ApplyDarkTheme(darkMode);

        // Set default values

        themeComboBox.SelectedIndex = Settings.Default.Theme;
        autoExitCheckbox.Checked = Settings.Default.AutoExit;
        outputLogCheckbox.Checked = Settings.Default.OutputLogFile;
        offscreenDrawCheckbox.Checked = Settings.Default.OffscreenDraw;
        showAudioInputsCheckbox.Checked = Settings.Default.ShowAudioInputs;
        streamTitleBox.Text = Settings.Default.StreamTitle;
        audioMeterCheckBox.Checked = Settings.Default.ShowAudioMeter;

        windowMethodComboBox.SelectedIndex = (int)captureState.WindowMethod;
        fullscreenMethodComboBox.SelectedIndex = (int)captureState.ScreenMethod;

        FrameRates selectedFramerate = (FrameRates)Settings.Default.CaptureFramerate;
        Array allFramerates = Enum.GetValues(typeof(FrameRates));
        captureFramerateComboBox.SelectedIndex = Array.IndexOf(allFramerates, selectedFramerate);

        // Set tooltips
        toolTip.SetToolTip(autoExitCheckbox, Resources.Tooltip_AutoExit);
        toolTip.SetToolTip(captureFramerateLabel, Resources.Tooltip_CaptureFramerate);
        toolTip.SetToolTip(captureFramerateComboBox, Resources.Tooltip_CaptureFramerate);
        toolTip.SetToolTip(fullscreenMethodLabel, Resources.Tooltip_FullscreenMethod);
        toolTip.SetToolTip(fullscreenMethodComboBox, Resources.Tooltip_FullscreenMethod);
        toolTip.SetToolTip(windowMethodLabel, Resources.Tooltip_WindowMethod);
        toolTip.SetToolTip(windowMethodComboBox, Resources.Tooltip_WindowMethod);
        toolTip.SetToolTip(outputLogCheckbox, Resources.Tooltip_OutputLog);
        toolTip.SetToolTip(offscreenDrawCheckbox, Resources.Tooltip_OffscreenDraw);
        toolTip.SetToolTip(showAudioInputsCheckbox, Resources.Tooltip_ShowAudioInputs);
        toolTip.SetToolTip(themeLabel, Resources.Tooltip_WindowTheme);
        toolTip.SetToolTip(themeComboBox, Resources.Tooltip_WindowTheme);
        toolTip.SetToolTip(streamTitleLabel, Resources.Tooltip_StreamTitle);
        toolTip.SetToolTip(streamTitleBox, Resources.Tooltip_StreamTitle);
        toolTip.SetToolTip(audioMeterCheckBox, Resources.Tooltip_ShowAudioMeter);

        Shown += (sender, e) => themeComboBox.Focus();
    }

    private void ApplyDarkTheme(bool darkMode)
    {
        if (darkMode)
        {
            BackColor = DarkThemeManager.DarkBackColor;
            ForeColor = Color.White;

            settingsTabs.BackTabColor = DarkThemeManager.DarkBackColor;
            settingsTabs.BorderColor = DarkThemeManager.DarkSecondColor;
            settingsTabs.HeaderColor = DarkThemeManager.DarkSecondColor;
            settingsTabs.TextColor = Color.White;
        }

        captureMethodGroup.SetDarkMode(darkMode);
        windowMethodComboBox.SetDarkMode(darkMode);
        fullscreenMethodComboBox.SetDarkMode(darkMode);
        captureFramerateComboBox.SetDarkMode(darkMode);
        themeComboBox.SetDarkMode(darkMode);
        streamTitleBox.SetDarkMode(darkMode);
        autoExitCheckbox.SetDarkMode(darkMode);
        outputLogCheckbox.SetDarkMode(darkMode);
        offscreenDrawCheckbox.SetDarkMode(darkMode);
        showAudioInputsCheckbox.SetDarkMode(darkMode);
        audioMeterCheckBox.SetDarkMode(darkMode);

        classicVolumeMixerLink.LinkColor = DarkThemeManager.AccentColor;
        audioDevicesLink.LinkColor = DarkThemeManager.AccentColor;
        settingsXMLLink.LinkColor = DarkThemeManager.AccentColor;
    }

    // Events

    private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    private void themeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        int theme = themeComboBox.SelectedIndex;
        // Nothing changed
        if (Settings.Default.Theme == theme)
        {
            return;
        }

        Settings.Default.Theme = theme;
        Logger.EmptyLine();
        Logger.Log($"Change settings: Theme={Settings.Default.Theme}. Restarting...");

        Application.Restart();
        Environment.Exit(0);
    }

    private void classicVolumeMixerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        string cplPath = Path.Combine(Environment.SystemDirectory, "sndvol.exe");
        _ = Process.Start(cplPath);
    }

    private void audioDevicesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        string cplPath = Path.Combine(Environment.SystemDirectory, "control.exe");
        _ = Process.Start(cplPath, "/name Microsoft.Sound");
    }

    private void settingsXMLLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        string settingsPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        Logger.Log($"Opening settings XML file: {settingsPath}");
        _ = Process.Start(settingsPath);
    }

    private void autoExitCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        // Nothing changed
        if (Settings.Default.AutoExit == autoExitCheckbox.Checked)
        {
            return;
        }

        Settings.Default.AutoExit = autoExitCheckbox.Checked;
        Logger.EmptyLine();
        Logger.Log("Change settings: AutoExit=" + Settings.Default.AutoExit);
    }

    private void outputLogCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        // Nothing changed
        if (Settings.Default.OutputLogFile == outputLogCheckbox.Checked)
        {
            return;
        }

        Settings.Default.OutputLogFile = outputLogCheckbox.Checked;
        Logger.EmptyLine();
        Logger.Log("Change settings: OutputLogFile=" + Settings.Default.OutputLogFile);
    }

    private void fullscreenMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        captureState.ScreenMethod = (CaptureState.ScreenCaptureMethod)fullscreenMethodComboBox.SelectedIndex;
        CaptureMethodChanged?.Invoke();
    }

    private void windowMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        captureState.WindowMethod = (CaptureState.WindowCaptureMethod)windowMethodComboBox.SelectedIndex;
        CaptureMethodChanged?.Invoke();
    }

    private void captureFramerateComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Array allFramerates = Enum.GetValues(typeof(FrameRates));
        FrameRates selectedFramerate = (FrameRates)allFramerates.GetValue(captureFramerateComboBox.SelectedIndex);

        // Nothing changed
        if (Settings.Default.CaptureFramerate == (int)selectedFramerate)
        {
            return;
        }

        Settings.Default.CaptureFramerate = (int)selectedFramerate;
        Logger.EmptyLine();
        Logger.Log("Change settings: CaptureFramerate=" + selectedFramerate);

        FramerateChanged?.Invoke();
    }

    private void offscreenDrawCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        // Nothing changed
        if (Settings.Default.OffscreenDraw == offscreenDrawCheckbox.Checked)
        {
            return;
        }

        Settings.Default.OffscreenDraw = offscreenDrawCheckbox.Checked;
        Logger.EmptyLine();
        Logger.Log("Change settings: OffscreenDraw=" + Settings.Default.OffscreenDraw);
    }

    private void SettingsForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _ = Process.Start(Resources.URL_CaptureMethodsInfoLink);
    }

    private void showAudioInputsCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        // Nothing changed
        if (Settings.Default.ShowAudioInputs == showAudioInputsCheckbox.Checked)
        {
            return;
        }

        Settings.Default.ShowAudioInputs = showAudioInputsCheckbox.Checked;
        Logger.EmptyLine();
        Logger.Log("Change settings: ShowAudioInputs=" + Settings.Default.ShowAudioInputs);

        ShowAudioInputsChanged?.Invoke();
    }

    private void streamTitleBox_TextChanged(object sender, EventArgs e)
    {
        // Nothing changed
        if (Settings.Default.StreamTitle == streamTitleBox.Text)
        {
            return;
        }
        try
        {
            Settings.Default.StreamTitle = streamTitleBox.Text;
            // Text could contain sensitive information, don't log it
            Logger.Log("Stream title saved successfully");
        }
        catch (ArgumentException ex)
        {
            Logger.Log("Failed to save stream title: " + ex.Message);
            // Saving could fail when auto-filling a character with many code points, like some emojis
            // Once the character has been completely filled, saving should succeed
        }
    }

    private void audioMeterCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        // Nothing changed
        if (Settings.Default.ShowAudioMeter == audioMeterCheckBox.Checked)
        {
            return;
        }

        Settings.Default.ShowAudioMeter = audioMeterCheckBox.Checked;
        Logger.EmptyLine();
        Logger.Log("Change settings: ShowAudioMeter=" + Settings.Default.ShowAudioMeter);
    }
}
