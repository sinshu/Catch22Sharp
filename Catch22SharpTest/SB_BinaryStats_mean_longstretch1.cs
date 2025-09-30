using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class SB_BinaryStats_mean_longstretch1
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.SB_BinaryStats_mean_longstretch1(TestData.Test1);
            var expected = TestData.Test1Output["SB_BinaryStats_mean_longstretch1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.SB_BinaryStats_mean_longstretch1(TestData.Test2);
            var expected = TestData.Test2Output["SB_BinaryStats_mean_longstretch1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.SB_BinaryStats_mean_longstretch1(TestData.TestShort);
            var expected = TestData.TestShortOutput["SB_BinaryStats_mean_longstretch1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.SB_BinaryStats_mean_longstretch1(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["SB_BinaryStats_mean_longstretch1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
