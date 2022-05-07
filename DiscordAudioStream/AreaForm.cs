using DLLs;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace DiscordAudioStream
{
	public partial class AreaForm : Form
	{
		private const int WM_NCLBUTTONDOWN = 0xA1;
		private const int HT_CAPTION = 0x2;

		private Point startPos;
		private Size startSize;

		private readonly System.Timers.Timer resizeTimer = new System.Timers.Timer();

		private readonly Rectangle bounds;

		public AreaForm()
		{
			InitializeComponent();
			this.bounds = SystemInformation.VirtualScreen;

			resizeTimer.Elapsed += new ElapsedEventHandler(resizeTimer_Elapsed);
			resizeTimer.Interval = 30;
		}

		private void resizeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Invoke((MethodInvoker)(() =>
			{
				Point curPos = PointToClient(Cursor.Position);

				int newWidth = startSize.Width + curPos.X - startPos.X;
				int newHeight = startSize.Height + curPos.Y - startPos.Y;

				// Clip to bottom-right corner
				newWidth = Math.Min(newWidth, bounds.Right - this.Left);
				newHeight = Math.Min(newHeight, bounds.Bottom - this.Top);

				Width = newWidth;
				Height = newHeight;
			}));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
		}

		private void AreaForm_MouseDown(object sender, MouseEventArgs e)
		{
			Cursor.Current = Cursors.SizeAll;
			User32.ReleaseCapture();
			User32.SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
		}

		private void dragBtn_MouseDown(object sender, MouseEventArgs e)
		{
			Cursor.Current = Cursors.SizeNWSE;
			startPos = PointToClient(Cursor.Position);
			startSize = Size;
			resizeTimer.Start();
		}

		private void AreaForm_MouseUp(object sender, MouseEventArgs e)
		{
			Cursor.Current = Cursors.Default;
		}

		private void dragBtn_MouseUp(object sender, MouseEventArgs e)
		{
			Cursor.Current = Cursors.Default;
			resizeTimer.Stop();
		}

		private void AreaForm_SizeChanged(object sender, EventArgs e)
		{
			// Redraw red rectangle
			Refresh();
		}

		// Called when the form stops moving
		private void AreaForm_ResizeEnd(object sender, EventArgs e)
		{
			// Clip to top-left corner
			Left = Math.Max(this.Left, bounds.Left);
			Top = Math.Max(this.Top, bounds.Top);

			// Clip to bottom-right corner
			Left = Math.Min(this.Left, bounds.Right - this.Width);
			Top = Math.Min(this.Top, bounds.Bottom - this.Height);
		}

		private void AreaForm_Deactivate(object sender, EventArgs e)
		{
			dragBtn.Visible = false;
			moveBtn.Visible = false;
			titleBtn.Visible = false;
		}

		private void AreaForm_MouseEnter(object sender, EventArgs e)
		{
			dragBtn.Visible = true;
			moveBtn.Visible = true;
			titleBtn.Visible = true;
			// Activate form so that AreaForm_Deactivate is called when we click somewhere
			Activate();
		}
	}
}
