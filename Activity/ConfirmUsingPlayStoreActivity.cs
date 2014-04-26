using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Utils;

namespace Activity
{
    /// <summary>
    /// Confirm using Play Store to download application
    /// Check this screen at {Project Automation}/Images/Template/FreeMyApp_Step3.png
    /// </summary>
    class ConfirmUsingPlayStoreActivity: Activity
    {
        public ConfirmUsingPlayStoreActivity(int mousePositionX, int mousePositionY, VirtualBox.IMouse Mouse, VirtualBox.IDisplay Display)
            : base(mousePositionX, mousePositionY, Mouse, Display)
        {

        }

        protected override bool IsMatchCriteria()
        {
            Image img = this.CaptureScreen(490, 250, 85, 60);
            Guid guid = Guid.NewGuid();

            string filePath = string.Format(@"{0}\Temp\{1}.png", Directory.GetCurrentDirectory(), guid.ToString());
            img.Save(filePath);
            byte[] hash1 = ImageUtils.Sha256HashFile(filePath);

            string templateFilePath = string.Format(@"{0}\Images\Template\{1}", Directory.GetCurrentDirectory(), "ConfirmUsingGoolePlay.png");
            byte[] hash2 = ImageUtils.Sha256HashFile(templateFilePath);

            //Compare this image with Refresh Image template
            bool isMatch = hash1.SequenceEqual(hash2);
            File.Delete(filePath);
            img = null;

            return isMatch;

        }
    }
}
