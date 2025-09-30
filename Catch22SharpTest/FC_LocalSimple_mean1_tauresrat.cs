using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class FC_LocalSimple_mean1_tauresrat
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.FC_LocalSimple_mean1_tauresrat(TestData.Test1);
            var expected = TestData.Test1Output["FC_LocalSimple_mean1_tauresrat"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.FC_LocalSimple_mean1_tauresrat(TestData.Test2);
            var expected = TestData.Test2Output["FC_LocalSimple_mean1_tauresrat"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.FC_LocalSimple_mean1_tauresrat(TestData.TestShort);
            var expected = TestData.TestShortOutput["FC_LocalSimple_mean1_tauresrat"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.FC_LocalSimple_mean1_tauresrat(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["FC_LocalSimple_mean1_tauresrat"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
