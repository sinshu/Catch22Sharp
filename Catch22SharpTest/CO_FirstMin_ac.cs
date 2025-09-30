using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class CO_FirstMin_ac
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.CO_FirstMin_ac(TestData.Test1);
            var expected = TestData.Test1Output["CO_FirstMin_ac"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.CO_FirstMin_ac(TestData.Test2);
            var expected = TestData.Test2Output["CO_FirstMin_ac"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.CO_FirstMin_ac(TestData.TestShort);
            var expected = TestData.TestShortOutput["CO_FirstMin_ac"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.CO_FirstMin_ac(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["CO_FirstMin_ac"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
