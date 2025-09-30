using System;
using System.Numerics;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        //
        //  SP_Summaries.c
        //
        //
        //  Created by Carl Henning Lubba on 23/09/2018.
        //
        private static int welch(ReadOnlySpan<double> y, int nfft, double fs, ReadOnlySpan<double> window, out double[] pxx, out double[] f)
        {
            int size = y.Length;
            int windowWidth = window.Length;

            double dt = 1.0 / fs;
            double df = 1.0 / nextpow2(windowWidth) / dt;
            double m = Stats.mean(y);

            int k = (int)Math.Floor(size / (windowWidth / 2.0)) - 1;
            double windowNorm = Stats.norm_(window);
            double kmu = k * Math.Pow(windowNorm, 2);

            double[] p = new double[nfft];

            Complex[] F = new Complex[nfft];
            Complex[] tw = new Complex[nfft];
            Fft.twiddles(tw.AsSpan());

            double[] xw = new double[windowWidth];
            for (int i = 0; i < k; i++)
            {
                int offset = (int)(i * windowWidth / 2.0);
                for (int j = 0; j < windowWidth; j++)
                {
                    xw[j] = window[j] * y[j + offset];
                }

                for (int j = 0; j < windowWidth; j++)
                {
                    Complex tmp = new Complex(xw[j] - m, 0.0);
                    F[j] = tmp;
                }
                for (int j = windowWidth; j < nfft; j++)
                {
                    F[j] = Complex.Zero;
                }

                Fft.fft(F.AsSpan(), tw.AsSpan());

                for (int l = 0; l < nfft; l++)
                {
                    double magnitude = Complex.Abs(F[l]);
                    p[l] += magnitude * magnitude;
                }
            }

            int nout = nfft / 2 + 1;
            pxx = new double[nout];
            for (int i = 0; i < nout; i++)
            {
                pxx[i] = p[i] / kmu * dt;
                if (i > 0 && i < nout - 1)
                {
                    pxx[i] *= 2;
                }
            }

            f = new double[nout];
            for (int i = 0; i < nout; i++)
            {
                f[i] = i * df;
            }

            return nout;
        }

        private static double SP_Summaries_welch_rect(ReadOnlySpan<double> y, string what)
        {
            int size = y.Length;

            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            double[] window = new double[size];
            for (int i = 0; i < size; i++)
            {
                window[i] = 1.0;
            }

            double fs = 1.0;
            int n = nextpow2(size);

            double[] S;
            double[] f;
            int nWelch = welch(y, n, fs, window.AsSpan(), out S, out f);

            double[] w = new double[nWelch];
            double[] Sw = new double[nWelch];

            double PI = 3.14159265359;
            for (int i = 0; i < nWelch; i++)
            {
                w[i] = 2 * PI * f[i];
                Sw[i] = S[i] / (2 * PI);
                if (double.IsInfinity(Sw[i]))
                {
                    return 0;
                }
            }

            double dw = nWelch > 1 ? w[1] - w[0] : 0.0;

            double[] csS = new double[nWelch];
            Stats.cumsum(Sw.AsSpan(), csS.AsSpan());

            double output = 0;

            if (what == "centroid")
            {
                double csSThres = csS[nWelch - 1] * 0.5;
                double centroid = 0;
                for (int i = 0; i < nWelch; i++)
                {
                    if (csS[i] > csSThres)
                    {
                        centroid = w[i];
                        break;
                    }
                }
                output = centroid;
            }
            else if (what == "area_5_1")
            {
                double area_5_1 = 0;
                for (int i = 0; i < nWelch / 5; i++)
                {
                    area_5_1 += Sw[i];
                }
                area_5_1 *= dw;

                output = area_5_1;
            }

            return output;
        }

        public static double SP_Summaries_welch_rect_area_5_1(ReadOnlySpan<double> y)
        {
            return SP_Summaries_welch_rect(y, "area_5_1");
        }

        public static double SP_Summaries_welch_rect_centroid(ReadOnlySpan<double> y)
        {
            return SP_Summaries_welch_rect(y, "centroid");
        }
    }
}
