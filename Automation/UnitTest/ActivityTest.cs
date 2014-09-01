using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using NSubstitute;
using VirtualBox;
using System.Drawing;
using System.IO;

namespace UnitTest
{
    /// <summary>
    ///This is a test class for All activity
    ///</summary>
    [TestClass()]
    public class ActivityTest
    {
        #region Test configuration

        public string TestDataPath
        {
            get
            {
                return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, @"Automation\UnitTest\TestData");
            }
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        //Use ClassInitialize to run code before running the first test in the class
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string fileName = Path.Combine(TestDataPath, "FreeMyApp_Step1.png");
            REFRESH_FREE_MYAPP_SCREEN = imageToByteArray(Image.FromFile(fileName));
        }

        #endregion Test configuration

        private byte[] REFRESH_FREE_MYAPP_SCREEN;
        

        /// <summary>
        /// Test LaunchFreeMyAppActivity
        /// </summary>
        [TestMethod()]
        public void LaunchFreeMyAppActivityTest()
        {
            var iAndroidDebugBridge = Substitute.For<IAndroidDebugBridge>();
            iAndroidDebugBridge.OpenFreeMyApp().Returns(true);
            Assert.AreEqual(true, iAndroidDebugBridge.OpenFreeMyApp());
        }

        /// <summary>
        /// Test RefreshActivity
        /// </summary>
        [TestMethod()]
        public void RefreshFreeMyAppActivityTest()
        {
            uint width, height, bitsPerPixel;
            int aXOrigin, aYOrigin;
            var iDisplay = Substitute.For<IDisplay>();
            
            //Mock data for IDisplay class's function
            iDisplay.When(x => x.GetScreenResolution(0, out width, out height, out bitsPerPixel, out aXOrigin, out aYOrigin))
                    .Do(x => width = 5);
            width = 800;
            height = 600;

            //
            iDisplay.TakeScreenShotPNGToArray(0, width, height).Returns(REFRESH_FREE_MYAPP_SCREEN);
            //End Mock data for IDisplay class's function

            //Default width/height AndroidX86 virtual machine
            
            
            Assert.AreEqual(5, width);
            
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
