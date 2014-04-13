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
        public LaunchFreeMyAppActivity(int mousePositionX, int mousePositionY) : base(mousePositionX, mousePositionY)
        {
            this._ActivityName = Constants.ActivityName.LAUNCH_FREE_MY_APPS;
        }
    }
}
