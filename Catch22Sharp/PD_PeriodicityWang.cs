using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static int PD_PeriodicityWang_th0_01(ReadOnlySpan<double> y)
        {
            int size = y.Length;

            // NaN check
            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return 0;
                }
            }

            const double th = 0.01;

            double[] ySpline = new double[size];

            // fit a spline with 3 nodes to the data
            SplineFit.splinefit(y, ySpline.AsSpan());

            // subtract spline from data to remove trend
            double[] ySub = new double[size];
            for (int i = 0; i < size; i++)
            {
                ySub[i] = y[i] - ySpline[i];
            }

            // compute autocorrelations up to 1/3 of the length of the time series
            int acmax = (int)Math.Ceiling(size / 3.0);

            double[] acf = new double[acmax];
            for (int tau = 1; tau <= acmax; tau++)
            {
                // correlation/ covariance the same, don't care for scaling (cov would be more efficient)
                acf[tau - 1] = Stats.autocov_lag(ySub.AsSpan(), tau);
            }

            // find troughs and peaks
            double[] troughs = new double[acmax];
            double[] peaks = new double[acmax];
            int nTroughs = 0;
            int nPeaks = 0;
            for (int i = 1; i < acmax - 1; i++)
            {
                double slopeIn = acf[i] - acf[i - 1];
                double slopeOut = acf[i + 1] - acf[i];

                if (slopeIn < 0 && slopeOut > 0)
                {
                    // trough at i
                    troughs[nTroughs] = i;
                    nTroughs += 1;
                }
                else if (slopeIn > 0 && slopeOut < 0)
                {
                    // peak at i
                    peaks[nPeaks] = i;
                    nPeaks += 1;
                }
            }

            // search through all peaks for one that meets the conditions:
            // (a) a trough before it
            // (b) difference between peak and trough is at least 0.01
            // (c) peak corresponds to positive correlation
            int @out = 0;

            for (int i = 0; i < nPeaks; i++)
            {
                int iPeak = (int)peaks[i];
                double thePeak = acf[iPeak];

                // find trough before this peak
                int j = -1;
                while (j + 1 < nTroughs && troughs[j + 1] < iPeak)
                {
                    j++;
                }
                if (j == -1)
                {
                    continue;
                }

                int iTrough = (int)troughs[j];
                double theTrough = acf[iTrough];

                // (b) difference between peak and trough is at least 0.01
                if (thePeak - theTrough < th)
                {
                    continue;
                }

                // (c) peak corresponds to positive correlation
                if (thePeak < 0)
                {
                    continue;
                }

                // use this frequency that first fulfils all conditions.
                @out = iPeak;
                break;
            }

            return @out;
        }
    }
}
