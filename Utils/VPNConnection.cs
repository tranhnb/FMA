using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Ras;
using System.Threading;
using System.IO;
using NLog;

namespace Utils
{
    class MyAsyncInfo
    {
        public Byte[] ByteArray { get; set; }
        public Stream MyStream { get; set; }

        public MyAsyncInfo(Byte[] array, Stream stream)
        {
            ByteArray = array;
            MyStream = stream;
        }
    }

    public class VPNConnectionEventArgs : EventArgs
    {
        private int _processId;
        /// <summary>
        /// VPN Connection Process ID
        /// </summary>
        public int processId
        {
            get
            {
                return _processId;
            }
        }

        public VPNConnectionEventArgs(int processId = 0)
            : base()
        { 
            this._processId = processId;
        }
    }

    /// <summary>
    /// Handle VPN connection
    /// </summary>
    public class VPNConnection
    {
        #region Variable and Properties
        
        public event EventHandler OnConnected;
        public event EventHandler OnError;
        public event EventHandler OnClosed;

        private const string OPENVPN_DIRECTORY = @"C:\Program Files\OpenVPN\config\";
        private Process process;
        private Byte[] buffer = new Byte[1024];
        private VPNConfInfoList vpnConfInfoList = new VPNConfInfoList(OPENVPN_DIRECTORY);
        private const bool IsFakeVPN = false;

        private Logger logger = LogManager.GetCurrentClassLogger();
        #endregion Variable and Properties

        #region Constructor
        
        public VPNConnection()
        {
            this.process = CreateShellProcess();
        }

        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// Connect to VPN. Handle the OnConnected Event to determine the connection is established.
        /// </summary>
        /// <param name="openVPNFile">OpenVPN configuration file</param>
        public void Connect(bool isFakeVPN)
        {
            //Get the next OpenVPN Configuration Files
            if (isFakeVPN)
            {
                OnConnected(null, new VPNConnectionEventArgs());
            }
            else {
                string openVPNFile = vpnConfInfoList.LoadNext();
                logger.Info(string.Format("Start open a tunnel, config file: {0}", openVPNFile));
                process.StartInfo.WorkingDirectory = OPENVPN_DIRECTORY;
                process.StartInfo.Arguments = string.Format("--config \"{0}\"", openVPNFile);
                process.Start();
                try
                {
                    Byte[] myByteArray = new Byte[1024];
                    process.StandardOutput.BaseStream.BeginRead(myByteArray, 0, myByteArray.Length,
                      ReadAsyncCallback, new MyAsyncInfo(myByteArray, process.StandardOutput.BaseStream));
                }
                catch (IOException)
                {
                    process.StandardOutput.BaseStream.Close();
                }
                process.WaitForExit();
            }
            
        }

        /// <summary>
        /// Close VPN connection
        /// </summary>
        public bool Close(int processId)
        {
            int _processId = processId;
            logger.Info(string.Format("Start close VPN connection: {0}, ProcessId: {1}", this.process.StartInfo.Arguments, _processId));
            Process process = Process.GetProcessById(_processId);
            try
            {
                process.Kill();
            }
            catch (Exception ex) {
                //TODO Log error
                logger.Error(ex.Message);
                return false;
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Create process for shell command
        /// </summary>
        /// <returns></returns>
        private Process CreateShellProcess(string fileName = "openvpn.exe")
        {
            Process p = new Process();
            p.StartInfo.FileName = fileName;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.LoadUserProfile = true;
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.Exited += new EventHandler(p_Exited);
            return p;
        }

        /// <summary>
        /// Read OpenVPN Stream
        /// </summary>
        /// <param name="ar"></param>
        private void ReadAsyncCallback(IAsyncResult ar)
        {
            MyAsyncInfo info = ar.AsyncState as MyAsyncInfo;

            int amountRead = 0;
            try
            {
                amountRead = info.MyStream.EndRead(ar);
            }
            catch (IOException)
            {
                info.MyStream.Close();
                return;
            }

            string text = Encoding.UTF8.GetString(info.ByteArray, 0, amountRead);
            if (IsConnected(text))
            {
                if (OnConnected != null)
                {
                    OnConnected(null, new VPNConnectionEventArgs(this.process.Id));
                }
            }
            else if (IsError(text))
            {
                if (OnError != null)
                {
                    OnError(null, new VPNConnectionEventArgs());
                }
            }

            try
            {
                info.MyStream.BeginRead(info.ByteArray, 0,
                  info.ByteArray.Length, ReadAsyncCallback, info);
            }
            catch (IOException)
            {
                info.MyStream.Close();
            }

        }

        /// <summary>
        /// Check the StartInfo response data to determine we are connected to a VPN
        /// Success connected data:
        ///     eded len=0 ret=1 a=0 u/d=up
        ///     Tue Jun 24 22:13:25 2014 us=31250 C:\WINDOWS\system32\route.exe ADD 212.98.168.40 MASK 255.255.255.255 192.168.1.1
        ///     Tue Jun 24 22:13:25 2014 us=31250 env_block: add PATH=C:\Windows\System32;C:\WINDOWS;C:\WINDOWS\System32\Wbem
        ///     Tue Jun 24 22:13:25 2014 us=46875 C:\WINDOWS\system32\route.exe ADD 0.0.0.0 MASK 128.0.0.0 10.101.192.1
        ///     Tue Jun 24 22:13:25 2014 us=46875 env_block: add PATH=C:\Windows\System32;C:\WINDOWS;C:\WINDOWS\System32\Wbem
        ///     Tue Jun 24 22:13:25 2014 us=78125 C:\WINDOWS\system32\route.exe ADD 128.0.0.0 MASK 128.0.0.0 10.101.192.1
        ///     Tue Jun 24 22:13:25 2014 us=78125 env_block: add PATH=C:\Windows\System32;C:\WINDOWS;C:\WINDOWS\System32\Wbem
        ///     Tue Jun 24 22:13:25 2014 us=93750 Initialization Sequence Completed
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool IsConnected(string data)
        {
            return !string.IsNullOrEmpty(data) && data.Contains("Initialization Sequence Completed");
        }

        /// <summary>
        /// Check the StartInfo response data to determine we can't connect to a VPN
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool IsError(string data)
        {
            return !string.IsNullOrEmpty(data) && data.Contains("Exiting due to fatal error");
        }

        #endregion Private Methods

        #region Event

        void p_Exited(object sender, EventArgs e)
        {
            if (OnClosed != null)
                OnClosed(null, null);
        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OnError != null)
                OnError(null, null);

        }

        #endregion Event

    }
}

