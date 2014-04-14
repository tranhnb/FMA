using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    /// <summary>
    /// Launch FreeMyApp application
    /// </summary>
    class LaunchFreeMyAppActivity: Activity
    {
        public LaunchFreeMyAppActivity(int mousePositionX, int mousePositionY, VirtualBox.IMouse Mouse)
            : base(mousePositionX, mousePositionY, Mouse)
        {
            this._ActivityName = Constants.ActivityName.LAUNCH_FREE_MY_APPS;
        }
    }
}
