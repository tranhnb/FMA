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
        public LaunchFreeMyAppActivity(int mousePositionX, int mousePositionY, VirtualBox.IMouse Mouse, VirtualBox.IDisplay Display)
            : base(mousePositionX, mousePositionY, Mouse, Display)
        {

        }
    }
}
