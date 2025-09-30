using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class PD_PeriodicityWang_th0_01
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.PD_PeriodicityWang_th0_01(TestData.Test1);
            var expected = TestData.Test1Output["PD_PeriodicityWang_th0_01"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.PD_PeriodicityWang_th0_01(TestData.Test2);
            var expected = TestData.Test2Output["PD_PeriodicityWang_th0_01"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.PD_PeriodicityWang_th0_01(TestData.TestShort);
            var expected = TestData.TestShortOutput["PD_PeriodicityWang_th0_01"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.PD_PeriodicityWang_th0_01(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["PD_PeriodicityWang_th0_01"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
