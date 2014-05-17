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
    /// Determine FMA/... has any application to install
    /// </summary>
    class DetermineApplicationActivity: Activity
    {
        public DetermineApplicationActivity(GuestInformation guestInfo, int mousePositionX, int mousePositionY)
            : base(guestInfo, mousePositionX, mousePositionY)
        {

        }

        protected override bool IsMatchCriteria()
        {
            Image img = this.CaptureScreen(349, 143, 104, 33);
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

            //If we are on FMA and the refresh button doesn't display so FMA has applications to be installed
            return !isMatch;

        }
    }
}
