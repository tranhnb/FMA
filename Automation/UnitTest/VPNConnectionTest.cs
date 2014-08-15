using Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace UnitTest
{
    
    
    /// <summary>
    ///This is a test class for VPNConnectionTest and is intended
    ///to contain all VPNConnectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VPNConnectionTest
    {


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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        private VPNConnection target = new VPNConnection();
        /// <summary>
        ///A test for Connect
        ///</summary>
        [TestMethod()]
        public void ConnectTest()
        {
            target.OnConnected += new EventHandler(target_Connected);
            target.OnError += new EventHandler(target_OnError);
            target.OnClosed += new EventHandler(target_OnClosed);
            target.Connect(false);
        }

        void target_OnClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void target_OnError(object sender, EventArgs e)
        {
            Assert.AreEqual(false, true);
        }

        void target_Connected(object sender, EventArgs e)
        {
            VPNConnectionEventArgs eventArgs = e as VPNConnectionEventArgs;
            bool isClose = target.Close(eventArgs.processId);
            Assert.AreEqual(isClose, true);
        }

        
    }
}
