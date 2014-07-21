using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Automation
{
    public enum Test { 
        Hoang,
        Nguyen
    }
    public partial class MainForm : Form
    {
        private const int GUEST_PORT = 5555;

        public MainForm()
        {
            InitializeComponent();
            Test test = Test.Hoang;
            switch (test) { 
                case Test.Hoang:
                    MessageBox.Show("Hoang");
                    break;
                case Test.Nguyen:
                    MessageBox.Show("Nguyen");
                    break;
                default:
                    MessageBox.Show("Error");
                    break;
                    
            }
        }

        private void btnStartDig_Click(object sender, EventArgs e)
        {
            Mining.GiftCardMiner miner = new Mining.GiftCardMiner(txtGuestName.Text, System.Net.IPAddress.Parse(txtGuestIP.Text), GUEST_PORT);
            miner.StartDig();
        }
    }
}
