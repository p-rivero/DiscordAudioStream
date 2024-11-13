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
        Logger.Log("Initializing MainForm. darkMode = " + darkMode);
        ShowMessage.UseDarkTheme = darkMode;

        Controller = new(this);

        this.darkMode = darkMode;
        if (darkMode)
        {
            HandleCreated += new(DarkThemeManager.FormHandleCreated);
        }

        InitializeComponent();
        ApplyDarkTheme(darkMode);
        previewBox.Visible = true;

        defaultWindowSize = Size;
        defaultPreviewSize = previewBox.Size;
        defaultPreviewLocation = previewBox.Location;

        previewBtn.Checked = Properties.Settings.Default.Preview;
        DisplayPreview(previewBtn.Checked);

        Controller.OnAudioMeterClosed += () => showAudioMeterToolStripMenuItem.Checked = false;

        toolTip.SetToolTip(captureCursorCheckBox, Properties.Resources.Tooltip_CaptureCursor);
        toolTip.SetToolTip(hideTaskbarCheckBox, Properties.Resources.Tooltip_HideTaskbar);
        toolTip.SetToolTip(areaComboBox, Properties.Resources.Tooltip_CaptureArea);
        toolTip.SetToolTip(areaLabel, Properties.Resources.Tooltip_CaptureArea);
        toolTip.SetToolTip(scaleComboBox, Properties.Resources.Tooltip_VideoScale);
        toolTip.SetToolTip(scaleLabel, Properties.Resources.Tooltip_VideoScale);
        toolTip.SetToolTip(inputDeviceComboBox, Properties.Resources.Tooltip_AudioSource);
        toolTip.SetToolTip(inputDeviceLabel, Properties.Resources.Tooltip_AudioSource);
        toolTip.SetToolTip(startButton, Properties.Resources.Tooltip_StartStream);

        toolStripLoadSlot1.Click += (sender, e) => Controller.LoadCapturePreset(1);
        toolStripLoadSlot2.Click += (sender, e) => Controller.LoadCapturePreset(2);
        toolStripLoadSlot3.Click += (sender, e) => Controller.LoadCapturePreset(3);
        toolStripLoadSlot4.Click += (sender, e) => Controller.LoadCapturePreset(4);
        toolStripLoadSlot5.Click += (sender, e) => Controller.LoadCapturePreset(5);
        toolStripStoreSlot1.Click += (sender, e) => MainController.SaveCapturePreset(1);
        toolStripStoreSlot2.Click += (sender, e) => MainController.SaveCapturePreset(2);
        toolStripStoreSlot3.Click += (sender, e) => MainController.SaveCapturePreset(3);
        toolStripStoreSlot4.Click += (sender, e) => MainController.SaveCapturePreset(4);
        toolStripStoreSlot5.Click += (sender, e) => MainController.SaveCapturePreset(5);
    }

    public MainController Controller { get; }

    internal Bitmap? OutputImage
    {
        get => previewBox.Image as Bitmap;
        private set
        {
            Image? old = previewBox.Image;
            previewBox.Image = value;
            old?.Dispose();
        }
    }

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
                _ = areaComboBox.Items.Add(new ComboBoxItemWithSeparator(item));
            }
            else
            {
                _ = areaComboBox.Items.Add(item);
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

    internal void RefreshCaptureUI()
    {
        Controller.RefreshScreens(restoreSavedItem: true);
        scaleComboBox.SelectedIndex = Properties.Settings.Default.ScaleIndex;
        captureCursorCheckBox.Checked = Properties.Settings.Default.CaptureCursor;
        hideTaskbarCheckBox.Checked = Properties.Settings.Default.HideTaskbar;
    }

    internal void SetPreviewUISize(Size newSize)
    {
        InvokeOnUI.RunAsync(() => previewBox.Size = newSize);
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
        }
        else
        {
            DisplayPreview(previewBtn.Checked);
            previewBox.Size = defaultPreviewSize;
            previewBox.Location = defaultPreviewLocation;
            CenterToScreen();
            Controller.HideAudioMeterForm();
            Text = "Discord Audio Stream";
        }
    }

    internal void UpdatePreview(Bitmap newImage, bool forceRefresh)
    {
        // This method is called in a worker thread, redraw the previewBox in the UI thread.
        // If needed, trigger a full redraw of the form, but do it in the worker thread to reduce
        // the load on the UI thread.
        HWND formHandle = InvokeOnUI.RunSync(() =>
        {
            if (!IsDisposed)
            {
                OutputImage = newImage;
            }
            return this.HWnd();
        });
        if (forceRefresh)
        {
            // Windows only refreshes the part of the window that is shown to the user. Therefore, if this
            // window is partially off-screen, it won't be streamed correctly in Discord.
            // Use PrintWindow to send a WM_PRINT to our own window handle, forcing a complete redraw.
            PInvoke.PrintWindow(formHandle, (HDC)IntPtr.Zero, 0).AssertSuccess("PrintWindow failed");
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
            managePresetsButton.Image = Properties.Resources.white_save;
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
            newSize.Width = defaultWindowSize.Width - (defaultPreviewSize.Width + 9);
            Size = newSize;
            OutputImage = null;
        }
    }

    // EVENTS

    private async void MainForm_Shown(object sender, EventArgs e)
    {
        ShowMessage.ParentWindow = this;
        // Wait for the black theme to be applied to avoid flashes
        await Task.Delay(100).ConfigureAwait(true);
        RefreshCaptureUI();
        Controller.Init();
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
        switch (e.KeyData)
        {
            case Keys.F1:
                aboutBtn.PerformClick();
                break;
            case Keys.Control | Keys.P:
                previewBtn.PerformClick();
                break;
            case Keys.Control | Keys.T:
                onTopBtn.PerformClick();
                break;
            case Keys.Control | Keys.Oemcomma:
                settingsBtn.PerformClick();
                break;
            case Keys.Control | Keys.V:
                volumeMixerButton.PerformClick();
                break;
            case Keys.Control | Keys.A:
                soundDevicesButton.PerformClick();
                break;
            case Keys.Control | Keys.Enter:
                if (!Controller.IsStreaming) { Controller.StartStream(false); }
                break;
            case Keys.Escape:
                if (Controller.IsStreaming) { Controller.Stop().AssertSuccess(); }
                break;
            case Keys.Control | Keys.D1:
                Controller.LoadCapturePreset(1);
                break;
            case Keys.Control | Keys.Shift | Keys.D1:
                MainController.SaveCapturePreset(1);
                break;
            case Keys.Control | Keys.D2:
                Controller.LoadCapturePreset(2);
                break;
            case Keys.Control | Keys.Shift | Keys.D2:
                MainController.SaveCapturePreset(2);
                break;
            case Keys.Control | Keys.D3:
                Controller.LoadCapturePreset(3);
                break;
            case Keys.Control | Keys.Shift | Keys.D3:
                MainController.SaveCapturePreset(3);
                break;
            case Keys.Control | Keys.D4:
                Controller.LoadCapturePreset(4);
                break;
            case Keys.Control | Keys.Shift | Keys.D4:
                MainController.SaveCapturePreset(4);
                break;
            case Keys.Control | Keys.D5:
                Controller.LoadCapturePreset(5);
                break;
            case Keys.Control | Keys.Shift | Keys.D5:
                MainController.SaveCapturePreset(5);
                break;
        }
    }

    private void previewBtn_CheckedChanged(object sender, EventArgs e)
    {
        Properties.Settings.Default.Preview = previewBtn.Checked;
        DisplayPreview(previewBtn.Checked);
    }

    private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Properties.Settings.Default.AlwaysOnTop = onTopBtn.Checked;
        TopMost = onTopBtn.Checked;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S4036", Justification = "This ms-settings URI is safe")]
    private void volumeMixerButton_Click(object sender, EventArgs e)
    {
        if (Environment.OSVersion.Version.Major >= 10)
        {
            _ = Process.Start("ms-settings:apps-volume");
        }
        else
        {
            // Use old volume mixer
            string cplPath = Path.Combine(Environment.SystemDirectory, "sndvol.exe");
            _ = Process.Start(cplPath);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S4036", Justification = "This ms-settings URI is safe")]
    private void soundDevicesButton_Click(object sender, EventArgs e)
    {
        if (Environment.OSVersion.Version >= new Version(10, 0, 17063))
        {
            _ = Process.Start("ms-settings:sound");
        }
        else
        {
            // Use old sound settings
            string cplPath = Path.Combine(Environment.SystemDirectory, "control.exe");
            _ = Process.Start(cplPath, "/name Microsoft.Sound");
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
        Controller.StartStream(skipAudioWarning: false);
    }

    private void areaComboBox_DropDown(object sender, EventArgs e)
    {
        // When the user expands the area combobox, update its elements
        Controller.RefreshScreens(restoreSavedItem: true);
    }

    private void areaComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Controller.SetVideoIndex(VideoIndex);
    }

    private void scaleComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        MainController.SetScaleIndex(scaleComboBox.SelectedIndex);
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
        Controller.Stop().AssertSuccess();
    }

    private void inputDeviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Controller.UpdateAudioIndex();
    }

    private void toolStripLoad_DropDownOpening(object sender, EventArgs e)
    {
        ToolStripMenuItem[] loadMenuItems = { toolStripLoadSlot1, toolStripLoadSlot2, toolStripLoadSlot3, toolStripLoadSlot4, toolStripLoadSlot5 };
        IList<bool> populatedPresets = MainController.GetPopulatedPresets();
        for (int i = 0; i < loadMenuItems.Length; i++)
        {
            loadMenuItems[i].Enabled = populatedPresets[i];
            loadMenuItems[i].Text = SlotText(i, populatedPresets[i]);
        }
    }

    private static string SlotText(int index, bool populated)
    {
        const int MIN_SLOT = 1;
        int slotNum = MIN_SLOT + index;
        return populated ? $"Slot {slotNum}   [Ctrl+{slotNum}]" : $"Slot {slotNum} (Empty)";
    }

    private void managePresetsButton_Click(object sender, EventArgs e)
    {
        streamContextMenu.Show(Cursor.Position);
    }

    private void streamContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        stopStreamToolStripMenuItem.Visible = Controller.IsStreaming;
        stopStreamToolStripSeparator.Visible = Controller.IsStreaming;
        showAudioMeterToolStripMenuItem.Visible = Controller.IsStreaming;
        showAudioMeterToolStripMenuItem.Enabled = HasSomeAudioSource;
        showAudioMeterToolStripMenuItem.Checked = HasSomeAudioSource && Properties.Settings.Default.ShowAudioMeter;
    }
}
