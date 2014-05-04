using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace Utils
{
    public class AndroidDebugBridge
    {
        #region Variable and Properties

        private const string CONNECTED = "already connected";
        private const string SHELL_COMMAND = "shell";
        private const string GUEST_INSTALLED_APP_FOLDER = "/data/app/";
        private const string LAST_MODIFIED_APK_COMMAND = @"ls -l /data/app/ | sort -k5.1n,5.4n -k5.6n,5.7n -k5.9n,5.10n | head -n 1 | tr -s [:space:] ' ' | cut -d\  -f 7";
        private const string PULL_APP_COMMAND = "pull {0} {1}";
        private const string TEMP_FOLDER = "Temp";

        #endregion

        #region Methods
        /// <summary>
        /// Connect to an Guest AndroidX86
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns>Process: If connected otherwise Null</returns>
        public Process Connect(IPAddress ipAddress, int port)
        {
            Process p = CreateShellProcess();
            p.StartInfo.Arguments = string.Format("connect {0}:{1}", ipAddress.ToString(), port);
            string result = ExecuteShellCommand(p);
            return result.Contains(CONNECTED) ? p : null;
        }

        /// <summary>
        /// Get newest install application
        /// </summary>
        /// <returns></returns>
        public string FindNewestInstalledApp(Process process)
        {
            process = CreateShellProcess();
            process.StartInfo.Arguments = string.Format("{0} \"{1}\"", SHELL_COMMAND, LAST_MODIFIED_APK_COMMAND);
            string result = ExecuteShellCommand(process);

            if (!string.IsNullOrEmpty(result) && result.Contains("apk"))
            {
                result = result.Trim();
                return GetLaunchableActivityName(process, result, string.Format("{0}{1}", GUEST_INSTALLED_APP_FOLDER, result));
            }
            else { 

            }
            return string.Empty;
        }

        /// <summary>
        /// Open application by packageName and activityName
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="activityName"></param>
        /// <returns>True if open successfully otherwise False</returns>
        public bool OpenApplication(Process process, string packageName, string activityName)
        { 
            //TODO
            return true;
        }

        /// <summary>
        /// Get Full launchable Activity Name of an apk file.
        /// </summary>
        /// <param name="guest_apk_path"></param>
        /// <returns></returns>
        private string GetLaunchableActivityName(Process process, string applicationName, string guest_apk_path)
        {
            //Pull the apk file to HOST machine
            string localPath = Path.Combine(
                                Directory.GetCurrentDirectory(),
                                TEMP_FOLDER,
                                string.Format("{0}_{1}.apk", applicationName.Substring(0, applicationName.IndexOf(".apk")),
                                Guid.NewGuid().ToString()));

            process.StartInfo.Arguments = string.Format(PULL_APP_COMMAND, guest_apk_path, localPath);

            string result = ExecuteShellCommand(process);
            string activityName = string.Empty;

            if (File.Exists(localPath))
            {
                //Dump apk file to read manifest xml.
                activityName = dumpAPKFile(process, localPath);
            }

            return activityName;
        }

        /// <summary>
        /// Dump manifest file from apk file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string dumpAPKFile(Process process, string filePath)
        {
            string destinationFolder = Path.Combine(Directory.GetCurrentDirectory(), TEMP_FOLDER, Guid.NewGuid().ToString());
            process.StartInfo.FileName = "java.exe";
            process.StartInfo.Arguments = string.Format(@"-jar Libs\apktool.jar d {0} {1}", filePath, destinationFolder);
            
            string result = ExecuteShellCommand(process);
            return string.Empty;
        }

        /// <summary>
        /// Create process for shell command
        /// </summary>
        /// <returns></returns>
        private Process CreateShellProcess(string fileName = "adb.exe")
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
        /// Execute shell command on guest machine
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private string ExecuteShellCommand(Process process)
        {
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        #endregion

    }
}
