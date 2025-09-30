using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class CO_trev_1_num
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.CO_trev_1_num(TestData.Test1);
            var expected = TestData.Test1Output["CO_trev_1_num"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.CO_trev_1_num(TestData.Test2);
            var expected = TestData.Test2Output["CO_trev_1_num"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.CO_trev_1_num(TestData.TestShort);
            var expected = TestData.TestShortOutput["CO_trev_1_num"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.CO_trev_1_num(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["CO_trev_1_num"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestRandom()
        {
            var actual = Catch22.CO_trev_1_num(TestData.TestRandom);
            var expected = TestData.TestRandomOutput["CO_trev_1_num"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestWave()
        {
            var actual = Catch22.CO_trev_1_num(TestData.TestWave);
            var expected = TestData.TestWaveOutput["CO_trev_1_num"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
