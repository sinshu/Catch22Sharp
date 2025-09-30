using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double MD_hrv_classic_pnn40(ReadOnlySpan<double> y)
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

            const int pNNx = 40;

            // compute diff
            double[] Dy = new double[size - 1];
            Stats.diff(y, Dy.AsSpan());

            double pnn40 = 0;
            for (int i = 0; i < size - 1; i++)
            {
                if (Math.Abs(Dy[i]) * 1000 > pNNx)
                {
                    pnn40 += 1;
                }
            }

            return pnn40 / (size - 1);
        }
    }
}
