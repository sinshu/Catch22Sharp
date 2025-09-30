using System;
using System.Numerics;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        private static int nextpow2(int n)
        {
            if (n <= 0)
            {
                return 1;
            }

            n--;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            n++;
            return n;
        }

        private static void dot_multiply(Span<Complex> a, Span<Complex> b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Span lengths must match.");
            }

            for (int i = 0; i < a.Length; i++)
            {
                Complex valueA = a[i];
                Complex valueB = b[i];
                a[i] = valueA * Complex.Conjugate(valueB);
            }
        }

        private static double[] co_autocorrs(Span<double> y)
        {
            int size = y.Length;
            if (size == 0)
            {
                return Array.Empty<double>();
            }

            double mean = Stats.mean(y);
            double zeroLag = 0.0;
            int nFFT = nextpow2(size) << 1;
            Complex[] spectrum = new Complex[nFFT];

            for (int i = 0; i < size; i++)
            {
                double centered = y[i] - mean;
                spectrum[i] = new Complex(centered, 0.0);
                zeroLag += centered * centered;
            }

            if (zeroLag == 0.0)
            {
                double[] constant = new double[size];
                if (constant.Length > 0)
                {
                    constant[0] = 1.0;
                }
                return constant;
            }

            Complex[] twiddles = new Complex[nFFT];
            Fft.twiddles(twiddles.AsSpan());
            dot_multiply_fft(spectrum.AsSpan(), twiddles.AsSpan());

            double divisor = spectrum[0].Real;
            if (divisor == 0.0)
            {
                divisor = zeroLag;
            }

            double[] autocorrs = new double[nFFT];
            if (divisor == 0.0)
            {
                return autocorrs;
            }

            for (int i = 0; i < nFFT; i++)
            {
                autocorrs[i] = spectrum[i].Real / divisor;
            }

            return autocorrs;
        }

        private static void dot_multiply_fft(Span<Complex> spectrum, Span<Complex> twiddles)
        {
            Fft.fft(spectrum, twiddles);
            dot_multiply(spectrum, spectrum);
            Fft.fft(spectrum, twiddles);
        }

        private static int co_firstzero(Span<double> y, int maxtau)
        {
            double[] autocorrs = co_autocorrs(y);
            int zerocrossind = 0;
            int limit = Math.Min(maxtau, autocorrs.Length);
            while (zerocrossind < limit && autocorrs[zerocrossind] > 0)
            {
                zerocrossind += 1;
            }
            return zerocrossind;
        }

        public static double[] CO_AutoCorr(Span<double> y, Span<int> tau)
        {
            double[] autocorrs = co_autocorrs(y);
            double[] output = new double[tau.Length];
            for (int i = 0; i < tau.Length; i++)
            {
                int idx = tau[i];
                output[i] = idx < autocorrs.Length ? autocorrs[idx] : 0.0;
            }
            return output;
        }

        public static double CO_f1ecac(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return 0;
                }
            }

            double[] autocorrs = co_autocorrs(y);
            double thresh = 1.0 / Math.Exp(1.0);
            double outVal = y.Length;
            for (int i = 0; i < y.Length - 2 && i + 1 < autocorrs.Length; i++)
            {
                if (autocorrs[i + 1] < thresh)
                {
                    double m = autocorrs[i + 1] - autocorrs[i];
                    double dy = thresh - autocorrs[i];
                    double dx = m != 0 ? dy / m : 0.0;
                    outVal = i + dx;
                    return outVal;
                }
            }

            return outVal;
        }

        public static double CO_Embed2_Basic_tau_incircle(Span<double> y, double radius, int tau)
        {
            int tauIntern = tau < 0 ? co_firstzero(y, y.Length) : tau;
            int denom = y.Length - tauIntern;
            if (denom <= 0)
            {
                return 0.0;
            }

            double insidecount = 0;
            for (int i = 0; i < denom; i++)
            {
                double v1 = y[i];
                double v2 = y[i + tauIntern];
                if (v1 * v1 + v2 * v2 < radius)
                {
                    insidecount += 1.0;
                }
            }

            return insidecount / denom;
        }

        public static double CO_Embed2_Dist_tau_d_expfit_meandiff(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            double[] yZscored = new double[y.Length];
            Stats.zscore_norm2(y, yZscored);
            Span<double> yWork = yZscored;

            int tau = co_firstzero(yWork, yWork.Length);
            if (tau > y.Length / 10)
            {
                tau = (int)Math.Floor(y.Length / 10.0);
            }

            if (tau <= 0 || yWork.Length - tau - 1 <= 0)
            {
                return 0.0;
            }

            int validLength = yWork.Length - tau - 1;
            double[] d = new double[yWork.Length - tau];
            for (int i = 0; i < validLength; i++)
            {
                double diff1 = yWork[i + 1] - yWork[i];
                double diff2 = yWork[i + tau] - yWork[i + tau + 1];
                d[i] = Math.Sqrt(diff1 * diff1 + diff2 * diff2);
                if (double.IsNaN(d[i]))
                {
                    return double.NaN;
                }
            }

            double l = Stats.mean(d.AsSpan(0, validLength));
            int nBins = HistCounts.num_bins_auto(d.AsSpan(0, validLength));
            if (nBins == 0)
            {
                return 0.0;
            }

            int[] histCounts = new int[nBins];
            double[] binEdges = new double[nBins + 1];
            HistCounts.histcounts_preallocated(d.AsSpan(0, validLength), nBins, histCounts.AsSpan(), binEdges.AsSpan());

            double[] histCountsNorm = new double[nBins];
            for (int i = 0; i < nBins; i++)
            {
                histCountsNorm[i] = histCounts[i] / (double)validLength;
            }

            double[] d_expfit_diff = new double[nBins];
            for (int i = 0; i < nBins; i++)
            {
                double mid = (binEdges[i] + binEdges[i + 1]) * 0.5;
                double expf = l != 0 ? Math.Exp(-mid / l) / l : 0.0;
                if (expf < 0)
                {
                    expf = 0.0;
                }
                d_expfit_diff[i] = Math.Abs(histCountsNorm[i] - expf);
            }

            return Stats.mean(d_expfit_diff);
        }

        public static int CO_FirstMin_ac(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return 0;
                }
            }

            double[] autocorrs = co_autocorrs(y);
            int minInd = y.Length;
            for (int i = 1; i < y.Length - 1 && i < autocorrs.Length - 1; i++)
            {
                if (autocorrs[i] < autocorrs[i - 1] && autocorrs[i] < autocorrs[i + 1])
                {
                    minInd = i;
                    break;
                }
            }

            return minInd;
        }

        public static double CO_trev_1_num(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            const int tau = 1;
            if (y.Length <= tau)
            {
                return 0.0;
            }

            double[] yZscored = new double[y.Length];
            Stats.zscore_norm2(y, yZscored);
            Span<double> yWork = yZscored;

            double[] diffTemp = new double[yWork.Length - tau];
            for (int i = 0; i < yWork.Length - tau; i++)
            {
                diffTemp[i] = Math.Pow(yWork[i + 1] - yWork[i], 3);
            }

            return Stats.mean(diffTemp);
        }

        public static double CO_HistogramAMI_even_2_5(Span<double> y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            const int tau = 2;
            const int numBins = 5;
            if (y.Length <= tau)
            {
                return 0.0;
            }

            double[] yZscored = new double[y.Length];
            Stats.zscore_norm2(y, yZscored);
            Span<double> yWork = yZscored;

            int length = yWork.Length - tau;
            double[] y1 = new double[length];
            double[] y2 = new double[length];
            for (int i = 0; i < length; i++)
            {
                y1[i] = yWork[i];
                y2[i] = yWork[i + tau];
            }

            double maxValue = Stats.max_(yWork);
            double minValue = Stats.min_(yWork);
            double binStep = (maxValue - minValue + 0.2) / numBins;
            double[] binEdges = new double[numBins + 1];
            for (int i = 0; i < numBins + 1; i++)
            {
                binEdges[i] = minValue + binStep * i - 0.1;
            }

            int[] bins1 = HistCounts.histbinassign(y1.AsSpan(), binEdges.AsSpan());
            int[] bins2 = HistCounts.histbinassign(y2.AsSpan(), binEdges.AsSpan());

            double[] bins12 = new double[length];
            double[] binEdges12 = new double[(numBins + 1) * (numBins + 1)];
            for (int i = 0; i < length; i++)
            {
                bins12[i] = (bins1[i] - 1) * (numBins + 1) + bins2[i];
            }
            for (int i = 0; i < (numBins + 1) * (numBins + 1); i++)
            {
                binEdges12[i] = i + 1;
            }

            int[] jointHistLinear = HistCounts.histcount_edges(bins12.AsSpan(), binEdges12.AsSpan());

            double[][] pij = new double[numBins][];
            for (int i = 0; i < numBins; i++)
            {
                pij[i] = new double[numBins];
            }

            int sumBins = 0;
            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    int idx = i * (numBins + 1) + j;
                    if (idx < jointHistLinear.Length)
                    {
                        pij[j][i] = jointHistLinear[idx];
                        sumBins += jointHistLinear[idx];
                    }
                }
            }

            if (sumBins == 0)
            {
                return 0.0;
            }

            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    pij[j][i] /= sumBins;
                }
            }

            double[] pi = new double[numBins];
            double[] pj = new double[numBins];
            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    pi[i] += pij[i][j];
                    pj[j] += pij[i][j];
                }
            }

            double ami = 0.0;
            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    if (pij[i][j] > 0 && pi[i] > 0 && pj[j] > 0)
                    {
                        ami += pij[i][j] * Math.Log(pij[i][j] / (pj[j] * pi[i]));
                    }
                }
            }

            return ami;
        }

    }
}

