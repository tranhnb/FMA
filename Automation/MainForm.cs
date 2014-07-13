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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStartDig_Click(object sender, EventArgs e)
        {
            Mining.GiftCardMiner miner = new Mining.GiftCardMiner("", null, 4444);
            miner.StartDig();
        }
    }
}
