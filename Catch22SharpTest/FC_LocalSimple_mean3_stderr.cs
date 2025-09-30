using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class FC_LocalSimple_mean3_stderr
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.FC_LocalSimple_mean3_stderr(TestData.Test1, TestData.Test1.Length);
            var expected = TestData.Test1Output["FC_LocalSimple_mean3_stderr"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.FC_LocalSimple_mean3_stderr(TestData.Test2, TestData.Test2.Length);
            var expected = TestData.Test2Output["FC_LocalSimple_mean3_stderr"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.FC_LocalSimple_mean3_stderr(TestData.TestShort, TestData.TestShort.Length);
            var expected = TestData.TestShortOutput["FC_LocalSimple_mean3_stderr"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.FC_LocalSimple_mean3_stderr(TestData.TestSinusoid, TestData.TestSinusoid.Length);
            var expected = TestData.TestSinusoidOutput["FC_LocalSimple_mean3_stderr"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
