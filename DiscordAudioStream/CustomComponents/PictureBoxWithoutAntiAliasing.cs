using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomComponents;
public class PictureBoxWithoutAntiAliasing : PictureBox
{
    public bool DisableAntiAliasing { get; set; }

    protected override void OnPaint(PaintEventArgs pe)
    {
        if (DisableAntiAliasing)
        {
            pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }
        base.OnPaint(pe);
    }
}
