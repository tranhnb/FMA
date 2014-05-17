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
        public LaunchFreeMyAppActivity(GuestInformation guestInfo, int mousePositionX, int mousePositionY)
            : base(guestInfo, mousePositionX, mousePositionY)
        {

        }

        protected override void DoProc()
        {
            //Launch freemyapp application
            if (this.androidDebugBridge.OpenFreeMyApp())
            {

            }
            else {
                throw new LaunchApplicationException();
            }
        }

    }
}
