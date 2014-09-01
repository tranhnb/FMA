using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using VirtualBox;

namespace Utils
{
    public interface ICaptureScreen
    {
        byte[] TakeScreenShot(IDisplay display, int offsetX, int offsetY, int image_width, int image_height);
    }
}
