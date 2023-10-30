using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using CustomComponents;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace DiscordAudioStream;

public partial class MainForm : Form
{
    private readonly bool darkMode;
    private readonly Size defaultWindowSize;
    private readonly Size defaultPreviewSize;
    private readonly Point defaultPreviewLocation;

    public MainForm(bool darkMode)
    {
        Logger.EmptyLine();
        Logger.Log("Initializing MainForm. darkMode = " + darkMode);

        Controller = new(this);

        this.darkMode = darkMode;
        if (darkMode)
        {
            HandleCreated += new(DarkThemeManager.FormHandleCreated);
        }

        InitializeComponent();
        previewBox.Visible = true;

        defaultWindowSize = Size;
        defaultPreviewSize = previewBox.Size;
        defaultPreviewLocation = previewBox.Location;

        inputDeviceComboBox.SelectedIndex = 0;

        Controller.RefreshScreens();
        Controller.RefreshAudioDevices();

        previewBtn.Checked = Properties.Settings.Default.Preview;
        DisplayPreview(previewBtn.Checked);

        scaleComboBox.SelectedIndex = Math.Min(Properties.Settings.Default.ScaleIndex, scaleComboBox.Items.Count - 1);

        Controller.OnAudioMeterClosed += () => showAudioMeterToolStripMenuItem.Checked = false;

        ApplyDarkTheme(darkMode);

        toolTip.SetToolTip(captureCursorCheckBox, Properties.Resources.Tooltip_CaptureCursor);
        toolTip.SetToolTip(hideTaskbarCheckBox, Properties.Resources.Tooltip_HideTaskbar);
        toolTip.SetToolTip(areaComboBox, Properties.Resources.Tooltip_CaptureArea);
        toolTip.SetToolTip(areaLabel, Properties.Resources.Tooltip_CaptureArea);
        toolTip.SetToolTip(scaleComboBox, Properties.Resources.Tooltip_VideoScale);
        toolTip.SetToolTip(scaleLabel, Properties.Resources.Tooltip_VideoScale);
        toolTip.SetToolTip(inputDeviceComboBox, Properties.Resources.Tooltip_AudioSource);
        toolTip.SetToolTip(inputDeviceLabel, Properties.Resources.Tooltip_AudioSource);
        toolTip.SetToolTip(startButton, Properties.Resources.Tooltip_StartStream);
    }

    public MainController Controller { get; }

    // INTERNAL METHODS (called from controller)

    internal int VideoIndex
    {
        get => areaComboBox.SelectedIndex;
        set => areaComboBox.SelectedIndex = value;
    }

    internal void SetVideoItems(IEnumerable<(string, bool)> items)
    {
        areaComboBox.Items.Clear();
        foreach ((string item, bool hasSeparator) in items)
        {
            if (hasSeparator)
            {
                areaComboBox.Items.Add(new ComboBoxItemWithSeparator(item));
            }
            else
            {
                areaComboBox.Items.Add(item);
            }
        }
        areaComboBox.RefreshSeparators();
    }

    internal bool HasSomeAudioSource => inputDeviceComboBox.SelectedIndex > 0;

    internal int AudioSourceIndex => inputDeviceComboBox.SelectedIndex - 1;

    internal void SetAudioElements(IEnumerable<string> elements, int defaultIndex)
    {
        inputDeviceComboBox.Items.Clear();
        inputDeviceComboBox.Items.AddRange(elements.ToArray());
        inputDeviceComboBox.SelectedIndex = defaultIndex;
    }

    internal bool HideTaskbar => hideTaskbarCheckBox.Checked;

    internal bool HideTaskbarEnabled
    {
        get => hideTaskbarCheckBox.Enabled;
        set => hideTaskbarCheckBox.Enabled = value;
    }

    internal void SetPreviewUISize(Size newSize)
    {
        BeginInvoke(new Action(() => previewBox.Size = newSize));
    }

