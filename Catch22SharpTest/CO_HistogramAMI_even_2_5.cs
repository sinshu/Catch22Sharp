using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class CO_HistogramAMI_even_2_5_Tests
    {
        private const double Tolerance = 1.0E-6;

        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.Test1);
            var expected = TestData.Test1Output["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.Test2);
            var expected = TestData.Test2Output["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.TestShort);
            var expected = TestData.TestShortOutput["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.CO_HistogramAMI_even_2_5(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["CO_HistogramAMI_even_2_5"];
            Assert.AreEqual(expected, actual, Tolerance);
        }
    }
}
