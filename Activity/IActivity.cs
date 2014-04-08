using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MouseCoordinates;

namespace Activity
{
    public interface IActivity
    {
        /// <summary>
        /// Send Mouse click event to virtual machine to start an activity
        /// </summary>
        void Start();

        /// <summary>
        /// Check an Activity is executed success or not
        /// </summary>
        void Finish();

        /// <summary>
        /// Activity is finished
        /// </summary>
        void End();
    }
}
