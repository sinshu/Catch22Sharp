using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class DN_OutlierInclude_n_001_mdrmd
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.DN_OutlierInclude_n_001_mdrmd(TestData.Test1);
            var expected = TestData.Test1Output["DN_OutlierInclude_n_001_mdrmd"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.DN_OutlierInclude_n_001_mdrmd(TestData.Test2);
            var expected = TestData.Test2Output["DN_OutlierInclude_n_001_mdrmd"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.DN_OutlierInclude_n_001_mdrmd(TestData.TestShort);
            var expected = TestData.TestShortOutput["DN_OutlierInclude_n_001_mdrmd"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.DN_OutlierInclude_n_001_mdrmd(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["DN_OutlierInclude_n_001_mdrmd"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestRandom()
        {
            var actual = Catch22.DN_OutlierInclude_n_001_mdrmd(TestData.TestRandom);
            var expected = TestData.TestRandomOutput["DN_OutlierInclude_n_001_mdrmd"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestWave()
        {
            var actual = Catch22.DN_OutlierInclude_n_001_mdrmd(TestData.TestWave);
            var expected = TestData.TestWaveOutput["DN_OutlierInclude_n_001_mdrmd"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
