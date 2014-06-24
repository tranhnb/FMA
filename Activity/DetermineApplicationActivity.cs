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
    /// Check this screen at {Project Automation}/Images/Template/FreeMyApp_Step1.png
    /// </summary>
    ///
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

            string templateFilePath = string.Format(@"{0}\Images\Template\{1}", Directory.GetCurrentDirectory(), "Refresh.png");
            
            //Compare this image with Refresh Image template
            bool isMatch1 = this.imageUtils.IsSubImage(filePath, templateFilePath);
            File.Delete(filePath);
            img = null;

            //If we are on FMA and the refresh button doesn't display so FMA has applications to be installed
            if (isMatch1)
                throw new NoApplicationException();
            else
                return true;

        }
    }
}
