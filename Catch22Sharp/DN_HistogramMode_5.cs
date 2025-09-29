using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double DN_HistogramMode_5(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            const int nBins = 5;

            double minVal = Stats.min_(y);
            double maxVal = Stats.max_(y);
            if (maxVal == minVal)
            {
                return minVal;
            }

            double[] yZscored = new double[y.Length];
            Stats.zscore_norm2(y, yZscored.AsSpan());
            Span<double> yWork = yZscored;

            int[] histCounts = new int[nBins];
            double[] binEdges = new double[nBins + 1];
            HistCounts.histcounts_preallocated(yWork, nBins, histCounts.AsSpan(), binEdges.AsSpan());

            double maxCount = 0;
            int numMaxs = 1;
            double outputValue = 0;
            for (int i = 0; i < nBins; i++)
            {
                double binMean = (binEdges[i] + binEdges[i + 1]) * 0.5;
                if (histCounts[i] > maxCount)
                {
                    maxCount = histCounts[i];
                    numMaxs = 1;
                    outputValue = binMean;
                }
                else if (histCounts[i] == maxCount)
                {
                    numMaxs += 1;
                    outputValue += binMean;
                }
            }

            return outputValue / numMaxs;
        }
    }
}
