using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MouseCoordinates;
using VirtualBox;

namespace Activity
{
    public interface IActivity
    {
        #region Variables and Properties

        /// <summary>
        /// Virtual Box IMouse Interface
        /// </summary>
        IMouse Mouse { get; set; }

        /// <summary>
        /// ActivityName defined in MouseCoordinates project: MousePosition.xml
        /// </summary>
        string ActivityName { get; }

        /// <summary>
        /// Mouse Postion X: defined in MouseCoordinates project: MousePosition.xml
        /// </summary>
        int MousePositionX { get; set; }

        /// <summary>
        /// Mouse Postion Y: defined in MouseCoordinates project: MousePosition.xml
        /// </summary>
        int MousePositionY { get; set; }

        #endregion

        /// <summary>
        /// Send Mouse click event to virtual machine to start an activity
        /// </summary>
        void Start();

        /// <summary>
        /// Check this activity is finish or on error then send the notification to the ActivityManager
        /// </summary>
        void Finish();

        /// <summary>
        /// Activity is executed successed
        /// </summary>
        void End();
    }
}
