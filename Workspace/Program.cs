using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Catch22Sharp;

public static class Program
{
    public static void Main(string[] args)
    {
        var testDataDirectory = @"..\..\..\..\testData";
        var expected = GetExpectedValues(Path.Combine(testDataDirectory, "test_output.txt"));
        foreach (var pair in expected)
        {
            Console.WriteLine(pair);
        }

        var data = GetData(Path.Combine(testDataDirectory, "test.txt"));
        var value = Catch22.DN_OutlierInclude_n_001_mdrmd(data);
        Console.WriteLine(value);
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

    private static double[] GetData(string path)
    {
        return File.ReadLines(path).Select(double.Parse).ToArray();
    }
}
