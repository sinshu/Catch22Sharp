using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        public static double SB_TransitionMatrix_3ac_sumdiagcov(ReadOnlySpan<double> y)
        {
            int size = y.Length;

            bool constant = true;
            double first = y[0];
            for (int i = 0; i < size; i++)
            {
                double value = y[i];
                if (double.IsNaN(value))
                {
                    return double.NaN;
                }

                if (value != first)
                {
                    constant = false;
                }
            }

            if (constant)
            {
                return double.NaN;
            }

            const int numGroups = 3;

            int tau = co_firstzero(y);

            double[] yFilt = new double[size];
            for (int i = 0; i < size; i++)
            {
                yFilt[i] = y[i];
            }

            int nDown = (size - 1) / tau + 1;
            double[] yDown = new double[nDown];
            for (int i = 0; i < nDown; i++)
            {
                yDown[i] = yFilt[i * tau];
            }

            int[] yCG = new int[nDown];
            sb_coarsegrain(yDown, "quantile", numGroups, yCG);

            double[,] T = new double[numGroups, numGroups];
            for (int j = 0; j < nDown - 1; j++)
            {
                int from = yCG[j] - 1;
                int to = yCG[j + 1] - 1;
                T[from, to] += 1.0;
            }

            double normalizer = nDown - 1;
            for (int i = 0; i < numGroups; i++)
            {
                for (int j = 0; j < numGroups; j++)
                {
                    T[i, j] /= normalizer;
                }
            }

            double[][] columns = new double[numGroups][];
            for (int i = 0; i < numGroups; i++)
            {
                columns[i] = new double[numGroups];
            }

            for (int i = 0; i < numGroups; i++)
            {
                columns[0][i] = T[i, 0];
                columns[1][i] = T[i, 1];
                columns[2][i] = T[i, 2];
            }

            double[,] COV = new double[numGroups, numGroups];
            for (int i = 0; i < numGroups; i++)
            {
                for (int j = i; j < numGroups; j++)
                {
                    double covTemp = Stats.cov(columns[i], columns[j]);
                    COV[i, j] = covTemp;
                    COV[j, i] = covTemp;
                }
            }

            double sumdiagcov = 0.0;
            for (int i = 0; i < numGroups; i++)
            {
                sumdiagcov += COV[i, i];
            }

            return sumdiagcov;
        }
    }
}
