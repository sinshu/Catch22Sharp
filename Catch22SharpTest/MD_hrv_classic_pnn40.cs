using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class MD_hrv_classic_pnn40
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.MD_hrv_classic_pnn40(TestData.Test1);
            var expected = TestData.Test1Output["MD_hrv_classic_pnn40"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.MD_hrv_classic_pnn40(TestData.Test2);
            var expected = TestData.Test2Output["MD_hrv_classic_pnn40"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.MD_hrv_classic_pnn40(TestData.TestShort);
            var expected = TestData.TestShortOutput["MD_hrv_classic_pnn40"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.MD_hrv_classic_pnn40(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["MD_hrv_classic_pnn40"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestRandom()
        {
            var actual = Catch22.MD_hrv_classic_pnn40(TestData.TestRandom);
            var expected = TestData.TestRandomOutput["MD_hrv_classic_pnn40"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestWave()
        {
            var actual = Catch22.MD_hrv_classic_pnn40(TestData.TestWave);
            var expected = TestData.TestWaveOutput["MD_hrv_classic_pnn40"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
