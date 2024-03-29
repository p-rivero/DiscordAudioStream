﻿namespace DiscordAudioStream
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.settingsTabs = new CustomComponents.DarkThemeTabControl();
            this.windowPage = new System.Windows.Forms.TabPage();
            this.audioMeterCheckBox = new CustomComponents.DarkThemeCheckBox();
            this.streamTitleBox = new CustomComponents.DarkThemeTextBox();
            this.streamTitleLabel = new System.Windows.Forms.Label();
            this.themeComboBox = new CustomComponents.DarkThemeComboBox();
            this.themeLabel = new System.Windows.Forms.Label();
            this.capturePage = new System.Windows.Forms.TabPage();
            this.captureFramerateComboBox = new CustomComponents.DarkThemeComboBox();
            this.captureMethodGroup = new CustomComponents.DarkThemeGroupBox();
            this.windowMethodLabel = new System.Windows.Forms.Label();
            this.fullscreenMethodComboBox = new CustomComponents.DarkThemeComboBox();
            this.fullscreenMethodLabel = new System.Windows.Forms.Label();
            this.windowMethodComboBox = new CustomComponents.DarkThemeComboBox();
            this.captureFramerateLabel = new System.Windows.Forms.Label();
            this.autoExitCheckbox = new CustomComponents.DarkThemeCheckBox();
            this.debugPage = new System.Windows.Forms.TabPage();
            this.settingsXMLLink = new System.Windows.Forms.LinkLabel();
            this.showAudioInputsCheckbox = new CustomComponents.DarkThemeCheckBox();
            this.offscreenDrawCheckbox = new CustomComponents.DarkThemeCheckBox();
            this.outputLogCheckbox = new CustomComponents.DarkThemeCheckBox();
            this.audioDevicesLink = new System.Windows.Forms.LinkLabel();
            this.classicVolumeMixerLink = new System.Windows.Forms.LinkLabel();
            this.settingsTabs.SuspendLayout();
            this.windowPage.SuspendLayout();
            this.capturePage.SuspendLayout();
            this.captureMethodGroup.SuspendLayout();
            this.debugPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsTabs
            // 
            this.settingsTabs.AllowDrop = true;
            this.settingsTabs.BackTabColor = System.Drawing.Color.White;
            this.settingsTabs.BorderColor = System.Drawing.SystemColors.ControlLight;
            this.settingsTabs.Controls.Add(this.windowPage);
            this.settingsTabs.Controls.Add(this.capturePage);
            this.settingsTabs.Controls.Add(this.debugPage);
            this.settingsTabs.HeaderColor = System.Drawing.SystemColors.ControlLight;
            this.settingsTabs.ItemSize = new System.Drawing.Size(240, 16);
            this.settingsTabs.Location = new System.Drawing.Point(9, 9);
            this.settingsTabs.Margin = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.settingsTabs.Name = "settingsTabs";
            this.settingsTabs.SelectedIndex = 0;
            this.settingsTabs.Size = new System.Drawing.Size(256, 187);
            this.settingsTabs.TabIndex = 99;
            this.settingsTabs.TextColor = System.Drawing.Color.Black;
            // 
            // windowPage
            // 
            this.windowPage.BackColor = System.Drawing.Color.White;
            this.windowPage.Controls.Add(this.audioMeterCheckBox);
            this.windowPage.Controls.Add(this.streamTitleBox);
            this.windowPage.Controls.Add(this.streamTitleLabel);
            this.windowPage.Controls.Add(this.themeComboBox);
            this.windowPage.Controls.Add(this.themeLabel);
            this.windowPage.Location = new System.Drawing.Point(4, 20);
            this.windowPage.Margin = new System.Windows.Forms.Padding(0);
            this.windowPage.Name = "windowPage";
            this.windowPage.Padding = new System.Windows.Forms.Padding(3);
            this.windowPage.Size = new System.Drawing.Size(248, 163);
            this.windowPage.TabIndex = 0;
            this.windowPage.Text = "Window";
            // 
            // audioMeterCheckBox
            // 
            this.audioMeterCheckBox.Location = new System.Drawing.Point(9, 86);
            this.audioMeterCheckBox.Name = "audioMeterCheckBox";
            this.audioMeterCheckBox.Size = new System.Drawing.Size(233, 19);
            this.audioMeterCheckBox.TabIndex = 3;
            this.audioMeterCheckBox.Text = "Show audio meter window";
            this.audioMeterCheckBox.UseVisualStyleBackColor = true;
            this.audioMeterCheckBox.CheckedChanged += new System.EventHandler(this.audioMeterCheckBox_CheckedChanged);
            // 
            // streamTitleBox
            // 
            this.streamTitleBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.streamTitleBox.Location = new System.Drawing.Point(100, 50);
            this.streamTitleBox.MaxLength = 100;
            this.streamTitleBox.Name = "streamTitleBox";
            this.streamTitleBox.Size = new System.Drawing.Size(142, 23);
            this.streamTitleBox.TabIndex = 2;
            this.streamTitleBox.TextChanged += new System.EventHandler(this.streamTitleBox_TextChanged);
            // 
            // streamTitleLabel
            // 
            this.streamTitleLabel.AutoSize = true;
            this.streamTitleLabel.Location = new System.Drawing.Point(6, 53);
            this.streamTitleLabel.Name = "streamTitleLabel";
            this.streamTitleLabel.Size = new System.Drawing.Size(70, 15);
            this.streamTitleLabel.TabIndex = 41;
            this.streamTitleLabel.Text = "Stream title:";
            // 
            // themeComboBox
            // 
            this.themeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Items.AddRange(new object[] {
            "System default",
            "Light theme",
            "Dark theme"});
            this.themeComboBox.Location = new System.Drawing.Point(100, 16);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(142, 24);
            this.themeComboBox.TabIndex = 1;
            this.themeComboBox.SelectedIndexChanged += new System.EventHandler(this.themeComboBox_SelectedIndexChanged);
            // 
            // themeLabel
            // 
            this.themeLabel.AutoSize = true;
            this.themeLabel.Location = new System.Drawing.Point(6, 19);
            this.themeLabel.Name = "themeLabel";
            this.themeLabel.Size = new System.Drawing.Size(46, 15);
            this.themeLabel.TabIndex = 39;
            this.themeLabel.Text = "Theme:";
            // 
            // capturePage
            // 
            this.capturePage.BackColor = System.Drawing.Color.White;
            this.capturePage.Controls.Add(this.captureFramerateComboBox);
            this.capturePage.Controls.Add(this.captureMethodGroup);
            this.capturePage.Controls.Add(this.captureFramerateLabel);
            this.capturePage.Controls.Add(this.autoExitCheckbox);
            this.capturePage.Location = new System.Drawing.Point(4, 20);
            this.capturePage.Margin = new System.Windows.Forms.Padding(0);
            this.capturePage.Name = "capturePage";
            this.capturePage.Padding = new System.Windows.Forms.Padding(3);
            this.capturePage.Size = new System.Drawing.Size(248, 163);
            this.capturePage.TabIndex = 3;
            this.capturePage.Text = "Capture";
            // 
            // captureFramerateComboBox
            // 
            this.captureFramerateComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.captureFramerateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.captureFramerateComboBox.FormattingEnabled = true;
            this.captureFramerateComboBox.Items.AddRange(new object[] {
            "15 FPS",
            "30 FPS",
            "60 FPS"});
            this.captureFramerateComboBox.Location = new System.Drawing.Point(128, 34);
            this.captureFramerateComboBox.Name = "captureFramerateComboBox";
            this.captureFramerateComboBox.Size = new System.Drawing.Size(108, 24);
            this.captureFramerateComboBox.TabIndex = 2;
            this.captureFramerateComboBox.SelectedIndexChanged += new System.EventHandler(this.captureFramerateComboBox_SelectedIndexChanged);
            // 
            // captureMethodGroup
            // 
            this.captureMethodGroup.Controls.Add(this.windowMethodLabel);
            this.captureMethodGroup.Controls.Add(this.fullscreenMethodComboBox);
            this.captureMethodGroup.Controls.Add(this.fullscreenMethodLabel);
            this.captureMethodGroup.Controls.Add(this.windowMethodComboBox);
            this.captureMethodGroup.Location = new System.Drawing.Point(9, 66);
            this.captureMethodGroup.Name = "captureMethodGroup";
            this.captureMethodGroup.Size = new System.Drawing.Size(233, 90);
            this.captureMethodGroup.TabIndex = 6;
            this.captureMethodGroup.TabStop = false;
            this.captureMethodGroup.Text = "Capture method";
            // 
            // windowMethodLabel
            // 
            this.windowMethodLabel.AutoSize = true;
            this.windowMethodLabel.Location = new System.Drawing.Point(6, 55);
            this.windowMethodLabel.Name = "windowMethodLabel";
            this.windowMethodLabel.Size = new System.Drawing.Size(54, 15);
            this.windowMethodLabel.TabIndex = 2;
            this.windowMethodLabel.Text = "Window:";
            // 
            // fullscreenMethodComboBox
            // 
            this.fullscreenMethodComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.fullscreenMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fullscreenMethodComboBox.FormattingEnabled = true;
            this.fullscreenMethodComboBox.Items.AddRange(new object[] {
            "DXGI Duplication",
            "Windows 10",
            "BitBlt"});
            this.fullscreenMethodComboBox.Location = new System.Drawing.Point(81, 22);
            this.fullscreenMethodComboBox.Name = "fullscreenMethodComboBox";
            this.fullscreenMethodComboBox.Size = new System.Drawing.Size(146, 24);
            this.fullscreenMethodComboBox.TabIndex = 3;
            this.fullscreenMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.fullscreenMethodComboBox_SelectedIndexChanged);
            // 
            // fullscreenMethodLabel
            // 
            this.fullscreenMethodLabel.AutoSize = true;
            this.fullscreenMethodLabel.Location = new System.Drawing.Point(6, 25);
            this.fullscreenMethodLabel.Name = "fullscreenMethodLabel";
            this.fullscreenMethodLabel.Size = new System.Drawing.Size(63, 15);
            this.fullscreenMethodLabel.TabIndex = 1;
            this.fullscreenMethodLabel.Text = "Fullscreen:";
            // 
            // windowMethodComboBox
            // 
            this.windowMethodComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.windowMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.windowMethodComboBox.FormattingEnabled = true;
            this.windowMethodComboBox.Items.AddRange(new object[] {
            "Windows 10",
            "BitBlt",
            "PrintWindow"});
            this.windowMethodComboBox.Location = new System.Drawing.Point(81, 52);
            this.windowMethodComboBox.Name = "windowMethodComboBox";
            this.windowMethodComboBox.Size = new System.Drawing.Size(146, 24);
            this.windowMethodComboBox.TabIndex = 4;
            this.windowMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.windowMethodComboBox_SelectedIndexChanged);
            // 
            // captureFramerateLabel
            // 
            this.captureFramerateLabel.AutoSize = true;
            this.captureFramerateLabel.Location = new System.Drawing.Point(6, 37);
            this.captureFramerateLabel.Name = "captureFramerateLabel";
            this.captureFramerateLabel.Size = new System.Drawing.Size(106, 15);
            this.captureFramerateLabel.TabIndex = 6;
            this.captureFramerateLabel.Text = "Capture framerate:";
            // 
            // autoExitCheckbox
            // 
            this.autoExitCheckbox.Location = new System.Drawing.Point(9, 9);
            this.autoExitCheckbox.Name = "autoExitCheckbox";
            this.autoExitCheckbox.Size = new System.Drawing.Size(233, 19);
            this.autoExitCheckbox.TabIndex = 1;
            this.autoExitCheckbox.Text = "Exit when captured window closes";
            this.autoExitCheckbox.UseVisualStyleBackColor = true;
            this.autoExitCheckbox.CheckedChanged += new System.EventHandler(this.autoExitCheckbox_CheckedChanged);
            // 
            // debugPage
            // 
            this.debugPage.BackColor = System.Drawing.Color.White;
            this.debugPage.Controls.Add(this.settingsXMLLink);
            this.debugPage.Controls.Add(this.showAudioInputsCheckbox);
            this.debugPage.Controls.Add(this.offscreenDrawCheckbox);
            this.debugPage.Controls.Add(this.outputLogCheckbox);
            this.debugPage.Controls.Add(this.audioDevicesLink);
            this.debugPage.Controls.Add(this.classicVolumeMixerLink);
            this.debugPage.Location = new System.Drawing.Point(4, 20);
            this.debugPage.Margin = new System.Windows.Forms.Padding(0);
            this.debugPage.Name = "debugPage";
            this.debugPage.Size = new System.Drawing.Size(248, 163);
            this.debugPage.TabIndex = 2;
            this.debugPage.Text = "Debug";
            // 
            // settingsXMLLink
            // 
            this.settingsXMLLink.AutoSize = true;
            this.settingsXMLLink.Location = new System.Drawing.Point(6, 138);
            this.settingsXMLLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.settingsXMLLink.Name = "settingsXMLLink";
            this.settingsXMLLink.Size = new System.Drawing.Size(126, 15);
            this.settingsXMLLink.TabIndex = 6;
            this.settingsXMLLink.TabStop = true;
            this.settingsXMLLink.Text = "Open settings XML file";
            this.settingsXMLLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.settingsXMLLink_LinkClicked);
            // 
            // showAudioInputsCheckbox
            // 
            this.showAudioInputsCheckbox.Location = new System.Drawing.Point(9, 59);
            this.showAudioInputsCheckbox.Name = "showAudioInputsCheckbox";
            this.showAudioInputsCheckbox.Size = new System.Drawing.Size(200, 19);
            this.showAudioInputsCheckbox.TabIndex = 3;
            this.showAudioInputsCheckbox.Text = "Show audio input devices";
            this.showAudioInputsCheckbox.UseVisualStyleBackColor = true;
            this.showAudioInputsCheckbox.CheckedChanged += new System.EventHandler(this.showAudioInputsCheckbox_CheckedChanged);
            // 
            // offscreenDrawCheckbox
            // 
            this.offscreenDrawCheckbox.Location = new System.Drawing.Point(9, 34);
            this.offscreenDrawCheckbox.Name = "offscreenDrawCheckbox";
            this.offscreenDrawCheckbox.Size = new System.Drawing.Size(200, 19);
            this.offscreenDrawCheckbox.TabIndex = 2;
            this.offscreenDrawCheckbox.Text = "Force screen redraw";
            this.offscreenDrawCheckbox.UseVisualStyleBackColor = true;
            this.offscreenDrawCheckbox.CheckedChanged += new System.EventHandler(this.offscreenDrawCheckbox_CheckedChanged);
            // 
            // outputLogCheckbox
            // 
            this.outputLogCheckbox.Location = new System.Drawing.Point(9, 9);
            this.outputLogCheckbox.Name = "outputLogCheckbox";
            this.outputLogCheckbox.Size = new System.Drawing.Size(200, 19);
            this.outputLogCheckbox.TabIndex = 1;
            this.outputLogCheckbox.Text = "Output log file";
            this.outputLogCheckbox.UseVisualStyleBackColor = true;
            this.outputLogCheckbox.CheckedChanged += new System.EventHandler(this.outputLogCheckbox_CheckedChanged);
            // 
            // audioDevicesLink
            // 
            this.audioDevicesLink.AutoSize = true;
            this.audioDevicesLink.Location = new System.Drawing.Point(6, 114);
            this.audioDevicesLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.audioDevicesLink.Name = "audioDevicesLink";
            this.audioDevicesLink.Size = new System.Drawing.Size(125, 15);
            this.audioDevicesLink.TabIndex = 5;
            this.audioDevicesLink.TabStop = true;
            this.audioDevicesLink.Text = "Manage audio devices";
            this.audioDevicesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.audioDevicesLink_LinkClicked);
            // 
            // classicVolumeMixerLink
            // 
            this.classicVolumeMixerLink.AutoSize = true;
            this.classicVolumeMixerLink.Location = new System.Drawing.Point(6, 90);
            this.classicVolumeMixerLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.classicVolumeMixerLink.Name = "classicVolumeMixerLink";
            this.classicVolumeMixerLink.Size = new System.Drawing.Size(149, 15);
            this.classicVolumeMixerLink.TabIndex = 4;
            this.classicVolumeMixerLink.TabStop = true;
            this.classicVolumeMixerLink.Text = "Open classic volume mixer";
            this.classicVolumeMixerLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.classicVolumeMixerLink_LinkClicked);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(274, 204);
            this.Controls.Add(this.settingsTabs);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.SettingsForm_HelpButtonClicked);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingsForm_KeyDown);
            this.settingsTabs.ResumeLayout(false);
            this.windowPage.ResumeLayout(false);
            this.windowPage.PerformLayout();
            this.capturePage.ResumeLayout(false);
            this.capturePage.PerformLayout();
            this.captureMethodGroup.ResumeLayout(false);
            this.captureMethodGroup.PerformLayout();
            this.debugPage.ResumeLayout(false);
            this.debugPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage debugPage;
        private System.Windows.Forms.TabPage windowPage;
        private CustomComponents.DarkThemeTabControl settingsTabs;
        private System.Windows.Forms.TabPage capturePage;
        private CustomComponents.DarkThemeCheckBox autoExitCheckbox;
        private System.Windows.Forms.Label fullscreenMethodLabel;
        private System.Windows.Forms.Label windowMethodLabel;
        private CustomComponents.DarkThemeComboBox fullscreenMethodComboBox;
        private CustomComponents.DarkThemeComboBox windowMethodComboBox;
        private CustomComponents.DarkThemeGroupBox captureMethodGroup;
        private System.Windows.Forms.LinkLabel audioDevicesLink;
        private System.Windows.Forms.LinkLabel classicVolumeMixerLink;
        private CustomComponents.DarkThemeCheckBox outputLogCheckbox;
        private CustomComponents.DarkThemeComboBox captureFramerateComboBox;
        private System.Windows.Forms.Label captureFramerateLabel;
        private CustomComponents.DarkThemeCheckBox offscreenDrawCheckbox;
        private System.Windows.Forms.ToolTip toolTip;
        private CustomComponents.DarkThemeCheckBox showAudioInputsCheckbox;
        private System.Windows.Forms.Label themeLabel;
        private CustomComponents.DarkThemeComboBox themeComboBox;
        private System.Windows.Forms.Label streamTitleLabel;
        private CustomComponents.DarkThemeTextBox streamTitleBox;
        private CustomComponents.DarkThemeCheckBox audioMeterCheckBox;
        private System.Windows.Forms.LinkLabel settingsXMLLink;
    }
}
