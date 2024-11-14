﻿namespace DiscordAudioStream
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.previewBox = new CustomComponents.PictureBoxWithoutAntiAliasing();
            this.streamContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showAudioMeterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoadSlot1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoadSlot2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoadSlot3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoadSlot4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoadSlot5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStoreSlot1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStoreSlot2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStoreSlot3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStoreSlot4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStoreSlot5 = new System.Windows.Forms.ToolStripMenuItem();
            this.stopStreamToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.stopStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip = new CustomComponents.DarkThemeToolStrip();
            this.previewBtn = new System.Windows.Forms.ToolStripButton();
            this.onTopBtn = new System.Windows.Forms.ToolStripButton();
            this.managePresetsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.volumeMixerButton = new System.Windows.Forms.ToolStripButton();
            this.soundDevicesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsBtn = new System.Windows.Forms.ToolStripButton();
            this.aboutBtn = new System.Windows.Forms.ToolStripButton();
            this.audioGroup = new CustomComponents.DarkThemeGroupBox();
            this.inputDeviceComboBox = new CustomComponents.DarkThemeComboBox();
            this.inputDeviceLabel = new System.Windows.Forms.Label();
            this.videoGroup = new CustomComponents.DarkThemeGroupBox();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.scaleComboBox = new CustomComponents.DarkThemeComboBox();
            this.hideTaskbarCheckBox = new CustomComponents.DarkThemeCheckBox();
            this.areaComboBox = new CustomComponents.DarkThemeComboBox();
            this.captureCursorCheckBox = new CustomComponents.DarkThemeCheckBox();
            this.areaLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.streamContextMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.audioGroup.SuspendLayout();
            this.videoGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // previewBox
            // 
            this.previewBox.BackColor = System.Drawing.Color.Black;
            this.previewBox.ContextMenuStrip = this.streamContextMenu;
            this.previewBox.DisableAntiAliasing = false;
            this.previewBox.InitialImage = null;
            this.previewBox.Location = new System.Drawing.Point(358, 9);
            this.previewBox.Margin = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(400, 225);
            this.previewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.previewBox.TabIndex = 26;
            this.previewBox.TabStop = false;
            // 
            // streamContextMenu
            // 
            this.streamContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAudioMeterToolStripMenuItem,
            this.toolStripLoad,
            this.toolStripStore,
            this.stopStreamToolStripSeparator,
            this.stopStreamToolStripMenuItem});
            this.streamContextMenu.Name = "contextMenu";
            this.streamContextMenu.Size = new System.Drawing.Size(180, 98);
            this.streamContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.streamContextMenu_Opening);
            // 
            // showAudioMeterToolStripMenuItem
            // 
            this.showAudioMeterToolStripMenuItem.Name = "showAudioMeterToolStripMenuItem";
            this.showAudioMeterToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.showAudioMeterToolStripMenuItem.Text = "Show audio meter";
            this.showAudioMeterToolStripMenuItem.Click += new System.EventHandler(this.showAudioMeterToolStripMenuItem_Click);
            // 
            // toolStripLoad
            // 
            this.toolStripLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLoadSlot1,
            this.toolStripLoadSlot2,
            this.toolStripLoadSlot3,
            this.toolStripLoadSlot4,
            this.toolStripLoadSlot5});
            this.toolStripLoad.Name = "toolStripLoad";
            this.toolStripLoad.Size = new System.Drawing.Size(179, 22);
            this.toolStripLoad.Text = "Load capture preset";
            this.toolStripLoad.DropDownOpening += new System.EventHandler(this.toolStripLoad_DropDownOpening);
            // 
            // toolStripLoadSlot1
            // 
            this.toolStripLoadSlot1.Name = "toolStripLoadSlot1";
            this.toolStripLoadSlot1.Size = new System.Drawing.Size(103, 22);
            this.toolStripLoadSlot1.Text = "Slot 1";
            // 
            // toolStripLoadSlot2
            // 
            this.toolStripLoadSlot2.Name = "toolStripLoadSlot2";
            this.toolStripLoadSlot2.Size = new System.Drawing.Size(103, 22);
            this.toolStripLoadSlot2.Text = "Slot 2";
            // 
            // toolStripLoadSlot3
            // 
            this.toolStripLoadSlot3.Name = "toolStripLoadSlot3";
            this.toolStripLoadSlot3.Size = new System.Drawing.Size(103, 22);
            this.toolStripLoadSlot3.Text = "Slot 3";
            // 
            // toolStripLoadSlot4
            // 
            this.toolStripLoadSlot4.Name = "toolStripLoadSlot4";
            this.toolStripLoadSlot4.Size = new System.Drawing.Size(103, 22);
            this.toolStripLoadSlot4.Text = "Slot 4";
            // 
            // toolStripLoadSlot5
            // 
            this.toolStripLoadSlot5.Name = "toolStripLoadSlot5";
            this.toolStripLoadSlot5.Size = new System.Drawing.Size(103, 22);
            this.toolStripLoadSlot5.Text = "Slot 5";
            // 
            // toolStripStore
            // 
            this.toolStripStore.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStoreSlot1,
            this.toolStripStoreSlot2,
            this.toolStripStoreSlot3,
            this.toolStripStoreSlot4,
            this.toolStripStoreSlot5});
            this.toolStripStore.Name = "toolStripStore";
            this.toolStripStore.Size = new System.Drawing.Size(179, 22);
            this.toolStripStore.Text = "Store capture preset";
            // 
            // toolStripStoreSlot1
            // 
            this.toolStripStoreSlot1.Name = "toolStripStoreSlot1";
            this.toolStripStoreSlot1.Size = new System.Drawing.Size(176, 22);
            this.toolStripStoreSlot1.Text = "Slot 1   [Ctrl+Alt+1]";
            // 
            // toolStripStoreSlot2
            // 
            this.toolStripStoreSlot2.Name = "toolStripStoreSlot2";
            this.toolStripStoreSlot2.Size = new System.Drawing.Size(176, 22);
            this.toolStripStoreSlot2.Text = "Slot 2   [Ctrl+Alt+2]";
            // 
            // toolStripStoreSlot3
            // 
            this.toolStripStoreSlot3.Name = "toolStripStoreSlot3";
            this.toolStripStoreSlot3.Size = new System.Drawing.Size(176, 22);
            this.toolStripStoreSlot3.Text = "Slot 3   [Ctrl+Alt+3]";
            // 
            // toolStripStoreSlot4
            // 
            this.toolStripStoreSlot4.Name = "toolStripStoreSlot4";
            this.toolStripStoreSlot4.Size = new System.Drawing.Size(176, 22);
            this.toolStripStoreSlot4.Text = "Slot 4   [Ctrl+Alt+4]";
            // 
            // toolStripStoreSlot5
            // 
            this.toolStripStoreSlot5.Name = "toolStripStoreSlot5";
            this.toolStripStoreSlot5.Size = new System.Drawing.Size(176, 22);
            this.toolStripStoreSlot5.Text = "Slot 5   [Ctrl+Alt+5]";
            // 
            // stopStreamToolStripSeparator
            // 
            this.stopStreamToolStripSeparator.Name = "stopStreamToolStripSeparator";
            this.stopStreamToolStripSeparator.Size = new System.Drawing.Size(176, 6);
            // 
            // stopStreamToolStripMenuItem
            // 
            this.stopStreamToolStripMenuItem.Name = "stopStreamToolStripMenuItem";
            this.stopStreamToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.stopStreamToolStripMenuItem.Text = "Stop stream";
            this.stopStreamToolStripMenuItem.Click += new System.EventHandler(this.stopStreamToolStripMenuItem_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startButton.Location = new System.Drawing.Point(219, 14);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(129, 34);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "START STREAM";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip.CanOverflow = false;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewBtn,
            this.onTopBtn,
            this.managePresetsButton,
            this.toolStripSeparator1,
            this.volumeMixerButton,
            this.soundDevicesButton,
            this.toolStripSeparator2,
            this.settingsBtn,
            this.aboutBtn});
            this.toolStrip.Location = new System.Drawing.Point(6, 9);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(9);
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(212, 45);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.TabStop = true;
            this.toolStrip.Text = "toolStrip1";
            // 
            // previewBtn
            // 
            this.previewBtn.AutoSize = false;
            this.previewBtn.CheckOnClick = true;
            this.previewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.previewBtn.Image = ((System.Drawing.Image)(resources.GetObject("previewBtn.Image")));
            this.previewBtn.Margin = new System.Windows.Forms.Padding(0);
            this.previewBtn.Name = "previewBtn";
            this.previewBtn.Size = new System.Drawing.Size(24, 24);
            this.previewBtn.Text = "Preview | Ctrl + P";
            this.previewBtn.ToolTipText = "Preview | Ctrl + P";
            this.previewBtn.CheckedChanged += new System.EventHandler(this.previewBtn_CheckedChanged);
            // 
            // onTopBtn
            // 
            this.onTopBtn.AutoSize = false;
            this.onTopBtn.Checked = global::DiscordAudioStream.Properties.Settings.Default.AlwaysOnTop;
            this.onTopBtn.CheckOnClick = true;
            this.onTopBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.onTopBtn.Image = ((System.Drawing.Image)(resources.GetObject("onTopBtn.Image")));
            this.onTopBtn.Margin = new System.Windows.Forms.Padding(0);
            this.onTopBtn.Name = "onTopBtn";
            this.onTopBtn.Size = new System.Drawing.Size(24, 24);
            this.onTopBtn.Text = "Always on top | Ctrl + T";
            this.onTopBtn.ToolTipText = "Always on top | Ctrl+T";
            this.onTopBtn.CheckedChanged += new System.EventHandler(this.onTopCheckBox_CheckedChanged);
            // 
            // managePresetsButton
            // 
            this.managePresetsButton.AutoSize = false;
            this.managePresetsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.managePresetsButton.Image = ((System.Drawing.Image)(resources.GetObject("managePresetsButton.Image")));
            this.managePresetsButton.Margin = new System.Windows.Forms.Padding(0);
            this.managePresetsButton.Name = "managePresetsButton";
            this.managePresetsButton.Size = new System.Drawing.Size(24, 24);
            this.managePresetsButton.Text = "Manage capture presets";
            this.managePresetsButton.Click += new System.EventHandler(this.managePresetsButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(2, 20);
            // 
            // volumeMixerButton
            // 
            this.volumeMixerButton.AutoSize = false;
            this.volumeMixerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.volumeMixerButton.Image = ((System.Drawing.Image)(resources.GetObject("volumeMixerButton.Image")));
            this.volumeMixerButton.Name = "volumeMixerButton";
            this.volumeMixerButton.Size = new System.Drawing.Size(24, 24);
            this.volumeMixerButton.Text = "Open volume mixer | Ctrl+V";
            this.volumeMixerButton.Click += new System.EventHandler(this.volumeMixerButton_Click);
            // 
            // soundDevicesButton
            // 
            this.soundDevicesButton.AutoSize = false;
            this.soundDevicesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.soundDevicesButton.Image = ((System.Drawing.Image)(resources.GetObject("soundDevicesButton.Image")));
            this.soundDevicesButton.Name = "soundDevicesButton";
            this.soundDevicesButton.Size = new System.Drawing.Size(24, 24);
            this.soundDevicesButton.Text = "Configure audio devices | Ctrl+A";
            this.soundDevicesButton.Click += new System.EventHandler(this.soundDevicesButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(2, 20);
            // 
            // settingsBtn
            // 
            this.settingsBtn.AutoSize = false;
            this.settingsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsBtn.Image = ((System.Drawing.Image)(resources.GetObject("settingsBtn.Image")));
            this.settingsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(24, 24);
            this.settingsBtn.Text = "Settings | Ctrl+Comma";
            this.settingsBtn.Click += new System.EventHandler(this.settingsBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.AutoSize = false;
            this.aboutBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboutBtn.Image = ((System.Drawing.Image)(resources.GetObject("aboutBtn.Image")));
            this.aboutBtn.Margin = new System.Windows.Forms.Padding(0);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(24, 24);
            this.aboutBtn.Text = "About | F1";
            this.aboutBtn.Click += new System.EventHandler(this.aboutBtn_Click);
            // 
            // audioGroup
            // 
            this.audioGroup.Controls.Add(this.inputDeviceComboBox);
            this.audioGroup.Controls.Add(this.inputDeviceLabel);
            this.audioGroup.Location = new System.Drawing.Point(9, 173);
            this.audioGroup.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.audioGroup.Name = "audioGroup";
            this.audioGroup.Size = new System.Drawing.Size(340, 56);
            this.audioGroup.TabIndex = 16;
            this.audioGroup.TabStop = false;
            this.audioGroup.Text = "Audio capture";
            // 
            // inputDeviceComboBox
            // 
            this.inputDeviceComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.inputDeviceComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.inputDeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputDeviceComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.inputDeviceComboBox.FormattingEnabled = true;
            this.inputDeviceComboBox.IntegralHeight = false;
            this.inputDeviceComboBox.Items.AddRange(new object[] {
            "None",
            "System sounds (Soundcard)"});
            this.inputDeviceComboBox.Location = new System.Drawing.Point(55, 22);
            this.inputDeviceComboBox.Name = "inputDeviceComboBox";
            this.inputDeviceComboBox.Size = new System.Drawing.Size(279, 24);
            this.inputDeviceComboBox.TabIndex = 7;
            this.inputDeviceComboBox.SelectedIndexChanged += new System.EventHandler(this.inputDeviceComboBox_SelectedIndexChanged);
            // 
            // inputDeviceLabel
            // 
            this.inputDeviceLabel.AutoSize = true;
            this.inputDeviceLabel.Location = new System.Drawing.Point(11, 26);
            this.inputDeviceLabel.Name = "inputDeviceLabel";
            this.inputDeviceLabel.Size = new System.Drawing.Size(38, 15);
            this.inputDeviceLabel.TabIndex = 26;
            this.inputDeviceLabel.Text = "Input:";
            // 
            // videoGroup
            // 
            this.videoGroup.Controls.Add(this.scaleLabel);
            this.videoGroup.Controls.Add(this.scaleComboBox);
            this.videoGroup.Controls.Add(this.hideTaskbarCheckBox);
            this.videoGroup.Controls.Add(this.areaComboBox);
            this.videoGroup.Controls.Add(this.captureCursorCheckBox);
            this.videoGroup.Controls.Add(this.areaLabel);
            this.videoGroup.Location = new System.Drawing.Point(9, 55);
            this.videoGroup.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.videoGroup.Name = "videoGroup";
            this.videoGroup.Size = new System.Drawing.Size(340, 109);
            this.videoGroup.TabIndex = 6;
            this.videoGroup.TabStop = false;
            this.videoGroup.Text = "Video capture";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(12, 52);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(37, 15);
            this.scaleLabel.TabIndex = 24;
            this.scaleLabel.Text = "Scale:";
            // 
            // scaleComboBox
            // 
            this.scaleComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.scaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scaleComboBox.FormattingEnabled = true;
            this.scaleComboBox.IntegralHeight = false;
            this.scaleComboBox.Items.AddRange(new object[] {
            "Limit to 720p",
            "Limit to 1080p",
            "100% window size",
            "50% window size",
            "25% window size",
            "20% window size (not recommended)",
            "15% window size (not recommended)",
            "10% window size (not recommended)"});
            this.scaleComboBox.Location = new System.Drawing.Point(55, 49);
            this.scaleComboBox.Name = "scaleComboBox";
            this.scaleComboBox.Size = new System.Drawing.Size(279, 24);
            this.scaleComboBox.TabIndex = 4;
            this.scaleComboBox.SelectedIndexChanged += new System.EventHandler(this.scaleComboBox_SelectedIndexChanged);
            // 
            // hideTaskbarCheckBox
            // 
            this.hideTaskbarCheckBox.Location = new System.Drawing.Point(132, 79);
            this.hideTaskbarCheckBox.Name = "hideTaskbarCheckBox";
            this.hideTaskbarCheckBox.Size = new System.Drawing.Size(92, 19);
            this.hideTaskbarCheckBox.TabIndex = 5;
            this.hideTaskbarCheckBox.Text = "Hide taskbar";
            this.hideTaskbarCheckBox.UseVisualStyleBackColor = true;
            this.hideTaskbarCheckBox.CheckedChanged += new System.EventHandler(this.hideTaskbarCheckBox_CheckedChanged);
            // 
            // areaComboBox
            // 
            this.areaComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.areaComboBox.DropDownHeight = 250;
            this.areaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.areaComboBox.IntegralHeight = false;
            this.areaComboBox.Location = new System.Drawing.Point(55, 19);
            this.areaComboBox.Name = "areaComboBox";
            this.areaComboBox.Size = new System.Drawing.Size(279, 24);
            this.areaComboBox.TabIndex = 3;
            this.areaComboBox.DropDown += new System.EventHandler(this.areaComboBox_DropDown);
            this.areaComboBox.SelectedIndexChanged += new System.EventHandler(this.areaComboBox_SelectedIndexChanged);
            // 
            // captureCursorCheckBox
            // 
            this.captureCursorCheckBox.Location = new System.Drawing.Point(230, 79);
            this.captureCursorCheckBox.Name = "captureCursorCheckBox";
            this.captureCursorCheckBox.Size = new System.Drawing.Size(104, 19);
            this.captureCursorCheckBox.TabIndex = 6;
            this.captureCursorCheckBox.Text = "Capture cursor";
            this.captureCursorCheckBox.UseVisualStyleBackColor = true;
            this.captureCursorCheckBox.CheckedChanged += new System.EventHandler(this.captureCursorCheckBox_CheckedChanged);
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Location = new System.Drawing.Point(15, 23);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(34, 15);
            this.areaLabel.TabIndex = 18;
            this.areaLabel.Text = "Area:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(767, 243);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.audioGroup);
            this.Controls.Add(this.videoGroup);
            this.DataBindings.Add(new System.Windows.Forms.Binding("TopMost", global::DiscordAudioStream.Properties.Settings.Default, "AlwaysOnTop", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Discord Audio Stream";
            this.TopMost = global::DiscordAudioStream.Properties.Settings.Default.AlwaysOnTop;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.streamContextMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.audioGroup.ResumeLayout(false);
            this.audioGroup.PerformLayout();
            this.videoGroup.ResumeLayout(false);
            this.videoGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label areaLabel;
        private CustomComponents.DarkThemeGroupBox videoGroup;
        private CustomComponents.DarkThemeCheckBox captureCursorCheckBox;
        private System.Windows.Forms.Label inputDeviceLabel;
        private CustomComponents.DarkThemeGroupBox audioGroup;
        private System.Windows.Forms.Timer updateTimer;
        private CustomComponents.DarkThemeComboBox areaComboBox;
        private CustomComponents.DarkThemeComboBox inputDeviceComboBox;
        private CustomComponents.DarkThemeToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton onTopBtn;
        private System.Windows.Forms.ToolStripButton aboutBtn;
        private System.Windows.Forms.ToolStripButton settingsBtn;
        private CustomComponents.DarkThemeCheckBox hideTaskbarCheckBox;
        private CustomComponents.PictureBoxWithoutAntiAliasing previewBox;
        private System.Windows.Forms.ToolStripButton previewBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label scaleLabel;
        private CustomComponents.DarkThemeComboBox scaleComboBox;
        private System.Windows.Forms.ToolStripButton volumeMixerButton;
        private System.Windows.Forms.ToolStripButton soundDevicesButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip streamContextMenu;
        private System.Windows.Forms.ToolStripMenuItem showAudioMeterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripStore;
        private System.Windows.Forms.ToolStripMenuItem toolStripStoreSlot1;
        private System.Windows.Forms.ToolStripMenuItem toolStripStoreSlot2;
        private System.Windows.Forms.ToolStripMenuItem toolStripStoreSlot3;
        private System.Windows.Forms.ToolStripMenuItem toolStripStoreSlot4;
        private System.Windows.Forms.ToolStripMenuItem toolStripStoreSlot5;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoad;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoadSlot1;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoadSlot2;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoadSlot3;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoadSlot4;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoadSlot5;
        private System.Windows.Forms.ToolStripButton managePresetsButton;
        private System.Windows.Forms.ToolStripSeparator stopStreamToolStripSeparator;
    }
}

