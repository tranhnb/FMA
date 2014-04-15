using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualBox;
using System.IO;

namespace Utils
{
    public class CaptureScreen
    {
        /// <summary>
        /// Return PNG byte array of screenshot
        /// </summary>
        /// <param name="display"></param>
        /// <returns>byte[]</returns>
        public byte[] TakeScreenShot(IDisplay display) {
            uint width, height, bitsPerPixel;
            int aXOrigin, aYOrigin;
            display.GetScreenResolution(0, out width, out height, out bitsPerPixel, out aXOrigin, out aYOrigin);
            return display.TakeScreenShotPNGToArray(0, width, height);
        }

    }
}
