using System;
using BetGame3.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Punter punter = Factory.GetPunterInstance("Ava");
            bool result = punter is Ava;
            Assert.AreEqual(result, true);
        }
    }
}
