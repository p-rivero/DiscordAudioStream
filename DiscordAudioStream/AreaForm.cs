using DLLs;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace DiscordAudioStream
{
	public partial class AreaForm : Form
	{
		public const int BORDER_WIDTH_PX = 2;

		private const int WM_NCLBUTTONDOWN = 0xA1;
		private const int HT_CAPTION = 0x2;

		private Point startPos;
		private Size startSize;
		private bool showMarker;

		private readonly System.Timers.Timer resizeTimer = new System.Timers.Timer();

		private readonly Rectangle bounds = GetScreenBounds();

		public AreaForm()
		{
			InitializeComponent();

			LocationChanged += (sender, e) => EnsureWithinBounds();
			SizeChanged += (sender, e) => Refresh();

			resizeTimer.Elapsed += new ElapsedEventHandler(ResizeTimerElapsed);
			resizeTimer.Interval = 30;
		}

		new public void Show()
		{
			base.Show();
			SetButtonsVisible(true);
			showMarker = true;
			// Restore saved position
			if (Properties.Settings.Default.AreaForm_Size != Size.Empty)
			{
				Location = Properties.Settings.Default.AreaForm_Position;
				Size = Properties.Settings.Default.AreaForm_Size;
				// Screen size may have changed since last time
				EnsureWithinBounds();
			}
		}
		new public void Hide()
		{
			// Save position
			Properties.Settings.Default.AreaForm_Position = Location;
			Properties.Settings.Default.AreaForm_Size = Size;
			Properties.Settings.Default.Save();
			base.Hide();
		}

		private void ResizeTimerElapsed(object sender, ElapsedEventArgs e)
		{
			Invoke((MethodInvoker)(() =>
			{
				Point curPos = PointToClient(Cursor.Position);

				int targetWidth = startSize.Width + curPos.X - startPos.X;
				int targetHeight = startSize.Height + curPos.Y - startPos.Y;

				// Clip to bottom-right corner
				Width = Math.Min(targetWidth, bounds.Right - this.Left);
				Height = Math.Min(targetHeight, bounds.Bottom - this.Top);
			}));
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			const int OFFSET = BORDER_WIDTH_PX / 2;
			if (showMarker)
			{
				Rectangle rect = ClientRectangle;
				using (Pen pen = new Pen(Color.Red, BORDER_WIDTH_PX))
				{
					e.Graphics.DrawRectangle(pen, rect.X + OFFSET, rect.Y + OFFSET, rect.Width - BORDER_WIDTH_PX, rect.Height - BORDER_WIDTH_PX);
				}
			}
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

		private void lockBtn_Click(object sender, EventArgs e)
		{
			// If the user has not seen the warning, display it
			if (!Properties.Settings.Default.SeenLockAreaDiag)
			{
				DialogResult r = MessageBox.Show(
					"This will hide the red area marker and you won't be able to move it anymore. " +
					"To show the red marker again, change the capture to anything else and then back to \"Custom area\".\n" +
					"This message won't be shown again.",
					"Lock area",
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Information,
					// The second button ("Cancel") is the default option
					MessageBoxDefaultButton.Button2
				);
				if (r == DialogResult.Cancel)
					return;
				Properties.Settings.Default.SeenLockAreaDiag = true;
				Properties.Settings.Default.Save();
				Logger.Log("Set SeenLockAreaDiag = true");
			}
			Logger.Log("Locking area");
			showMarker = false;
			// Hide buttons
			SetButtonsVisible(false);
			// Redraw (lack of) red rectangle
			Refresh();
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

		private void EnsureWithinBounds()
		{
			// Adjust size if needed
			Height = Math.Min(Height, bounds.Height);
			Width = Math.Min(Width, bounds.Width);
			// Clip to top-left corner
			Left = Math.Max(Left, bounds.Left);
			Top = Math.Max(Top, bounds.Top);
			// Clip to bottom-right corner
			Left = Math.Min(Left, bounds.Right - Width);
			Top = Math.Min(Top, bounds.Bottom - Height);
		}

		private void AreaForm_Deactivate(object sender, EventArgs e)
		{
			SetButtonsVisible(false);
		}

		private void AreaForm_MouseEnter(object sender, EventArgs e)
		{
			SetButtonsVisible(true);
		}

		private void SetButtonsVisible(bool visible)
		{
			dragBtn.Visible = visible;
			moveBtn.Visible = visible;
			titleBtn.Visible = visible;
			lockBtn.Visible = visible;
			// Activate form so that AreaForm_Deactivate is called when we click somewhere
			if (visible) Activate();
		}

		private static Rectangle GetScreenBounds()
		{
			// Assume the screen size won't change while the program is running
			Rectangle screen = SystemInformation.VirtualScreen;

			// Allow moving out of bounds to compensate for red border
			return new Rectangle(screen.X - BORDER_WIDTH_PX, screen.Y - BORDER_WIDTH_PX, 
				screen.Width + 2 * BORDER_WIDTH_PX, screen.Height + 2 * BORDER_WIDTH_PX);
		}

		private void AreaForm_ResizeEnd(object sender, EventArgs e)
		{
			// Workaround for Windows not allowing users to move a window above the screen bounds
			if (Top == 0)
			{
				Top = -BORDER_WIDTH_PX;
			}
		}
	}
}
