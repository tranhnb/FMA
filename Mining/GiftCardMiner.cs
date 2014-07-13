using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using VirtualBox;
using Activity;
using NLog;

namespace Mining
{
    /// <summary>
    /// Download, open application, get GiftCard from FreeMyApp. Each GiftCardMiner handle a virtual machine ^__^.
    /// </summary>
    public class GiftCardMiner
    {
        #region Variable and Properties

        //------------------------ VirtualBox information------------------------//
        private VirtualBoxClient vBoxClient;
        private IMachine machine;
        private Session machineSession;
        private GuestInformation guestInfo;
        //---------------------- End VirtualBox information----------------------//

        private bool isRequestStop = false;
        private Thread digThread;

        //List of activity for GiftCardMiner
        private List<IActivity> activityList;

        //Last Activity Has been done
        private IActivity lastActivity;

        //Miner stops his job for a while ^_^
        private int tea_break_time = 1 * 60 * 1000; //Miliseconds

        //Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion Variable and Properties

        #region Constructor

        public GiftCardMiner(string virtualMachineName, IPAddress guestIPAddress, int port)
        {        
            //Connect to Guest Virtual Machine
            ConnectGuest(virtualMachineName, guestIPAddress.ToString(), port);
            InitFlow();
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// Start determine, download and open application thread
        /// </summary>
        public void StartDig()
        {
            digThread = new Thread(Dig);
            digThread.Priority = ThreadPriority.BelowNormal;
            digThread.Start();
        }

        /// <summary>
        /// Stop dig thread
        /// </summary>
        public void StopDig()
        {
            isRequestStop = true;
        }

        /// <summary>
        /// Suspend thread for a while
        /// </summary>
        public void StartTeaBreak()
        {
            Thread.Sleep(tea_break_time);
        }

        /// <summary>
        /// Connect to Guest machine
        /// </summary>
        private void ConnectGuest(string virtualMachineName, string guestIPAddress, int port)
        {
            vBoxClient = new VirtualBoxClient();
            machine = vBoxClient.VirtualBox.FindMachine(virtualMachineName);
            if (machine != null)
            {
                machineSession = vBoxClient.Session;
                machine.LockMachine(machineSession, LockType.LockType_Shared);
                this.guestInfo = new GuestInformation()
                {
                    Name = virtualMachineName,
                    Mouse = machineSession.Console.Mouse,
                    Display = machineSession.Console.Display,
                    IPAddress = guestIPAddress,
                    port = port
                };
            }
            else {
                throw new Exception("Can't connect to guest: " + virtualMachineName);
            }
            
        }

        /// <summary>
        /// Create a flow of activities
        /// </summary>
        private void InitFlow()
        {
            activityList = Activity.FlowBuilder.CreateFlow(guestInfo);
        }
        /// <summary>
        /// Start mining flow.
        /// </summary>
        private void Dig()
        {
            while (!isRequestStop)
            {
                logger.Info("Start Dig");
                try
                {
                    foreach (IActivity activity in this.activityList)
                    {
                        activity.Start();
                    }
                }
                //No application to download
                catch (NoApplicationException ex)
                {
                    StartTeaBreak();
                    //Keep hard working Mine mine mine mine FOREVER ^_^
                    Dig();
                }
                catch (Exception ex)
                {
                    StopDig();
                    logger.Error(ex.Message);
                }
                
            }
        }

        #endregion Method
    }
}



