namespace DiscordAudioStream
{
	partial class AudioMeterForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioMeterForm));
			this.volumeMeterLeft = new CustomComponents.CustomAudioMeter();
			this.volumeMeterRight = new CustomComponents.CustomAudioMeter();
			this.dbLabel60 = new System.Windows.Forms.Label();
			this.rightLabel = new System.Windows.Forms.Label();
			this.leftLabel = new System.Windows.Forms.Label();
			this.dbLabel0 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// volumeMeterLeft
			// 
			this.volumeMeterLeft.Amplitude = 0F;
			this.volumeMeterLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.volumeMeterLeft.ForeColor = System.Drawing.Color.DarkGreen;
			this.volumeMeterLeft.Location = new System.Drawing.Point(47, 12);
			this.volumeMeterLeft.MaxDb = 0F;
			this.volumeMeterLeft.MinDb = -60F;
			this.volumeMeterLeft.Name = "volumeMeterLeft";
			this.volumeMeterLeft.Size = new System.Drawing.Size(20, 272);
			this.volumeMeterLeft.TabIndex = 1;
			this.volumeMeterLeft.Text = "Audio level";
			// 
			// volumeMeterRight
			// 
			this.volumeMeterRight.Amplitude = 0F;
			this.volumeMeterRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.volumeMeterRight.ForeColor = System.Drawing.Color.DarkGreen;
			this.volumeMeterRight.Location = new System.Drawing.Point(77, 12);
			this.volumeMeterRight.MaxDb = 0F;
			this.volumeMeterRight.MinDb = -60F;
			this.volumeMeterRight.Name = "volumeMeterRight";
			this.volumeMeterRight.Size = new System.Drawing.Size(20, 272);
			this.volumeMeterRight.TabIndex = 2;
			this.volumeMeterRight.Text = "Audio level";
			// 
			// dbLabel60
			// 
			this.dbLabel60.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.dbLabel60.AutoSize = true;
			this.dbLabel60.Location = new System.Drawing.Point(103, 276);
			this.dbLabel60.Name = "dbLabel60";
			this.dbLabel60.Size = new System.Drawing.Size(38, 13);
			this.dbLabel60.TabIndex = 3;
			this.dbLabel60.Text = "-60 dB";
			// 
			// rightLabel
			// 
			this.rightLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.rightLabel.AutoSize = true;
			this.rightLabel.Location = new System.Drawing.Point(79, 289);
			this.rightLabel.Name = "rightLabel";
			this.rightLabel.Size = new System.Drawing.Size(15, 13);
			this.rightLabel.TabIndex = 4;
			this.rightLabel.Text = "R";
			// 
			// leftLabel
			// 
			this.leftLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.leftLabel.AutoSize = true;
			this.leftLabel.Location = new System.Drawing.Point(50, 289);
			this.leftLabel.Name = "leftLabel";
			this.leftLabel.Size = new System.Drawing.Size(13, 13);
			this.leftLabel.TabIndex = 5;
			this.leftLabel.Text = "L";
			// 
			// dbLabel0
			// 
			this.dbLabel0.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.dbLabel0.AutoSize = true;
			this.dbLabel0.Location = new System.Drawing.Point(103, 9);
			this.dbLabel0.Name = "dbLabel0";
			this.dbLabel0.Size = new System.Drawing.Size(29, 13);
			this.dbLabel0.TabIndex = 6;
			this.dbLabel0.Text = "0 dB";
			// 
			// AudioMeterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(144, 311);
			this.Controls.Add(this.dbLabel0);
			this.Controls.Add(this.leftLabel);
			this.Controls.Add(this.rightLabel);
			this.Controls.Add(this.dbLabel60);
			this.Controls.Add(this.volumeMeterRight);
			this.Controls.Add(this.volumeMeterLeft);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(160, 9999);
			this.MinimumSize = new System.Drawing.Size(80, 150);
			this.Name = "AudioMeterForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Audio Meter";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private CustomComponents.CustomAudioMeter volumeMeterLeft;
		private CustomComponents.CustomAudioMeter volumeMeterRight;
		private System.Windows.Forms.Label dbLabel60;
		private System.Windows.Forms.Label rightLabel;
		private System.Windows.Forms.Label leftLabel;
		private System.Windows.Forms.Label dbLabel0;
	}
}