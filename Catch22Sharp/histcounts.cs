using System;

namespace Catch22Sharp
{
    public static class HistCounts
    {
        public static int num_bins_auto(Span<double> y)
        {
            int size = y.Length;

            double maxVal = Stats.max_(y);
            double minVal = Stats.min_(y);

            if (Stats.stddev(y) < 0.001)
            {
                return 0;
            }

            return (int)Math.Ceiling((maxVal - minVal) / (3.5 * Stats.stddev(y) / Math.Pow(size, 1 / 3.0)));
        }

        public static int histcounts_preallocated(Span<double> y, int nBins, Span<int> binCounts, Span<double> binEdges)
        {
            int size = y.Length;

            // check min and max of input array
            double minVal = double.MaxValue;
            double maxVal = -double.MaxValue;
            for (int i = 0; i < size; i++)
            {
                // printf("histcountInput %i: %1.3f\\n", i, y[i]);

                if (y[i] < minVal)
                {
                    minVal = y[i];
                }
                if (y[i] > maxVal)
                {
                    maxVal = y[i];
                }
            }

            // and derive bin width from it
            double binStep = (maxVal - minVal) / nBins;

            // variable to store counted occurances in
            for (int i = 0; i < nBins; i++)
            {
                binCounts[i] = 0;
            }

            for (int i = 0; i < size; i++)
            {
                int binInd = (int)((y[i] - minVal) / binStep);
                if (binInd < 0)
                {
                    binInd = 0;
                }
                if (binInd >= nBins)
                {
                    binInd = nBins - 1;
                }
                //printf("histcounts, i=%i, binInd=%i, nBins=%i\\n", i, binInd, nBins);
                binCounts[binInd] += 1;
            }

            for (int i = 0; i < nBins + 1; i++)
            {
                binEdges[i] = i * binStep + minVal;
            }

            /*
             // debug
             for(i=0;i<nBins;i++)
             {
             printf("%i: count %i, edge %1.3f\\n", i, binCounts[i], binEdges[i]);
             }
             */

            return 0;
        }

        public static int histcounts(Span<double> y, int nBins, out int[] binCounts, out double[] binEdges)
        {
            int size = y.Length;

            // check min and max of input array
            double minVal = double.MaxValue;
            double maxVal = -double.MaxValue;
            for (int i = 0; i < size; i++)
            {
                // printf("histcountInput %i: %1.3f\\n", i, y[i]);

                if (y[i] < minVal)
                {
                    minVal = y[i];
                }
                if (y[i] > maxVal)
                {
                    maxVal = y[i];
                }
            }

            // if no number of bins given, choose spaces automatically
            if (nBins <= 0)
            {
                nBins = (int)Math.Ceiling((maxVal - minVal) / (3.5 * Stats.stddev(y) / Math.Pow(size, 1 / 3.0)));
            }

            // and derive bin width from it
            double binStep = (maxVal - minVal) / nBins;

            // variable to store counted occurances in
            binCounts = new int[nBins];
            for (int i = 0; i < nBins; i++)
            {
                binCounts[i] = 0;
            }

            for (int i = 0; i < size; i++)
            {
                int binInd = (int)((y[i] - minVal) / binStep);
                if (binInd < 0)
                {
                    binInd = 0;
                }
                if (binInd >= nBins)
                {
                    binInd = nBins - 1;
                }
                binCounts[binInd] += 1;
            }

            binEdges = new double[nBins + 1];
            for (int i = 0; i < nBins + 1; i++)
            {
                binEdges[i] = i * binStep + minVal;
            }

            /*
            // debug
            for(i=0;i<nBins;i++)
            {
                printf("%i: count %i, edge %1.3f\\n", i, binCounts[i], binEdges[i]);
            }
            */

            return nBins;
        }

        public static int[] histbinassign(Span<double> y, Span<double> binEdges)
        {
            int size = y.Length;

            // variable to store counted occurances in
            int[] binIdentity = new int[size];
            for (int i = 0; i < size; i++)
            {
                // if not in any bin -> 0
                binIdentity[i] = 0;

                // go through bin edges
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
            int size = y.Length;
            int nEdges = binEdges.Length;

            int[] histcounts = new int[nEdges];
            for (int i = 0; i < nEdges; i++)
            {
                histcounts[i] = 0;
            }

            for (int i = 0; i < size; i++)
            {
                // go through bin edges
                for (int j = 0; j < nEdges; j++)
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
