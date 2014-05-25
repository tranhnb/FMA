using Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ImageUtilsTest and is intended
    ///to contain all ImageUtilsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ImageUtilsTest
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


        /// <summary>
        ///A test for IsSubImage
        ///</summary>
        [TestMethod()]
        public void IsSubImageTest()
        {

            string path = Path.Combine(TestDataPath, "UnitTest", "TestData");
            
            ImageUtils target = new ImageUtils(); // TODO: Initialize to an appropriate value
            string parentImagePath = path + @"\Parent.png";
            string childImagePath = path + @"\Child.png";
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual = true;
            actual = target.IsSubImage(parentImagePath, childImagePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        public string TestDataPath
        {
            get
            {
                return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            }
        }
    }
}
