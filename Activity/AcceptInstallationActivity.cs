using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    class AcceptInstallationActivity: Activity
    {
        public AcceptInstallationActivity(int mousePositionX, int mousePositionY, VirtualBox.IMouse Mouse)
            : base(mousePositionX, mousePositionY, Mouse)
        {
            this._ActivityName = Constants.ActivityName.ACCEPT_INSTALLATION;
        }
    }
}
