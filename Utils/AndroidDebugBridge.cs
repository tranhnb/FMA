using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Utils
{
    public class AndroidDebugBridge
    {
        #region Variable and Properties

        private const string CONNECTED = "already connected";
        private const string GUEST_INSTALLED_APP_FOLDER = "/data/app/";
        private const string LAST_MODIFIED_APK_COMMAND = "shell \"ls -l /data/app/ | sort -k5.1n,5.4n -k5.6n,5.7n -k5.9n,5.10n | head -n 1 | tr -s [:space:] ' ' | cut -d\\  -f 7 \" ";
        private const string FIND_RUNNING_APPLICATION_BY_NAME_COMMAND = "shell \"ps | grep {0} | tr -s [:space:] ' ' | cut -d\\  -f 9\"";
        private const string STOP_APPLICATION = "shell pm clear {0}";
        private const string PULL_APP_COMMAND = "pull {0} {1}";
        private const string TEMP_FOLDER = "Temp";
        private const string ANDROID_MANIFEST = "AndroidManifest.xml";
        private const string LAUNCH_APPLICATION_COMMAND = "shell \"am start -n {0} \"";
        private const string FREEMYAPP_ACTIVITY_NAME = "com.fiksu.fma.android/com.fiksu.fma.android.MainActivity";

        private Process process = null;
        private string device_ipAddress = string.Empty;
        private int port = 5555;
        
        #endregion

        #region Constructor

        public AndroidDebugBridge(IPAddress ipAddress, int port)
        {
            this.device_ipAddress = ipAddress.ToString();
            this.port = port;
        }

        #endregion Construtor

        #region Methods

        /// <summary>
        /// Connect to an Guest AndroidX86
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns>Process: If connected otherwise Null</returns>
        public bool Connect()
        {
            process = CreateShellProcess();
            process.StartInfo.Arguments = string.Format("connect {0}:{1}", device_ipAddress, port);
            string result = ExecuteShellCommand(process);
            return result.Contains(CONNECTED);
        }

        /// <summary>
        /// Get newest install application
        /// </summary>
        /// <returns></returns>
        public string FindNewestInstalledApp()
        {
            process.StartInfo.Arguments = BuildArgument(LAST_MODIFIED_APK_COMMAND);
            string result = ExecuteShellCommand(process);

            if (!string.IsNullOrEmpty(result) && result.Contains("apk"))
            {
                result = result.Trim();
                return GetLaunchableActivityName(result, string.Format("{0}{1}", GUEST_INSTALLED_APP_FOLDER, result));
            }
            else { 

            }
            return string.Empty;
        }

        /// <summary>
        /// Open FreeMyApp application
        /// </summary>
        /// <returns></returns>
        public bool OpenFreeMyApp()
        {
            return OpenApplication(FREEMYAPP_ACTIVITY_NAME);
        }

        /// <summary>
        /// Open application by packageName and activityName
        /// </summary>
        /// <param name="application_launch_name">In format: package/activityname. We use this to launch application in Guest Machine</param>
        /// <returns>True if open successfully otherwise False</returns>
        public bool OpenApplication(string application_launch_name)
        {
            process.StartInfo.Arguments = BuildArgument(string.Format(LAUNCH_APPLICATION_COMMAND, application_launch_name));
            string result = ExecuteShellCommand(process);
            
            //Success case
            //Starting: Intent { cmp=com.minhtt.monngonvietnam/.MonNgonVietNam }
            //Warning: Activity not started, its current task has been brought to the front

            //Error case
            //Starting: Intent { cmp=com.minhtt.monngonvietnam/.MonNgonVietNam1 }
            //Error type 3
            //Error: Activity class {com.minhtt.monngonvietnam/com.minhtt.monngonvietnam.MonNgonVietNam1} does not exist.

            return !result.Contains("Error");
            
        }

        /// <summary>
        /// Check an application is running
        /// </summary>
        /// <param name="packageName">package name</param>
        /// <returns>True if the application is running otherwise False</returns>
        public bool IsApplicationRunning(string packageName)
        {
            process.StartInfo.Arguments = BuildArgument(string.Format(FIND_RUNNING_APPLICATION_BY_NAME_COMMAND, packageName));
            string result = ExecuteShellCommand(process);

            return !string.IsNullOrEmpty(result) && result.Trim().ToLower().Equals(packageName.ToLower());
        }

        /// <summary>
        /// Stop running application
        /// </summary>
        /// <param name="process"></param>
        /// <param name="packageName"></param>
        /// <returns>True if the application is stopped otherwise False</returns>
        public bool StopApplication(string packageName)
        {
            process.StartInfo.Arguments = BuildArgument(string.Format(STOP_APPLICATION, packageName));
            string result = ExecuteShellCommand(process);

            return !string.IsNullOrEmpty(result) && result.Trim().Equals("Success");
        }

        /// <summary>
        /// Get Full launchable Activity Name of an apk file.
        /// </summary>
        /// <param name="guest_apk_path"></param>
        /// <returns></returns>
        private string GetLaunchableActivityName(string applicationName, string guest_apk_path)
        {
            //Pull the apk file to HOST machine
            string localPath = Path.Combine(
                                Directory.GetCurrentDirectory(),
                                TEMP_FOLDER,
                                string.Format("{0}_{1}.apk", applicationName.Substring(0, applicationName.IndexOf(".apk")),
                                Guid.NewGuid().ToString()));

            process.StartInfo.Arguments = BuildArgument(string.Format(PULL_APP_COMMAND, guest_apk_path, localPath));

            string result = ExecuteShellCommand(process);
            string activityName = string.Empty;

            if (File.Exists(localPath))
            {
                //Dump apk file to read manifest xml.
                string manifest_data = dumpAPKFile(localPath);
                activityName = GetLaunchableActivityName(manifest_data);
                File.Delete(localPath);
            }

            return activityName;
        }

        /// <summary>
        /// Dump manifest file from apk file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Manifest file path</returns>
        private string dumpAPKFile(string filePath)
        {
            string destinationFolder = Path.Combine(Directory.GetCurrentDirectory(), TEMP_FOLDER, Guid.NewGuid().ToString());
            process.StartInfo.FileName = "java.exe";
            process.StartInfo.Arguments = string.Format(@"-jar Libs\apktool.jar d {0} {1}", filePath, destinationFolder);
            
            string result = ExecuteShellCommand(process);
            string manifest_file = Path.Combine(destinationFolder, ANDROID_MANIFEST);

            string xmlData = string.Empty;
            if(File.Exists(manifest_file))
                xmlData = File.ReadAllText(manifest_file);

            Directory.Delete(destinationFolder, true);
            return xmlData;
        }

        /// <summary>
        /// Parse Manifest file to retrieve package name and activity name
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetLaunchableActivityName(string xmlString) {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);
            XmlNode rootNode = xml.SelectSingleNode("manifest");

            string packageName = string.Empty;
            string activityName = string.Empty;

            if (rootNode != null)
            {
                XmlAttribute packageAttribute = rootNode.Attributes["package"] as XmlAttribute;

                packageName = packageAttribute != null ? packageAttribute.Value : string.Empty;
                XmlNodeList nodeList = rootNode.SelectNodes("application/activity");

                if (nodeList.Count >= 1) {
                    XmlNode node = nodeList[0];
                    XmlAttribute attribute = node.Attributes["android:name"] as XmlAttribute;
                    activityName =  attribute != null ? attribute.Value : string.Empty;
                }

                return !string.IsNullOrEmpty(packageName) && !string.IsNullOrEmpty(activityName) ? string.Format("{0}/{1}", packageName, activityName) : string.Empty;
            }
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
        /// Build tranformed argument to a standard argument
        /// </summary>
        /// <param name="tranformedArgument"></param>
        /// <returns></returns>
        private string BuildArgument(string tranformedArgument)
        {
            return string.Format("-s {0}:{1} {2}", device_ipAddress, port, tranformedArgument);
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
