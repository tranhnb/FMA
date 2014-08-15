using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace Automation
{
    public enum Test { 
        Hoang,
        Nguyen
    }
    public partial class MainForm : Form
    {
        private const int GUEST_PORT = 5555;

        private Mining.GiftCardMiner miner;
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStartDig_Click(object sender, EventArgs e)
        {
            miner = new Mining.GiftCardMiner(txtGuestName.Text, System.Net.IPAddress.Parse(txtGuestIP.Text), GUEST_PORT);
            miner.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            miner.StopDig();
        }

        private void btnTakeScreenShot_Click(object sender, EventArgs e)
        {
            Utils.CaptureScreen screen = new CaptureScreen();
            byte[] byteArray = screen.TakeScreenShot(miner.DisplayController);
            Image img = screen.ByteArrayToImage(byteArray);
            img.Save(@"C:\ScreenShot.png");
        }
    }
}
