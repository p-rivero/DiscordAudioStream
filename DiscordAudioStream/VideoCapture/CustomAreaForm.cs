using System.Drawing;
using System.Windows.Forms;

using Windows.Win32;
using Windows.Win32.Foundation;

namespace DiscordAudioStream.VideoCapture;

public partial class CustomAreaForm : Form
{
    public const int BORDER_WIDTH_PX = 2;
    public const string WINDOW_TITLE = "Recording area - Discord Audio Stream";

    public bool FormHidden { get; set; }

    private Point startPos;
    private Size startSize;
    private bool showMarker;

    private readonly Rectangle bounds = GetScreenBounds();

    public CustomAreaForm()
    {
        InitializeComponent();

        LocationChanged += (sender, e) => EnsureWithinBounds();
        SizeChanged += (sender, e) => Refresh();
        Text = WINDOW_TITLE;
    }

    public new void Show()
    {
        base.Show();
        showMarker = !FormHidden;
    }

    private void resizeTimer_Tick(object sender, EventArgs e)
    {
        Point curPos = PointToClient(Cursor.Position);

        int targetWidth = startSize.Width + curPos.X - startPos.X;
        int targetHeight = startSize.Height + curPos.Y - startPos.Y;

        // Clip to bottom-right corner
        Width = Math.Min(targetWidth, bounds.Right - Left);
        Height = Math.Min(targetHeight, bounds.Bottom - Top);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        const int OFFSET = BORDER_WIDTH_PX / 2;
        if (showMarker)
        {
            Rectangle rect = ClientRectangle;
            using Pen pen = new(Color.Red, BORDER_WIDTH_PX);
            e.Graphics.DrawRectangle(pen, rect.X + OFFSET, rect.Y + OFFSET, rect.Width - BORDER_WIDTH_PX, rect.Height - BORDER_WIDTH_PX);
        }
    }

    private void AreaForm_MouseDown(object sender, MouseEventArgs e)
    {
        Cursor.Current = Cursors.SizeAll;
        PInvoke.ReleaseCapture().AssertSuccess("Could not release capture");
        _ = PInvoke.SendMessage(this.HWnd(), PInvoke.WM_NCLBUTTONDOWN, (WPARAM)PInvoke.HTCAPTION, (LPARAM)0);
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
            DialogResult r = ShowMessage.Information()
                .Title("Lock area")
                .Text("This will hide the red area marker and you won't be able to move it anymore.")
                .Text("To show the red marker again, change the capture to anything else and then back to \"Custom area\".")
                .Text("This message won't be shown again.")
                .Cancelable()
                .GetResult();
            if (r == DialogResult.Cancel)
            {
                return;
            }

            Properties.Settings.Default.SeenLockAreaDiag = true;
            Logger.Log("Set SeenLockAreaDiag = true");
        }
        Logger.Log("Locking area");
        showMarker = false;
        SetButtonsVisible(false);
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
        if (visible)
        {
            Activate();
        }
    }

    private static Rectangle GetScreenBounds()
    {
        // Assume the screen size won't change while the program is running
        Rectangle screen = SystemInformation.VirtualScreen;

        // Allow moving out of bounds to compensate for red border
        return new Rectangle(
            screen.X - BORDER_WIDTH_PX,
            screen.Y - BORDER_WIDTH_PX,
            screen.Width + 2 * BORDER_WIDTH_PX,
            screen.Height + 2 * BORDER_WIDTH_PX
        );
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