    internal void EnableStreamingUI(bool streaming)
    {
        // Autosize the form only while streaming
        AutoSize = streaming;

        // Disable UI elements while streaming
        videoGroup.Visible = !streaming;
        audioGroup.Visible = !streaming;
        startButton.Visible = !streaming;
        toolStrip.Visible = !streaming;

        if (streaming)
        {
            // If we start streaming, override previewBtn and enable the previewBox
            DisplayPreview(true);
            previewBox.Location = Point.Empty;
            Controller.ShowAudioMeterForm(darkMode);
            Text = Properties.Settings.Default.StreamTitle;
            previewBox.ContextMenuStrip = streamContextMenu;

            showAudioMeterToolStripMenuItem.Enabled = HasSomeAudioSource;
            showAudioMeterToolStripMenuItem.Checked = HasSomeAudioSource && Properties.Settings.Default.ShowAudioMeter;
        }
        else
        {
            DisplayPreview(previewBtn.Checked);
            previewBox.Size = defaultPreviewSize;
            previewBox.Location = defaultPreviewLocation;
            CenterToScreen();
            Controller.HideAudioMeterForm();
            Text = "Discord Audio Stream";
            previewBox.ContextMenuStrip = null;
        }
    }

    internal void UpdatePreview(Bitmap newImage, bool forceRefresh, HWND handle)
    {
        // This method is called in a worker thread, redraw the previewBox in the UI thread.
        // If needed, trigger a full redraw of the form, but do it in the worker thread to reduce
        // the load on the UI thread.
        Invoke(new Action(() =>
        {
            if (IsDisposed)
            {
                Logger.Log("Attempting to update preview after disposing: ignore");
                return;
            }
            previewBox.Image?.Dispose();
            previewBox.Image = newImage;
        }));
        if (forceRefresh)
        {
            // Windows only refreshes the part of the window that is shown to the user. Therefore, if this
            // window is partially off-screen, it won't be streamed correctly in Discord.
            // Use PrintWindow to send a WM_PRINT to our own window handle, forcing a complete redraw.
            PInvoke.PrintWindow(handle, (HDC)IntPtr.Zero, 0).AssertSuccess("PrintWindow failed");
        }
    }

    // PRIVATE METHODS

    private void ApplyDarkTheme(bool darkMode)
    {
        if (darkMode)
        {
            ForeColor = Color.White;
            BackColor = DarkThemeManager.DarkBackColor;

            aboutBtn.Image = Properties.Resources.white_about;
            onTopBtn.Image = Properties.Resources.white_ontop;
            volumeMixerButton.Image = Properties.Resources.white_mixer;
            soundDevicesButton.Image = Properties.Resources.white_speaker;
            settingsBtn.Image = Properties.Resources.white_settings;
            previewBtn.Image = Properties.Resources.white_preview;
        }

        videoGroup.SetDarkMode(darkMode);
        audioGroup.SetDarkMode(darkMode);
        toolStrip.SetDarkMode(darkMode, false);
        inputDeviceComboBox.SetDarkMode(darkMode);
        areaComboBox.SetDarkMode(darkMode);
        scaleComboBox.SetDarkMode(darkMode);
        captureCursorCheckBox.SetDarkMode(darkMode);
        hideTaskbarCheckBox.SetDarkMode(darkMode);
    }

    private void DisplayPreview(bool visible)
    {
        previewBox.Visible = visible;

        if (visible)
        {
            Size = defaultWindowSize;
        }
        else
        {
            Size newSize = Size;
            newSize.Width = defaultWindowSize.Width - (defaultPreviewSize.Width + 10);
            Size = newSize;

            previewBox.Image?.Dispose();
            previewBox.Image = null;
        }
    }

    // EVENTS

