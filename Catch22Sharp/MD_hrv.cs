using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double MD_hrv_classic_pnn40(Span<double> y, int size)
        {
            Span<double> ySpan = size < y.Length ? y.Slice(0, size) : y;

            for (int i = 0; i < ySpan.Length; i++)
            {
                if (double.IsNaN(ySpan[i]))
                {
                    return double.NaN;
                }
            }

            if (ySpan.Length <= 1)
            {
                return double.NaN;
            }

            const int pNNx = 40;

            double[] Dy = new double[ySpan.Length - 1];
            Stats.diff(ySpan, Dy.AsSpan());

            double pnn40 = 0.0;
            for (int i = 0; i < Dy.Length; i++)
            {
                if (Math.Abs(Dy[i]) * 1000.0 > pNNx)
                {
                    pnn40 += 1.0;
                }
            }

            return pnn40 / (ySpan.Length - 1);
        }
    }
}
