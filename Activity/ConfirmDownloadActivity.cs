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
    /// Confirm download app on FMA cofirmation screen.
    /// Check this screen at {Project Automation}/Images/Template/FreeMyApp_Step2.png
    /// </summary>
    class ConfirmDownloadActivity : Activity
    {
        public ConfirmDownloadActivity(GuestInformation guestInfo, int mousePositionX, int mousePositionY)
            : base(guestInfo, mousePositionX, mousePositionY)
        {

        }

        protected override bool IsMatchCriteria()
        {
            Image img = this.CaptureScreen(370, 270, 85, 42);
            Guid guid = Guid.NewGuid();

            string filePath = string.Format(@"{0}\Temp\{1}.png", Directory.GetCurrentDirectory(), guid.ToString());
            img.Save(filePath);
            byte[] hash1 = ImageUtils.Sha256HashFile(filePath);

            string templateFilePath = string.Format(@"{0}\Images\Template\{1}", Directory.GetCurrentDirectory(), "Confirm_Download_App.png");
            byte[] hash2 = ImageUtils.Sha256HashFile(templateFilePath);

            //Compare this image with Refresh Image template
            bool isMatch = hash1.SequenceEqual(hash2);
            File.Delete(filePath);
            img = null;

            return isMatch;

        }
    }
}
