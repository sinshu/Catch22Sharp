using System;
using System.Numerics;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        private static int nextpow2(int n)
        {
            n--;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            n++;
            return n;
        }

        private static void dot_multiply(Span<Complex> a, Span<Complex> b, int size)
        {
            for (int i = 0; i < size; i++)
            {
                a[i] = a[i] * Complex.Conjugate(b[i]);
            }
        }

        private static double[] co_autocorrs(Span<double> y)
        {
            int size = y.Length;
            double m;
            int nFFT;
            m = Stats.mean(y);
            nFFT = nextpow2(size) << 1;

            Complex[] F = new Complex[nFFT];
            Complex[] tw = new Complex[nFFT];
            for (int i = 0; i < size; i++)
            {
                F[i] = new Complex(y[i] - m, 0.0);
            }
            for (int i = size; i < nFFT; i++)
            {
                F[i] = Complex.Zero;
            }

            Fft.twiddles(tw.AsSpan());
            Fft.fft(F.AsSpan(), tw.AsSpan());
            dot_multiply(F.AsSpan(), F.AsSpan(), nFFT);
            Fft.fft(F.AsSpan(), tw.AsSpan());
            Complex divisor = F[0];
            for (int i = 0; i < nFFT; i++)
            {
                F[i] /= divisor;
            }

            double[] @out = new double[nFFT];
            for (int i = 0; i < nFFT; i++)
            {
                @out[i] = F[i].Real;
            }
            return @out;
        }

        private static int co_firstzero(Span<double> y, int maxtau)
        {
            double[] autocorrs = co_autocorrs(y);

            int zerocrossind = 0;
            while (zerocrossind < maxtau && autocorrs[zerocrossind] > 0)
            {
                zerocrossind += 1;
            }

            return zerocrossind;
        }

        public static double[] CO_AutoCorr(Span<double> y, Span<int> tau)
        {
            int tau_size = tau.Length;
            double[] autocorrs = co_autocorrs(y);
            double[] @out = new double[tau_size];
            for (int i = 0; i < tau_size; i++)
            {
                @out[i] = autocorrs[tau[i]];
            }
            return @out;
        }

        public static double CO_f1ecac(Span<double> y)
        {
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return 0;
                }
            }

            // compute autocorrelations
            double[] autocorrs = co_autocorrs(y);

            // threshold to cross
            double thresh = 1.0 / Math.Exp(1);

            double @out = y.Length;
            for (int i = 0; i < y.Length - 2; i++)
            {
                // printf("i=%d autocorrs_i=%1.3f\n", i, autocorrs[i]);
                if (autocorrs[i + 1] < thresh)
                {
                    double m = autocorrs[i + 1] - autocorrs[i];
                    double dy = thresh - autocorrs[i];
                    double dx = dy / m;
                    @out = i + dx;
                    // printf("thresh=%1.3f AC(i)=%1.3f AC(i-1)=%1.3f m=%1.3f dy=%1.3f dx=%1.3f out=%1.3f\n", thresh, autocorrs[i], autocorrs[i-1], m, dy, dx, out);
                    return @out;
                }
            }

            return @out;
        }

        public static double CO_Embed2_Basic_tau_incircle(Span<double> y, double radius, int tau)
        {
            int size = y.Length;
            int tauIntern = 0;

            if (tau < 0)
            {
                tauIntern = co_firstzero(y, size);
            }
            else
            {
                tauIntern = tau;
            }

            double insidecount = 0;
            for (int i = 0; i < size - tauIntern; i++)
            {
                if (y[i] * y[i] + y[i + tauIntern] * y[i + tauIntern] < radius)
                {
                    insidecount += 1;
                }
            }

            return insidecount / (size - tauIntern);
        }

        public static double CO_Embed2_Dist_tau_d_expfit_meandiff(Span<double> y)
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

            int tau = co_firstzero(y, size);

            //printf("co_firstzero ran\n");

            if (tau > (double)size / 10)
            {
                tau = (int)Math.Floor(size / 10.0);
            }
            //printf("tau = %i\n", tau);

            double[] d = new double[size - tau];
            for (int i = 0; i < size - tau - 1; i++)
            {
                d[i] = Math.Sqrt((y[i + 1] - y[i]) * (y[i + 1] - y[i]) + (y[i + tau] - y[i + tau + 1]) * (y[i + tau] - y[i + tau + 1]));

                //printf("d[%i]: %1.3f\n", i, d[i]);
                if (double.IsNaN(d[i]))
                {
                    return double.NaN;
                }

                /*
                if(i<100)
                    printf("%i, y[i]=%1.3f, y[i+1]=%1.3f, y[i+tau]=%1.3f, y[i+tau+1]=%1.3f, d[i]: %1.3f\n", i, y[i], y[i+1], y[i+tau], y[i+tau+1], d[i]);
                 */
            }

            //printf("embedding finished\n");

            // mean for exponential fit
            double l = Stats.mean(d.AsSpan(0, size - tau - 1));

            // count histogram bin contents
            /*
             int * histCounts;
            double * binEdges;
            int nBins = histcounts(d, size-tau-1, -1, &histCounts, &binEdges);
             */

            int nBins = HistCounts.num_bins_auto(d.AsSpan(0, size - tau - 1));
            if (nBins == 0)
            {
                return 0;
            }
            int[] histCounts = new int[nBins];
            double[] binEdges = new double[nBins + 1];
            HistCounts.histcounts_preallocated(d.AsSpan(0, size - tau - 1), nBins, histCounts.AsSpan(), binEdges.AsSpan());

            //printf("histcount ran\n");

            // normalise to probability
            double[] histCountsNorm = new double[nBins];
            for (int i = 0; i < nBins; i++)
            {
                //printf("histCounts %i: %i\n", i, histCounts[i]);
                histCountsNorm[i] = histCounts[i] / (double)(size - tau - 1);
                //printf("histCounts norm %i: %1.3f\n", i, histCountsNorm[i]);
            }

            /*
            for(int i = 0; i < nBins; i++){
                printf("histCounts[%i] = %i\n", i, histCounts[i]);
            }
            for(int i = 0; i < nBins; i++){
                printf("histCountsNorm[%i] = %1.3f\n", i, histCountsNorm[i]);
            }
            for(int i = 0; i < nBins+1; i++){
                printf("binEdges[%i] = %1.3f\n", i, binEdges[i]);
            }
            */

            //printf("histcounts normed\n");

            double[] d_expfit_diff = new double[nBins];
            for (int i = 0; i < nBins; i++)
            {
                double expf = Math.Exp(-(binEdges[i] + binEdges[i + 1]) * 0.5 / l) / l;
                if (expf < 0)
                {
                    expf = 0;
                }
                d_expfit_diff[i] = Math.Abs(histCountsNorm[i] - expf);
                //printf("d_expfit_diff %i: %1.3f\n", i, d_expfit_diff[i]);
            }

            double @out = Stats.mean(d_expfit_diff.AsSpan());

            //printf("out = %1.6f\n", out);
            //printf("reached free statements\n");

            return @out;
        }

        public static int CO_FirstMin_ac(Span<double> y)
        {
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return 0;
                }
            }

            double[] autocorrs = co_autocorrs(y);

            int minInd = y.Length;
            for (int i = 1; i < y.Length - 1; i++)
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
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            int tau = 1;

            double[] diffTemp = new double[y.Length - 1];

            for (int i = 0; i < y.Length - tau; i++)
            {
                diffTemp[i] = Math.Pow(y[i + 1] - y[i], 3);
            }

            double @out;

            @out = Stats.mean(diffTemp.AsSpan(0, y.Length - tau));

            return @out;
        }

        public static double CO_HistogramAMI_even_2_5(Span<double> y)
        {
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            int tau = 2;
            int numBins = 5;

            double[] y1 = new double[y.Length - tau];
            double[] y2 = new double[y.Length - tau];

            for (int i = 0; i < y.Length - tau; i++)
            {
                y1[i] = y[i];
                y2[i] = y[i + tau];
            }

            // set bin edges
            double maxValue = Stats.max_(y);
            double minValue = Stats.min_(y);

            double binStep = (maxValue - minValue + 0.2) / 5;
            double[] binEdges = new double[5 + 1];
            for (int i = 0; i < numBins + 1; i++)
            {
                binEdges[i] = minValue + binStep * i - 0.1;
                // printf("binEdges[%i] = %1.3f\\n", i, binEdges[i]);
            }

            // count histogram bin contents
            int[] bins1 = HistCounts.histbinassign(y1.AsSpan(), binEdges.AsSpan());

            int[] bins2 = HistCounts.histbinassign(y2.AsSpan(), binEdges.AsSpan());

            /*
            // debug
            for(int i = 0; i < size-tau; i++){
                printf("bins1[%i] = %i, bins2[%i] = %i\\n", i, bins1[i], i, bins2[i]);
            }
            */

            // joint
            double[] bins12 = new double[y.Length - tau];
            double[] binEdges12 = new double[(5 + 1) * (5 + 1)];

            for (int i = 0; i < y.Length - tau; i++)
            {
                bins12[i] = (bins1[i] - 1) * (numBins + 1) + bins2[i];
                // printf("bins12[%i] = %1.3f\\n", i, bins12[i]);
            }

            for (int i = 0; i < (numBins + 1) * (numBins + 1); i++)
            {
                binEdges12[i] = i + 1;
                // printf("binEdges12[%i] = %1.3f\\n", i, binEdges12[i]);
            }

            // fancy solution for joint histogram here
            int[] jointHistLinear = HistCounts.histcount_edges(bins12.AsSpan(), binEdges12.AsSpan());

            /*
            // debug
            for(int i = 0; i < (numBins+1)*(numBins+1); i++){
                printf("jointHistLinear[%i] = %i\\n", i, jointHistLinear[i]);
            }
            */

            // transfer to 2D histogram (no last bin, as in original implementation)
            double[,] pij = new double[numBins, numBins];
            int sumBins = 0;
            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    pij[j, i] = jointHistLinear[i * (numBins + 1) + j];

                    // printf("pij[%i][%i]=%1.3f\\n", i, j, pij[i][j]);

                    sumBins += (int)pij[j, i];
                }
            }

            // normalise
            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    pij[j, i] /= sumBins;
                }
            }

            // marginals
            double[] pi = new double[5];
            double[] pj = new double[5];
            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    pi[i] += pij[i, j];
                    pj[j] += pij[i, j];
                    // printf("pij[%i][%i]=%1.3f, pi[%i]=%1.3f, pj[%i]=%1.3f\\n", i, j, pij[i][j], i, pi[i], j, pj[j]);
                }
            }

            /*
            // debug
            for(int i = 0; i < numBins; i++){
                printf("pi[%i]=%1.3f, pj[%i]=%1.3f\\n", i, pi[i], i, pj[i]);
            }
            */

            // mutual information
            double ami = 0;
            for (int i = 0; i < numBins; i++)
            {
                for (int j = 0; j < numBins; j++)
                {
                    if (pij[i, j] > 0)
                    {
                        //printf("pij[%i][%i]=%1.3f, pi[%i]=%1.3f, pj[%i]=%1.3f, logarg=, %1.3f, log(...)=%1.3f\\n",
                        //       i, j, pij[i][j], i, pi[i], j, pj[j], pij[i][j]/(pi[i]*pj[j]), log(pij[i][j]/(pi[i]*pj[j])));
                        ami += pij[i, j] * Math.Log(pij[i, j] / (pj[j] * pi[i]));
                    }
                }
            }

            return ami;
        }

    }
}

