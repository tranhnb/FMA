using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using VirtualBox;
using Activity;
using NLog;
using Utils;

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
        //Check wether a miner process finised on a tunnel
        private bool isFinishedOnATunnel = false;
        private Thread digThread;

        //List of activity for GiftCardMiner
        private List<IActivity> activityList;

        //Last Activity Has been done
        private IActivity lastActivity;

        //Miner stops his job for a while ^_^
        private int tea_break_time = 1 * 60 * 1000; //Miliseconds

        //Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();


        //VPN connection
        private VPNConnection vpnConnection = new VPNConnection();
        //How many times did the miner refresh FMA.
        private int fma_refresh_times = 0;
        private const int MAX_FMA_REFRESH_TIMES = 10;

        //Use to start a fake 
        private bool isFakeVPN = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["FakeVPN"]);

        public IDisplay DisplayController
        {
            get {
                return this.machineSession.Console.Display;
            }
        }
        #endregion Variable and Properties

        #region Constructor

        public GiftCardMiner(string virtualMachineName, IPAddress guestIPAddress, int port)
        {        
            //Connect to Guest Virtual Machine
            ConnectGuest(virtualMachineName, guestIPAddress.ToString(), port);
            InitFlow();
            //VPN connection event handler
            vpnConnection.OnConnected += new EventHandler(vpnConnection_OnConnected);
            vpnConnection.OnError += new EventHandler(vpnConnection_OnError);
        }

        #endregion Constructor

        #region Method

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
            else
            {
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
        /// Start determine, download and open application thread
        /// </summary>
        public void Start()
        {
            digThread = new Thread(OpenTunnel);
            digThread.Priority = ThreadPriority.BelowNormal;
            digThread.Start();
        }

        /// <summary>
        /// Force stop dig thread
        /// </summary>
        public void StopDig()
        {
            isRequestStop = true;
        }

        public void FinishOnATunnel() {
            isFinishedOnATunnel = true;
        }

        /// <summary>
        /// Suspend thread for a while
        /// </summary>
        public void StartTeaBreak()
        {
            Thread.Sleep(tea_break_time);
        }


        /// <summary>
        /// Setup a tunnel by make a VPN connection
        /// </summary>
        private void OpenTunnel()
        {
            //If we still can't determine new application in a specific times 
            //then connect to another VPN
            if (fma_refresh_times == 0 || this.IsExceedMaxTimes())
            {
                vpnConnection.Connect(isFakeVPN);
            }
            else
            {
                vpnConnection_OnConnected(null, null);
            }
        }

        /// <summary>
        /// Start mining flow.
        /// </summary>
        private void Dig()
        {
            while (!isRequestStop && !isFinishedOnATunnel && !IsExceedMaxTimes())
            {
                logger.Info("Start Dig");
                try
                {
                    foreach (IActivity activity in this.activityList)
                    {
                        if (isRequestStop)
                            break;
                        else
                        activity.Start();
                    }
                }
                //No application to download
                catch (NoApplicationException ex)
                {
                    logger.Warn("NoApplicationException");
                    fma_refresh_times++;
                    StartTeaBreak();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    FinishOnATunnel();
                }
            }

            if (isRequestStop)
            {
                logger.Info("Miner is forced to stop");
            }
            // Otherwis Start dig by a new tunnel
            else if ( isFinishedOnATunnel || IsExceedMaxTimes() )
            {
                OpenTunnel();
            }
        }

        /// <summary>
        /// Check wether fma_refresh_times reach the MAX_FMA_REFRESH_TIMES
        /// </summary>
        /// <returns></returns>
        private bool IsExceedMaxTimes()
        {
            return fma_refresh_times >= MAX_FMA_REFRESH_TIMES;
        }
        #endregion Method


        #region EventHandler

        void vpnConnection_OnError(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void vpnConnection_OnConnected(object sender, EventArgs e)
        {
            Dig();
        }

        
        #endregion
    }
}



