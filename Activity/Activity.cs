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

        /// <summary>
        /// Contains list of ActivtyName and Mouse Coordinates. Defined in MousePosition.xml.
        /// </summary>
        private static MousePositionList mousePositionList;

        #endregion Variable and Property

        #region Constructor

        /// <summary>
        /// Static constructor
        /// </summary>
        static Activity() 
        {
            mousePositionList = new MousePositionList();
        }

        /// <summary>
        /// Activity Instance Constructor
        /// </summary>
        /// <param name="mousePositionX"></param>
        /// <param name="mousePositionY"></param>
        public Activity(int mousePositionX, int mousePositionY, IMouse Mouse)
        {
            // TODO: Complete member initialization
            this.Mouse = Mouse;
            this.MousePositionX = mousePositionX;
            this.MousePositionY = mousePositionY;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Create Activity Instance by ActivityName
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="Mouse"></param>
        /// <param name="mousePosX">Define a runtime Mouse Position X. Applied when work with control(button) has dynamic position. This is optional parameter</param>
        /// <param name="mousePosY">Like mousePosX. This is optional parameter</param>
        /// <returns></returns>
        public static IActivity CreateActivity(string activityName, IMouse Mouse, int mousePosX = -1, int mousePosY = -1)
        {
            //Assign FunctionName, Mouse.X and Mouse.Y
            MousePosition mousePosition = mousePositionList[activityName] as MousePosition;
            
            if (mousePosition != null)
            {
                int x = mousePosX != -1 ? mousePosX : mousePosition.X;
                int y = mousePosY != -1 ? mousePosY : mousePosition.Y;

                switch (activityName)
                {
                    case Constants.ActivityName.LAUNCH_FREE_MY_APPS:
                        return new LaunchFreeMyAppActivity(x, y, Mouse);

                    case Constants.ActivityName.INSTALL_APPLICATION:
                        return new InstallApplicationAvtivity(x, y, Mouse);

                    case Constants.ActivityName.ACCEPT_INSTALLATION:
                        return new AcceptInstallationActivity(x, y, Mouse);

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
            this.Mouse.PutMouseEventAbsolute(this.MousePositionX, this.MousePositionY, 0, 0, 0x001);
            this.Mouse.PutMouseEventAbsolute(this.MousePositionX, this.MousePositionY, 0, 0, 0x000);

          
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

