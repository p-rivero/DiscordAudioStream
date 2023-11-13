using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DiscordAudioStream;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Controls;
using Windows.Win32.UI.WindowsAndMessaging;

namespace CustomComponents;

internal partial class DarkThemeMessageBox : Form
{
    private static readonly Dictionary<DialogResult, string> buttonTexts = new()
    {
        { DialogResult.OK, "OK" },
        { DialogResult.Cancel, "Cancel" },
        { DialogResult.Yes, "Yes" },
        { DialogResult.No, "No" },
        { DialogResult.Abort, "Abort" },
        { DialogResult.Retry, "Retry" },
        { DialogResult.Ignore, "Ignore" },
    };

    private static readonly Size LabelPadding = new(100, 125);

    private MessageBoxButtons buttons;
    private MessageBoxIcon icon;
    private MessageBoxDefaultButton defaultButton;

    public string MessageText
    {
        get => messageLabel.Text;
        init
        {
            messageLabel.Text = value;
            Size = messageLabel.Size + LabelPadding;
            UpdateIconPosition();
        }
    }

    public string Title
    {
        get => Text;
        init => Text = value;
    }

    public MessageBoxButtons Buttons
    {
        get => buttons;
        init
        {
            buttons = value;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            switch (value)
            {
                case MessageBoxButtons.OK:
                    ActivateButton(button3, DialogResult.OK);
                    break;
                case MessageBoxButtons.OKCancel:
                    ActivateButton(button2, DialogResult.OK);
                    ActivateButton(button3, DialogResult.Cancel);
                    break;
                case MessageBoxButtons.YesNo:
                    ActivateButton(button2, DialogResult.Yes);
                    ActivateButton(button3, DialogResult.No);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    ActivateButton(button1, DialogResult.Yes);
                    ActivateButton(button2, DialogResult.No);
                    ActivateButton(button3, DialogResult.Cancel);
                    break;
                case MessageBoxButtons.RetryCancel:
                    ActivateButton(button2, DialogResult.Cancel);
                    ActivateButton(button3, DialogResult.Retry);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    ActivateButton(button1, DialogResult.Abort);
                    ActivateButton(button2, DialogResult.Retry);
                    ActivateButton(button3, DialogResult.Ignore);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid MessageBoxButtons");
            }
        }
    }

    public MessageBoxIcon MessageIcon
    {
        get => icon;
        init
        {
            icon = value;
            PCWSTR pszName = value switch
            {
                MessageBoxIcon.Error => PInvoke.IDI_ERROR,
                MessageBoxIcon.Question => PInvoke.IDI_QUESTION,
                MessageBoxIcon.Warning => PInvoke.IDI_WARNING,
                MessageBoxIcon.Information => PInvoke.IDI_INFORMATION,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid MessageBoxIcon"),
            };
            try
            {
                PInvoke.LoadIconMetric(HINSTANCE.Null, pszName, _LI_METRIC.LIM_LARGE, out HICON hIcon).AssertSuccess("LoadIconMetric failed");
                iconPicture.Image = Icon.FromHandle(hIcon).ToBitmap();
            }
            catch (Exception e) when (e is ExternalException or EntryPointNotFoundException)
            {
                Logger.Log("Cannot load MessageBox icon");
                Logger.Log(e);
            }
        }
    }

    public MessageBoxDefaultButton DefaultButton
    {
        get => defaultButton;
        init
        {
            defaultButton = value;
            int numberOfButtons = buttons switch
            {
                MessageBoxButtons.OK => 1,
                MessageBoxButtons.OKCancel => 2,
                MessageBoxButtons.YesNo => 2,
                MessageBoxButtons.RetryCancel => 2,
                MessageBoxButtons.AbortRetryIgnore => 3,
                MessageBoxButtons.YesNoCancel => 3,
                _ => throw new InvalidDataException()
            };
            Button button = (value, numberOfButtons) switch
            {
                (MessageBoxDefaultButton.Button1, 1) => button3,
                (MessageBoxDefaultButton.Button2, 1) => throw new InvalidDataException(),
                (MessageBoxDefaultButton.Button3, 1) => throw new InvalidDataException(),
                (MessageBoxDefaultButton.Button1, 2) => button2,
                (MessageBoxDefaultButton.Button2, 2) => button3,
                (MessageBoxDefaultButton.Button3, 2) => throw new InvalidDataException(),
                (MessageBoxDefaultButton.Button1, 3) => button1,
                (MessageBoxDefaultButton.Button2, 3) => button2,
                (MessageBoxDefaultButton.Button3, 3) => button3,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid MessageBoxDefaultButton"),
            };
            button.Select();
        }
    }

    public DarkThemeMessageBox(bool darkMode)
    {
        if (darkMode)
        {
            HandleCreated += DarkThemeManager.FormHandleCreated;
        }

        InitializeComponent();
        ApplyDarkTheme(darkMode);

        Shown += (sender, e) =>
        {
            if (Owner != null)
            {
                CenterToParent();
            }
            else
            {
                CenterToScreen();
            }
        };
    }

    private void ApplyDarkTheme(bool darkMode)
    {
        if (darkMode)
        {
            BackColor = DarkThemeManager.DarkBackColor;
            ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatStyle = FlatStyle.Flat;
            button3.FlatStyle = FlatStyle.Flat;
        }
    }

    private static void ActivateButton(Button button, DialogResult result)
    {
        button.Visible = true;
        button.Text = buttonTexts[result];
        button.DialogResult = result;
    }

    private void UpdateIconPosition()
    {
        int textCenterY = messageLabel.Location.Y + messageLabel.Size.Height / 2;
        int iconTargetY = textCenterY - iconPicture.Size.Height / 2;
        iconPicture.Location = new(iconPicture.Location.X, iconTargetY);
    }

    private void AboutForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }
}
