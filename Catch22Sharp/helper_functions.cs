using System;

namespace Catch22Sharp
{
    internal static class HelperFunctions
    {
        // wrapper for qsort for array of doubles. Sorts in-place
        public static void sort(Span<double> y)
        {
            double[] tmp = y.ToArray();
            Array.Sort(tmp);

            /*
            for(int i=0; i < size; i++){
                printf("y[%i]=%1.4f\n", i, y[i]);
            }
            for(int i=0; i < size; i++){
                printf("sorted[%i]=%1.4f\n", i, tmp[i]);
            }
             */
            tmp.AsSpan().CopyTo(y);
        }

        // linearly spaced vector
        public static void linspace(double start, double end, int num_groups, Span<double> @out)
        {
            double step_size = (end - start) / (num_groups - 1);
            for (int i = 0; i < num_groups; i++)
            {
                @out[i] = start;
                start += step_size;
            }
            return;
        }

        public static double quantile(ReadOnlySpan<double> y, double quant)
        {
            int size = y.Length;
            double quant_idx;
            double q;
            double value;
            int idx_left;
            int idx_right;
            double[] tmp = y.ToArray();
            Array.Sort(tmp);

            // out of range limit?
            q = 0.5 / size;
            if (quant < q)
            {
                value = tmp[0];
                return value;
            }

            if (quant > 1 - q)
            {
                value = tmp[size - 1];
                return value;
            }

            quant_idx = size * quant - 0.5;
            idx_left = (int)Math.Floor(quant_idx);
            idx_right = (int)Math.Ceiling(quant_idx);
            value = tmp[idx_left] + (quant_idx - idx_left) * (tmp[idx_right] - tmp[idx_left]) / (idx_right - idx_left);
            return value;
        }

        public static void binarize(ReadOnlySpan<double> a, Span<int> b, string how)
        {
            double m = 0.0;
            if (how == "mean")
            {
                m = Stats.mean(a);
            }
            else if (how == "median")
            {
                m = Stats.median(a);
            }

            for (int i = 0; i < a.Length; i++)
            {
                b[i] = a[i] > m ? 1 : 0;
            }
            return;
        }

        public static double f_entropy(ReadOnlySpan<double> a)
        {
            double f = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > 0)
                {
                    f += a[i] * Math.Log(a[i]);
                }
            }

            return -1 * f;
        }

        public static void subset(ReadOnlySpan<int> a, int start, int end, Span<int> b)
        {
            int j = 0;
            for (int i = start; i < end; i++)
            {
                b[j++] = a[i];
            }
            return;
        }
    }
}
