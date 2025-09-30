using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double IN_AutoMutualInfoStats_40_gaussian_fmmi(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            int tau = 40;
            int halfLengthCeil = (int)Math.Ceiling(y.Length / 2.0);
            if (tau > halfLengthCeil)
            {
                tau = halfLengthCeil;
            }

            if (tau <= 0)
            {
                return 0.0;
            }

            double[] ami = new double[tau];
            for (int i = 0; i < tau; i++)
            {
                double ac = Stats.autocorr_lag(y, i + 1);
                double term = 1.0 - ac * ac;
                if (term <= 0.0)
                {
                    ami[i] = double.PositiveInfinity;
                }
                else
                {
                    ami[i] = -0.5 * Math.Log(term);
                }
            }

            double fmmi = tau;
            for (int i = 1; i < tau - 1; i++)
            {
                if (ami[i] < ami[i - 1] && ami[i] < ami[i + 1])
                {
                    fmmi = i;
                    break;
                }
            }

            return fmmi;
        }
    }
}
