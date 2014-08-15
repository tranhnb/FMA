using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualBox;
using MouseCoordinates;
using System.Drawing;
using Utils;
using System.Net;
using Activity.Activities;
using System.Threading;
using NLog;


namespace Activity
{
    public abstract class Activity : IActivity
    {
        #region Variable and Property

        private const int LEFT_MOUSE_CLICK = 0x001;
        private const int LEFT_MOUSE_RELEASE = 0x000;
        protected Logger logger = LogManager.GetCurrentClassLogger();

        //Delay time after each activity excution (in milisecond);
        protected int delay_time = 60 * 1000;

        protected ActivityType _ActivityType;

        public ActivityType ActivityType
        {
            get {
                return _ActivityType;
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
        /// Communicate with android virtual machine
        /// </summary>
        protected AndroidDebugBridge androidDebugBridge = null;
        protected ImageUtils imageUtils = null;
        /// <summary>
        /// Virtual machine information
        /// </summary>
        private GuestInformation GuestInformation;

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
        /// Empty constructor. It's used for Activity doesn't handle mouse click.
        /// </summary>
        public Activity() 
        { 

        }
        /// <summary>
        /// Activity Instance Constructor
        /// </summary>
        /// <param name="mousePositionX"></param>
        /// <param name="mousePositionY"></param>
        public Activity(GuestInformation guestInfo, int mousePositionX, int mousePositionY)
        {
            this.GuestInformation = guestInfo;
            this.MousePositionX = mousePositionX;
            this.MousePositionY = mousePositionY;
            this.androidDebugBridge = new AndroidDebugBridge(IPAddress.Parse(guestInfo.IPAddress), guestInfo.port);
            this.imageUtils = new ImageUtils();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create Activity Instance by ActivityName
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="guestIPAddress"></param>
        /// <param name="port"></param>
        /// <param name="Mouse"></param>
        /// <param name="Display"></param>
        /// <param name="mousePosX">Define a runtime Mouse Position X. Applied when work with control(button) has dynamic position. This is optional parameter</param>
        /// <param name="mousePosY">Like mousePosX. This is optional parameter</param>
        /// <returns></returns>
        public static IActivity CreateActivity(ActivityType activityName, GuestInformation guestInfo, int mousePosX = -1, int mousePosY = -1)
        {
            //Assign FunctionName, Mouse.X and Mouse.Y
            MousePosition mousePosition = mousePositionList[activityName.ToString()] as MousePosition;
            
            
            if (mousePosition != null)
            {
                int x = mousePosX != -1 ? mousePosX : mousePosition.X;
                int y = mousePosY != -1 ? mousePosY : mousePosition.Y;

                switch (activityName)
                {
                    case ActivityType.LAUNCH_FREE_MY_APPS:
                        return new LaunchFreeMyAppActivity(guestInfo, x, y)
                        { 
                            _ActivityType = ActivityType.LAUNCH_FREE_MY_APPS,
                        };

                    case ActivityType.INSTALL_APPLICATION:
                        return new InstallApplicationAvtivity(guestInfo, x, y)
                        {
                            _ActivityType = ActivityType.INSTALL_APPLICATION
                        };

                    case ActivityType.ACCEPT_INSTALLATION:
                        return new AcceptInstallationActivity(guestInfo, x, y)
                        {
                            _ActivityType = ActivityType.ACCEPT_INSTALLATION
                        };

                    case ActivityType.REFRESH_FREE_MY_APP:
                        return new RefreshActivity(guestInfo, x, y)
                        {
                            _ActivityType = ActivityType.REFRESH_FREE_MY_APP
                        };

                    case ActivityType.DETERMINE_APPLICATION:
                        return new DetermineApplicationActivity(guestInfo, x, y)
                        {
                            _ActivityType = ActivityType.DETERMINE_APPLICATION
                        };

                    case ActivityType.CONFIRM_DOWNLOAD:
                        return new ConfirmDownloadActivity(guestInfo, x, y)
                        {
                            _ActivityType = ActivityType.CONFIRM_DOWNLOAD
                        };

                    case ActivityType.CONFIRM_USING_PLAYSTORE:
                        return new ConfirmUsingPlayStoreActivity(guestInfo, x, y)
                        {
                            _ActivityType = ActivityType.CONFIRM_USING_PLAYSTORE
                        };
                        
                    default:
                        throw new Exception("Not implementation");

                }
            }
            else {
                throw new Exception(activityName + " is not defined.");
            }
            
            
        }

        /// <summary>
        /// Init variable
        /// </summary>
        public virtual void Init()
        {
            this.InitPoint();
        }

        /// <summary>
        /// Check the current status is matched criteria for starting an activity
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsMatchCriteria()
        {
            return true;
        }

        /// <summary>
        /// Start activity by send Mouse click event to virtual machine to start an activity or do something else.
        /// </summary>
        public virtual ActivityResult Start()
        {
            try
            {
                Init();
                logger.Info(string.Format("{0} started", this.ActivityType.ToString()));
                if (IsMatchCriteria())
                {
                    ActivityResult result = DoProc();
                    Thread.Sleep(this.delay_time);
                    return result;
                }
                else
                {
                    return new ActivityResult(false, string.Format("{0} doesn't match criteria", this.GetType().Name));
                }

            }
            catch (Exception ex) {
                throw ex;
                
            }

        }

        /// <summary>
        /// Start the activity. The inherited class can override this method if in-need.
        /// </summary>
        protected virtual ActivityResult DoProc()
        {
            this.GuestInformation.Mouse.PutMouseEventAbsolute(this.MousePositionX, this.MousePositionY, 0, 0, LEFT_MOUSE_CLICK);
            this.GuestInformation.Mouse.PutMouseEventAbsolute(this.MousePositionX, this.MousePositionY, 0, 0, LEFT_MOUSE_RELEASE);
            return new ActivityResult(true, string.Empty);
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

        /// <summary>
        /// Take a screenshot of entrie screen
        /// </summary>
        /// <returns></returns>
        protected Image CaptureScreen()
        {
            //Take screenshot
            CaptureScreen screen = new CaptureScreen();
            byte[] byteArray = screen.TakeScreenShot(this.GuestInformation.Display);

            return ImageFromByteArray(byteArray);
        }

        /// <summary>
        /// Take a part of screen shot
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="image_width"></param>
        /// <param name="image_height"></param>
        /// <returns></returns>
        protected Image CaptureScreen(int offsetX, int offsetY, int image_width, int image_height)
        {
            //Take screenshot
            CaptureScreen screen = new CaptureScreen();
            byte[] byteArray = screen.TakeScreenShot(this.GuestInformation.Display, offsetX, offsetY, image_width, image_height);
            return ImageFromByteArray(byteArray);
            
        }

        private Image ImageFromByteArray(byte[] byteArray)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }


        /// <summary>
        /// Find point to send mouse click
        /// </summary>
        /// <returns></returns>
        protected virtual void InitPoint()
        {

        }
        
        

        #endregion Methods

    }
}

