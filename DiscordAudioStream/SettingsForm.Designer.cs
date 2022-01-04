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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.audioPage = new System.Windows.Forms.TabPage();
            this.winSoundBtn = new System.Windows.Forms.Button();
            this.mixerBtn = new System.Windows.Forms.Button();
            this.themePage = new System.Windows.Forms.TabPage();
            this.restartLabel = new System.Windows.Forms.Label();
            this.systemThemeRadio = new CustomComponents.DarkThemeRadioButton();
            this.darkThemeRadio = new CustomComponents.DarkThemeRadioButton();
            this.lightThemeRadio = new CustomComponents.DarkThemeRadioButton();
            this.settingsTabs = new CustomComponents.DarkThemeTabControl();
            this.capturePage = new System.Windows.Forms.TabPage();
            this.experimentalCaptureLabel = new System.Windows.Forms.Label();
            this.experimentalCaptureCheckBox = new CustomComponents.DarkThemeCheckBox();
            this.autoExitCheckbox = new CustomComponents.DarkThemeCheckBox();
            this.audioPage.SuspendLayout();
            this.themePage.SuspendLayout();
            this.settingsTabs.SuspendLayout();
            this.capturePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // audioPage
            // 
            this.audioPage.BackColor = System.Drawing.Color.White;
            this.audioPage.Controls.Add(this.winSoundBtn);
            this.audioPage.Controls.Add(this.mixerBtn);
            this.audioPage.Location = new System.Drawing.Point(4, 20);
            this.audioPage.Margin = new System.Windows.Forms.Padding(0);
            this.audioPage.Name = "audioPage";
            this.audioPage.Size = new System.Drawing.Size(248, 121);
            this.audioPage.TabIndex = 2;
            this.audioPage.Text = "Audio";
            // 
            // winSoundBtn
            // 
            this.winSoundBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.winSoundBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.winSoundBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.winSoundBtn.Image = ((System.Drawing.Image)(resources.GetObject("winSoundBtn.Image")));
            this.winSoundBtn.Location = new System.Drawing.Point(9, 58);
            this.winSoundBtn.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.winSoundBtn.Name = "winSoundBtn";
            this.winSoundBtn.Size = new System.Drawing.Size(230, 40);
            this.winSoundBtn.TabIndex = 3;
            this.winSoundBtn.Text = " Manage audio devices";
            this.winSoundBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.winSoundBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.winSoundBtn.UseVisualStyleBackColor = false;
            this.winSoundBtn.Click += new System.EventHandler(this.winSoundBtn_Click);
            // 
            // mixerBtn
            // 
            this.mixerBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.mixerBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.mixerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mixerBtn.Image = ((System.Drawing.Image)(resources.GetObject("mixerBtn.Image")));
            this.mixerBtn.Location = new System.Drawing.Point(9, 9);
            this.mixerBtn.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.mixerBtn.Name = "mixerBtn";
            this.mixerBtn.Size = new System.Drawing.Size(230, 40);
            this.mixerBtn.TabIndex = 2;
            this.mixerBtn.Text = " Open classic volume mixer";
            this.mixerBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mixerBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.mixerBtn.UseVisualStyleBackColor = false;
            this.mixerBtn.Click += new System.EventHandler(this.mixerBtn_Click);
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
            this.themePage.Size = new System.Drawing.Size(248, 121);
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
            // settingsTabs
            // 
            this.settingsTabs.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.settingsTabs.AllowDrop = true;
            this.settingsTabs.BackTabColor = System.Drawing.Color.White;
            this.settingsTabs.BorderColor = System.Drawing.SystemColors.ControlLight;
            this.settingsTabs.ClosingButtonColor = System.Drawing.Color.WhiteSmoke;
            this.settingsTabs.ClosingMessage = null;
            this.settingsTabs.Controls.Add(this.themePage);
            this.settingsTabs.Controls.Add(this.audioPage);
            this.settingsTabs.Controls.Add(this.capturePage);
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
            this.settingsTabs.Size = new System.Drawing.Size(256, 145);
            this.settingsTabs.TabIndex = 13;
            this.settingsTabs.TextColor = System.Drawing.Color.Black;
            // 
            // capturePage
            // 
            this.capturePage.BackColor = System.Drawing.Color.White;
            this.capturePage.Controls.Add(this.experimentalCaptureLabel);
            this.capturePage.Controls.Add(this.experimentalCaptureCheckBox);
            this.capturePage.Controls.Add(this.autoExitCheckbox);
            this.capturePage.Location = new System.Drawing.Point(4, 20);
            this.capturePage.Margin = new System.Windows.Forms.Padding(0);
            this.capturePage.Name = "capturePage";
            this.capturePage.Padding = new System.Windows.Forms.Padding(3);
            this.capturePage.Size = new System.Drawing.Size(248, 121);
            this.capturePage.TabIndex = 3;
            this.capturePage.Text = "Capture";
            // 
            // experimentalCaptureLabel
            // 
            this.experimentalCaptureLabel.AutoSize = true;
            this.experimentalCaptureLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.experimentalCaptureLabel.Location = new System.Drawing.Point(6, 63);
            this.experimentalCaptureLabel.Margin = new System.Windows.Forms.Padding(3);
            this.experimentalCaptureLabel.MaximumSize = new System.Drawing.Size(240, 0);
            this.experimentalCaptureLabel.Name = "experimentalCaptureLabel";
            this.experimentalCaptureLabel.Size = new System.Drawing.Size(234, 52);
            this.experimentalCaptureLabel.TabIndex = 39;
            this.experimentalCaptureLabel.Text = "* Use an alternative API for recording single windows without bringing them to fr" +
    "ont. Turn it off if you experience flickering. Requires Windows 8 or higher.";
            // 
            // experimentalCaptureCheckBox
            // 
            this.experimentalCaptureCheckBox.AutoSize = true;
            this.experimentalCaptureCheckBox.Location = new System.Drawing.Point(9, 34);
            this.experimentalCaptureCheckBox.Name = "experimentalCaptureCheckBox";
            this.experimentalCaptureCheckBox.Size = new System.Drawing.Size(213, 19);
            this.experimentalCaptureCheckBox.TabIndex = 1;
            this.experimentalCaptureCheckBox.Text = "Use experimental capture method *";
            this.experimentalCaptureCheckBox.UseVisualStyleBackColor = true;
            this.experimentalCaptureCheckBox.CheckedChanged += new System.EventHandler(this.experimentalCaptureCheckBox_CheckedChanged);
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
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(274, 163);
            this.Controls.Add(this.settingsTabs);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingsForm_KeyDown);
            this.audioPage.ResumeLayout(false);
            this.themePage.ResumeLayout(false);
            this.themePage.PerformLayout();
            this.settingsTabs.ResumeLayout(false);
            this.capturePage.ResumeLayout(false);
            this.capturePage.PerformLayout();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.TabPage audioPage;
        private System.Windows.Forms.Button winSoundBtn;
        private System.Windows.Forms.Button mixerBtn;
        private System.Windows.Forms.TabPage themePage;
        private System.Windows.Forms.Label restartLabel;
        private CustomComponents.DarkThemeRadioButton systemThemeRadio;
        private CustomComponents.DarkThemeRadioButton darkThemeRadio;
        private CustomComponents.DarkThemeRadioButton lightThemeRadio;
        private CustomComponents.DarkThemeTabControl settingsTabs;
        private System.Windows.Forms.TabPage capturePage;
        private CustomComponents.DarkThemeCheckBox autoExitCheckbox;
        private CustomComponents.DarkThemeCheckBox experimentalCaptureCheckBox;
        private System.Windows.Forms.Label experimentalCaptureLabel;
    }
}
