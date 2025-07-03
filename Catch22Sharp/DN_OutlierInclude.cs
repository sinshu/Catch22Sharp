using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double DN_OutlierInclude_np_001_mdrmd(Span<double> y, int sign)
        {
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            double inc = 0.01;
            int tot = 0;
            double[] yWork = new double[y.Length];

            // apply sign and check constant time series
            bool constantFlag = true;
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] != y[0])
                {
                    constantFlag = false;
                }
                yWork[i] = sign * y[i];
                if (yWork[i] >= 0)
                {
                    tot += 1;
                }
            }
            if (constantFlag)
            {
                return 0;
            }

            double maxVal = Stats.max_(yWork.AsSpan());
            if (maxVal < inc)
            {
                return 0;
            }

            int nThresh = (int)(maxVal / inc) + 1;
            double[] r = new double[y.Length];
            double[] msDti1 = new double[nThresh];
            double[] msDti3 = new double[nThresh];
            double[] msDti4 = new double[nThresh];

            for (int j = 0; j < nThresh; j++)
            {
                int highSize = 0;
                for (int i = 0; i < y.Length; i++)
                {
                    if (yWork[i] >= j * inc)
                    {
                        r[highSize] = i + 1;
                        highSize += 1;
                    }
                }
                double[] Dt_exc = new double[Math.Max(0, highSize - 1)];
                for (int i = 0; i < highSize - 1; i++)
                {
                    Dt_exc[i] = r[i + 1] - r[i];
                }
                msDti1[j] = highSize > 1 ? Stats.mean(Dt_exc.AsSpan(0, highSize - 1)) : 0.0;
                msDti3[j] = (highSize - 1) * 100.0 / tot;
                msDti4[j] = highSize > 0 ? Stats.median(r.AsSpan(0, highSize)) / ((double)y.Length / 2) - 1 : 0.0;
            }

            int trimthr = 2;
            int mj = 0;
            int fbi = nThresh - 1;
            for (int i = 0; i < nThresh; i++)
            {
                if (msDti3[i] > trimthr)
                {
                    mj = i;
                }
                if (double.IsNaN(msDti1[nThresh - 1 - i]))
                {
                    fbi = nThresh - 1 - i;
                }
            }
            int trimLimit = Math.Min(mj, fbi);
            double outputScalar = Stats.median(msDti4.AsSpan(0, trimLimit + 1));
            return outputScalar;
        }

        public static double DN_OutlierInclude_p_001_mdrmd(Span<double> y)
        {
            return DN_OutlierInclude_np_001_mdrmd(y, 1);
        }

        public static double DN_OutlierInclude_n_001_mdrmd(Span<double> y)
        {
            return DN_OutlierInclude_np_001_mdrmd(y, -1);
        }

        public static double DN_OutlierInclude_abs_001(Span<double> y)
        {
            double inc = 0.01;
            double maxAbs = 0;
            double[] yAbs = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                yAbs[i] = Math.Abs(y[i]);
                if (yAbs[i] > maxAbs)
                {
                    maxAbs = yAbs[i];
                }
            }
            int nThresh = (int)(maxAbs / inc) + 1;
            double[] highInds = new double[y.Length];
            double[] msDti3 = new double[nThresh];
            double[] msDti4 = new double[nThresh];
            for (int j = 0; j < nThresh; j++)
            {
                int highSize = 0;
                for (int i = 0; i < y.Length; i++)
                {
                    if (yAbs[i] >= j * inc)
                    {
                        highInds[highSize] = i;
                        highSize += 1;
                    }
                }
                double medianOut = highSize > 0 ? Stats.median(highInds.AsSpan(0, highSize)) : 0.0;
                msDti3[j] = (highSize - 1) * 100.0 / y.Length;
                msDti4[j] = (y.Length > 0) ? medianOut / (y.Length / 2.0) - 1 : 0.0;
            }
            int trimthr = 2;
            int mj = 0;
            for (int i = 0; i < nThresh; i++)
            {
                if (msDti3[i] > trimthr)
                {
                    mj = i;
                }
            }
            double outputScalar = Stats.median(msDti4.AsSpan(0, mj));
            return outputScalar;
        }
    }
}
