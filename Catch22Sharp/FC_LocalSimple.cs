using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        private static void abs_diff(Span<double> a, Span<double> b)
        {
            for (int i = 1; i < a.Length; i++)
            {
                b[i - 1] = Math.Abs(a[i] - a[i - 1]);
            }
        }

        public static double fc_local_simple(Span<double> y, int size, int train_length)
        {
            Span<double> ySpan = y.Slice(0, size);
            double[] y1 = new double[size - 1];
            abs_diff(ySpan, y1.AsSpan());
            double m = Stats.mean(y1.AsSpan());
            return m;
        }

        public static double FC_LocalSimple_mean_tauresrat(Span<double> y, int size, int train_length)
        {
            Span<double> ySpan = y.Slice(0, size);
            // NaN check
            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(ySpan[i]))
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
                    yest += ySpan[i + j];
                }

                yest /= train_length;
                res[i] = ySpan[i + train_length] - yest;
            }

            double resAC1stZ = co_firstzero(res.AsSpan(), size - train_length);
            double yAC1stZ = co_firstzero(ySpan, size);
            double output = resAC1stZ / yAC1stZ;

            return output;
        }

        public static double FC_LocalSimple_mean_stderr(Span<double> y, int size, int train_length)
        {
            Span<double> ySpan = y.Slice(0, size);
            // NaN check
            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(ySpan[i]))
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
                    yest += ySpan[i + j];
                }

                yest /= train_length;
                res[i] = ySpan[i + train_length] - yest;
            }

            double output = Stats.stddev(res.AsSpan());

            return output;
        }

        public static double FC_LocalSimple_mean3_stderr(Span<double> y, int size)
        {
            return FC_LocalSimple_mean_stderr(y, size, 3);
        }

        public static double FC_LocalSimple_mean1_tauresrat(Span<double> y, int size)
        {
            return FC_LocalSimple_mean_tauresrat(y, size, 1);
        }

        public static double FC_LocalSimple_mean_taures(Span<double> y, int size, int train_length)
        {
            Span<double> ySpan = y.Slice(0, size);
            double[] res = new double[size - train_length];
            for (int i = 0; i < size - train_length; i++)
            {
                double yest = 0.0;
                for (int j = 0; j < train_length; j++)
                {
                    yest += ySpan[i + j];
                }

                yest /= train_length;
                res[i] = ySpan[i + train_length] - yest;
            }

            int output = co_firstzero(res.AsSpan(), size - train_length);
            return output;
        }

        public static double FC_LocalSimple_lfit_taures(Span<double> y, int size)
        {
            Span<double> ySpan = y.Slice(0, size);
            // set tau from first AC zero crossing
            int train_length = co_firstzero(ySpan, size);

            double[] xReg = new double[train_length];
            for (int i = 1; i < train_length + 1; i++)
            {
                xReg[i - 1] = i;
            }

            double[] res = new double[size - train_length];

            for (int i = 0; i < size - train_length; i++)
            {
                Span<double> yReg = ySpan.Slice(i, train_length);
                Stats.linreg(train_length, xReg.AsSpan(), yReg, out double m, out double b);

                // fprintf(stdout, "i=%i, m=%f, b=%f\\n", i, m, b);

                res[i] = ySpan[i + train_length] - (m * (train_length + 1) + b);
            }

            int output = co_firstzero(res.AsSpan(), size - train_length);

            return output;
        }
    }
}
