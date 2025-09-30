using System;

namespace Catch22Sharp
{
    public partial class Catch22
    {
        public static double IN_AutoMutualInfoStats_40_gaussian_fmmi(ReadOnlySpan<double> y)
        {
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            // maximum time delay
            int tau = 40;

            // don't go above half the signal length
            if (tau > Math.Ceiling(y.Length / 2.0))
            {
                tau = (int)Math.Ceiling(y.Length / 2.0);
            }

            // compute autocorrelations and compute automutual information
            double[] ami = new double[y.Length];
            for (int i = 0; i < tau; i++)
            {
                double ac = Stats.autocorr_lag(y, i + 1);
                ami[i] = -0.5 * Math.Log(1 - ac * ac);
                // printf("ami[%i]=%1.7f\\n", i, ami[i]);
            }

            // find first minimum of automutual information
            double fmmi = tau;
            for (int i = 1; i < tau - 1; i++)
            {
                if (ami[i] < ami[i - 1] & ami[i] < ami[i + 1])
                {
                    fmmi = i;
                    // printf("found minimum at %i\\n", i);
                    break;
                }
            }

            return fmmi;
        }
    }
}
