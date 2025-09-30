using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class CO_HistogramAMI_even_2_5
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.Test1);
            var expected = TestData.Test1Output["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.Test2);
            var expected = TestData.Test2Output["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.TestShort);
            var expected = TestData.TestShortOutput["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestRandom()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.TestRandom);
            var expected = TestData.TestRandomOutput["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestWave()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.TestWave);
            var expected = TestData.TestWaveOutput["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
