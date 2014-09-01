using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public interface IAndroidDebugBridge
    {
        bool OpenFreeMyApp();
        bool OpenApplication(string application_launch_name);
    }
}
