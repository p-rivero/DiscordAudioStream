﻿
using System.Windows.Forms;

namespace DiscordAudioStream;

public class ShowMessage
{
    private readonly MessageBoxIcon icon;
    private string title;
    private string text = "";
    private bool acceptByDefault;
    private Action? ifYes;
    private Action? ifNo;
    private Action? ifOk;
    private Action? ifCancel;

    private bool HasYesNo => ifYes != null || ifNo != null;
    private bool HasOk => ifOk != null;
    private bool HasCancel => ifCancel != null;

    public static ShowMessage Information()
    {
        return new(MessageBoxIcon.Information, "Information");
    }

    public static ShowMessage Question()
    {
        return new(MessageBoxIcon.Question, "Question");
    }

    public static ShowMessage Warning()
    {
        return new(MessageBoxIcon.Warning, "Warning");
    }

    public static ShowMessage Error()
    {
        return new(MessageBoxIcon.Error, "Error");
    }

    private ShowMessage(MessageBoxIcon icon, string title)
    {
        this.icon = icon;
        this.title = title;
    }

    public ShowMessage Title(string title)
    {
        this.title = title;
        return this;
    }

    public ShowMessage Text(string text)
    {
        if (string.IsNullOrEmpty(this.text))
        {
            this.text = text;
        }
        else
        {
            this.text += "\n";
            this.text += text;
        }
        return this;
    }

    public ShowMessage IfYes(Action action)
    {
        AssertYesNoCancel();
        ifYes = action;
        return this;
    }

    public ShowMessage IfNo(Action action)
    {
        AssertYesNoCancel();
        ifNo = action;
        return this;
    }

    public ShowMessage IfOk(Action action)
    {
        AssertOkCancel();
        ifOk = action;
        return this;
    }

    public ShowMessage IfCancel(Action action)
    {
        ifCancel = action;
        return this;
    }

    public ShowMessage Cancelable()
    {
        ifCancel = () => { };
        return this;
    }

    public ShowMessage AcceptByDefault()
    {
        acceptByDefault = true;
        return this;
    }

    public DialogResult GetResult()
    {
        DialogResult result = ShowMessageBox();
        switch (result)
        {
            case DialogResult.OK:
                ifOk?.Invoke();
                break;
            case DialogResult.Cancel:
                ifCancel?.Invoke();
                break;
            case DialogResult.Yes:
                ifYes?.Invoke();
                break;
            case DialogResult.No:
                ifNo?.Invoke();
                break;
            default:
                break;
        }
        return result;
    }

    public void Show()
    {
        _ = GetResult();
    }

    private DialogResult ShowMessageBox()
    {
        return MessageBox.Show(text, title, GetButtons(), icon, GetDefaultButton());
    }

    private void AssertYesNoCancel()
    {
        if (HasOk)
        {
            throw new InvalidOperationException("Cannot use OK and Yes/No at the same time");
        }
    }

    private void AssertOkCancel()
    {
        if (HasYesNo)
        {
            throw new InvalidOperationException("Cannot use OK and Yes/No at the same time");
        }
    }

    private MessageBoxButtons GetButtons()
    {
        if (HasYesNo)
        {
            return HasCancel ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo;
        }
        else
        {
            return HasCancel ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK;
        }
    }

    private MessageBoxDefaultButton GetDefaultButton()
    {
        if (acceptByDefault)
        {
            return MessageBoxDefaultButton.Button1; // Ok/Yes
        }

        if (HasYesNo && HasCancel)
        {
            return MessageBoxDefaultButton.Button3; // Cancel
        }
        else if (HasYesNo || HasCancel)
        {
            return MessageBoxDefaultButton.Button2; // Cancel/No
        }
        else
        {
            return MessageBoxDefaultButton.Button1; // Ok/Yes
        }
    }
}