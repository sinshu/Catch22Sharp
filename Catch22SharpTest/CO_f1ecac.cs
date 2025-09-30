using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class CO_f1ecac_Tests
    {
        private const double Tolerance = 1.0E-6;

        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.CO_f1ecac(TestData.Test1);
            var expected = TestData.Test1Output["CO_f1ecac"];
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.CO_f1ecac(TestData.Test2);
            var expected = TestData.Test2Output["CO_f1ecac"];
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.CO_f1ecac(TestData.TestShort);
            var expected = TestData.TestShortOutput["CO_f1ecac"];
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.CO_f1ecac(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["CO_f1ecac"];
            Assert.AreEqual(expected, actual, Tolerance);
        }
    }
}
