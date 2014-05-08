using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualBox;
using Utils;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Automation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        VirtualBoxClient vBoxClient;
        IMachine machine;
        Session machineSession;

        
        Utils.AndroidDebugBridge adb = new AndroidDebugBridge(System.Net.IPAddress.Parse("192.168.1.13"), 5555);

        private void button1_Click(object sender, EventArgs e)
        {
            if(vBoxClient == null)
                vBoxClient = new VirtualBoxClient();
            if (machine == null)
            {
                machine = vBoxClient.VirtualBox.FindMachine("test");

            }
            if (machineSession == null)
            {
                machineSession = vBoxClient.Session;
                machine.LockMachine(machineSession, LockType.LockType_Shared);
            }
        }


        /// <summary>
        /// Click to install application on Google Play screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstallApp_Click(object sender, EventArgs e)
        {
            Activity.IActivity installActivity = Activity.Activity.CreateActivity(Constants.ActivityName.INSTALL_APPLICATION, machineSession.Console.Mouse, machineSession.Console.Display);
            installActivity.Start();
        }

        /// <summary>
        /// Accept installation on the popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcceptInstallation_Click(object sender, EventArgs e)
        {
            Activity.IActivity acceptInstallation = Activity.Activity.CreateActivity(Constants.ActivityName.ACCEPT_INSTALLATION, machineSession.Console.Mouse, machineSession.Console.Display);
            acceptInstallation.Start();
        }

        /// <summary>
        /// Take VirtualMachine ScreenShot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTakeScreenShot_Click(object sender, EventArgs e)
        {
            Utils.CaptureScreen screen = new CaptureScreen();
            byte[] byteArray = screen.TakeScreenShot(machineSession.Console.Display);
            Image img = byteArrayToImage(byteArray);
            img.Save(@"C:\ScreenShot.png");
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;

        }


        private void btnModi_Click(object sender, EventArgs e)
        {
            MODI.Document md = new MODI.Document();
            
            md.Create(@"Images\Test\Accept1.JPG");
            md.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true);
            MODI.Image image = (MODI.Image)md.Images[0];
            md = null;
            string returnText = image.Layout.Text;
            returnText = returnText.Replace("\r\n", "");
        }

        private void btnFindSubImage_Click(object sender, EventArgs e)
        {
            Bitmap big = new Bitmap("c:\\temp_big.JPG");
            //Bitmap small = new Bitmap("c:\\temp_small.JPG");

            Color c = Color.FromArgb(181, 203, 58);

            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(big.Width, big.Height);
            Point? point = ImageUtils.FindAllPixelLocation(big, c, startPoint, endPoint);

            if (!point.HasValue)
            {
                MessageBox.Show("Failed!");
                return;
            }

            Image imgShow = (Image)big.Clone();
            Graphics gBmp = Graphics.FromImage(imgShow);

            // draw a red circle
            Color red = Color.Red;
            Brush redBrush = new SolidBrush(red);
            gBmp.FillEllipse(redBrush, point.Value.X, point.Value.Y, 10, 10);

            pictureBox1.Image = imgShow;

            //imgShow.Dispose();
            gBmp.Dispose();
            redBrush.Dispose();

            return;

            //another solution

            //ImageChecker checker = new ImageChecker(big, small);
            //Point p = checker.bigContainsSmall(102, 27, 0, 100, 0, 100);
            //MessageBox.Show("X: " + p.X.ToString() + "---Y: " + p.Y.ToString());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Activity.IActivity refreshFMA = Activity.Activity.CreateActivity(Constants.ActivityName.REFRESH_FREE_MY_APP, machineSession.Console.Mouse, machineSession.Console.Display);
            refreshFMA.Start();
        }


        private void btnDetermineApplication_Click(object sender, EventArgs e)
        {
            Activity.IActivity determineApplication = Activity.Activity.CreateActivity(Constants.ActivityName.DETERMINE_APPLICATION, machineSession.Console.Mouse, machineSession.Console.Display);
            determineApplication.Start();
        }

        private void btnConfirmDownload_Click(object sender, EventArgs e)
        {
            Activity.IActivity confirmDownloadApp = Activity.Activity.CreateActivity(Constants.ActivityName.CONFIRM_DOWNLOAD, machineSession.Console.Mouse, machineSession.Console.Display);
            confirmDownloadApp.Start();
        }

        private void btnConfirmUsingPlayStore_Click(object sender, EventArgs e)
        {
            Activity.IActivity confirmUsingPlayStore = Activity.Activity.CreateActivity(Constants.ActivityName.CONFIRM_USING_PLAYSTORE, machineSession.Console.Mouse, machineSession.Console.Display);
            confirmUsingPlayStore.Start();
        }

        private void btnInstalledApps_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.FileName = @"D:\TranHNB\CFF\FreeMyApp\Tool\adt-bundle-windows-x86-20140321\adt-bundle-windows-x86-20140321\sdk\platform-tools\adb.exe connect 192.168.1.13:5555";
            p.StartInfo.FileName = @"D:\TranHNB\CFF\FreeMyApp\Tool\adt-bundle-windows-x86-20140321\adt-bundle-windows-x86-20140321\sdk\platform-tools\adb.exe";
            //p.StartInfo.Arguments = "connect 192.168.1.13:5555";
            p.StartInfo.Arguments = "shell \"pm list packages -f\"";
            
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            MessageBox.Show(this, output, "List installed apps", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Open an installed app in 30s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLaunchApp_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = @"D:\TranHNB\CFF\FreeMyApp\Tool\adt-bundle-windows-x86-20140321\adt-bundle-windows-x86-20140321\sdk\build-tools\android-4.4.2\aapt.exe";
            p.StartInfo.Arguments = @"l -a C:\xemchitay.apk";
            
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            MessageBox.Show(this, output, "List installed apps", MessageBoxButtons.OK, MessageBoxIcon.Information);
            

        }

        /// <summary>
        /// Connect to Guest machine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            System.Net.IPAddress ip = System.Net.IPAddress.Parse("192.168.1.13");
            int port = 5555;

            bool isConnected = adb.Connect();
            if (isConnected)
            {
                MessageBox.Show(this, isConnected.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void btnNewestInstallApp_Click(object sender, EventArgs e)
        {
            string app_name = adb.FindNewestInstalledApp();
            txtPackage_ActivityName.Text = app_name;
            
        }

        private void btnOpenApplication_Click(object sender, EventArgs e)
        {
            string fullName = txtPackage_ActivityName.Text;
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show(this, "Please execute Get Newest Install Application to retrieve ActivityName First: ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                adb.OpenApplication(fullName);
            }
        }

        private void btnCheckApplicationRunning_Click(object sender, EventArgs e)
        {
            string packageName = txtPackageName.Text;
            if (string.IsNullOrEmpty(packageName))
            {
                MessageBox.Show(this, "Please input package name to check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                bool isRunning = adb.IsApplicationRunning(packageName);
                MessageBox.Show(this, isRunning.ToString());
            }

        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            string packageName = txtPackageName.Text;
            if (string.IsNullOrEmpty(packageName))
            {
                MessageBox.Show(this, "Please input package name to check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                bool isStopped = adb.StopApplication(packageName);
                MessageBox.Show(this, isStopped.ToString());
            }
        }

        private void btnOpenFMA_Click(object sender, EventArgs e)
        {
            bool isOpen = adb.OpenFreeMyApp();
            MessageBox.Show(this, isOpen.ToString());
        }



        
    }
}

