using QuickLibrary;
using System.Windows.Forms;

namespace quick_screen_recorder
{
	public class BoundedNumericBox : QlibNumericBox
	{
		public new decimal Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				if (value < base.Minimum) value = base.Minimum;
				else if (value > base.Maximum) value = base.Maximum;
				base.Value = value;

				base.Text = value.ToString();
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.X > base.Width - 18)
			{
				Focus();
				if (e.Y > base.Height / 2)
				{
					Value--;
				}
				else
				{
					Value++;
				}
			}
		}

		protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			//if (e.KeyChar == '\r' || e.KeyChar == '\n')
			//{
			//    Value = decimal.Parse(base.Text);
			//    e.Handled = true;
			//    return;
			//}

			base.OnTextBoxKeyPress(source, e);

			base.Text = Value.ToString();
		}
	}
}
