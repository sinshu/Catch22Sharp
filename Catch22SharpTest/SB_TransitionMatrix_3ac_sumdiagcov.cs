using Catch22Sharp;

namespace Catch22SharpTest
{
    [TestClass]
    public class SB_TransitionMatrix_3ac_sumdiagcov
    {
        [TestMethod]
        public void Test1()
        {
            var actual = Catch22.SB_TransitionMatrix_3ac_sumdiagcov(TestData.Test1);
            var expected = TestData.Test1Output["SB_TransitionMatrix_3ac_sumdiagcov"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void Test2()
        {
            var actual = Catch22.SB_TransitionMatrix_3ac_sumdiagcov(TestData.Test2);
            var expected = TestData.Test2Output["SB_TransitionMatrix_3ac_sumdiagcov"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestShort()
        {
            var actual = Catch22.SB_TransitionMatrix_3ac_sumdiagcov(TestData.TestShort);
            var expected = TestData.TestShortOutput["SB_TransitionMatrix_3ac_sumdiagcov"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }

        [TestMethod]
        public void TestSinusoid()
        {
            var actual = Catch22.SB_TransitionMatrix_3ac_sumdiagcov(TestData.TestSinusoid);
            var expected = TestData.TestSinusoidOutput["SB_TransitionMatrix_3ac_sumdiagcov"];
            Assert.AreEqual(expected, actual, 1.0E-6);
        }
    }
}
