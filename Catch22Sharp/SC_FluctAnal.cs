using System;

namespace Catch22Sharp
{
    public partial class Catch22
    {
        private static double SC_FluctAnal_2_50_1_logi_prop_r1(ReadOnlySpan<double> y, int lag, string how)
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

            // generate log spaced tau vector
            double linLow = Math.Log(5);
            double linHigh = Math.Log(size / 2);

            int nTauSteps = 50;
            double tauStep = (linHigh - linLow) / (nTauSteps - 1);

            int[] tau = new int[nTauSteps];
            for (int i = 0; i < nTauSteps; i++)
            {
                tau[i] = (int)Math.Round(Math.Exp(linLow + i * tauStep));
            }

            // check for uniqueness, use ascending order
            int nTau = nTauSteps;
            for (int i = 0; i < nTauSteps - 1; i++)
            {
                while (i < nTau - 1 && tau[i] == tau[i + 1])
                {
                    for (int j = i + 1; j < nTauSteps - 1; j++)
                    {
                        tau[j] = tau[j + 1];
                    }
                    nTau -= 1;
                }
            }

            // fewer than 12 points -> leave.
            if (nTau < 12)
            {
                return 0;
            }

            int sizeCS = size / lag;
            double[] yCS = new double[sizeCS];

            // transform input vector to cumsum
            yCS[0] = y[0];
            for (int i = 0; i < sizeCS - 1; i++)
            {
                yCS[i + 1] = yCS[i] + y[(i + 1) * lag];
            }

            // first generate a support for regression (detrending)
            double[] xReg = new double[tau[nTau - 1]];
            for (int i = 0; i < tau[nTau - 1]; i++)
            {
                xReg[i] = i + 1;
            }

            // iterate over taus, cut signal, detrend and save amplitude of remaining signal
            double[] F = new double[nTau];
            for (int i = 0; i < nTau; i++)
            {
                int nBuffer = sizeCS / tau[i];
                double[] buffer = new double[tau[i]];
                double m = 0.0, b = 0.0;

                F[i] = 0;
                for (int j = 0; j < nBuffer; j++)
                {
                    Stats.linreg(xReg.AsSpan(0, tau[i]), yCS.AsSpan(j * tau[i], tau[i]), out m, out b);

                    for (int k = 0; k < tau[i]; k++)
                    {
                        buffer[k] = yCS[j * tau[i] + k] - (m * (k + 1) + b);
                    }

                    if (string.Equals(how, "rsrangefit", StringComparison.Ordinal))
                    {
                        F[i] += Math.Pow(Stats.max_(buffer.AsSpan()) - Stats.min_(buffer.AsSpan()), 2);
                    }
                    else if (string.Equals(how, "dfa", StringComparison.Ordinal))
                    {
                        for (int k = 0; k < tau[i]; k++)
                        {
                            F[i] += buffer[k] * buffer[k];
                        }
                    }
                    else
                    {
                        return 0.0;
                    }
                }

                if (string.Equals(how, "rsrangefit", StringComparison.Ordinal))
                {
                    F[i] = Math.Sqrt(F[i] / nBuffer);
                }
                else if (string.Equals(how, "dfa", StringComparison.Ordinal))
                {
                    F[i] = Math.Sqrt(F[i] / (nBuffer * tau[i]));
                }
            }

            double[] logtt = new double[nTau];
            double[] logFF = new double[nTau];
            int ntt = nTau;

            for (int i = 0; i < nTau; i++)
            {
                logtt[i] = Math.Log(tau[i]);
                logFF[i] = Math.Log(F[i]);
            }

            int minPoints = 6;
            int nsserr = ntt - 2 * minPoints + 1;
            double[] sserr = new double[nsserr];
            double[] buffer2 = new double[ntt - minPoints + 1];
            for (int i = minPoints; i < ntt - minPoints + 1; i++)
            {
                double m1 = 0.0, b1 = 0.0;
                double m2 = 0.0, b2 = 0.0;

                sserr[i - minPoints] = 0.0;

                Stats.linreg(logtt.AsSpan(0, i), logFF.AsSpan(0, i), out m1, out b1);
                Stats.linreg(logtt.AsSpan(i - 1, ntt - i + 1), logFF.AsSpan(i - 1, ntt - i + 1), out m2, out b2);

                for (int j = 0; j < i; j++)
                {
                    buffer2[j] = logtt[j] * m1 + b1 - logFF[j];
                }

                sserr[i - minPoints] += Stats.norm_(buffer2.AsSpan(0, i));

                for (int j = 0; j < ntt - i + 1; j++)
                {
                    buffer2[j] = logtt[j + i - 1] * m2 + b2 - logFF[j + i - 1];
                }

                sserr[i - minPoints] += Stats.norm_(buffer2.AsSpan(0, ntt - i + 1));
            }

            double firstMinInd = 0.0;
            double minimum = Stats.min_(sserr.AsSpan());
            for (int i = 0; i < nsserr; i++)
            {
                if (sserr[i] == minimum)
                {
                    firstMinInd = i + minPoints - 1;
                    break;
                }
            }

            return (firstMinInd + 1) / ntt;
        }

        public static double SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1(ReadOnlySpan<double> y)
        {
            return SC_FluctAnal_2_50_1_logi_prop_r1(y, 2, "dfa");
        }

        public static double SC_FluctAnal_2_rsrangefit_50_1_logi_prop_r1(ReadOnlySpan<double> y)
        {
            return SC_FluctAnal_2_50_1_logi_prop_r1(y, 1, "rsrangefit");
        }
    }
}
