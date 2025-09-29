using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class DN_HistogramMode_10_Tests
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.DN_HistogramMode_10(TestData.Test1);
            var expected = TestData.Test1Output["DN_HistogramMode_10"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.DN_HistogramMode_10(TestData.Test2);
            var expected = TestData.Test2Output["DN_HistogramMode_10"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.DN_HistogramMode_10(TestData.TestShort);
            var expected = TestData.TestShortOutput["DN_HistogramMode_10"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.DN_HistogramMode_10(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["DN_HistogramMode_10"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
