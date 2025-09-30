using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1(TestData.Test1);
            var expected = TestData.Test1Output["SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1(TestData.Test2);
            var expected = TestData.Test2Output["SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1(TestData.TestShort);
            var expected = TestData.TestShortOutput["SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
