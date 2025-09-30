using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double MD_hrv_classic_pnn40(Span<double> y, int size)
        {
            Span<double> ySpan = y.Slice(0, size);

            // NaN check
            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(ySpan[i]))
                {
                    return double.NaN;
                }
            }

            const int pNNx = 40;

            // compute diff
            double[] Dy = new double[size - 1];
            Stats.diff(ySpan, Dy.AsSpan());

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
