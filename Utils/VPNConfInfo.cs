using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace Utils
{
    class VPNConfInfoList : Queue<string>
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openVPNDir">OpenVPN Configuration Dir</param>
        public VPNConfInfoList(string openVPNDir)
        {
            //Load a Litst of OpenVPN confirguration file
            string[] files = Directory.GetFiles(openVPNDir, "inCloak.com USA, New York S1.ovpn");
            
            //Load conf file into Queue
            for (int i = 0; i < files.Length; i++)
            {
                this.Enqueue(files[i]);
            }

        }

        /// <summary>
        /// Load the next OpenVPN confiugration file on the list
        /// </summary>
        /// <returns></returns>
        public string LoadNext() {
            return this.Dequeue();
        }

        #endregion Constructor

    }
}
