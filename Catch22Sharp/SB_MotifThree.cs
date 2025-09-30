using System;

namespace Catch22Sharp
{
    public partial class Catch22
    {
        public static double SB_MotifThree_quantile_hh(ReadOnlySpan<double> y)
        {
            int size = y.Length;
            for (int i = 0; i < size; i++)
            {
                if (double.IsNaN(y[i]))
                {
                    return double.NaN;
                }
            }

            int alphabetSize = 3;
            int[] yt = new int[size];

            sb_coarsegrain(y, "quantile", alphabetSize, yt.AsSpan());

            int[][] r1 = new int[alphabetSize][];
            int[] sizes_r1 = new int[alphabetSize];

            for (int i = 0; i < alphabetSize; i++)
            {
                r1[i] = new int[size];
                int r_idx = 0;
                for (int j = 0; j < size; j++)
                {
                    if (yt[j] == i + 1)
                    {
                        r1[i][r_idx++] = j;
                    }
                }
                sizes_r1[i] = r_idx;
            }

            for (int i = 0; i < alphabetSize; i++)
            {
                if (sizes_r1[i] != 0 && r1[i][sizes_r1[i] - 1] == size - 1)
                {
                    sizes_r1[i]--;
                }
            }

            int[][][] r2 = new int[alphabetSize][][];
            int[][] sizes_r2 = new int[alphabetSize][];
            double[][] out2 = new double[alphabetSize][];

            for (int i = 0; i < alphabetSize; i++)
            {
                r2[i] = new int[alphabetSize][];
                sizes_r2[i] = new int[alphabetSize];
                out2[i] = new double[alphabetSize];

                for (int j = 0; j < alphabetSize; j++)
                {
                    r2[i][j] = new int[size];
                    int dynamic_idx = 0;
                    for (int k = 0; k < sizes_r1[i]; k++)
                    {
                        int idx = r1[i][k] + 1;
                        if (idx < size && yt[idx] == j + 1)
                        {
                            r2[i][j][dynamic_idx++] = r1[i][k];
                        }
                    }
                    sizes_r2[i][j] = dynamic_idx;
                    double denom = size > 1 ? size - 1 : 1;
                    out2[i][j] = sizes_r2[i][j] / denom;
                }
            }

            double hh = 0.0;
            for (int i = 0; i < alphabetSize; i++)
            {
                hh += HelperFunctions.f_entropy(out2[i].AsSpan());
            }

            return hh;
        }

