using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualBox;
using System.IO;
using System.Drawing;

namespace Utils
{
    public class CaptureScreen : ICaptureScreen
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

        /// <summary>
        /// Return PNG byte array of screenshot
        /// </summary>
        /// <param name="display"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns>byte[]</returns>
        public byte[] TakeScreenShot(IDisplay display, int offsetX, int offsetY, int image_width, int image_height)
        {
            uint width, height, bitsPerPixel;
            int aXOrigin, aYOrigin;

            display.GetScreenResolution(0, out width, out height, out bitsPerPixel, out aXOrigin, out aYOrigin);
    
            byte[] byteArray = display.TakeScreenShotPNGToArray(0, width, height);

            return SliceImage(byteArray, offsetX, offsetY, image_width, image_height);
            
        }

        /// <summary>
        /// Slice SubImage
        /// </summary>
        /// <param name="imageByte"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="image_width"></param>
        /// <param name="image_height"></param>
        /// <returns></returns>
        private byte[] SliceImage(byte[] imageByte, int offsetX, int offsetY, int image_width, int image_height)
        {
            ImageConverter ic = new ImageConverter();
            Image img = (Image)ic.ConvertFrom(imageByte);
            Rectangle cloneRect = new Rectangle(offsetX, offsetY, image_width, image_height);
            Bitmap sourceImage = new Bitmap(img);
            Bitmap childImage = sourceImage.Clone(cloneRect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            sourceImage = null;
            img = null;

            return (byte[])ic.ConvertTo(childImage, typeof(byte[]));
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;

        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public string TestDataPath
        {
            get
            {
                return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            }
        }
    }
}
