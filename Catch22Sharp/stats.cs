using System;

namespace Catch22Sharp
{
    public static class Stats
    {
        public static double min_(Span<double> a)
        {
            int size = a.Length;
            double m = a[0];
            for (int i = 1; i < size; i++)
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
            int size = a.Length;
            double m = a[0];
            for (int i = 1; i < size; i++)
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
            int size = a.Length;
            double m = 0.0;
            for (int i = 0; i < size; i++)
            {
                m += a[i];
            }
            m /= size;
            return m;
        }

        public static double sum(Span<double> a)
        {
            int size = a.Length;
            double m = 0.0;
            for (int i = 0; i < size; i++)
            {
                m += a[i];
            }
            return m;
        }

        public static void cumsum(Span<double> a, Span<double> b)
        {
            int size = a.Length;
            b[0] = a[0];
            for (int i = 1; i < size; i++)
            {
                b[i] = a[i] + b[i - 1];
                //printf("b[%i]%1.3f = a[%i]%1.3f + b[%i-1]%1.3f\\n", i, b[i], i, a[i], i, a[i-1]);
            }
        }

        public static void icumsum(Span<int> a, Span<int> b)
        {
            int size = a.Length;
            b[0] = a[0];
            for (int i = 1; i < size; i++)
            {
                b[i] = a[i] + b[i - 1];
                //printf("b[%i]%1.3f = a[%i]%1.3f + b[%i-1]%1.3f\\n", i, b[i], i, a[i], i, a[i-1]);
            }
        }

        public static double isum(Span<int> a)
        {
            int size = a.Length;
            double m = 0.0;
            for (int i = 0; i < size; i++)
            {
                m += a[i];
            }
            return m;
        }

        public static double median(Span<double> a)
        {
            int size = a.Length;
            double m;
            double[] b = new double[size];
            a.CopyTo(b);
            HelperFunctions.sort(b.AsSpan());
            if (size % 2 == 1)
            {
                m = b[size / 2];
            }
            else
            {
                int m1 = size / 2;
                int m2 = m1 - 1;
                m = (b[m1] + b[m2]) / 2.0;
            }
            return m;
        }

        public static double stddev(Span<double> a)
        {
            int size = a.Length;
            double m = mean(a);
            double sd = 0.0;
            for (int i = 0; i < size; i++)
            {
                sd += Math.Pow(a[i] - m, 2);
            }
            sd = Math.Sqrt(sd / (size - 1));
            return sd;
        }

        public static double cov(Span<double> x, Span<double> y)
        {
            int size = x.Length;
            double covariance = 0;
            double meanX = mean(x);
            double meanY = mean(y);
            for (int i = 0; i < size; i++)
            {
                // double xi =x[i];
                // double yi =y[i];
                covariance += (x[i] - meanX) * (y[i] - meanY);
            }
            return covariance / (size - 1);
        }

        public static double cov_mean(Span<double> x, Span<double> y)
        {
            int size = x.Length;
            double covariance = 0;
            for (int i = 0; i < size; i++)
            {
                // double xi =x[i];
                // double yi =y[i];
                covariance += x[i] * y[i];
            }
            return covariance / size;
        }

        public static double corr(Span<double> x, Span<double> y)
        {
            int size = x.Length;
            double nom = 0;
            double denomX = 0;
            double denomY = 0;
            double meanX = mean(x);
            double meanY = mean(y);
            for (int i = 0; i < size; i++)
            {
                nom += (x[i] - meanX) * (y[i] - meanY);
                denomX += (x[i] - meanX) * (x[i] - meanX);
                denomY += (y[i] - meanY) * (y[i] - meanY);
                //printf("x[%i]=%1.3f, y[%i]=%1.3f, nom[%i]=%1.3f, denomX[%i]=%1.3f, denomY[%i]=%1.3f\\n", i, x[i], i, y[i], i, nom, i, denomX, i, denomY);
            }
            return nom / Math.Sqrt(denomX * denomY);
        }

        public static double autocorr_lag(Span<double> x, int lag)
        {
            int size = x.Length;
            return corr(x.Slice(0, size - lag), x.Slice(lag, size - lag));
        }

        public static double autocov_lag(Span<double> x, int lag)
        {
            int size = x.Length;
            return cov_mean(x.Slice(0, size - lag), x.Slice(lag, size - lag));
        }

        public static void zscore_norm(Span<double> a)
        {
            int size = a.Length;
            double m = mean(a);
            double sd = stddev(a);
            for (int i = 0; i < size; i++)
            {
                a[i] = (a[i] - m) / sd;
            }
        }

        public static void zscore_norm2(Span<double> a, Span<double> b)
        {
            int size = a.Length;
            double m = mean(a);
            double sd = stddev(a);
            for (int i = 0; i < size; i++)
            {
                b[i] = (a[i] - m) / sd;
            }
        }

        public static double moment(Span<double> a, int start, int end, int r)
        {
            int win_size = end - start + 1;
            Span<double> window = a.Slice(start, win_size);
            double m = mean(window);
            double mr = 0.0;
            for (int i = 0; i < win_size; i++)
            {
                mr += Math.Pow(window[i] - m, r);
            }
            mr /= win_size;
            mr /= stddev(window); //normalize
            return mr;
        }

        public static void diff(Span<double> a, Span<double> b)
        {
            int size = a.Length;
            for (int i = 1; i < size; i++)
            {
                b[i - 1] = a[i] - a[i - 1];
            }
        }

        public static int linreg(int n, Span<double> x, Span<double> y, out double m, out double b)
        {
            double sumx = 0.0;                      /* sum of x     */
            double sumx2 = 0.0;                     /* sum of x**2  */
            double sumxy = 0.0;                     /* sum of x * y */
            double sumy = 0.0;                      /* sum of y     */
            double sumy2 = 0.0;                     /* sum of y**2  */

            for (int i = 0; i < n; i++)
            {
                sumx += x[i];
                sumx2 += x[i] * x[i];
                sumxy += x[i] * y[i];
                sumy += y[i];
                sumy2 += y[i] * y[i];
            }

            double denom = n * sumx2 - sumx * sumx;
            if (denom == 0)
            {
                // singular matrix. can't solve the problem.
                m = 0;
                b = 0;
                return 1;
            }

            m = (n * sumxy - sumx * sumy) / denom;
            b = (sumy * sumx2 - sumx * sumxy) / denom;

            /*if (r!=NULL) {
                *r = (sumxy - sumx * sumy / n) /
                sqrt((sumx2 - sumx * sumx/n) *
                     (sumy2 - sumy * sumy/n));
            }
            */

            return 0;
        }

        public static double norm_(Span<double> a)
        {
            int size = a.Length;
            double @out = 0.0;
            for (int i = 0; i < size; i++)
            {
                @out += a[i] * a[i];
            }
            @out = Math.Sqrt(@out);
            return @out;
        }
    }
}
