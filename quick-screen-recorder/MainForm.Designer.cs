﻿namespace quick_screen_recorder
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
            this.folderTextBox = new System.Windows.Forms.TextBox();
            this.browseFolderBtn = new QuickLibrary.QlibTextButton();
            this.folderLabel = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.fileLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.qualityLabel = new System.Windows.Forms.Label();
            this.areaLabel = new System.Windows.Forms.Label();
            this.videoGroup = new QuickLibrary.QlibGroupBox();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.scaleComboBox = new QuickLibrary.QlibComboBox();
            this.refreshScreensBtn = new System.Windows.Forms.Button();
            this.hideTaskbarCheckBox = new QuickLibrary.QlibCheckBox();
            this.sizelabel = new System.Windows.Forms.Label();
            this.yNumeric = new quick_screen_recorder.BoundedNumericBox();
            this.xNumeric = new quick_screen_recorder.BoundedNumericBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.widthNumeric = new quick_screen_recorder.BoundedNumericBox();
            this.heightNumeric = new quick_screen_recorder.BoundedNumericBox();
            this.areaComboBox = new QuickLibrary.QlibComboBox();
            this.captureCursorCheckBox = new QuickLibrary.QlibCheckBox();
            this.qualityComboBox = new QuickLibrary.QlibComboBox();
            this.inputDeviceLabel = new System.Windows.Forms.Label();
            this.audioGroup = new QuickLibrary.QlibGroupBox();
            this.refreshAudioBtn = new System.Windows.Forms.Button();
            this.inputDeviceComboBox = new QuickLibrary.QlibComboBox();
            this.separateAudioCheckBox = new QuickLibrary.QlibCheckBox();
            this.generalGroup = new QuickLibrary.QlibGroupBox();
            this.aviLabel = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.recButton = new System.Windows.Forms.Button();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.toolStrip = new QuickLibrary.QlibToolStrip();
            this.previewBtn = new System.Windows.Forms.ToolStripButton();
            this.onTopBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.volumeMixerButton = new System.Windows.Forms.ToolStripButton();
            this.soundDevicesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsBtn = new System.Windows.Forms.ToolStripButton();
            this.aboutBtn = new System.Windows.Forms.ToolStripButton();
            this.startButton = new System.Windows.Forms.Button();
            this.videoGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumeric)).BeginInit();
            this.audioGroup.SuspendLayout();
            this.generalGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderTextBox
            // 
            this.folderTextBox.AllowDrop = true;
            this.folderTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.folderTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.folderTextBox.Location = new System.Drawing.Point(73, 22);
            this.folderTextBox.Name = "folderTextBox";
            this.folderTextBox.Size = new System.Drawing.Size(185, 23);
            this.folderTextBox.TabIndex = 3;
            this.folderTextBox.TextChanged += new System.EventHandler(this.folderTextBox_TextChanged);
            this.folderTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.folderTextBox_DragDrop);
            this.folderTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.folderTextBox_DragEnter);
            // 
            // browseFolderBtn
            // 
            this.browseFolderBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.browseFolderBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.browseFolderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseFolderBtn.Location = new System.Drawing.Point(265, 22);
            this.browseFolderBtn.Name = "browseFolderBtn";
            this.browseFolderBtn.Size = new System.Drawing.Size(69, 23);
            this.browseFolderBtn.TabIndex = 4;
            this.browseFolderBtn.Text = "Browse";
            this.browseFolderBtn.UseVisualStyleBackColor = false;
            this.browseFolderBtn.Click += new System.EventHandler(this.browseFolderBtn_Click);
            // 
            // folderLabel
            // 
            this.folderLabel.AutoSize = true;
            this.folderLabel.Location = new System.Drawing.Point(24, 26);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new System.Drawing.Size(43, 15);
            this.folderLabel.TabIndex = 3;
            this.folderLabel.Text = "Folder:";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.fileNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileNameTextBox.Location = new System.Drawing.Point(73, 52);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(230, 23);
            this.fileNameTextBox.TabIndex = 5;
            this.fileNameTextBox.Text = "NewVideo1";
            // 
            // fileLabel
            // 
            this.fileLabel.AutoSize = true;
            this.fileLabel.Location = new System.Drawing.Point(6, 55);
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size(61, 15);
            this.fileLabel.TabIndex = 5;
            this.fileLabel.Text = "File name:";
            // 
            // qualityLabel
            // 
            this.qualityLabel.AutoSize = true;
            this.qualityLabel.Location = new System.Drawing.Point(19, 84);
            this.qualityLabel.Name = "qualityLabel";
            this.qualityLabel.Size = new System.Drawing.Size(48, 15);
            this.qualityLabel.TabIndex = 10;
            this.qualityLabel.Text = "Quality:";
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Location = new System.Drawing.Point(86, 22);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(34, 15);
            this.areaLabel.TabIndex = 18;
            this.areaLabel.Text = "Area:";
            // 
            // videoGroup
            // 
            this.videoGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.videoGroup.Controls.Add(this.scaleLabel);
            this.videoGroup.Controls.Add(this.scaleComboBox);
            this.videoGroup.Controls.Add(this.refreshScreensBtn);
            this.videoGroup.Controls.Add(this.hideTaskbarCheckBox);
            this.videoGroup.Controls.Add(this.sizelabel);
            this.videoGroup.Controls.Add(this.yNumeric);
            this.videoGroup.Controls.Add(this.xNumeric);
            this.videoGroup.Controls.Add(this.locationLabel);
            this.videoGroup.Controls.Add(this.widthNumeric);
            this.videoGroup.Controls.Add(this.heightNumeric);
            this.videoGroup.Controls.Add(this.areaComboBox);
            this.videoGroup.Controls.Add(this.captureCursorCheckBox);
            this.videoGroup.Controls.Add(this.areaLabel);
            this.videoGroup.Location = new System.Drawing.Point(9, 55);
            this.videoGroup.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.videoGroup.Name = "videoGroup";
            this.videoGroup.Size = new System.Drawing.Size(340, 138);
            this.videoGroup.TabIndex = 6;
            this.videoGroup.TabStop = false;
            this.videoGroup.Text = "Video capture";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(38, 52);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(82, 15);
            this.scaleLabel.TabIndex = 24;
            this.scaleLabel.Text = "Scale window:";
            // 
            // scaleComboBox
            // 
            this.scaleComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.scaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scaleComboBox.FormattingEnabled = true;
            this.scaleComboBox.IntegralHeight = false;
            this.scaleComboBox.Items.AddRange(new object[] {
            "Full resolution",
            "50% resolution",
            "25% Resolution"});
            this.scaleComboBox.Location = new System.Drawing.Point(127, 49);
            this.scaleComboBox.Name = "scaleComboBox";
            this.scaleComboBox.Size = new System.Drawing.Size(177, 24);
            this.scaleComboBox.TabIndex = 23;
            this.scaleComboBox.SelectedIndexChanged += new System.EventHandler(this.scaleComboBox_SelectedIndexChanged);
            // 
            // refreshScreensBtn
            // 
            this.refreshScreensBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.refreshScreensBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.refreshScreensBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshScreensBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshScreensBtn.Image")));
            this.refreshScreensBtn.Location = new System.Drawing.Point(311, 19);
            this.refreshScreensBtn.Margin = new System.Windows.Forms.Padding(0);
            this.refreshScreensBtn.Name = "refreshScreensBtn";
            this.refreshScreensBtn.Size = new System.Drawing.Size(23, 23);
            this.refreshScreensBtn.TabIndex = 9;
            this.refreshScreensBtn.UseVisualStyleBackColor = false;
            this.refreshScreensBtn.Click += new System.EventHandler(this.refreshScreensBtn_Click);
            // 
            // hideTaskbarCheckBox
            // 
            this.hideTaskbarCheckBox.Location = new System.Drawing.Point(132, 114);
            this.hideTaskbarCheckBox.Name = "hideTaskbarCheckBox";
            this.hideTaskbarCheckBox.Size = new System.Drawing.Size(92, 19);
            this.hideTaskbarCheckBox.TabIndex = 14;
            this.hideTaskbarCheckBox.Text = "Hide taskbar";
            this.hideTaskbarCheckBox.UseVisualStyleBackColor = true;
            this.hideTaskbarCheckBox.CheckedChanged += new System.EventHandler(this.hideTaskbarCheckBox_CheckedChanged);
            // 
            // sizelabel
            // 
            this.sizelabel.AutoSize = true;
            this.sizelabel.Location = new System.Drawing.Point(197, 85);
            this.sizelabel.Name = "sizelabel";
            this.sizelabel.Size = new System.Drawing.Size(30, 15);
            this.sizelabel.TabIndex = 22;
            this.sizelabel.Text = "Size:";
            // 
            // yNumeric
            // 
            this.yNumeric.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yNumeric.Location = new System.Drawing.Point(127, 83);
            this.yNumeric.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.yNumeric.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.yNumeric.Name = "yNumeric";
            this.yNumeric.Size = new System.Drawing.Size(47, 23);
            this.yNumeric.TabIndex = 11;
            this.yNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.yNumeric.ValueChanged += new System.EventHandler(this.yNumeric_ValueChanged);
            // 
            // xNumeric
            // 
            this.xNumeric.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xNumeric.Location = new System.Drawing.Point(73, 83);
            this.xNumeric.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.xNumeric.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.xNumeric.Name = "xNumeric";
            this.xNumeric.Size = new System.Drawing.Size(47, 23);
            this.xNumeric.TabIndex = 10;
            this.xNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.xNumeric.ValueChanged += new System.EventHandler(this.xNumeric_ValueChanged);
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(11, 85);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(56, 15);
            this.locationLabel.TabIndex = 19;
            this.locationLabel.Text = "Location:";
            // 
            // widthNumeric
            // 
            this.widthNumeric.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.widthNumeric.Location = new System.Drawing.Point(233, 83);
            this.widthNumeric.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.widthNumeric.Minimum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.widthNumeric.Name = "widthNumeric";
            this.widthNumeric.Size = new System.Drawing.Size(47, 23);
            this.widthNumeric.TabIndex = 12;
            this.widthNumeric.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.widthNumeric.ValueChanged += new System.EventHandler(this.widthNumeric_ValueChanged);
            // 
            // heightNumeric
            // 
            this.heightNumeric.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.heightNumeric.Location = new System.Drawing.Point(287, 83);
            this.heightNumeric.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.heightNumeric.Minimum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.heightNumeric.Name = "heightNumeric";
            this.heightNumeric.Size = new System.Drawing.Size(47, 23);
            this.heightNumeric.TabIndex = 13;
            this.heightNumeric.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.heightNumeric.ValueChanged += new System.EventHandler(this.heightNumeric_ValueChanged);
            // 
            // areaComboBox
            // 
            this.areaComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.areaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.areaComboBox.FormattingEnabled = true;
            this.areaComboBox.IntegralHeight = false;
            this.areaComboBox.Location = new System.Drawing.Point(127, 19);
            this.areaComboBox.Name = "areaComboBox";
            this.areaComboBox.Size = new System.Drawing.Size(177, 24);
            this.areaComboBox.TabIndex = 8;
            this.areaComboBox.SelectedIndexChanged += new System.EventHandler(this.areaComboBox_SelectedIndexChanged);
            // 
            // captureCursorCheckBox
            // 
            this.captureCursorCheckBox.Checked = true;
            this.captureCursorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.captureCursorCheckBox.Location = new System.Drawing.Point(230, 114);
            this.captureCursorCheckBox.Name = "captureCursorCheckBox";
            this.captureCursorCheckBox.Size = new System.Drawing.Size(104, 19);
            this.captureCursorCheckBox.TabIndex = 15;
            this.captureCursorCheckBox.Text = "Capture cursor";
            this.captureCursorCheckBox.UseVisualStyleBackColor = true;
            this.captureCursorCheckBox.CheckedChanged += new System.EventHandler(this.captureCursorCheckBox_CheckedChanged);
            // 
            // qualityComboBox
            // 
            this.qualityComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.qualityComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.qualityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.qualityComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.qualityComboBox.FormattingEnabled = true;
            this.qualityComboBox.IntegralHeight = false;
            this.qualityComboBox.Items.AddRange(new object[] {
            "25% - Low (Motion JPEG)",
            "50% - Medium (Motion JPEG)",
            "75% - High (Motion JPEG)",
            "100% - Original (Motion JPEG)",
            "Uncompressed"});
            this.qualityComboBox.Location = new System.Drawing.Point(73, 81);
            this.qualityComboBox.Name = "qualityComboBox";
            this.qualityComboBox.Size = new System.Drawing.Size(261, 24);
            this.qualityComboBox.TabIndex = 7;
            this.qualityComboBox.SelectedIndexChanged += new System.EventHandler(this.qualityComboBox_SelectedIndexChanged);
            // 
            // inputDeviceLabel
            // 
            this.inputDeviceLabel.AutoSize = true;
            this.inputDeviceLabel.Location = new System.Drawing.Point(45, 26);
            this.inputDeviceLabel.Name = "inputDeviceLabel";
            this.inputDeviceLabel.Size = new System.Drawing.Size(75, 15);
            this.inputDeviceLabel.TabIndex = 26;
            this.inputDeviceLabel.Text = "Input device:";
            // 
            // audioGroup
            // 
            this.audioGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.audioGroup.Controls.Add(this.refreshAudioBtn);
            this.audioGroup.Controls.Add(this.inputDeviceComboBox);
            this.audioGroup.Controls.Add(this.separateAudioCheckBox);
            this.audioGroup.Controls.Add(this.inputDeviceLabel);
            this.audioGroup.Location = new System.Drawing.Point(9, 202);
            this.audioGroup.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.audioGroup.Name = "audioGroup";
            this.audioGroup.Size = new System.Drawing.Size(340, 56);
            this.audioGroup.TabIndex = 16;
            this.audioGroup.TabStop = false;
            this.audioGroup.Text = "Audio capture";
            // 
            // refreshAudioBtn
            // 
            this.refreshAudioBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.refreshAudioBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.refreshAudioBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshAudioBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshAudioBtn.Image")));
            this.refreshAudioBtn.Location = new System.Drawing.Point(311, 22);
            this.refreshAudioBtn.Margin = new System.Windows.Forms.Padding(0);
            this.refreshAudioBtn.Name = "refreshAudioBtn";
            this.refreshAudioBtn.Size = new System.Drawing.Size(23, 23);
            this.refreshAudioBtn.TabIndex = 18;
            this.refreshAudioBtn.UseVisualStyleBackColor = false;
            this.refreshAudioBtn.Click += new System.EventHandler(this.refreshBtn_Click);
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
            this.inputDeviceComboBox.Location = new System.Drawing.Point(127, 22);
            this.inputDeviceComboBox.Name = "inputDeviceComboBox";
            this.inputDeviceComboBox.Size = new System.Drawing.Size(177, 24);
            this.inputDeviceComboBox.TabIndex = 17;
            this.inputDeviceComboBox.SelectedIndexChanged += new System.EventHandler(this.inputDeviceComboBox_SelectedIndexChanged);
            // 
            // separateAudioCheckBox
            // 
            this.separateAudioCheckBox.Location = new System.Drawing.Point(123, 53);
            this.separateAudioCheckBox.Name = "separateAudioCheckBox";
            this.separateAudioCheckBox.Size = new System.Drawing.Size(211, 19);
            this.separateAudioCheckBox.TabIndex = 19;
            this.separateAudioCheckBox.Text = "Write audio to a separate file (.wav)";
            this.separateAudioCheckBox.UseVisualStyleBackColor = true;
            this.separateAudioCheckBox.Visible = false;
            // 
            // generalGroup
            // 
            this.generalGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.generalGroup.Controls.Add(this.aviLabel);
            this.generalGroup.Controls.Add(this.folderTextBox);
            this.generalGroup.Controls.Add(this.browseFolderBtn);
            this.generalGroup.Controls.Add(this.folderLabel);
            this.generalGroup.Controls.Add(this.fileNameTextBox);
            this.generalGroup.Controls.Add(this.fileLabel);
            this.generalGroup.Controls.Add(this.qualityComboBox);
            this.generalGroup.Controls.Add(this.qualityLabel);
            this.generalGroup.Location = new System.Drawing.Point(9, -134);
            this.generalGroup.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.generalGroup.Name = "generalGroup";
            this.generalGroup.Size = new System.Drawing.Size(340, 126);
            this.generalGroup.TabIndex = 2;
            this.generalGroup.TabStop = false;
            this.generalGroup.Text = "Recording";
            this.generalGroup.Visible = false;
            // 
            // aviLabel
            // 
            this.aviLabel.AutoSize = true;
            this.aviLabel.Location = new System.Drawing.Point(309, 55);
            this.aviLabel.Name = "aviLabel";
            this.aviLabel.Size = new System.Drawing.Size(25, 15);
            this.aviLabel.TabIndex = 26;
            this.aviLabel.Text = ".avi";
            // 
            // recButton
            // 
            this.recButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.recButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.recButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.recButton.Image = ((System.Drawing.Image)(resources.GetObject("recButton.Image")));
            this.recButton.Location = new System.Drawing.Point(223, 111);
            this.recButton.Margin = new System.Windows.Forms.Padding(0);
            this.recButton.Name = "recButton";
            this.recButton.Size = new System.Drawing.Size(120, 43);
            this.recButton.TabIndex = 0;
            this.recButton.Text = " REC (Alt+R)";
            this.recButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.recButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.recButton.UseVisualStyleBackColor = false;
            this.recButton.Visible = false;
            this.recButton.Click += new System.EventHandler(this.recButton_Click);
            // 
            // previewBox
            // 
            this.previewBox.BackColor = System.Drawing.Color.Black;
            this.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewBox.InitialImage = null;
            this.previewBox.Location = new System.Drawing.Point(358, 9);
            this.previewBox.Margin = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(437, 248);
            this.previewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.previewBox.TabIndex = 26;
            this.previewBox.TabStop = false;
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
            this.toolStripSeparator1,
            this.volumeMixerButton,
            this.soundDevicesButton,
            this.toolStripSeparator2,
            this.settingsBtn,
            this.aboutBtn});
            this.toolStrip.Location = new System.Drawing.Point(9, 9);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(9);
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(186, 43);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.TabStop = true;
            this.toolStrip.Text = "toolStrip1";
            // 
            // previewBtn
            // 
            this.previewBtn.AutoSize = false;
            this.previewBtn.CheckOnClick = true;
            this.previewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.previewBtn.Image = ((System.Drawing.Image)(resources.GetObject("previewBtn.Image")));
            this.previewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewBtn.Margin = new System.Windows.Forms.Padding(0);
            this.previewBtn.Name = "previewBtn";
            this.previewBtn.Size = new System.Drawing.Size(24, 25);
            this.previewBtn.Text = "Preview | Ctrl + P";
            this.previewBtn.ToolTipText = "Preview | Ctrl + P";
            this.previewBtn.CheckedChanged += new System.EventHandler(this.previewBtn_CheckedChanged);
            // 
            // onTopBtn
            // 
            this.onTopBtn.AutoSize = false;
            this.onTopBtn.CheckOnClick = true;
            this.onTopBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.onTopBtn.Image = ((System.Drawing.Image)(resources.GetObject("onTopBtn.Image")));
            this.onTopBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.onTopBtn.Margin = new System.Windows.Forms.Padding(0);
            this.onTopBtn.Name = "onTopBtn";
            this.onTopBtn.Size = new System.Drawing.Size(24, 25);
            this.onTopBtn.Text = "Always on top | Ctrl + T";
            this.onTopBtn.ToolTipText = "Always on top | Ctrl+T";
            this.onTopBtn.CheckedChanged += new System.EventHandler(this.onTopCheckBox_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(2, 20);
            // 
            // volumeMixerButton
            // 
            this.volumeMixerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.volumeMixerButton.Image = ((System.Drawing.Image)(resources.GetObject("volumeMixerButton.Image")));
            this.volumeMixerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.volumeMixerButton.Name = "volumeMixerButton";
            this.volumeMixerButton.Size = new System.Drawing.Size(23, 22);
            this.volumeMixerButton.Text = "Open volume mixer";
            this.volumeMixerButton.Click += new System.EventHandler(this.volumeMixerButton_Click);
            // 
            // soundDevicesButton
            // 
            this.soundDevicesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.soundDevicesButton.Image = ((System.Drawing.Image)(resources.GetObject("soundDevicesButton.Image")));
            this.soundDevicesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.soundDevicesButton.Name = "soundDevicesButton";
            this.soundDevicesButton.Size = new System.Drawing.Size(23, 22);
            this.soundDevicesButton.Text = "Configure audio devices";
            this.soundDevicesButton.Click += new System.EventHandler(this.soundDevicesButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(2, 20);
            // 
            // settingsBtn
            // 
            this.settingsBtn.AutoSize = false;
            this.settingsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsBtn.Image = ((System.Drawing.Image)(resources.GetObject("settingsBtn.Image")));
            this.settingsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(24, 25);
            this.settingsBtn.Text = "About | F1";
            this.settingsBtn.ToolTipText = "Settings | Ctrl+Comma";
            this.settingsBtn.Click += new System.EventHandler(this.settingsBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.AutoSize = false;
            this.aboutBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboutBtn.Image = ((System.Drawing.Image)(resources.GetObject("aboutBtn.Image")));
            this.aboutBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboutBtn.Margin = new System.Windows.Forms.Padding(0);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(24, 25);
            this.aboutBtn.Text = "About | F1";
            this.aboutBtn.Click += new System.EventHandler(this.aboutBtn_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startButton.Location = new System.Drawing.Point(208, 14);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(140, 34);
            this.startButton.TabIndex = 27;
            this.startButton.Text = "START STREAM";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(804, 267);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.generalGroup);
            this.Controls.Add(this.audioGroup);
            this.Controls.Add(this.videoGroup);
            this.Controls.Add(this.recButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quick Screen Recorder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.videoGroup.ResumeLayout(false);
            this.videoGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumeric)).EndInit();
            this.audioGroup.ResumeLayout(false);
            this.audioGroup.PerformLayout();
            this.generalGroup.ResumeLayout(false);
            this.generalGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button recButton;
		private System.Windows.Forms.TextBox folderTextBox;
		private QuickLibrary.QlibTextButton browseFolderBtn;
		private System.Windows.Forms.Label folderLabel;
		private System.Windows.Forms.TextBox fileNameTextBox;
		private System.Windows.Forms.Label fileLabel;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label qualityLabel;
		private System.Windows.Forms.Label areaLabel;
		private QuickLibrary.QlibGroupBox videoGroup;
		private QuickLibrary.QlibCheckBox captureCursorCheckBox;
		private System.Windows.Forms.Label inputDeviceLabel;
		private QuickLibrary.QlibGroupBox audioGroup;
		private QuickLibrary.QlibGroupBox generalGroup;
		private System.Windows.Forms.Label aviLabel;
		private System.Windows.Forms.Timer updateTimer;
		private QuickLibrary.QlibComboBox areaComboBox;
		private QuickLibrary.QlibComboBox qualityComboBox;
		private QuickLibrary.QlibComboBox inputDeviceComboBox;
		private BoundedNumericBox heightNumeric;
		private BoundedNumericBox widthNumeric;
		private System.Windows.Forms.Button refreshAudioBtn;
		private QuickLibrary.QlibToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton onTopBtn;
		private System.Windows.Forms.ToolStripButton aboutBtn;
		private System.Windows.Forms.ToolStripButton settingsBtn;
		private QuickLibrary.QlibCheckBox separateAudioCheckBox;
		private System.Windows.Forms.Label locationLabel;
		private BoundedNumericBox xNumeric;
		private BoundedNumericBox yNumeric;
		private System.Windows.Forms.Label sizelabel;
		private QuickLibrary.QlibCheckBox hideTaskbarCheckBox;
		private System.Windows.Forms.Button refreshScreensBtn;
		private System.Windows.Forms.PictureBox previewBox;
		private System.Windows.Forms.ToolStripButton previewBtn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label scaleLabel;
        private QuickLibrary.QlibComboBox scaleComboBox;
        private System.Windows.Forms.ToolStripButton volumeMixerButton;
        private System.Windows.Forms.ToolStripButton soundDevicesButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

