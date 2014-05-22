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

            string templateFilePath = string.Format(@"{0}\Images\Template\{1}", Directory.GetCurrentDirectory(), "Refresh_1.png");
            byte[] hash2 = ImageUtils.Sha256HashFile(templateFilePath);

            string templateFilePath2 = string.Format(@"{0}\Images\Template\{1}", Directory.GetCurrentDirectory(), "Refresh_2.png");
            byte[] hash3 = ImageUtils.Sha256HashFile(templateFilePath2);

            //Compare this image with Refresh Image template
            bool isMatch1 = hash1.SequenceEqual(hash2);
            bool isMatch2 = hash1.SequenceEqual(hash3);
            File.Delete(filePath);
            img = null;

            //If we are on FMA and the refresh button doesn't display so FMA has applications to be installed
            if (isMatch1 || isMatch2)
                throw new NoApplicationException();
            else
                return true;

        }
    }
}
