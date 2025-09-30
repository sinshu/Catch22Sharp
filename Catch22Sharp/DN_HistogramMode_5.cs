using System;

namespace Catch22Sharp
{
    public partial class Catch22
    {
        public static double DN_HistogramMode_5(ReadOnlySpan<double> y)
        {
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            const int nBins = 5;

            HistCounts.histcounts(y, nBins, out int[] histCounts, out double[] binEdges);

            /*
            for(int i = 0; i < nBins; i++){
                printf("histCounts[%i] = %i\\n", i, histCounts[i]);
            }
            for(int i = 0; i < nBins+1; i++){
                printf("binEdges[%i] = %1.3f\\n", i, binEdges[i]);
            }
             */

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
