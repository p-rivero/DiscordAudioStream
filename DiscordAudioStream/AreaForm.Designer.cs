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
            this.titleBtn = new System.Windows.Forms.Button();
            this.dragBtn__ = new System.Windows.Forms.Button();
            this.moveBtn = new System.Windows.Forms.Button();
            this.dragBtn = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dragBtn)).BeginInit();
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
            // dragBtn__
            // 
            this.dragBtn__.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dragBtn__.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dragBtn__.Enabled = false;
            this.dragBtn__.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.dragBtn__.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dragBtn__.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dragBtn__.Image = global::DiscordAudioStream.Properties.Resources.resize;
            this.dragBtn__.Location = new System.Drawing.Point(412, 252);
            this.dragBtn__.Name = "dragBtn__";
            this.dragBtn__.Size = new System.Drawing.Size(56, 56);
            this.dragBtn__.TabIndex = 0;
            this.dragBtn__.UseVisualStyleBackColor = false;
            // 
            // moveBtn
            // 
            this.moveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.moveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.moveBtn.Enabled = false;
            this.moveBtn.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.moveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moveBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moveBtn.Image = global::DiscordAudioStream.Properties.Resources.move;
            this.moveBtn.Location = new System.Drawing.Point(350, 252);
            this.moveBtn.Name = "moveBtn";
            this.moveBtn.Size = new System.Drawing.Size(56, 56);
            this.moveBtn.TabIndex = 2;
            this.moveBtn.UseVisualStyleBackColor = false;
            // 
            // dragBtn
            // 
            this.dragBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dragBtn.BackgroundImage = global::DiscordAudioStream.Properties.Resources.resize;
            this.dragBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.dragBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dragBtn.Location = new System.Drawing.Point(412, 182);
            this.dragBtn.Name = "dragBtn";
            this.dragBtn.Size = new System.Drawing.Size(56, 56);
            this.dragBtn.TabIndex = 3;
            this.dragBtn.TabStop = false;
            this.dragBtn.Visible = false;
            // 
            // AreaForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.MediumBlue;
            this.ClientSize = new System.Drawing.Size(480, 320);
            this.Controls.Add(this.dragBtn);
            this.Controls.Add(this.moveBtn);
            this.Controls.Add(this.titleBtn);
            this.Controls.Add(this.dragBtn__);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(160, 160);
            this.Name = "AreaForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recording area - Discord Audio Stream";
            this.TransparencyKey = System.Drawing.Color.MediumBlue;
            this.Deactivate += new System.EventHandler(this.AreaForm_Deactivate);
            this.ResizeEnd += new System.EventHandler(this.AreaForm_ResizeEnd);
            this.LocationChanged += new System.EventHandler(this.AreaForm_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.AreaForm_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AreaForm_MouseDown);
            this.MouseEnter += new System.EventHandler(this.AreaForm_MouseEnter);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AreaForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.dragBtn)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button dragBtn__;
		private System.Windows.Forms.Button titleBtn;
        private System.Windows.Forms.Button moveBtn;
        private System.Windows.Forms.PictureBox dragBtn;
    }
}