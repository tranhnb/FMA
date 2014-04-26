using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.IO;

namespace Utils
{

    /// <summary>
    /// Find subImage position by Color pixel
    /// </summary>
    public class ImageUtils
    {
        /// <summary>
        /// Find a color is existed at an specific point
        /// </summary>
        /// <param name="img"></param>
        /// <param name="color"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Point? FindAllPixelLocation(Bitmap img, Color color, Point startPoint, Point endPoint)
        {
            int c = color.ToArgb();
            for (int x = startPoint.X; x <= endPoint.X; x++)
            {
                for (int y = startPoint.Y; y < endPoint.Y; y++)
                {
                    if (c.Equals(img.GetPixel(x, y).ToArgb()))
                    {
                        return new Point(x, y);
                    }
                        
                }
            }
            return null;
        }

        /// <summary>
        /// Hash image file to byte array
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] Sha256HashFile(string file)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (Stream input = File.OpenRead(file))
                {
                    return sha256.ComputeHash(input);
                }
            }
        }
    }



    
}
