using System;

namespace Catch22Sharp
{
    public partial class Catch22
    {
        public static double DN_OutlierInclude_np_001_mdrmd(ReadOnlySpan<double> y, int sign)
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

            double inc = 0.01;
            int tot = 0;
            double[] yWork = new double[size];

            // apply sign and check constant time series
            bool constantFlag = true;
            for (int i = 0; i < size; i++)
            {
                if (y[i] != y[0])
                {
                    constantFlag = false;
                }

                // apply sign, save in new variable
                yWork[i] = sign * y[i];

                // count pos/ negs
                if (yWork[i] >= 0)
                {
                    tot += 1;
                }
            }
            if (constantFlag)
            {
                return 0;
            }

            // find maximum (or minimum, depending on sign)
            double maxVal = Stats.max_(yWork.AsSpan());

            // maximum value too small? return 0
            if (maxVal < inc)
            {
                return 0;
            }

            int nThresh = (int)(maxVal / inc) + 1;

            // save the indices where y > threshold
            double[] r = new double[size];

            // save the median over indices with absolute value > threshold
            double[] msDti1 = new double[nThresh];
            double[] msDti3 = new double[nThresh];
            double[] msDti4 = new double[nThresh];

            for (int j = 0; j < nThresh; j++)
            {
                //printf("j=%i, thr=%1.3f\\n", j, j*inc);

                int highSize = 0;

                for (int i = 0; i < size; i++)
                {
                    if (yWork[i] >= j * inc)
                    {
                        r[highSize] = i + 1;
                        //printf("r[%i]=%1.f \\n", highSize, r[highSize]);
                        highSize += 1;
                    }
                }

                // intervals between high-values
                double[] dtExc = new double[Math.Max(0, highSize - 1)];

                for (int i = 0; i < highSize - 1; i++)
                {
                    //printf("i=%i, r[i+1]=%1.f, r[i]=%1.f \\n", i, r[i+1], r[i]);
                    dtExc[i] = r[i + 1] - r[i];
                }

                msDti1[j] = highSize > 1 ? Stats.mean(dtExc.AsSpan(0, highSize - 1)) : double.NaN;
                msDti3[j] = (highSize - 1) * 100.0 / tot;
                msDti4[j] = highSize > 0 ? Stats.median(r.AsSpan(0, highSize)) / (size / 2.0) - 1 : 0.0;

                //printf("msDti1[%i] = %1.3f, msDti13[%i] = %1.3f, msDti4[%i] = %1.3f\\n",
                //       j, msDti1[j], j, msDti3[j], j, msDti4[j]);
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

            double outputScalar;
            int trimLimit = mj < fbi ? mj : fbi;
            outputScalar = Stats.median(msDti4.AsSpan(0, trimLimit + 1));

            return outputScalar;
        }

        public static double DN_OutlierInclude_p_001_mdrmd(ReadOnlySpan<double> y)
        {
            return DN_OutlierInclude_np_001_mdrmd(y, 1);
        }

        public static double DN_OutlierInclude_n_001_mdrmd(ReadOnlySpan<double> y)
        {
            return DN_OutlierInclude_np_001_mdrmd(y, -1);
        }

        public static double DN_OutlierInclude_abs_001(ReadOnlySpan<double> y)
        {
            int size = y.Length;
            double inc = 0.01;
            double maxAbs = 0;
            double[] yAbs = new double[size];

            for (int i = 0; i < size; i++)
            {
                // yAbs[i] = (y[i] > 0) ? y[i] : -y[i];
                yAbs[i] = Math.Abs(y[i]);

                if (yAbs[i] > maxAbs)
                {
                    maxAbs = yAbs[i];
                }
            }

            int nThresh = (int)(maxAbs / inc) + 1;

            // save the indices where y > threshold
            double[] highInds = new double[size];

            // save the median over indices with absolute value > threshold
            double[] msDti3 = new double[nThresh];
            double[] msDti4 = new double[nThresh];

            for (int j = 0; j < nThresh; j++)
            {
                int highSize = 0;

                for (int i = 0; i < size; i++)
                {
                    if (yAbs[i] >= j * inc)
                    {
                        // fprintf(stdout, "%i, ", i);

                        highInds[highSize] = i;
                        highSize += 1;
                    }
                }

                // median
                double medianOut = Stats.median(highInds.AsSpan(0, highSize));

                msDti3[j] = (highSize - 1) * 100.0 / size;
                msDti4[j] = medianOut / (size / 2.0) - 1;
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
