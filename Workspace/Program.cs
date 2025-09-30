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
        var data = GetData(Path.Combine(testDataDirectory, "test.txt"));
        var catch22 = data.Catch22();
        foreach (var (name, value) in catch22.GetNameValuePairs())
        {
            Console.WriteLine($"{name}, {value}");
        }
    }

    private static double[] GetData(string path)
    {
        return File.ReadLines(path).Select(double.Parse).ToArray();
    }
}
