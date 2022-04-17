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
		private Size curSize;

		private System.Timers.Timer resizeTimer = new System.Timers.Timer();

		private int startX = -1;
		private int startY = -1;
		private int screenWidth = -1;
		private int screenHeight = -1;

		private int oldWidth = -1;
		private int oldHeight = -1;

		public AreaForm()
		{
			InitializeComponent();

			resizeTimer.Elapsed += new ElapsedEventHandler(resizeTimer_Elapsed);
			resizeTimer.Interval = 50;
		}

		private void resizeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Invoke((MethodInvoker)(() => {
				Point curPos = PointToClient(Cursor.Position);

				int newWidth = curSize.Width + curPos.X - startPos.X;
				int newHeight = curSize.Height + curPos.Y - startPos.Y;

				if (newWidth == oldWidth && newHeight == oldHeight)
					return;

				oldWidth = newWidth;
				oldHeight = newHeight;

				int limitEndX = startX + screenWidth;
				int limitEndY = startY + screenHeight;

				if (Left + newWidth > limitEndX) newWidth = limitEndX - Left;
				if (Top + newHeight > limitEndY) newHeight = limitEndY - Top;

				// Omit 2 pixels for red border
				MainForm mainForm = Owner as MainForm;
				mainForm.SetAreaWidth(newWidth - 2);
				mainForm.SetAreaHeight(newHeight - 2);
				mainForm.SetPreviewSize(new Size(newWidth-2, newHeight-2));
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
				Point downPos = PointToClient(Cursor.Position);
				if (downPos.X > dragBtn.Location.X && downPos.X < dragBtn.Location.X + dragBtn.Width &&
					downPos.Y > dragBtn.Location.Y && downPos.Y < dragBtn.Location.Y + dragBtn.Height)
				{
					Cursor.Current = Cursors.SizeNWSE;
					startPos = downPos;
					curSize = Size;
					resizeTimer.Start();
				}
				else
				{
					Cursor.Current = Cursors.SizeAll;
					User32.ReleaseCapture();
					User32.SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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
			Refresh();
			// Omit 2 pixels for red border
			(Owner as MainForm).SetMaximumX(screenWidth - Width - 2);
			(Owner as MainForm).SetMaximumY(screenHeight - Height - 2);
		}

		private void AreaForm_LocationChanged(object sender, EventArgs e)
		{
			// Omit 1 pixel for red border
			(Owner as MainForm).SetAreaX(Left + 1);
			(Owner as MainForm).SetAreaY(Top + 1);
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

			if (Left < startX) Left = startX;
			if (Top < startY) Top = startY;

			if (Left + Width > limitEndX) Left = limitEndX - Width;
			if (Top + Height > limitEndY) Top = limitEndY - Height;
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
			Activate();
		}
    }
}
