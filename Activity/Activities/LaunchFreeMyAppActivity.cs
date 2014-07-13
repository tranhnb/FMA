using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Activity.Activities;

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

        protected override ActivityResult DoProc()
        {
            //Launch freemyapp application
            if (this.androidDebugBridge.OpenFreeMyApp())
            {
                return new ActivityResult(true, string.Empty);
            }
            else {
                return new ActivityResult(false, string.Format("{0} is failed", this.GetType().Name));
            }
        }

    }
}
