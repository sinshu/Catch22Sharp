using System;

namespace Catch22Sharp
{
    public static partial class Catch22
    {
        private static double[] co_autocorrs(Span<double> y)
        {
            int size = y.Length;
            double mean = Stats.mean(y);
            double[] centered = new double[size];
            for (int i = 0; i < size; i++)
            {
                centered[i] = y[i] - mean;
            }

            double zeroLag = 0.0;
            for (int i = 0; i < size; i++)
            {
                zeroLag += centered[i] * centered[i];
            }

            double[] autocorrs = new double[size > 0 ? size : 1];
            if (zeroLag == 0.0)
            {
                if (autocorrs.Length > 0)
                {
                    autocorrs[0] = 1.0;
                }
                return autocorrs;
            }

            for (int lag = 0; lag < size; lag++)
            {
                double sum = 0.0;
                for (int i = 0; i < size - lag; i++)
                {
                    sum += centered[i] * centered[i + lag];
                }
                autocorrs[lag] = sum / zeroLag;
            }

            return autocorrs;
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
            int nBins = num_bins_auto(d.AsSpan(0, validLength));
            if (nBins == 0)
            {
                return 0.0;
            }

            int[] histCounts = new int[nBins];
            double[] binEdges = new double[nBins + 1];
            histcounts_preallocated(d.AsSpan(0, validLength), nBins, histCounts, binEdges);

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

            double[] diffTemp = new double[y.Length - tau];
            for (int i = 0; i < y.Length - tau; i++)
            {
                diffTemp[i] = Math.Pow(y[i + 1] - y[i], 3);
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

            int length = y.Length - tau;
            double[] y1 = new double[length];
            double[] y2 = new double[length];
            for (int i = 0; i < length; i++)
            {
                y1[i] = y[i];
                y2[i] = y[i + tau];
            }

            double maxValue = Stats.max_(y);
            double minValue = Stats.min_(y);
            double binStep = (maxValue - minValue + 0.2) / numBins;
            double[] binEdges = new double[numBins + 1];
            for (int i = 0; i < numBins + 1; i++)
            {
                binEdges[i] = minValue + binStep * i - 0.1;
            }

            int[] bins1 = histbinassign(y1, binEdges);
            int[] bins2 = histbinassign(y2, binEdges);

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

            int[] jointHistLinear = histcount_edges(bins12, binEdges12);

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

        private static int num_bins_auto(Span<double> y)
        {
            double maxVal = Stats.max_(y);
            double minVal = Stats.min_(y);
            double std = Stats.stddev(y);
            if (std < 0.001)
            {
                return 0;
            }

            double bins = (maxVal - minVal) / (3.5 * std / Math.Pow(y.Length, 1.0 / 3.0));
            return (int)Math.Ceiling(bins);
        }

        private static void histcounts_preallocated(Span<double> y, int nBins, int[] binCounts, double[] binEdges)
        {
            double minVal = double.MaxValue;
            double maxVal = double.MinValue;
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] < minVal)
                {
                    minVal = y[i];
                }
                if (y[i] > maxVal)
                {
                    maxVal = y[i];
                }
            }

            double binStep = nBins > 0 ? (maxVal - minVal) / nBins : 0.0;
            if (binStep == 0.0)
            {
                binStep = 1.0;
            }

            Array.Clear(binCounts, 0, nBins);
            for (int i = 0; i < y.Length; i++)
            {
                int binInd = (int)((y[i] - minVal) / binStep);
                if (binInd < 0)
                {
                    binInd = 0;
                }
                if (binInd >= nBins)
                {
                    binInd = nBins - 1;
                }
                binCounts[binInd] += 1;
            }

            for (int i = 0; i < nBins + 1; i++)
            {
                binEdges[i] = i * binStep + minVal;
            }
        }

        private static int[] histbinassign(double[] y, double[] binEdges)
        {
            int[] binIdentity = new int[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                binIdentity[i] = 0;
                for (int j = 0; j < binEdges.Length; j++)
                {
                    if (y[i] < binEdges[j])
                    {
                        binIdentity[i] = j;
                        break;
                    }
                }
            }
            return binIdentity;
        }

        private static int[] histcount_edges(double[] y, double[] binEdges)
        {
            int[] histcounts = new int[binEdges.Length];
            for (int i = 0; i < y.Length; i++)
            {
                for (int j = 0; j < binEdges.Length; j++)
                {
                    if (y[i] <= binEdges[j])
                    {
                        histcounts[j] += 1;
                        break;
                    }
                }
            }
            return histcounts;
        }
    }
}

