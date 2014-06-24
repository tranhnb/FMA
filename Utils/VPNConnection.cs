using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Ras;
using System.Threading;
using System.IO;

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

    /// <summary>
    /// Handle VPN connection
    /// </summary>
    public class VPNConnection
    {
        private Process process;
        private Byte[] buffer = new Byte[1024];
        public event EventHandler Connected;

        public VPNConnection()
        {
            process = CreateShellProcess();
        }

        /// <summary>
        /// Connect to VPN. Handle the Connected Event to determine the connection is established.
        /// </summary>
        public void Connect()
        {
            process.StartInfo.WorkingDirectory = @"C:\Program Files\OpenVPN\config\";
            process.StartInfo.Arguments = "--config \"inCloak.com Belarus, Minsk.ovpn\"";
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
        }

        /// <summary>
        /// Share VPN connection to VirtualBox Host-Only Network
        /// </summary>
        /// <returns></returns>
        public bool Share()
        {
            RasEntry[] _RasEntrys = RasEntry.GetEntrys();
            
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            return true;
        }

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
                if (Connected != null)
                    Connected(null, null);
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

    }
}
