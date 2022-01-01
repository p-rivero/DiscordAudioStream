using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quick_screen_recorder
{
	public interface IScreenCaptureMaster
	{
		void GetCaptureArea(out int width, out int height, out int x, out int y);
		bool IsCapturingCursor();
	}
}
