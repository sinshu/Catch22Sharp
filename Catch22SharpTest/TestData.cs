using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Catch22Sharp;

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

        public static double[] TestRandom = default!;
        public static Dictionary<string, double> TestRandomOutput = default!;

        public static double[] TestWave = default!;
        public static Dictionary<string, double> TestWaveOutput = default!;

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

            TestRandom = GetData(Path.Combine(testDataDirectory, "testRandom.txt"));
            TestRandomOutput = GetExpectedValues(Path.Combine(testDataDirectory, "testRandom_output.txt"));

            TestWave = GetData(Path.Combine(testDataDirectory, "testWave.txt"));
            TestWaveOutput = GetExpectedValues(Path.Combine(testDataDirectory, "testWave_output.txt"));
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

        [TestMethod]
        public void TestRandomData()
        {
            Assert.IsNotNull(TestRandom);
            Assert.IsNotNull(TestRandomOutput);
        }

        [TestMethod]
        public void TestWaveData()
        {
            Assert.IsNotNull(TestWave);
            Assert.IsNotNull(TestWaveOutput);
        }

        [TestMethod]
        public void Catch22Object()
        {
            var catch22 = new Catch22(Test1);
            foreach (var (name, expected) in Test1Output)
            {
                var actual = catch22[name];
                Assert.AreEqual(expected, actual, 1.0E-6);
            }

            foreach (var (name, value) in catch22.GetNameValuePairs())
            {
                Assert.AreEqual(catch22[name], value);
            }
        }

        private static double[] GetData(string path)
        {
            var data = File.ReadLines(path).Select(double.Parse).ToArray();
            return Normalize(data);
        }

        private static double[] GetData2(string path)
        {
            var data = File.ReadAllText(path).Split(' ').Select(double.Parse).ToArray();
            return Normalize(data);
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

        private static double[] Normalize(double[] data)
        {
            if (data.Length <= 1)
            {
                return data;
            }

            Stats.zscore_norm(data.AsSpan());
            return data;
        }
    }
}
