namespace DiscordAudioStream
{
	partial class AreaForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AreaForm));
			this.titleBtn = new System.Windows.Forms.Button();
			this.dragBtn = new System.Windows.Forms.PictureBox();
			this.moveBtn = new System.Windows.Forms.PictureBox();
			this.lockBtn = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.dragBtn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.moveBtn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lockBtn)).BeginInit();
			this.SuspendLayout();
			// 
			// titleBtn
			// 
			this.titleBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.titleBtn.Enabled = false;
			this.titleBtn.FlatAppearance.BorderColor = System.Drawing.Color.Red;
			this.titleBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.titleBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.titleBtn.Location = new System.Drawing.Point(12, 12);
			this.titleBtn.Name = "titleBtn";
			this.titleBtn.Size = new System.Drawing.Size(90, 25);
			this.titleBtn.TabIndex = 1;
			this.titleBtn.Text = "Screen area";
			this.titleBtn.UseVisualStyleBackColor = false;
			// 
			// dragBtn
			// 
			this.dragBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.dragBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.dragBtn.BackgroundImage = global::DiscordAudioStream.Properties.Resources.resize;
			this.dragBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.dragBtn.Location = new System.Drawing.Point(412, 252);
			this.dragBtn.Name = "dragBtn";
			this.dragBtn.Size = new System.Drawing.Size(56, 56);
			this.dragBtn.TabIndex = 3;
			this.dragBtn.TabStop = false;
			this.dragBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dragBtn_MouseDown);
			this.dragBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dragBtn_MouseUp);
			// 
			// moveBtn
			// 
			this.moveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.moveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.moveBtn.BackgroundImage = global::DiscordAudioStream.Properties.Resources.move;
			this.moveBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.moveBtn.Location = new System.Drawing.Point(350, 252);
			this.moveBtn.Name = "moveBtn";
			this.moveBtn.Size = new System.Drawing.Size(56, 56);
			this.moveBtn.TabIndex = 4;
			this.moveBtn.TabStop = false;
			this.moveBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AreaForm_MouseDown);
			this.moveBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AreaForm_MouseUp);
			// 
			// lockBtn
			// 
			this.lockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lockBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.lockBtn.BackgroundImage = global::DiscordAudioStream.Properties.Resources._lock;
			this.lockBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.lockBtn.Location = new System.Drawing.Point(12, 252);
			this.lockBtn.Name = "lockBtn";
			this.lockBtn.Size = new System.Drawing.Size(56, 56);
			this.lockBtn.TabIndex = 5;
			this.lockBtn.TabStop = false;
			this.lockBtn.Click += new System.EventHandler(this.lockBtn_Click);
			// 
			// AreaForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Lime;
			this.ClientSize = new System.Drawing.Size(480, 320);
			this.Controls.Add(this.lockBtn);
			this.Controls.Add(this.moveBtn);
			this.Controls.Add(this.dragBtn);
			this.Controls.Add(this.titleBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(204, 160);
			this.Name = "AreaForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Recording area - Discord Audio Stream";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.Lime;
			this.Deactivate += new System.EventHandler(this.AreaForm_Deactivate);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AreaForm_MouseDown);
			this.MouseEnter += new System.EventHandler(this.AreaForm_MouseEnter);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AreaForm_MouseUp);
			((System.ComponentModel.ISupportInitialize)(this.dragBtn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.moveBtn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lockBtn)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button titleBtn;
        private System.Windows.Forms.PictureBox dragBtn;
		private System.Windows.Forms.PictureBox moveBtn;
		private System.Windows.Forms.PictureBox lockBtn;
	}
}