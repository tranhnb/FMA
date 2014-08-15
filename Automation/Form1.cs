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
using Activity;
using Mining;
using Activity.Activities;

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
        Mining.GiftCardMiner miner;
        string guestIP = "192.168.1.7";
        Utils.AndroidDebugBridge adb = new AndroidDebugBridge(System.Net.IPAddress.Parse("192.168.1.7"), 5555);
        GuestInformation guest = new GuestInformation();
        ImageUtils imageUtils = new ImageUtils();

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

            guest.port = 5555;
            guest.IPAddress = guestIP;
            guest.Mouse = machineSession.Console.Mouse;
            guest.Display = machineSession.Console.Display;
        }


        /// <summary>
        /// Click to install application on Google Play screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstallApp_Click(object sender, EventArgs e)
        {
            Activity.IActivity installActivity = Activity.Activity.CreateActivity(ActivityType.INSTALL_APPLICATION, guest);
            installActivity.Start();
        }

        /// <summary>
        /// Accept installation on the popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcceptInstallation_Click(object sender, EventArgs e)
        {
            Activity.IActivity acceptInstallation = Activity.Activity.CreateActivity(ActivityType.ACCEPT_INSTALLATION, guest);
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
            Point? point = this.imageUtils.FindAllPixelLocation(big, c, startPoint, endPoint);

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

            gBmp.Dispose();
            redBrush.Dispose();

            return;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Activity.IActivity refreshFMA = Activity.Activity.CreateActivity(ActivityType.REFRESH_FREE_MY_APP, guest);
            refreshFMA.Start();
        }


        private void btnDetermineApplication_Click(object sender, EventArgs e)
        {
            Activity.IActivity determineApplication = Activity.Activity.CreateActivity(ActivityType.DETERMINE_APPLICATION, guest);
            determineApplication.Start();
        }

        private void btnConfirmDownload_Click(object sender, EventArgs e)
        {
            Activity.IActivity confirmDownloadApp = Activity.Activity.CreateActivity(ActivityType.CONFIRM_DOWNLOAD, guest);
            confirmDownloadApp.Start();
        }

        private void btnConfirmUsingPlayStore_Click(object sender, EventArgs e)
        {
            Activity.IActivity confirmUsingPlayStore = Activity.Activity.CreateActivity(ActivityType.CONFIRM_USING_PLAYSTORE, guest);
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
            System.Net.IPAddress ip = System.Net.IPAddress.Parse("192.168.1.7");
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

        private void btnThread_Click(object sender, EventArgs e)
        {
            string guestMachineName = "test";
            IPAddress ipAddress = IPAddress.Parse(guestIP);
            miner = new Mining.GiftCardMiner(guestMachineName, ipAddress, 5555);
        }

        private void btnStopThread_Click(object sender, EventArgs e)
        {
            if (miner != null)
                miner.StopDig();
        }

        private void btnStartMiner_Click(object sender, EventArgs e)
        {
            if (miner != null)
                miner.Start();
        }

        #region Tab 2

        private void btnCheckCaptureScreen_Click(object sender, EventArgs e)
        {
            CaptureScreen screen = new CaptureScreen();
            byte[] rootHash = null;
            bool isIdentical = true;

            for (int i = 0; i < 10; i++)
            {
                byte[] byteArray = screen.TakeScreenShot(this.guest.Display, 349, 143, 104, 33);
                Image img = ImageFromByteArray(byteArray);
                string filePath = string.Format(@"{0}\Temp\{1}.png", Directory.GetCurrentDirectory(), i.ToString());
                img.Save(filePath);
                if (rootHash == null)
                    rootHash = this.imageUtils.Sha256HashFile(filePath);

                byte[] currentHash = this.imageUtils.Sha256HashFile(filePath);
                if (!rootHash.SequenceEqual(currentHash))
                {
                    isIdentical = false;
                    
                    return;
                }
            }

            if(!isIdentical)
                MessageBox.Show("Capture screen is error");
            else
                MessageBox.Show("All pics are identical");
        }


        private Image ImageFromByteArray(byte[] byteArray)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void btnSelectImage1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            DialogResult result =  openFileDialog1.ShowDialog(this);
            if(result.Equals(DialogResult.OK))
            {
                txtImage1.Text = openFileDialog1.FileName;
            }
        }

   

        private void btnCompareImages_Click(object sender, EventArgs e)
        {
            bool isIdentical = true;
            for (int i = 0, len = openFileDialog1.FileNames.Length; i < len; i++)
            {
                byte[] image1Hash = this.imageUtils.Sha256HashFile(openFileDialog1.FileNames[i]);
                byte[] image2Hash = null;
                if (i + 1 < len)
                {
                    image2Hash = this.imageUtils.Sha256HashFile(openFileDialog1.FileNames[i + 1]);
                }
                if (image2Hash != null && !image1Hash.SequenceEqual(image2Hash))
                {
                    isIdentical = false;
                    break;
                }
            }
            if (!isIdentical)
            {
                MessageBox.Show("Images are not identical");
            }
            else {
                MessageBox.Show("Images are identical");
            }
        }
        

        #endregion Tab 2

        

        

        

    }
}


