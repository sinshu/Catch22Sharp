using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class SB_MotifThree_quantile_hh
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.SB_MotifThree_quantile_hh(TestData.Test1);
            var expected = TestData.Test1Output["SB_MotifThree_quantile_hh"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.SB_MotifThree_quantile_hh(TestData.Test2);
            var expected = TestData.Test2Output["SB_MotifThree_quantile_hh"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.SB_MotifThree_quantile_hh(TestData.TestShort);
            var expected = TestData.TestShortOutput["SB_MotifThree_quantile_hh"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.SB_MotifThree_quantile_hh(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["SB_MotifThree_quantile_hh"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
