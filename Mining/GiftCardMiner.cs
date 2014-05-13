using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using VirtualBox;
using Activity;

namespace Mining
{
    /// <summary>
    /// Download, open application, get GiftCard from FreeMyApp. Each GiftCardMiner handle a virtual machine ^__^.
    /// </summary>
    public class GiftCardMiner
    {
        #region Variable and Properties
        
        private VirtualBoxClient vBoxClient;
        private IMachine machine;
        private Session machineSession;
        
        private GuestInformation guestInfo;
        
        private bool isRequestStop = false;
        private Thread digThread;

        private List<IActivity> activityList;

        #endregion Variable and Properties

        #region Constructor

        public GiftCardMiner(string virtualMachineName, IPAddress guestIPAddress, int port)
        {        
            //Connect to Guest Virtual Machine
            ConnectGuest(virtualMachineName, guestIPAddress.ToString(), port);
            InitFlow();

            digThread = new Thread(Dig);
            digThread.Priority = ThreadPriority.BelowNormal;
            
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// Start determine, download and open application thread
        /// </summary>
        public void StartDig()
        {
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
                Console.WriteLine("Start Dig");

                foreach (IActivity activity in this.activityList)
                {
                    activity.Start();
                }

                Thread.Sleep(1000);
            }
        }

        #endregion Method
    }
}



