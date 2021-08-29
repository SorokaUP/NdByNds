using System;
using Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UTModel_5_08
    {
        [TestMethod]
        public void Book08Line()
        {
            Model_5_08 model = new Model_5_08(Core.BookType.Book08, 0);
            object[] data = { 0 };
            string sTest = model.GetBodyBook08(data);
            Assert.AreEqual(sTest, "1");
        }

        [TestMethod]
        public void Book09Line()
        {
            Model_5_08 model = new Model_5_08(Core.BookType.Book09, 0);
            object[] data = { 0 };
            string sTest = model.GetBodyBook09(data);
            Assert.AreEqual(sTest, "1");
        }

        [TestMethod]
        public void Book10Line()
        {
            Model_5_08 model = new Model_5_08(Core.BookType.Book10, 0);
            object[] data = { 0 };
            string sTest = model.GetBodyBook10(data);
            Assert.AreEqual(sTest, "1");
        }

        [TestMethod]
        public void Book11Line()
        {
            Model_5_08 model = new Model_5_08(Core.BookType.Book11, 0);
            object[] data = { 0 };
            string sTest = model.GetBodyBook11(data);
            Assert.AreEqual(sTest, "1");
        }
    }
}
