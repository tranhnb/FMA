using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Activity.Activities;

namespace Activity
{
    class FindNewInstalledAppActivity : Activity
    {
        public FindNewInstalledAppActivity() { }

        public override ActivityResult Start()
        {
            //Process p = new Process();
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.CreateNoWindow = true;
            ////p.StartInfo.FileName = @"D:\TranHNB\CFF\FreeMyApp\Tool\adt-bundle-windows-x86-20140321\adt-bundle-windows-x86-20140321\sdk\platform-tools\adb.exe connect 192.168.1.13:5555";
            //p.StartInfo.FileName = @"D:\TranHNB\CFF\FreeMyApp\Tool\adt-bundle-windows-x86-20140321\adt-bundle-windows-x86-20140321\sdk\platform-tools\adb.exe";
            ////p.StartInfo.Arguments = "connect 192.168.1.13:5555";
            //p.StartInfo.Arguments = "shell \"pm list packages -f\"";

            //p.Start();
            //// Do not wait for the child process to exit before
            //// reading to the end of its redirected stream.
            //// p.WaitForExit();
            //// Read the output stream first and then wait.
            //string output = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();
            return null;
            
        }
    }
}