    private void MainForm_Load(object sender, EventArgs e)
    {
        Controller.Init();
        onTopBtn.Checked = Properties.Settings.Default.AlwaysOnTop;
        captureCursorCheckBox.Checked = Properties.Settings.Default.CaptureCursor;
        hideTaskbarCheckBox.Checked = Properties.Settings.Default.HideTaskbar;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        if (e.CloseReason == CloseReason.WindowsShutDown)
        {
            return;
        }

        // MainController.Stop() returns false if the form has to be closed
        e.Cancel = Controller.Stop();
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control)
        {
            switch (e.KeyCode)
            {
                case Keys.P:
                    previewBtn.PerformClick();
                    break;
                case Keys.T:
                    onTopBtn.PerformClick();
                    break;
                case Keys.Oemcomma:
                    settingsBtn.PerformClick();
                    break;
                case Keys.V:
                    volumeMixerButton.PerformClick();
                    break;
                case Keys.A:
                    soundDevicesButton.PerformClick();
                    break;
                case Keys.Enter:
                    if (!Controller.IsStreaming)
                    {
                        Controller.StartStream(false);
                    }
                    break;
            }
        }
        else
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    aboutBtn.PerformClick();
                    break;
                case Keys.Escape:
                    if (Controller.IsStreaming)
                    {
                        Controller.Stop();
                    }
                    break;
            }
        }
    }

    private void previewBtn_CheckedChanged(object sender, EventArgs e)
    {
        Properties.Settings.Default.Preview = previewBtn.Checked;
        Properties.Settings.Default.Save();

        DisplayPreview(previewBtn.Checked);
    }

    private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Properties.Settings.Default.AlwaysOnTop = onTopBtn.Checked;
        Properties.Settings.Default.Save();

        TopMost = onTopBtn.Checked;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S4036", Justification = "This ms-settings URI is safe")]
    private void volumeMixerButton_Click(object sender, EventArgs e)
    {
        if (Environment.OSVersion.Version.Major >= 10)
        {
            Process.Start("ms-settings:apps-volume");
        }
        else
        {
            // Use old volume mixer
            string cplPath = Path.Combine(Environment.SystemDirectory, "sndvol.exe");
            Process.Start(cplPath);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S4036", Justification = "This ms-settings URI is safe")]
    private void soundDevicesButton_Click(object sender, EventArgs e)
    {
        if (Environment.OSVersion.Version >= new Version(10, 0, 17063))
        {
            Process.Start("ms-settings:sound");
        }
        else
        {
            // Use old sound settings
            string cplPath = Path.Combine(Environment.SystemDirectory, "control.exe");
            Process.Start(cplPath, "/name Microsoft.Sound");
        }
    }

    private void settingsBtn_Click(object sender, EventArgs e)
    {
        Controller.ShowSettingsForm(darkMode);
    }

    private void aboutBtn_Click(object sender, EventArgs e)
    {
        Controller.ShowAboutForm(darkMode);
    }

    private void startButton_Click(object sender, EventArgs e)
    {
        Controller.StartStream(false);
    }

    private void areaComboBox_DropDown(object sender, EventArgs e)
    {
        // When the user expands the area combobox, update its elements
        Controller.UpdateAreaComboBox(VideoIndex);
    }

    private void areaComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Controller.SetVideoIndex(VideoIndex);
    }

    private void scaleComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Controller.SetScaleIndex(scaleComboBox.SelectedIndex);
    }

    private void hideTaskbarCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Controller.SetHideTaskbar(hideTaskbarCheckBox.Checked);
    }

    private void captureCursorCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Controller.SetCapturingCursor(captureCursorCheckBox.Checked);
    }

    private void showAudioMeterToolStripMenuItem_Click(object sender, EventArgs e)
    {
        bool show = !showAudioMeterToolStripMenuItem.Checked;

        showAudioMeterToolStripMenuItem.Checked = show;
        Properties.Settings.Default.ShowAudioMeter = show;
        Properties.Settings.Default.Save();

        if (show)
        {
            Controller.ShowAudioMeterForm(darkMode);
        }
        else
        {
            Controller.HideAudioMeterForm();
        }
    }

    private void stopStreamToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Controller.Stop();
    }
}
