using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Catch22SharpTest
{
    [TestClass]
    public class TestData
    {
        private static readonly string testDataDirectory = @"..\..\..\..\testData";

        public static double[] Test1 = default!;
        public static Dictionary<string, double> Test1Output = default!;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            Test1 = GetData(Path.Combine(testDataDirectory, "test.txt"));
            Test1Output = GetExpectedValues(Path.Combine(testDataDirectory, "test_output.txt"));
        }

        [TestMethod]
        public void Test1Data()
        {
            Assert.IsNotNull(Test1);
            Assert.IsNotNull(Test1Output);
        }

        private static double[] GetData(string path)
        {
            return File.ReadLines(path).Select(double.Parse).ToArray();
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
