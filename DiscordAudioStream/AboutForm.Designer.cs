namespace DiscordAudioStream
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.productLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.basedOnLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.projectLink = new System.Windows.Forms.LinkLabel();
            this.infoGroup = new CustomComponents.DarkThemeGroupBox();
            this.moduleArtLabel = new System.Windows.Forms.LinkLabel();
            this.byLabel = new System.Windows.Forms.Label();
            this.screenRecorderLink = new System.Windows.Forms.LinkLabel();
            this.licenseLabel = new System.Windows.Forms.Label();
            this.updatesLink = new System.Windows.Forms.LinkLabel();
            this.issuesLink = new System.Windows.Forms.LinkLabel();
            this.pagesGroup = new CustomComponents.DarkThemeGroupBox();
            this.authorLink = new System.Windows.Forms.LinkLabel();
            this.licenseLink = new System.Windows.Forms.LinkLabel();
            this.aboutTooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.infoGroup.SuspendLayout();
            this.pagesGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(9, 14);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(256, 80);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 12;
            this.logoPictureBox.TabStop = false;
            // 
            // productLabel
            // 
            this.productLabel.AutoSize = true;
            this.productLabel.Location = new System.Drawing.Point(6, 19);
            this.productLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.productLabel.Name = "productLabel";
            this.productLabel.Size = new System.Drawing.Size(122, 15);
            this.productLabel.TabIndex = 27;
            this.productLabel.Text = "Discord Audio Stream";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(6, 40);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(3);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(45, 15);
            this.versionLabel.TabIndex = 28;
            this.versionLabel.Text = "Version";
            // 
            // basedOnLabel
            // 
            this.basedOnLabel.AutoSize = true;
            this.basedOnLabel.Location = new System.Drawing.Point(6, 155);
            this.basedOnLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.basedOnLabel.MaximumSize = new System.Drawing.Size(190, 0);
            this.basedOnLabel.Name = "basedOnLabel";
            this.basedOnLabel.Size = new System.Drawing.Size(58, 15);
            this.basedOnLabel.TabIndex = 29;
            this.basedOnLabel.Text = "Based on ";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(6, 64);
            this.descriptionLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.descriptionLabel.MaximumSize = new System.Drawing.Size(244, 0);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(244, 30);
            this.descriptionLabel.TabIndex = 31;
            this.descriptionLabel.Text = "Utility for streaming the entire screen (or use OBS) with Discord... with audio!";
            // 
            // projectLink
            // 
            this.projectLink.AutoSize = true;
            this.projectLink.Location = new System.Drawing.Point(62, 28);
            this.projectLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.projectLink.Name = "projectLink";
            this.projectLink.Size = new System.Drawing.Size(73, 15);
            this.projectLink.TabIndex = 4;
            this.projectLink.TabStop = true;
            this.projectLink.Text = "Project page";
            this.aboutTooltip.SetToolTip(this.projectLink, "Open project page in browser");
            this.projectLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.projectLink_LinkClicked);
            // 
            // infoGroup
            // 
            this.infoGroup.Controls.Add(this.moduleArtLabel);
            this.infoGroup.Controls.Add(this.byLabel);
            this.infoGroup.Controls.Add(this.screenRecorderLink);
            this.infoGroup.Controls.Add(this.licenseLabel);
            this.infoGroup.Controls.Add(this.updatesLink);
            this.infoGroup.Controls.Add(this.versionLabel);
            this.infoGroup.Controls.Add(this.descriptionLabel);
            this.infoGroup.Controls.Add(this.basedOnLabel);
            this.infoGroup.Controls.Add(this.productLabel);
            this.infoGroup.Location = new System.Drawing.Point(9, 98);
            this.infoGroup.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.infoGroup.Name = "infoGroup";
            this.infoGroup.Size = new System.Drawing.Size(256, 226);
            this.infoGroup.TabIndex = 34;
            this.infoGroup.TabStop = false;
            this.infoGroup.Text = "Info";
            // 
            // moduleArtLabel
            // 
            this.moduleArtLabel.AutoSize = true;
            this.moduleArtLabel.Location = new System.Drawing.Point(23, 170);
            this.moduleArtLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.moduleArtLabel.Name = "moduleArtLabel";
            this.moduleArtLabel.Size = new System.Drawing.Size(162, 15);
            this.moduleArtLabel.TabIndex = 2;
            this.moduleArtLabel.TabStop = true;
            this.moduleArtLabel.Text = "Module Art (Eugene Volynko)";
            this.aboutTooltip.SetToolTip(this.moduleArtLabel, "Open \"Quick Screen Recorder\" in GitHub");
            this.moduleArtLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.moduleArtLabel_LinkClicked);
            // 
            // byLabel
            // 
            this.byLabel.AutoSize = true;
            this.byLabel.Location = new System.Drawing.Point(6, 170);
            this.byLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.byLabel.MaximumSize = new System.Drawing.Size(190, 0);
            this.byLabel.Name = "byLabel";
            this.byLabel.Size = new System.Drawing.Size(20, 15);
            this.byLabel.TabIndex = 38;
            this.byLabel.Text = "by";
            // 
            // screenRecorderLink
            // 
            this.screenRecorderLink.AutoSize = true;
            this.screenRecorderLink.Location = new System.Drawing.Point(58, 155);
            this.screenRecorderLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.screenRecorderLink.Name = "screenRecorderLink";
            this.screenRecorderLink.Size = new System.Drawing.Size(126, 15);
            this.screenRecorderLink.TabIndex = 1;
            this.screenRecorderLink.TabStop = true;
            this.screenRecorderLink.Text = "Quick Screen Recorder";
            this.aboutTooltip.SetToolTip(this.screenRecorderLink, "Open \"Quick Screen Recorder\" in GitHub");
            this.screenRecorderLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.screenRecorderLink_LinkClicked);
            // 
            // licenseLabel
            // 
            this.licenseLabel.AutoSize = true;
            this.licenseLabel.Location = new System.Drawing.Point(6, 193);
            this.licenseLabel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.licenseLabel.Name = "licenseLabel";
            this.licenseLabel.Size = new System.Drawing.Size(93, 15);
            this.licenseLabel.TabIndex = 37;
            this.licenseLabel.Text = "License: GPL-3.0";
            // 
            // updatesLink
            // 
            this.updatesLink.AutoSize = true;
            this.updatesLink.Location = new System.Drawing.Point(6, 106);
            this.updatesLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.updatesLink.Name = "updatesLink";
            this.updatesLink.Size = new System.Drawing.Size(127, 15);
            this.updatesLink.TabIndex = 0;
            this.updatesLink.TabStop = true;
            this.updatesLink.Text = "Check for new releases";
            this.updatesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.updatesLink_LinkClicked);
            // 
            // issuesLink
            // 
            this.issuesLink.AutoSize = true;
            this.issuesLink.Location = new System.Drawing.Point(151, 28);
            this.issuesLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.issuesLink.Name = "issuesLink";
            this.issuesLink.Size = new System.Drawing.Size(38, 15);
            this.issuesLink.TabIndex = 5;
            this.issuesLink.TabStop = true;
            this.issuesLink.Text = "Issues";
            this.aboutTooltip.SetToolTip(this.issuesLink, "Open GitHub issues page in browser");
            this.issuesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.issuesLink_LinkClicked);
            // 
            // pagesGroup
            // 
            this.pagesGroup.Controls.Add(this.authorLink);
            this.pagesGroup.Controls.Add(this.licenseLink);
            this.pagesGroup.Controls.Add(this.projectLink);
            this.pagesGroup.Controls.Add(this.issuesLink);
            this.pagesGroup.Location = new System.Drawing.Point(9, 333);
            this.pagesGroup.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pagesGroup.Name = "pagesGroup";
            this.pagesGroup.Size = new System.Drawing.Size(256, 63);
            this.pagesGroup.TabIndex = 37;
            this.pagesGroup.TabStop = false;
            this.pagesGroup.Text = "Links";
            // 
            // authorLink
            // 
            this.authorLink.AutoSize = true;
            this.authorLink.Location = new System.Drawing.Point(6, 28);
            this.authorLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.authorLink.Name = "authorLink";
            this.authorLink.Size = new System.Drawing.Size(44, 15);
            this.authorLink.TabIndex = 3;
            this.authorLink.TabStop = true;
            this.authorLink.Text = "Author";
            this.aboutTooltip.SetToolTip(this.authorLink, "Open author page in GitHub");
            this.authorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.authorLink_LinkClicked);
            // 
            // licenseLink
            // 
            this.licenseLink.AutoSize = true;
            this.licenseLink.Location = new System.Drawing.Point(202, 28);
            this.licenseLink.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.licenseLink.Name = "licenseLink";
            this.licenseLink.Size = new System.Drawing.Size(46, 15);
            this.licenseLink.TabIndex = 6;
            this.licenseLink.TabStop = true;
            this.licenseLink.Text = "License";
            this.aboutTooltip.SetToolTip(this.licenseLink, "Open license page in browser");
            this.licenseLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.licenseLink_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(274, 404);
            this.Controls.Add(this.pagesGroup);
            this.Controls.Add(this.infoGroup);
            this.Controls.Add(this.logoPictureBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.infoGroup.ResumeLayout(false);
            this.infoGroup.PerformLayout();
            this.pagesGroup.ResumeLayout(false);
            this.pagesGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label productLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label basedOnLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.LinkLabel projectLink;
        private CustomComponents.DarkThemeGroupBox infoGroup;
        private System.Windows.Forms.LinkLabel issuesLink;
        private CustomComponents.DarkThemeGroupBox pagesGroup;
        private System.Windows.Forms.Label licenseLabel;
        private System.Windows.Forms.LinkLabel licenseLink;
        private System.Windows.Forms.ToolTip aboutTooltip;
        private System.Windows.Forms.LinkLabel updatesLink;
        private System.Windows.Forms.LinkLabel screenRecorderLink;
        private System.Windows.Forms.LinkLabel moduleArtLabel;
        private System.Windows.Forms.Label byLabel;
        private System.Windows.Forms.LinkLabel authorLink;
    }
}
