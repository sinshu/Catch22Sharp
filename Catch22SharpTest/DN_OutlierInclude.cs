using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class DN_OutlierInclude
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.DN_OutlierInclude_n_001_mdrmd(TestData.Test1);
            var expected = TestData.Test1Output["DN_OutlierInclude_n_001_mdrmd"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
