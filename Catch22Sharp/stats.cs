using System;

namespace Catch22Sharp
{
    public static class Stats
    {
        public static double min_(Span<double> a)
        {
            double m = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] < m)
                {
                    m = a[i];
                }
            }
            return m;
        }

        public static double max_(Span<double> a)
        {
            double m = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] > m)
                {
                    m = a[i];
                }
            }
            return m;
        }

        public static double mean(Span<double> a)
        {
            double m = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                m += a[i];
            }
            m /= a.Length;
            return m;
        }

        public static double sum(Span<double> a)
        {
            double m = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                m += a[i];
            }
            return m;
        }

        public static void cumsum(Span<double> a, Span<double> b)
        {
            b[0] = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                b[i] = a[i] + b[i - 1];
            }
        }

        public static void icumsum(Span<int> a, Span<int> b)
        {
            b[0] = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                b[i] = a[i] + b[i - 1];
            }
        }

        public static double isum(Span<int> a)
        {
            double m = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                m += a[i];
            }
            return m;
        }

        public static double median(Span<double> a)
        {
            double m;
            double[] b = new double[a.Length];
            a.CopyTo(b);
            Array.Sort(b);
            if (a.Length % 2 == 1)
            {
                m = b[a.Length / 2];
            }
            else
            {
                int m1 = a.Length / 2;
                int m2 = m1 - 1;
                m = (b[m1] + b[m2]) / 2.0;
            }
            return m;
        }

        public static double stddev(Span<double> a)
        {
            double m = mean(a);
            double sd = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sd += Math.Pow(a[i] - m, 2);
            }
            sd = Math.Sqrt(sd / (a.Length - 1));
            return sd;
        }

        public static double cov(Span<double> x, Span<double> y)
        {
            double covariance = 0;
            double meanX = mean(x);
            double meanY = mean(y);
            for (int i = 0; i < x.Length; i++)
            {
                covariance += (x[i] - meanX) * (y[i] - meanY);
            }
            return covariance / (x.Length - 1);
        }

        public static double cov_mean(Span<double> x, Span<double> y)
        {
            double covariance = 0;
            for (int i = 0; i < x.Length; i++)
            {
                covariance += x[i] * y[i];
            }
            return covariance / x.Length;
        }

        public static double corr(Span<double> x, Span<double> y)
        {
            double nom = 0;
            double denomX = 0;
            double denomY = 0;
            double meanX = mean(x);
            double meanY = mean(y);
            for (int i = 0; i < x.Length; i++)
            {
                nom += (x[i] - meanX) * (y[i] - meanY);
                denomX += (x[i] - meanX) * (x[i] - meanX);
                denomY += (y[i] - meanY) * (y[i] - meanY);
            }
            return nom / Math.Sqrt(denomX * denomY);
        }

        public static double autocorr_lag(Span<double> x, int lag)
        {
            return corr(x.Slice(0, x.Length - lag), x.Slice(lag, x.Length - lag));
        }

        public static double autocov_lag(Span<double> x, int lag)
        {
            return cov_mean(x.Slice(0, x.Length - lag), x.Slice(lag, x.Length - lag));
        }

        public static void zscore_norm(Span<double> a)
        {
            double m = mean(a);
            double sd = stddev(a);
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (a[i] - m) / sd;
            }
        }

        public static void zscore_norm2(Span<double> a, Span<double> b)
        {
            double m = mean(a);
            double sd = stddev(a);
            for (int i = 0; i < a.Length; i++)
            {
                b[i] = (a[i] - m) / sd;
            }
        }

        public static double moment(Span<double> a, int start, int end, int r)
        {
            int win_size = end - start + 1;
            Span<double> sub = a.Slice(start, win_size);
            double m = mean(sub);
            double mr = 0.0;
            for (int i = 0; i < win_size; i++)
            {
                mr += Math.Pow(sub[i] - m, r);
            }
            mr /= win_size;
            mr /= stddev(sub); //normalize
            return mr;
        }

        public static void diff(Span<double> a, Span<double> b)
        {
            for (int i = 1; i < a.Length; i++)
            {
                b[i - 1] = a[i] - a[i - 1];
            }
        }

        public static int linreg(int n, Span<double> x, Span<double> y, out double m, out double b)
        {
            double sumx = 0.0;
            double sumx2 = 0.0;
            double sumxy = 0.0;
            double sumy = 0.0;
            double sumy2 = 0.0;
            for (int i = 0; i < n; i++)
            {
                sumx += x[i];
                sumx2 += x[i] * x[i];
                sumxy += x[i] * y[i];
                sumy += y[i];
                sumy2 += y[i] * y[i];
            }
            double denom = (n * sumx2 - sumx * sumx);
            if (denom == 0)
            {
                m = 0;
                b = 0;
                return 1;
            }
            m = (n * sumxy - sumx * sumy) / denom;
            b = (sumy * sumx2 - sumx * sumxy) / denom;
            return 0;
        }

        public static double norm_(Span<double> a)
        {
            double outVal = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                outVal += a[i] * a[i];
            }
            outVal = Math.Sqrt(outVal);
            return outVal;
        }
    }
}
