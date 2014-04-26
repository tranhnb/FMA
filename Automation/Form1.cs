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

            //IEventListener listener = machineSession.Console.Mouse.EventSource.CreateListener();


            //machineSession.Console.Mouse.PutMouseEvent(0, 0, 0, 0, 0x001);
            //machineSession.Console.Mouse.PutMouseEvent(0, 0, 0, 0, 0x000);
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

        
    }
}

