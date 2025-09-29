using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double DN_HistogramMode_10(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            const int nBins = 10;

            double minVal = Stats.min_(y);
            double maxVal = Stats.max_(y);
            if (maxVal == minVal)
            {
                return minVal;
            }

            double[] yZscored = new double[y.Length];
            Stats.zscore_norm2(y, yZscored.AsSpan());
            Span<double> yWork = yZscored;

            double minZ = Stats.min_(yWork);
            double maxZ = Stats.max_(yWork);
            double binStep = (maxZ - minZ) / nBins;
            int[] histCounts = new int[nBins];
            double[] binEdges = new double[nBins + 1];

            for (int i = 0; i < yWork.Length; i++)
            {
                double value = yWork[i];
                int binInd = (int)((value - minZ) / binStep);
                if (binInd < 0)
                {
                    binInd = 0;
                }
                if (binInd >= nBins)
                {
                    binInd = nBins - 1;
                }
                histCounts[binInd] += 1;
            }

            for (int i = 0; i < nBins + 1; i++)
            {
                binEdges[i] = i * binStep + minZ;
            }

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
