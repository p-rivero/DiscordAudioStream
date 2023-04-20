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
			this.volumeMeter = new NAudio.Gui.VolumeMeter();
			this.SuspendLayout();
			// 
			// volumeMeter
			// 
			this.volumeMeter.Amplitude = 0F;
			this.volumeMeter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.volumeMeter.ForeColor = System.Drawing.Color.DarkGreen;
			this.volumeMeter.Location = new System.Drawing.Point(12, 12);
			this.volumeMeter.MaxDb = 0F;
			this.volumeMeter.MinDb = -60F;
			this.volumeMeter.Name = "volumeMeter";
			this.volumeMeter.Size = new System.Drawing.Size(120, 237);
			this.volumeMeter.TabIndex = 1;
			this.volumeMeter.Text = "Audio level";
			// 
			// AudioMeterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(144, 261);
			this.Controls.Add(this.volumeMeter);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximumSize = new System.Drawing.Size(160, 9999);
			this.MinimumSize = new System.Drawing.Size(60, 100);
			this.Name = "AudioMeterForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Audio Meter";
			this.ResumeLayout(false);

		}

		#endregion

		private NAudio.Gui.VolumeMeter volumeMeter;
	}
}