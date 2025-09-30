using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double DN_HistogramMode_10(Span<double> y)
        {
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            const int nBins = 10;

            HistCounts.histcounts(y, nBins, out int[] histCounts, out double[] binEdges);

            double maxCount = 0;
            int numMaxs = 1;
            double @out = 0;
            for (int i = 0; i < nBins; i++)
            {
                // printf("binInd=%i, binCount=%i, binEdge=%1.3f \\n", i, histCounts[i], binEdges[i]);

                if (histCounts[i] > maxCount)
                {
                    maxCount = histCounts[i];
                    numMaxs = 1;
                    @out = (binEdges[i] + binEdges[i + 1]) * 0.5;
                }
                else if (histCounts[i] == maxCount)
                {
                    numMaxs += 1;
                    @out += (binEdges[i] + binEdges[i + 1]) * 0.5;
                }
            }
            @out = @out / numMaxs;

            return @out;
        }
    }
}
