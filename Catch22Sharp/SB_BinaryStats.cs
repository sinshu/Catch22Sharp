using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double SB_BinaryStats_diff_longstretch0(ReadOnlySpan<double> y)
        {
            int size = y.Length;

            // NaN check
            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            int[] yBin = new int[size - 1];
            for (int i = 0; i < size - 1; i++)
            {
                double diffTemp = y[i + 1] - y[i];
                yBin[i] = diffTemp < 0 ? 0 : 1;
            }

            int maxstretch0 = 0;
            int last1 = 0;
            for (int i = 0; i < size - 1; i++)
            {
                if (yBin[i] == 1 | i == size - 2)
                {
                    int stretch0 = i - last1;
                    if (stretch0 > maxstretch0)
                    {
                        maxstretch0 = stretch0;
                    }
                    last1 = i;
                }
            }

            return maxstretch0;
        }

        public static double SB_BinaryStats_mean_longstretch1(ReadOnlySpan<double> y)
        {
            int size = y.Length;

            // NaN check
            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            int[] yBin = new int[size - 1];
            double yMean = Stats.mean(y);
            for (int i = 0; i < size - 1; i++)
            {
                yBin[i] = (y[i] - yMean <= 0) ? 0 : 1;
            }

            int maxstretch1 = 0;
            int last1 = 0;
            for (int i = 0; i < size - 1; i++)
            {
                if (yBin[i] == 0 | i == size - 2)
                {
                    int stretch1 = i - last1;
                    if (stretch1 > maxstretch1)
                    {
                        maxstretch1 = stretch1;
                    }
                    last1 = i;
                }
            }

            return maxstretch1;
        }
    }
}
