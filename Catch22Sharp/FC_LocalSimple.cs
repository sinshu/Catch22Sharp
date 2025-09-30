using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        private static void abs_diff(ReadOnlySpan<double> a, Span<double> b)
        {
            if (a.Length <= 1)
            {
                return;
            }

            for (int i = 1; i < a.Length; i++)
            {
                b[i - 1] = Math.Abs(a[i] - a[i - 1]);
            }
        }

        public static double fc_local_simple(Span<double> y, int size, int train_length)
        {
            if (size <= 1)
            {
                return 0.0;
            }

            Span<double> ySpan = size < y.Length ? y.Slice(0, size) : y;
            double[] y1 = new double[ySpan.Length - 1];
            abs_diff(ySpan, y1.AsSpan());
            return Stats.mean(y1.AsSpan());
        }

        public static double FC_LocalSimple_mean_tauresrat(Span<double> y, int size, int train_length)
        {
            Span<double> ySpan = size < y.Length ? y.Slice(0, size) : y;
            for (int i = 0; i < ySpan.Length; i++)
            {
                if (double.IsNaN(ySpan[i]))
                {
                    return double.NaN;
                }
            }

            int residualLength = ySpan.Length - train_length;
            if (residualLength <= 0)
            {
                return 0.0;
            }

            double[] res = new double[residualLength];
            for (int i = 0; i < residualLength; i++)
            {
                double yest = 0.0;
                for (int j = 0; j < train_length; j++)
                {
                    yest += ySpan[i + j];
                }

                yest /= train_length;
                res[i] = ySpan[i + train_length] - yest;
            }

            int resAC1stZ = co_firstzero(res.AsSpan(), residualLength);
            int yAC1stZ = co_firstzero(ySpan, ySpan.Length);
            if (yAC1stZ == 0)
            {
                return 0.0;
            }

            double output = resAC1stZ / (double)yAC1stZ;
            return output;
        }

        public static double FC_LocalSimple_mean_stderr(Span<double> y, int size, int train_length)
        {
            Span<double> ySpan = size < y.Length ? y.Slice(0, size) : y;
            for (int i = 0; i < ySpan.Length; i++)
            {
                if (double.IsNaN(ySpan[i]))
                {
                    return double.NaN;
                }
            }

            int residualLength = ySpan.Length - train_length;
            if (residualLength <= 1)
            {
                return 0.0;
            }

            double[] yZscored = new double[ySpan.Length];
            Stats.zscore_norm2(ySpan, yZscored.AsSpan());
            Span<double> yWork = yZscored.AsSpan();

            double[] res = new double[residualLength];
            for (int i = 0; i < residualLength; i++)
            {
                double yest = 0.0;
                for (int j = 0; j < train_length; j++)
                {
                    yest += yWork[i + j];
                }

                yest /= train_length;
                res[i] = yWork[i + train_length] - yest;
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
            Span<double> ySpan = size < y.Length ? y.Slice(0, size) : y;
            int residualLength = ySpan.Length - train_length;
            if (residualLength <= 0)
            {
                return 0.0;
            }

            double[] res = new double[residualLength];
            for (int i = 0; i < residualLength; i++)
            {
                double yest = 0.0;
                for (int j = 0; j < train_length; j++)
                {
                    yest += ySpan[i + j];
                }

                yest /= train_length;
                res[i] = ySpan[i + train_length] - yest;
            }

            int output = co_firstzero(res.AsSpan(), residualLength);
            return output;
        }

        public static double FC_LocalSimple_lfit_taures(Span<double> y, int size)
        {
            Span<double> ySpan = size < y.Length ? y.Slice(0, size) : y;
            int train_length = co_firstzero(ySpan, ySpan.Length);
            if (train_length <= 0)
            {
                return 0.0;
            }

            double[] xReg = new double[train_length];
            for (int i = 0; i < train_length; i++)
            {
                xReg[i] = i + 1;
            }

            int residualLength = ySpan.Length - train_length;
            if (residualLength <= 0)
            {
                return 0.0;
            }

            double[] res = new double[residualLength];
            for (int i = 0; i < residualLength; i++)
            {
                Span<double> yReg = ySpan.Slice(i, train_length);
                Stats.linreg(train_length, xReg.AsSpan(), yReg, out double m, out double b);
                res[i] = ySpan[i + train_length] - (m * (train_length + 1) + b);
            }

            int output = co_firstzero(res.AsSpan(), residualLength);
            return output;
        }
    }
}
