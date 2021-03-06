﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MouseCoordinates;
using VirtualBox;
using Activity.Activities;

namespace Activity
{
    public interface IActivity
    {
        #region Variables and Properties


        /// <summary>
        /// ActivityName defined in MouseCoordinates project: MousePosition.xml
        /// </summary>
        ActivityType ActivityType { get; }

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
        /// Do initialize before start the activity such as: Capture screen, check the current screen is match Activty working criteria
        /// </summary>
        void Init();

        /// <summary>
        /// Send Mouse click event to virtual machine to start an activity
        /// </summary>
        ActivityResult Start();

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
