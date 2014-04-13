using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualBox;
using MouseCoordinates;

namespace Activity
{
    public abstract class Activity : IActivity
    {
        #region Variable and Property

        public IMouse Mouse
        {
            get;
            set;
        }

        protected string _ActivityName;

        public string ActivityName
        {
            get {
                return _ActivityName;
            }
        }

        public int MousePositionX
        {
            get;
            set;
        }

        public int MousePositionY
        {
            get;
            set;
        }

        #endregion Variable and Property

        #region Constructor
        
        public Activity(int mousePositionX, int mousePositionY)
        {
            // TODO: Complete member initialization
            this.MousePositionX = mousePositionX;
            this.MousePositionY = mousePositionY;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Create Activity Instance by ActivityName
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static IActivity CreateActivity(string activityName)
        {
            //Assign FunctionName, Mouse.X and Mouse.Y
            MousePositionList mousePositionList = new MousePositionList();
            MousePosition mousePosition = mousePositionList[activityName] as MousePosition;
            if (mousePosition != null)
            {
                switch (activityName)
                {
                    case Constants.ActivityName.INSTALL_APPLICATION:
                        return new InstallApplicationAvtivity(mousePosition.X, mousePosition.Y);

                    case Constants.ActivityName.LAUNCH_FREE_MY_APPS:
                        return new LaunchFreeMyAppActivity();

                    default:
                        throw new Exception("Not implementation");

                }
            }
            else {
                throw new Exception(activityName + " is not defined.");
            }
            
            
        }

        /// <summary>
        /// Send Mouse click event to virtual machine to start an activity
        /// </summary>
        public virtual void Start()
        {
            
        }

        /// <summary>
        /// Check this activity is finish or on error then send the notification to the ActivityManager
        /// </summary>
        public virtual void Finish()
        {

        }

        /// <summary>
        /// Activity is executed successed
        /// </summary>
        public virtual void End()
        {

        }

        #endregion Methods
    }
}
