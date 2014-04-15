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
            //VirtualBox.VirtualBox virtualBox = new VirtualBox.VirtualBox();
            //IMachine machine =  virtualBox.FindMachine("test");
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

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// Click to install application on Google Play screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstallApp_Click(object sender, EventArgs e)
        {
            Activity.IActivity installActivity = Activity.Activity.CreateActivity(Constants.ActivityName.INSTALL_APPLICATION, machineSession.Console.Mouse);
            installActivity.Start();
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
    }
}
