using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using Utils;

namespace Activity
{
    /// <summary>
    /// Refresh free my app activity
    /// </summary>
    class RefreshActivity : Activity
    {
        
        public RefreshActivity(int mousePositionX, int mousePositionY, VirtualBox.IMouse Mouse, VirtualBox.IDisplay Display)
            : base(mousePositionX, mousePositionY, Mouse, Display)
        {
        }

        protected override bool IsMatchCriteria()
        {
            Image img = this.CaptureScreen(380, 230, 85, 25);
            //Image img = this.CaptureScreen();
            Guid guid = Guid.NewGuid();

            string filePath = string.Format(@"{0}\Temp\{1}.png", Directory.GetCurrentDirectory(), guid.ToString());
            img.Save(filePath);
            byte[] hash1 = ImageUtils.Sha256HashFile(filePath);

            string templateFilePath = string.Format(@"{0}\Images\Template\{1}", Directory.GetCurrentDirectory(), "Refresh.png");
            byte[] hash2 = ImageUtils.Sha256HashFile(templateFilePath);

            //Compare this image with Refresh Image template
            bool isMatch = hash1.SequenceEqual(hash2);
            File.Delete(filePath);
            img = null;

            return isMatch;
            
        }

        
    }
}

