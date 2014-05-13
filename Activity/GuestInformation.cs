using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    /// <summary>
    /// Contains Guest information
    /// </summary>
    public struct GuestInformation
    {
        public string Name; //Virtualbox machine name
        public string IPAddress;
        public int port;
        public VirtualBox.IMouse Mouse;
        public VirtualBox.IDisplay Display;
    }
}
