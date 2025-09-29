using System;

namespace Catch22Sharp
{
    public static class HelperFunctions
    {
        public static void sort(Span<double> values)
        {
            double[] copy = values.ToArray();
            Array.Sort(copy);
            copy.AsSpan().CopyTo(values);
        }

        public static void linspace(double start, double end, int numGroups, Span<double> output)
        {
            if (numGroups <= 0)
            {
                return;
            }

            double stepSize = numGroups == 1 ? 0.0 : (end - start) / (numGroups - 1);
            for (int i = 0; i < numGroups; i++)
            {
                output[i] = start;
                start += stepSize;
            }
        }

        public static double quantile(Span<double> values, double quant)
        {
            int size = values.Length;
            if (size == 0)
            {
                return double.NaN;
            }

            double[] tmp = values.ToArray();
            Array.Sort(tmp);

            double q = 0.5 / size;
            if (quant < q)
            {
                return tmp[0];
            }

            if (quant > 1 - q)
            {
                return tmp[size - 1];
            }

            double quantIdx = size * quant - 0.5;
            int idxLeft = (int)Math.Floor(quantIdx);
            int idxRight = (int)Math.Ceiling(quantIdx);
            if (idxRight == idxLeft)
            {
                return tmp[idxLeft];
            }

            double value = tmp[idxLeft] + (quantIdx - idxLeft) * (tmp[idxRight] - tmp[idxLeft]) / (idxRight - idxLeft);
            return value;
        }

        public static void binarize(Span<double> input, Span<int> output, string how)
        {
            double threshold = 0.0;
            if (how == "mean")
            {
                threshold = Stats.mean(input);
            }
            else if (how == "median")
            {
                threshold = Stats.median(input);
            }

            for (int i = 0; i < input.Length && i < output.Length; i++)
            {
                output[i] = input[i] > threshold ? 1 : 0;
            }
        }

        public static double f_entropy(Span<double> input)
        {
            double entropy = 0.0;
            for (int i = 0; i < input.Length; i++)
            {
                double value = input[i];
                if (value > 0)
                {
                    entropy += value * Math.Log(value);
                }
            }

            return -entropy;
        }

        public static void subset(Span<int> input, int start, int end, Span<int> output)
        {
            int j = 0;
            for (int i = start; i < end && j < output.Length; i++)
            {
                output[j++] = input[i];
            }
        }
    }
}
