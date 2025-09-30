using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class IN_AutoMutualInfoStats_40_gaussian_fmmi
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.IN_AutoMutualInfoStats_40_gaussian_fmmi(TestData.Test1);
            var expected = TestData.Test1Output["IN_AutoMutualInfoStats_40_gaussian_fmmi"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.IN_AutoMutualInfoStats_40_gaussian_fmmi(TestData.Test2);
            var expected = TestData.Test2Output["IN_AutoMutualInfoStats_40_gaussian_fmmi"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.IN_AutoMutualInfoStats_40_gaussian_fmmi(TestData.TestShort);
            var expected = TestData.TestShortOutput["IN_AutoMutualInfoStats_40_gaussian_fmmi"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.IN_AutoMutualInfoStats_40_gaussian_fmmi(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["IN_AutoMutualInfoStats_40_gaussian_fmmi"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
