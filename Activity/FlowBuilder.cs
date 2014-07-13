using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Activity.Activities;

namespace Activity
{
    /// <summary>
    /// Define a list of sequence activities
    /// </summary>
    public class FlowBuilder
    {
        /// <summary>
        /// Create an Activity by Name
        /// </summary>
        /// <param name="guestInformation"></param>
        /// <returns></returns>
        public static IActivity CreateActivity(ActivityType activityName, GuestInformation guestInformation)
        {
            return Activity.CreateActivity(activityName, guestInformation);

        }
        public static List<IActivity> CreateFlow(GuestInformation guestInformation)
        {
            List<IActivity> flow = new List<IActivity>();
            foreach (ActivityType activityName in Enum.GetValues(typeof(ActivityType)))
            {
                flow.Add(Activity.CreateActivity(activityName, guestInformation));
            }
            return flow;
        }
    }
}
