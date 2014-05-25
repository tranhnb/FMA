﻿using System;
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

        public RefreshActivity(GuestInformation guestInfo, int mousePositionX, int mousePositionY)
            : base(guestInfo, mousePositionX, mousePositionY)
        {
        }

        protected override bool IsMatchCriteria()
        {
            Image img = this.CaptureScreen(301, 130, 214, 61);
            Guid guid = Guid.NewGuid();

            string filePath = string.Format(@"{0}\Temp\{1}.png", Directory.GetCurrentDirectory(), guid.ToString());
            img.Save(filePath);
            
            string templateFilePath = string.Format(@"{0}\Images\Template\{1}", Directory.GetCurrentDirectory(), "Refresh.png");
            
            //Compare this image with Refresh Image template
            bool isMatch1 = this.imageUtils.IsSubImage(filePath, templateFilePath);
            File.Delete(filePath);
            img = null;

            return isMatch1;
            
        }

        
    }
}

