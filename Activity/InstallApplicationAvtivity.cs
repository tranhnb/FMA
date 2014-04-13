using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    /// <summary>
    /// Install an application from google play detail screen
    /// It send mouse click to Install button on Google Play the watch out when the installation process
    /// </summary>
    class InstallApplicationAvtivity : Activity
    {
        public InstallApplicationAvtivity(int mousePositionX, int mousePositionY) : base(mousePositionX, mousePositionY)
        {
            this._ActivityName = Constants.ActivityName.INSTALL_APPLICATION;
        }   
    }
}

