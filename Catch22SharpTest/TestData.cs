using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Catch22SharpTest
{
    [TestClass]
    public class TestData
    {
        private static readonly string testDataDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "testData"));

        public static double[] Test1 = default!;
        public static Dictionary<string, double> Test1Output = default!;

        public static double[] Test2 = default!;
        public static Dictionary<string, double> Test2Output = default!;

        public static double[] TestShort = default!;
        public static Dictionary<string, double> TestShortOutput = default!;

        public static double[] TestSinusoid = default!;
        public static Dictionary<string, double> TestSinusoidOutput = default!;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            Test1 = GetData(Path.Combine(testDataDirectory, "test.txt"));
            Test1Output = GetExpectedValues(Path.Combine(testDataDirectory, "test_output.txt"));

            Test2 = GetData2(Path.Combine(testDataDirectory, "test2.txt"));
            Test2Output = GetExpectedValues(Path.Combine(testDataDirectory, "test2_output.txt"));

            TestShort = GetData(Path.Combine(testDataDirectory, "testShort.txt"));
            TestShortOutput = GetExpectedValues(Path.Combine(testDataDirectory, "testShort_output.txt"));

            TestSinusoid = GetData(Path.Combine(testDataDirectory, "testSinusoid.txt"));
            TestSinusoidOutput = GetExpectedValues(Path.Combine(testDataDirectory, "testSinusoid_output.txt"));
        }

        [TestMethod]
        public void Test1Data()
        {
            Assert.IsNotNull(Test1);
            Assert.IsNotNull(Test1Output);
        }

        [TestMethod]
        public void Test2Data()
        {
            Assert.IsNotNull(Test2);
            Assert.IsNotNull(Test2Output);
        }

        [TestMethod]
        public void TestShortData()
        {
            Assert.IsNotNull(TestShort);
            Assert.IsNotNull(TestShortOutput);
        }

        [TestMethod]
        public void TestSinusoidData()
        {
            Assert.IsNotNull(TestSinusoid);
            Assert.IsNotNull(TestSinusoidOutput);
        }

        private static double[] GetData(string path)
        {
            return File.ReadLines(path).Select(double.Parse).ToArray();
        }

        private static double[] GetData2(string path)
        {
            return File.ReadAllText(path).Split(' ').Select(double.Parse).ToArray();
        }

        private static Dictionary<string, double> GetExpectedValues(string path)
        {
            var dic = new Dictionary<string, double>();

            foreach (var line in File.ReadLines(path))
            {
                if (line.Length == 0)
                {
                    break;
                }

                var split = line.Split(", ");
                var name = split[1];
                var value = double.Parse(split[0]);
                dic.Add(name, value);
            }

            return dic;
        }
    }
}
