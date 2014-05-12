using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    /// <summary>
    /// Define a list of sequence activities
    /// </summary>
    public class FlowBuilder
    {
        private static string[] activityNameInOrder = new string[7]{
            Constants.ActivityName.LAUNCH_FREE_MY_APPS,
            Constants.ActivityName.DETERMINE_APPLICATION,
            Constants.ActivityName.REFRESH_FREE_MY_APP,
            Constants.ActivityName.CONFIRM_DOWNLOAD,
            Constants.ActivityName.CONFIRM_USING_PLAYSTORE,
            Constants.ActivityName.INSTALL_APPLICATION,
            Constants.ActivityName.ACCEPT_INSTALLATION,
        };

            
        public static List<IActivity> CreateFlow(VirtualBox.IMouse guestMouse, VirtualBox.IDisplay guestDisplay)
        {
            List<IActivity> flow = new List<IActivity>();
            for (int i = 0; i < activityNameInOrder.Length; i++)
            {
                flow.Add(Activity.CreateActivity(activityNameInOrder[i], guestMouse, guestDisplay));
            }
            
            return flow;

        }
    }
}
