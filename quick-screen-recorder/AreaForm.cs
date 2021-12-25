using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace quick_screen_recorder
{
	public partial class AreaForm : Form
	{
		private const int WM_NCLBUTTONDOWN = 0xA1;
		private const int HT_CAPTION = 0x2;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool ReleaseCapture();

		private Point startPos;
		private Size curSize;

		private System.Timers.Timer resizeTimer = new System.Timers.Timer();

		private int startX = -1;
		private int startY = -1;
		private int screenWidth = -1;
		private int screenHeight = -1;

		public AreaForm()
		{
			InitializeComponent();

			resizeTimer.Elapsed += new ElapsedEventHandler(resizeTimer_Elapsed);
			resizeTimer.Interval = 50;
		}

		private void resizeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.Invoke((MethodInvoker)(() => {
				Point curPos = this.PointToClient(Cursor.Position);

				int newWidth = curSize.Width + curPos.X - startPos.X;
				int newHeight = curSize.Height + curPos.Y - startPos.Y;

				int limitEndX = startX + screenWidth;
				int limitEndY = startY + screenHeight;

				if (this.Left + newWidth > limitEndX) newWidth = limitEndX - this.Left;
				if (this.Top + newHeight > limitEndY) newHeight = limitEndY - this.Top;

				// Omit 2 pixels for red border
				(this.Owner as MainForm).SetAreaWidth(newWidth - 2);
				(this.Owner as MainForm).SetAreaHeight(newHeight - 2);
			}));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
		}

		private void AreaForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Point downPos = this.PointToClient(Cursor.Position);
				if (downPos.X > dragBtn.Location.X && downPos.X < dragBtn.Location.X + dragBtn.Width &&
					downPos.Y > dragBtn.Location.Y && downPos.Y < dragBtn.Location.Y + dragBtn.Height)
				{
					Cursor.Current = Cursors.SizeNWSE;
					startPos = downPos;
					curSize = this.Size;
					resizeTimer.Start();
				}
				else
				{
					Cursor.Current = Cursors.SizeAll;
					ReleaseCapture();
					SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
				}
			}
		}

		private void AreaForm_MouseUp(object sender, MouseEventArgs e)
		{
			resizeTimer.Stop();
			Cursor.Current = Cursors.Default;
		}

		private void AreaForm_SizeChanged(object sender, EventArgs e)
		{
			this.Refresh();
			// Omit 2 pixels for red border
			(this.Owner as MainForm).SetMaximumX(screenWidth - this.Width - 2);
			(this.Owner as MainForm).SetMaximumY(screenHeight - this.Height - 2);
		}

		private void AreaForm_LocationChanged(object sender, EventArgs e)
		{
			// Omit 1 pixel for red border
			(this.Owner as MainForm).SetAreaX(this.Left + 1);
			(this.Owner as MainForm).SetAreaY(this.Top + 1);
		}

		public void SetMaximumArea(Rectangle screen)
		{
			screenWidth = screen.Width;
			screenHeight = screen.Height;
			startX = screen.X;
			startY = screen.Y;
		}

		private void AreaForm_ResizeEnd(object sender, EventArgs e)
		{
			int limitEndX = startX + screenWidth;
			int limitEndY = startY + screenHeight;

			if (this.Left < startX) this.Left = startX;
			if (this.Top < startY) this.Top = startY;

			if (this.Left + this.Width > limitEndX) this.Left = limitEndX - this.Width;
			if (this.Top + this.Height > limitEndY) this.Top = limitEndY - this.Height;
		}

        private void AreaForm_Activated(object sender, EventArgs e)
        {
			dragBtn.Visible = true;
			titleBtn.Visible = true;
		}

        private void AreaForm_Deactivate(object sender, EventArgs e)
        {
			dragBtn.Visible = false;
			titleBtn.Visible = false;
		}
    }
}
