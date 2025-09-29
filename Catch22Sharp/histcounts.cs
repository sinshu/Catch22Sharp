using System;

namespace Catch22Sharp
{
    public static class HistCounts
    {
        public static int num_bins_auto(Span<double> y)
        {
            if (y.Length == 0)
            {
                return 0;
            }

            double maxVal = Stats.max_(y);
            double minVal = Stats.min_(y);
            double std = Stats.stddev(y);

            if (std < 0.001)
            {
                return 0;
            }

            double denominator = 3.5 * std / Math.Pow(y.Length, 1.0 / 3.0);
            if (denominator == 0.0)
            {
                return 0;
            }

            double bins = (maxVal - minVal) / denominator;
            return (int)Math.Ceiling(bins);
        }

        public static void histcounts_preallocated(Span<double> y, int nBins, Span<int> binCounts, Span<double> binEdges)
        {
            double minVal = double.MaxValue;
            double maxVal = double.MinValue;
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] < minVal)
                {
                    minVal = y[i];
                }
                if (y[i] > maxVal)
                {
                    maxVal = y[i];
                }
            }

            double binStep = nBins != 0 ? (maxVal - minVal) / nBins : 0.0;

            for (int i = 0; i < nBins; i++)
            {
                binCounts[i] = 0;
            }

            for (int i = 0; i < y.Length; i++)
            {
                double ratio = binStep != 0.0 ? (y[i] - minVal) / binStep : 0.0;
                int binInd = (int)ratio;
                if (binInd < 0)
                {
                    binInd = 0;
                }
                if (binInd >= nBins)
                {
                    binInd = nBins - 1;
                }

                if (binInd >= 0 && binInd < nBins)
                {
                    binCounts[binInd] += 1;
                }
            }

            for (int i = 0; i < nBins + 1; i++)
            {
                binEdges[i] = i * binStep + minVal;
            }
        }

        public static int histcounts(Span<double> y, int nBins, out int[] binCounts, out double[] binEdges)
        {
            double minVal = double.MaxValue;
            double maxVal = double.MinValue;
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] < minVal)
                {
                    minVal = y[i];
                }
                if (y[i] > maxVal)
                {
                    maxVal = y[i];
                }
            }

            if (nBins <= 0)
            {
                nBins = num_bins_auto(y);
            }

            double binStep = nBins != 0 ? (maxVal - minVal) / nBins : 0.0;

            binCounts = new int[Math.Max(nBins, 0)];
            for (int i = 0; i < binCounts.Length; i++)
            {
                binCounts[i] = 0;
            }

            for (int i = 0; i < y.Length; i++)
            {
                double ratio = binStep != 0.0 ? (y[i] - minVal) / binStep : 0.0;
                int binInd = (int)ratio;
                if (binInd < 0)
                {
                    binInd = 0;
                }
                if (binInd >= nBins)
                {
                    binInd = nBins - 1;
                }

                if (nBins > 0 && binInd >= 0 && binInd < nBins)
                {
                    binCounts[binInd] += 1;
                }
            }

            binEdges = new double[Math.Max(nBins, 0) + 1];
            for (int i = 0; i < binEdges.Length; i++)
            {
                binEdges[i] = i * binStep + minVal;
            }

            return nBins;
        }

        public static int[] histbinassign(Span<double> y, Span<double> binEdges)
        {
            int[] binIdentity = new int[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                binIdentity[i] = 0;
                for (int j = 0; j < binEdges.Length; j++)
                {
                    if (y[i] < binEdges[j])
                    {
                        binIdentity[i] = j;
                        break;
                    }
                }
            }

            return binIdentity;
        }

        public static int[] histcount_edges(Span<double> y, Span<double> binEdges)
        {
            int[] histcounts = new int[binEdges.Length];
            for (int i = 0; i < y.Length; i++)
            {
                for (int j = 0; j < binEdges.Length; j++)
                {
                    if (y[i] <= binEdges[j])
                    {
                        histcounts[j] += 1;
                        break;
                    }
                }
            }

            return histcounts;
        }
    }
}
