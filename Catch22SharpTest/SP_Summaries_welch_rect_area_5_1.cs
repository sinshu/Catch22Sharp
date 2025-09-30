using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class SP_Summaries_welch_rect_area_5_1
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.SP_Summaries_welch_rect_area_5_1(TestData.Test1);
            var expected = TestData.Test1Output["SP_Summaries_welch_rect_area_5_1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.SP_Summaries_welch_rect_area_5_1(TestData.Test2);
            var expected = TestData.Test2Output["SP_Summaries_welch_rect_area_5_1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.SP_Summaries_welch_rect_area_5_1(TestData.TestShort);
            var expected = TestData.TestShortOutput["SP_Summaries_welch_rect_area_5_1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.SP_Summaries_welch_rect_area_5_1(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["SP_Summaries_welch_rect_area_5_1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestRandom()
        {
            var actual = Catch22.SP_Summaries_welch_rect_area_5_1(TestData.TestRandom);
            var expected = TestData.TestRandomOutput["SP_Summaries_welch_rect_area_5_1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestWave()
        {
            var actual = Catch22.SP_Summaries_welch_rect_area_5_1(TestData.TestWave);
            var expected = TestData.TestWaveOutput["SP_Summaries_welch_rect_area_5_1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
