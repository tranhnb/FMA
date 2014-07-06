using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Activity.Publishers;

namespace Activity.Management
{
    /// <summary>
    /// Determine FMA application state: Which screen are we on? What is the next step....
    /// </summary>
    public class TaskMaster
    {
        #region Publishers

        public ActivityPublisher acceptInstallationPublisher = new ActivityPublisher();
        private AcceptInstallationActivity acceptInstallationActivity;

        #endregion Publishers

        #region Constructor

        public TaskMaster() {

            acceptInstallationPublisher.Subscribe(acceptInstallationActivity);
        }

        #endregion Constructor

    }
}