        public static double[] sb_motifthree(ReadOnlySpan<double> y, string how)
        {
            int size = y.Length;
            int alphabetSize = 3;
            int[] yt = new int[size];
            double[] @out = new double[124];
            int out_idx = 0;

            if (string.CompareOrdinal(how, "quantile") == 0)
            {
                sb_coarsegrain(y, how, alphabetSize, yt.AsSpan());
            }
            else if (string.CompareOrdinal(how, "diffquant") == 0)
            {
                if (size < 2)
                {
                    return @out;
                }

                double[] diff_y = new double[size - 1];
                Stats.diff(y, diff_y.AsSpan());
                sb_coarsegrain(diff_y.AsSpan(), how, alphabetSize, yt.AsSpan(0, size - 1));
                size -= 1;
            }
            else
            {
                throw new InvalidOperationException("ERROR in sb_motifthree: Unknown how method");
            }

            int[][] r1 = new int[alphabetSize][];
            int[] sizes_r1 = new int[alphabetSize];
            double[] out1 = new double[alphabetSize];

            for (int i = 0; i < alphabetSize; i++)
            {
                r1[i] = new int[size];
                int r_idx = 0;
                for (int j = 0; j < size; j++)
                {
                    if (yt[j] == i + 1)
                    {
                        r1[i][r_idx++] = j;
                    }
                }
                sizes_r1[i] = r_idx;
                double denom = size > 0 ? size : 1;
                double tmp = sizes_r1[i] / denom;
                out1[i] = tmp;
                @out[out_idx++] = tmp;
            }

            @out[out_idx++] = HelperFunctions.f_entropy(out1.AsSpan());

            for (int i = 0; i < alphabetSize; i++)
            {
                if (sizes_r1[i] != 0 && r1[i][sizes_r1[i] - 1] == size - 1)
                {
                    sizes_r1[i]--;
                }
            }

            int[][][] r2 = new int[alphabetSize][][];
            int[][] sizes_r2 = new int[alphabetSize][];
            double[][] out2 = new double[alphabetSize][];

            for (int i = 0; i < alphabetSize; i++)
            {
                r2[i] = new int[alphabetSize][];
                sizes_r2[i] = new int[alphabetSize];
                out2[i] = new double[alphabetSize];

                for (int j = 0; j < alphabetSize; j++)
                {
                    r2[i][j] = new int[size];
                    int dynamic_idx = 0;
                    for (int k = 0; k < sizes_r1[i]; k++)
                    {
                        int idx = r1[i][k] + 1;
                        if (idx < size && yt[idx] == j + 1)
                        {
                            r2[i][j][dynamic_idx++] = r1[i][k];
                        }
                    }
                    sizes_r2[i][j] = dynamic_idx;
                    double denom = size > 1 ? size - 1 : 1;
                    double tmp = sizes_r2[i][j] / denom;
                    out2[i][j] = tmp;
                    @out[out_idx++] = tmp;
                }
            }

            double entropySum = 0.0;
            for (int i = 0; i < alphabetSize; i++)
            {
                entropySum += HelperFunctions.f_entropy(out2[i].AsSpan());
            }

            @out[out_idx++] = entropySum;

            for (int i = 0; i < alphabetSize; i++)
            {
                for (int j = 0; j < alphabetSize; j++)
                {
                    if (sizes_r2[i][j] != 0 && r2[i][j][sizes_r2[i][j] - 1] == size - 2)
                    {
                        sizes_r2[i][j]--;
                    }
                }
            }

            int[][][][] r3 = new int[alphabetSize][][][];
            int[][][] sizes_r3 = new int[alphabetSize][][];
            double[][][] out3 = new double[alphabetSize][][];

            for (int i = 0; i < alphabetSize; i++)
            {
                r3[i] = new int[alphabetSize][][];
                sizes_r3[i] = new int[alphabetSize][];
                out3[i] = new double[alphabetSize][];

                for (int j = 0; j < alphabetSize; j++)
                {
                    r3[i][j] = new int[alphabetSize][];
                    sizes_r3[i][j] = new int[alphabetSize];
                    out3[i][j] = new double[alphabetSize];

                    for (int k = 0; k < alphabetSize; k++)
                    {
                        r3[i][j][k] = new int[size];
                        int dynamic_idx = 0;
                        for (int l = 0; l < sizes_r2[i][j]; l++)
                        {
                            int idx = r2[i][j][l] + 2;
                            if (idx < size && yt[idx] == k + 1)
                            {
                                r3[i][j][k][dynamic_idx++] = r2[i][j][l];
                            }
                        }
                        sizes_r3[i][j][k] = dynamic_idx;
                        double denom = size > 2 ? size - 2 : 1;
                        double tmp = sizes_r3[i][j][k] / denom;
                        out3[i][j][k] = tmp;
                        @out[out_idx++] = tmp;
                    }
                }
            }

            entropySum = 0.0;
            for (int i = 0; i < alphabetSize; i++)
            {
                for (int j = 0; j < alphabetSize; j++)
                {
                    entropySum += HelperFunctions.f_entropy(out3[i][j].AsSpan());
                }
            }

            @out[out_idx++] = entropySum;

            for (int i = 0; i < alphabetSize; i++)
            {
                for (int j = 0; j < alphabetSize; j++)
                {
                    for (int k = 0; k < alphabetSize; k++)
                    {
                        if (sizes_r3[i][j][k] != 0 && r3[i][j][k][sizes_r3[i][j][k] - 1] == size - 3)
                        {
                            sizes_r3[i][j][k]--;
                        }
                    }
                }
            }

            int[][][][][] r4 = new int[alphabetSize][][][][];
            int[][][][] sizes_r4 = new int[alphabetSize][][][];
            double[][][][] out4 = new double[alphabetSize][][][];

            for (int i = 0; i < alphabetSize; i++)
            {
                r4[i] = new int[alphabetSize][][][];
                sizes_r4[i] = new int[alphabetSize][][];
                out4[i] = new double[alphabetSize][][];

                for (int j = 0; j < alphabetSize; j++)
                {
                    r4[i][j] = new int[alphabetSize][][];
                    sizes_r4[i][j] = new int[alphabetSize][];
                    out4[i][j] = new double[alphabetSize][];

                    for (int k = 0; k < alphabetSize; k++)
                    {
                        r4[i][j][k] = new int[alphabetSize][];
                        sizes_r4[i][j][k] = new int[alphabetSize];
                        out4[i][j][k] = new double[alphabetSize];

                        for (int l = 0; l < alphabetSize; l++)
                        {
                            r4[i][j][k][l] = new int[size];
                            int dynamic_idx = 0;
                            for (int m = 0; m < sizes_r3[i][j][k]; m++)
                            {
                                int idx = r3[i][j][k][m] + 3;
                                if (idx < size && yt[idx] == l + 1)
                                {
                                    r4[i][j][k][l][dynamic_idx++] = r3[i][j][k][m];
                                }
                            }
                            sizes_r4[i][j][k][l] = dynamic_idx;
                            double denom = size > 3 ? size - 3 : 1;
                            double tmp = sizes_r4[i][j][k][l] / denom;
                            out4[i][j][k][l] = tmp;
                            @out[out_idx++] = tmp;
                        }
                    }
                }
            }

            entropySum = 0.0;
            for (int i = 0; i < alphabetSize; i++)
            {
                for (int j = 0; j < alphabetSize; j++)
                {
                    for (int k = 0; k < alphabetSize; k++)
                    {
                        entropySum += HelperFunctions.f_entropy(out4[i][j][k].AsSpan());
                    }
                }
            }

            @out[out_idx++] = entropySum;

            return @out;
        }
    }
}
