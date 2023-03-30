namespace DiscordAudioStream
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
			this.capturePage = new System.Windows.Forms.TabPage();
			this.captureFramerateComboBox = new CustomComponents.DarkThemeComboBox();
			this.captureMethodGroup = new CustomComponents.DarkThemeGroupBox();
			this.windowMethodLabel = new System.Windows.Forms.Label();
			this.fullscreenMethodComboBox = new CustomComponents.DarkThemeComboBox();
			this.fullscreenMethodLabel = new System.Windows.Forms.Label();
			this.windowMethodComboBox = new CustomComponents.DarkThemeComboBox();
			this.captureFramerateLabel = new System.Windows.Forms.Label();
			this.autoExitCheckbox = new CustomComponents.DarkThemeCheckBox();
			this.themePage = new System.Windows.Forms.TabPage();
			this.restartLabel = new System.Windows.Forms.Label();
			this.systemThemeRadio = new CustomComponents.DarkThemeRadioButton();
			this.darkThemeRadio = new CustomComponents.DarkThemeRadioButton();
			this.lightThemeRadio = new CustomComponents.DarkThemeRadioButton();
			this.debugPage = new System.Windows.Forms.TabPage();
			this.showAudioInputsCheckbox = new CustomComponents.DarkThemeCheckBox();
			this.offscreenDrawCheckbox = new CustomComponents.DarkThemeCheckBox();
			this.outputLogCheckbox = new CustomComponents.DarkThemeCheckBox();
			this.audioDevicesLink = new System.Windows.Forms.LinkLabel();
			this.classicVolumeMixerLink = new System.Windows.Forms.LinkLabel();
			this.settingsTabs.SuspendLayout();
			this.capturePage.SuspendLayout();
			this.captureMethodGroup.SuspendLayout();
			this.themePage.SuspendLayout();
			this.debugPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// settingsTabs
			// 
			this.settingsTabs.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
			this.settingsTabs.AllowDrop = true;
			this.settingsTabs.BackTabColor = System.Drawing.Color.White;
			this.settingsTabs.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.settingsTabs.ClosingButtonColor = System.Drawing.Color.WhiteSmoke;
			this.settingsTabs.ClosingMessage = null;
			this.settingsTabs.Controls.Add(this.capturePage);
			this.settingsTabs.Controls.Add(this.themePage);
			this.settingsTabs.Controls.Add(this.debugPage);
			this.settingsTabs.DraggableTabs = false;
			this.settingsTabs.HeaderColor = System.Drawing.SystemColors.ControlLight;
			this.settingsTabs.HorizontalLineColor = System.Drawing.Color.Transparent;
			this.settingsTabs.ItemSize = new System.Drawing.Size(240, 16);
			this.settingsTabs.Location = new System.Drawing.Point(9, 9);
			this.settingsTabs.Margin = new System.Windows.Forms.Padding(0, 0, 0, 9);
			this.settingsTabs.Name = "settingsTabs";
			this.settingsTabs.SelectedIndex = 0;
			this.settingsTabs.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.settingsTabs.ShowClosingButton = false;
			this.settingsTabs.ShowClosingMessage = false;
			this.settingsTabs.Size = new System.Drawing.Size(256, 187);
			this.settingsTabs.TabIndex = 13;
			this.settingsTabs.TextColor = System.Drawing.Color.Black;
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
			this.captureFramerateComboBox.TabIndex = 7;
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
			this.fullscreenMethodComboBox.TabIndex = 5;
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
			this.windowMethodComboBox.TabIndex = 3;
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
			this.autoExitCheckbox.AutoSize = true;
			this.autoExitCheckbox.Location = new System.Drawing.Point(9, 9);
			this.autoExitCheckbox.Name = "autoExitCheckbox";
			this.autoExitCheckbox.Size = new System.Drawing.Size(207, 19);
			this.autoExitCheckbox.TabIndex = 0;
			this.autoExitCheckbox.Text = "Exit when captured window closes";
			this.autoExitCheckbox.UseVisualStyleBackColor = true;
			this.autoExitCheckbox.CheckedChanged += new System.EventHandler(this.autoExitCheckbox_CheckedChanged);
			// 
			// themePage
			// 
			this.themePage.BackColor = System.Drawing.Color.White;
			this.themePage.Controls.Add(this.restartLabel);
			this.themePage.Controls.Add(this.systemThemeRadio);
			this.themePage.Controls.Add(this.darkThemeRadio);
			this.themePage.Controls.Add(this.lightThemeRadio);
			this.themePage.Location = new System.Drawing.Point(4, 20);
			this.themePage.Margin = new System.Windows.Forms.Padding(0);
			this.themePage.Name = "themePage";
			this.themePage.Padding = new System.Windows.Forms.Padding(3);
			this.themePage.Size = new System.Drawing.Size(248, 163);
			this.themePage.TabIndex = 0;
			this.themePage.Text = "Theme";
			// 
			// restartLabel
			// 
			this.restartLabel.AutoSize = true;
			this.restartLabel.Location = new System.Drawing.Point(6, 96);
			this.restartLabel.Margin = new System.Windows.Forms.Padding(3);
			this.restartLabel.Name = "restartLabel";
			this.restartLabel.Size = new System.Drawing.Size(120, 15);
			this.restartLabel.TabIndex = 38;
			this.restartLabel.Text = "* App restart required";
			// 
			// systemThemeRadio
			// 
			this.systemThemeRadio.Checked = true;
			this.systemThemeRadio.Location = new System.Drawing.Point(9, 9);
			this.systemThemeRadio.Margin = new System.Windows.Forms.Padding(9);
			this.systemThemeRadio.Name = "systemThemeRadio";
			this.systemThemeRadio.Size = new System.Drawing.Size(123, 19);
			this.systemThemeRadio.TabIndex = 0;
			this.systemThemeRadio.TabStop = true;
			this.systemThemeRadio.Text = "Use system setting";
			this.systemThemeRadio.UseVisualStyleBackColor = true;
			this.systemThemeRadio.CheckedChanged += new System.EventHandler(this.systemThemeRadio_CheckedChanged);
			// 
			// darkThemeRadio
			// 
			this.darkThemeRadio.Location = new System.Drawing.Point(9, 65);
			this.darkThemeRadio.Margin = new System.Windows.Forms.Padding(9, 0, 9, 9);
			this.darkThemeRadio.Name = "darkThemeRadio";
			this.darkThemeRadio.Size = new System.Drawing.Size(49, 19);
			this.darkThemeRadio.TabIndex = 2;
			this.darkThemeRadio.Text = "Dark";
			this.darkThemeRadio.UseVisualStyleBackColor = true;
			this.darkThemeRadio.CheckedChanged += new System.EventHandler(this.darkThemeRadio_CheckedChanged);
			// 
			// lightThemeRadio
			// 
			this.lightThemeRadio.Location = new System.Drawing.Point(9, 37);
			this.lightThemeRadio.Margin = new System.Windows.Forms.Padding(9, 0, 9, 9);
			this.lightThemeRadio.Name = "lightThemeRadio";
			this.lightThemeRadio.Size = new System.Drawing.Size(52, 19);
			this.lightThemeRadio.TabIndex = 1;
			this.lightThemeRadio.Text = "Light";
			this.lightThemeRadio.UseVisualStyleBackColor = true;
			this.lightThemeRadio.CheckedChanged += new System.EventHandler(this.lightThemeRadio_CheckedChanged);
			// 
			// debugPage
			// 
			this.debugPage.BackColor = System.Drawing.Color.White;
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
			// showAudioInputsCheckbox
			// 
			this.showAudioInputsCheckbox.AutoSize = true;
			this.showAudioInputsCheckbox.Location = new System.Drawing.Point(9, 59);
			this.showAudioInputsCheckbox.Name = "showAudioInputsCheckbox";
			this.showAudioInputsCheckbox.Size = new System.Drawing.Size(161, 19);
			this.showAudioInputsCheckbox.TabIndex = 41;
			this.showAudioInputsCheckbox.Text = "Show audio input devices";
			this.showAudioInputsCheckbox.UseVisualStyleBackColor = true;
			this.showAudioInputsCheckbox.CheckedChanged += new System.EventHandler(this.showAudioInputsCheckbox_CheckedChanged);
			// 
			// offscreenDrawCheckbox
			// 
			this.offscreenDrawCheckbox.AutoSize = true;
			this.offscreenDrawCheckbox.Location = new System.Drawing.Point(9, 34);
			this.offscreenDrawCheckbox.Name = "offscreenDrawCheckbox";
			this.offscreenDrawCheckbox.Size = new System.Drawing.Size(131, 19);
			this.offscreenDrawCheckbox.TabIndex = 40;
			this.offscreenDrawCheckbox.Text = "Force screen redraw";
			this.offscreenDrawCheckbox.UseVisualStyleBackColor = true;
			this.offscreenDrawCheckbox.CheckedChanged += new System.EventHandler(this.offscreenDrawCheckbox_CheckedChanged);
			// 
			// outputLogCheckbox
			// 
			this.outputLogCheckbox.AutoSize = true;
			this.outputLogCheckbox.Location = new System.Drawing.Point(9, 9);
			this.outputLogCheckbox.Name = "outputLogCheckbox";
			this.outputLogCheckbox.Size = new System.Drawing.Size(103, 19);
			this.outputLogCheckbox.TabIndex = 6;
			this.outputLogCheckbox.Text = "Output log file";
			this.outputLogCheckbox.UseVisualStyleBackColor = true;
			this.outputLogCheckbox.CheckedChanged += new System.EventHandler(this.outputLogCheckbox_CheckedChanged);
			// 
			// audioDevicesLink
			// 
			this.audioDevicesLink.AutoSize = true;
			this.audioDevicesLink.Location = new System.Drawing.Point(6, 117);
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
			this.capturePage.ResumeLayout(false);
			this.capturePage.PerformLayout();
			this.captureMethodGroup.ResumeLayout(false);
			this.captureMethodGroup.PerformLayout();
			this.themePage.ResumeLayout(false);
			this.themePage.PerformLayout();
			this.debugPage.ResumeLayout(false);
			this.debugPage.PerformLayout();
			this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.TabPage debugPage;
        private System.Windows.Forms.TabPage themePage;
        private System.Windows.Forms.Label restartLabel;
        private CustomComponents.DarkThemeRadioButton systemThemeRadio;
        private CustomComponents.DarkThemeRadioButton darkThemeRadio;
        private CustomComponents.DarkThemeRadioButton lightThemeRadio;
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
	}
}
