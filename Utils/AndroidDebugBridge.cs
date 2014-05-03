using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;

namespace Utils
{
    public class AndroidDebugBridge
    {
        #region Variable and Properties

        private const string CONNECTED = "already connected";
        private const string SHELL_COMMAND = "shell";
        private const string LAST_MODIFIED_APK_COMMAND = "ls -l /data/app/ | sort -k5.1n,5.4n -k5.6n,5.7n -k5.9n,5.10n | head -n 1";

        #endregion
        /// <summary>
        /// Connect to an Guest AndroidX86
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns>True: If connected otherwise False</returns>
        public bool Connect(IPAddress ipAddress, int port)
        {
            Process p = CreateShellProcess();
            p.StartInfo.Arguments = string.Format("connect {0}:{1}", ipAddress.ToString(), port);
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output.Contains(CONNECTED);
        }

        /// <summary>
        /// Get newest install application
        /// </summary>
        /// <returns></returns>
        public string FindNewestInstalledApp()
        {
            Process p = CreateShellProcess();
            p.StartInfo.Arguments = string.Format("{0} \"{1}\"", SHELL_COMMAND, LAST_MODIFIED_APK_COMMAND);
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return string.Empty;
        }

        /// <summary>
        /// Open application by packageName and activityName
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="activityName"></param>
        /// <returns>True if open successfully otherwise False</returns>
        public bool OpenApplication(string packageName, string activityName)
        { 
            //TODO
            return true;
        }

        /// <summary>
        /// Create process for shell command
        /// </summary>
        /// <returns></returns>
        private Process CreateShellProcess()
        {
            Process p = new Process();
            p.StartInfo.FileName = @"adb.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            return p;
        }
    }
}
