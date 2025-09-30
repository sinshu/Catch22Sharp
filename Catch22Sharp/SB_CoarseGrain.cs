using System;

namespace Catch22Sharp
{
    public partial class Catch22
    {
        public static void sb_coarsegrain(ReadOnlySpan<double> y, string how, int numGroups, Span<int> labels)
        {
            if (string.CompareOrdinal(how, "quantile") == 1)
            {
                throw new InvalidOperationException("ERROR in sb_coarsegrain: unknown coarse-graining method");
            }

            int size = y.Length;
            if (labels.Length < size)
            {
                throw new ArgumentException("labels span must be at least as long as y", nameof(labels));
            }

            double[] th = new double[numGroups + 1];
            double[] ls = new double[numGroups + 1];

            HelperFunctions.linspace(0, 1, numGroups + 1, ls.AsSpan());
            for (int i = 0; i < numGroups + 1; i++)
            {
                th[i] = HelperFunctions.quantile(y, ls[i]);
            }

            th[0] -= 1;

            for (int i = 0; i < numGroups; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (y[j] > th[i] && y[j] <= th[i + 1])
                    {
                        labels[j] = i + 1;
                    }
                }
            }
        }
    }
}
