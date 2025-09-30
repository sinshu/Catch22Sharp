using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        private static void abs_diff(ReadOnlySpan<double> a, Span<double> b)
        {
            for (int i = 1; i < a.Length; i++)
            {
                b[i - 1] = Math.Abs(a[i] - a[i - 1]);
            }
        }

        public static double fc_local_simple(ReadOnlySpan<double> y, int train_length)
        {
            int size = y.Length;
            double[] y1 = new double[size - 1];
            abs_diff(y, y1.AsSpan());
            double m = Stats.mean(y1.AsSpan());
            return m;
        }

        public static double FC_LocalSimple_mean_tauresrat(ReadOnlySpan<double> y, int train_length)
        {
            int size = y.Length;
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            double[] res = new double[size - train_length];

            for (int i = 0; i < size - train_length; i++)
            {
                double yest = 0.0;
                for (int j = 0; j < train_length; j++)
                {
                    yest += y[i + j];
                }

                yest /= train_length;
                res[i] = y[i + train_length] - yest;
            }

            double resAC1stZ = co_firstzero(res.AsSpan());
            double yAC1stZ = co_firstzero(y);
            double output = resAC1stZ / yAC1stZ;

            return output;
        }

        public static double FC_LocalSimple_mean_stderr(ReadOnlySpan<double> y, int train_length)
        {
            int size = y.Length;
            // NaN check
            for (int i = 0; i < y.Length; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            double[] res = new double[size - train_length];
            for (int i = 0; i < size - train_length; i++)
            {
                double yest = 0.0;
                for (int j = 0; j < train_length; j++)
                {
                    yest += y[i + j];
                }

                yest /= train_length;
                res[i] = y[i + train_length] - yest;
            }

            double output = Stats.stddev(res.AsSpan());

            return output;
        }

        public static double FC_LocalSimple_mean3_stderr(ReadOnlySpan<double> y)
        {
            return FC_LocalSimple_mean_stderr(y, 3);
        }

        public static double FC_LocalSimple_mean1_tauresrat(ReadOnlySpan<double> y)
        {
            return FC_LocalSimple_mean_tauresrat(y, 1);
        }

        public static double FC_LocalSimple_mean_taures(ReadOnlySpan<double> y, int train_length)
        {
            int size = y.Length;
            double[] res = new double[size - train_length];
            for (int i = 0; i < size - train_length; i++)
            {
                double yest = 0.0;
                for (int j = 0; j < train_length; j++)
                {
                    yest += y[i + j];
                }

                yest /= train_length;
                res[i] = y[i + train_length] - yest;
            }

            int output = co_firstzero(res.AsSpan());
            return output;
        }

        public static double FC_LocalSimple_lfit_taures(ReadOnlySpan<double> y)
        {
            int size = y.Length;
            // set tau from first AC zero crossing
            int train_length = co_firstzero(y);

            double[] xReg = new double[train_length];
            for (int i = 1; i < train_length + 1; i++)
            {
                xReg[i - 1] = i;
            }

            double[] res = new double[size - train_length];

            for (int i = 0; i < size - train_length; i++)
            {
                ReadOnlySpan<double> yReg = y.Slice(i, train_length);
                Stats.linreg(xReg.AsSpan(), yReg, out double m, out double b);

                // fprintf(stdout, "i=%i, m=%f, b=%f\\n", i, m, b);

                res[i] = y[i + train_length] - (m * (train_length + 1) + b);
            }

            int output = co_firstzero(res.AsSpan());

            return output;
        }
    }
}
