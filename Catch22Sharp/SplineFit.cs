using System;

namespace Catch22Sharp
{
    internal static class SplineFit
    {
        private const int pieces = 2;
        private const int nBreaks = 3;
        private const int deg = 3;
        private const int nSpline = 4;
        private const int piecesExt = 8; // 3 * deg - 1

        public static void matrix_multiply(int sizeA1, int sizeA2, ReadOnlySpan<double> A, int sizeB1, int sizeB2, ReadOnlySpan<double> B, Span<double> C)
        {
            if (sizeA2 != sizeB1)
            {
                return;
            }

            for (int i = 0; i < sizeA1; i++)
            {
                for (int j = 0; j < sizeB2; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < sizeB1; k++)
                    {
                        sum += A[i * sizeA2 + k] * B[k * sizeB2 + j];
                    }
                    C[i * sizeB2 + j] = sum;
                }
            }
        }

        public static void matrix_times_vector(int sizeA1, int sizeA2, ReadOnlySpan<double> A, ReadOnlySpan<double> b, Span<double> c)
        {
            if (sizeA2 != b.Length)
            {
                return;
            }

            for (int i = 0; i < sizeA1; i++)
            {
                double sum = 0;
                for (int k = 0; k < sizeA2; k++)
                {
                    sum += A[i * sizeA2 + k] * b[k];
                }
                c[i] = sum;
            }
        }

        public static void gauss_elimination(int size, ReadOnlySpan<double> A, ReadOnlySpan<double> b, Span<double> x)
        {
            double[,] AElim = new double[size, size];
            double[] bElim = new double[size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    AElim[i, j] = A[i * size + j];
                }
                bElim[i] = b[i];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    double factor = AElim[j, i] / AElim[i, i];
                    bElim[j] -= factor * bElim[i];
                    for (int k = i; k < size; k++)
                    {
                        AElim[j, k] -= factor * AElim[i, k];
                    }
                }
            }

            for (int i = size - 1; i >= 0; i--)
            {
                double bMinusATemp = bElim[i];
                for (int j = i + 1; j < size; j++)
                {
                    bMinusATemp -= x[j] * AElim[i, j];
                }
                x[i] = bMinusATemp / AElim[i, i];
            }
        }

        public static void lsqsolve_sub(int sizeA1, int sizeA2, ReadOnlySpan<double> A, ReadOnlySpan<double> b, Span<double> x)
        {
            double[] AT = new double[sizeA2 * sizeA1];
            double[] ATA = new double[sizeA2 * sizeA2];
            double[] ATb = new double[sizeA2];

            for (int i = 0; i < sizeA1; i++)
            {
                for (int j = 0; j < sizeA2; j++)
                {
                    AT[j * sizeA1 + i] = A[i * sizeA2 + j];
                }
            }

            matrix_multiply(sizeA2, sizeA1, AT, sizeA1, sizeA2, A, ATA);
            matrix_times_vector(sizeA2, sizeA1, AT, b, ATb);
            gauss_elimination(sizeA2, ATA, ATb, x);
        }

        private static int iLimit(int x, int lim)
        {
            return x < lim ? x : lim;
        }

        public static int splinefit(ReadOnlySpan<double> y, Span<double> yOut)
        {
            int size = y.Length;

            int[] breaks = new int[nBreaks];
            breaks[0] = 0;
            breaks[1] = (int)Math.Floor(size / 2.0) - 1;
            breaks[2] = size - 1;

            int[] h0 = new int[2];
            h0[0] = breaks[1] - breaks[0];
            h0[1] = breaks[2] - breaks[1];

            int[] hCopy = new int[4];
            hCopy[0] = h0[0];
            hCopy[1] = h0[1];
            hCopy[2] = h0[0];
            hCopy[3] = h0[1];

            int[] hl = new int[deg];
            hl[0] = hCopy[deg - 0];
            hl[1] = hCopy[deg - 1];
            hl[2] = hCopy[deg - 2];

            int[] hlCS = new int[deg];
            Stats.icumsum(hl, hlCS);

            int[] bl = new int[deg];
            for (int i = 0; i < deg; i++)
            {
                bl[i] = breaks[0] - hlCS[i];
            }

            int[] hr = new int[deg];
            hr[0] = hCopy[0];
            hr[1] = hCopy[1];
            hr[2] = hCopy[2];

            int[] hrCS = new int[deg];
            Stats.icumsum(hr, hrCS);

            int[] br = new int[deg];
            for (int i = 0; i < deg; i++)
            {
                br[i] = breaks[2] + hrCS[i];
            }

            int[] breaksExt = new int[3 * deg];
            for (int i = 0; i < deg; i++)
            {
                breaksExt[i] = bl[deg - 1 - i];
                breaksExt[i + deg] = breaks[i];
                breaksExt[i + 2 * deg] = br[i];
            }

            int[] hExt = new int[3 * deg - 1];
            for (int i = 0; i < deg * 3 - 1; i++)
            {
                hExt[i] = breaksExt[i + 1] - breaksExt[i];
            }

            double[,] coefs = new double[nSpline * piecesExt, nSpline + 1];
            for (int i = 0; i < nSpline * piecesExt; i++)
            {
                for (int j = 0; j < nSpline; j++)
                {
                    coefs[i, j] = 0;
                }
            }
            for (int i = 0; i < nSpline * piecesExt; i += nSpline)
            {
                coefs[i, 0] = 1;
            }

            int[,] ii = new int[deg + 1, piecesExt];
            for (int i = 0; i < piecesExt; i++)
            {
                ii[0, i] = iLimit(0 + i, piecesExt - 1);
                ii[1, i] = iLimit(1 + i, piecesExt - 1);
                ii[2, i] = iLimit(2 + i, piecesExt - 1);
                ii[3, i] = iLimit(3 + i, piecesExt - 1);
            }

            double[] H = new double[(deg + 1) * piecesExt];
            for (int i = 0; i < nSpline * piecesExt; i++)
            {
                int iiFlat = ii[i % nSpline, i / nSpline];
                H[i] = hExt[iiFlat];
            }

            double[,] Q = new double[nSpline, piecesExt];
            for (int k = 1; k < nSpline; k++)
            {
                for (int j = 0; j < k; j++)
                {
                    for (int l = 0; l < nSpline * piecesExt; l++)
                    {
                        coefs[l, j] *= H[l] / (k - j);
                    }
                }

                for (int l = 0; l < nSpline * piecesExt; l++)
                {
                    double sum = 0;
                    for (int m = 0; m < nSpline; m++)
                    {
                        sum += coefs[l, m];
                    }
                    Q[l % nSpline, l / nSpline] = sum;
                }

                for (int l = 0; l < piecesExt; l++)
                {
                    for (int m = 1; m < nSpline; m++)
                    {
                        Q[m, l] += Q[m - 1, l];
                    }
                }

                for (int l = 0; l < nSpline * piecesExt; l++)
                {
                    if (l % nSpline == 0)
                    {
                        coefs[l, k] = 0;
                    }
                    else
                    {
                        coefs[l, k] = Q[l % nSpline - 1, l / nSpline];
                    }
                }

                double[] fmax = new double[piecesExt * nSpline];
                for (int i = 0; i < piecesExt; i++)
                {
                    for (int j = 0; j < nSpline; j++)
                    {
                        fmax[i * nSpline + j] = Q[nSpline - 1, i];
                    }
                }

                for (int j = 0; j < k + 1; j++)
                {
                    for (int l = 0; l < nSpline * piecesExt; l++)
                    {
                        coefs[l, j] /= fmax[l];
                    }
                }

                for (int i = 0; i < (nSpline * piecesExt) - deg; i++)
                {
                    for (int j = 0; j < k + 1; j++)
                    {
                        coefs[i, j] -= coefs[deg + i, j];
                    }
                }
                for (int i = 0; i < nSpline * piecesExt; i += nSpline)
                {
                    coefs[i, k] = 0;
                }
            }

            double[] scale = new double[nSpline * piecesExt];
            for (int i = 0; i < nSpline * piecesExt; i++)
            {
                scale[i] = 1;
            }
            for (int k = 0; k < nSpline - 1; k++)
            {
                for (int i = 0; i < nSpline * piecesExt; i++)
                {
                    scale[i] /= H[i];
                }
                for (int i = 0; i < nSpline * piecesExt; i++)
                {
                    coefs[i, (nSpline - 1) - (k + 1)] *= scale[i];
                }
            }

            int[,] jj = new int[nSpline, pieces];
            for (int i = 0; i < nSpline; i++)
            {
                for (int j = 0; j < pieces; j++)
                {
                    if (i == 0)
                    {
                        jj[i, j] = nSpline * (1 + j);
                    }
                    else
                    {
                        jj[i, j] = deg;
                    }
                }
            }

            for (int i = 1; i < nSpline; i++)
            {
                for (int j = 0; j < pieces; j++)
                {
                    jj[i, j] += jj[i - 1, j];
                }
            }

            double[,] coefsOut = new double[nSpline * pieces, nSpline];
            for (int i = 0; i < nSpline * pieces; i++)
            {
                int jjFlat = jj[i % nSpline, i / nSpline] - 1;
                for (int j = 0; j < nSpline; j++)
                {
                    coefsOut[i, j] = coefs[jjFlat, j];
                }
            }

            int[] xsB = new int[size * nSpline];
            int[] indexB = new int[size * nSpline];

            int breakInd = 1;
            for (int i = 0; i < size; i++)
            {
                if (breakInd < nBreaks - 1 && i >= breaks[breakInd])
                {
                    breakInd += 1;
                }
                for (int j = 0; j < nSpline; j++)
                {
                    xsB[i * nSpline + j] = i - breaks[breakInd - 1];
                    indexB[i * nSpline + j] = j + (breakInd - 1) * nSpline;
                }
            }

            double[] vB = new double[size * nSpline];
            for (int i = 0; i < size * nSpline; i++)
            {
                vB[i] = coefsOut[indexB[i], 0];
            }

            for (int i = 1; i < nSpline; i++)
            {
                for (int j = 0; j < size * nSpline; j++)
                {
                    vB[j] = vB[j] * xsB[j] + coefsOut[indexB[j], i];
                }
            }

            double[] A = new double[size * (nSpline + 1)];
            for (int i = 0; i < (nSpline + 1) * size; i++)
            {
                A[i] = 0;
            }
            breakInd = 0;
            for (int i = 0; i < nSpline * size; i++)
            {
                if (i / nSpline >= breaks[1])
                {
                    breakInd = 1;
                }
                A[(i % nSpline) + breakInd + (i / nSpline) * (nSpline + 1)] = vB[i];
            }

            double[] x = new double[nSpline + 1];
            lsqsolve_sub(size, nSpline + 1, A, y, x);

            double[,] C = new double[pieces + nSpline - 1, nSpline * pieces];
            for (int i = 0; i < nSpline + 1; i++)
            {
                for (int j = 0; j < nSpline * pieces; j++)
                {
                    C[i, j] = 0;
                }
            }

            for (int i = 0; i < nSpline * nSpline * pieces; i++)
            {
                int CRow = i % nSpline + (i / nSpline) % 2;
                int CCol = i / nSpline;
                int coefRow = i % (nSpline * 2);
                int coefCol = i / (nSpline * 2);
                C[CRow, CCol] = coefsOut[coefRow, coefCol];
            }

            double[,] coefsSpline = new double[pieces, nSpline];
            for (int i = 0; i < pieces; i++)
            {
                for (int j = 0; j < nSpline; j++)
                {
                    coefsSpline[i, j] = 0;
                }
            }

            for (int j = 0; j < nSpline * pieces; j++)
            {
                int coefCol = j / pieces;
                int coefRow = j % pieces;
                for (int i = 0; i < nSpline + 1; i++)
                {
                    coefsSpline[coefRow, coefCol] += C[i, j] * x[i];
                }
            }

            for (int i = 0; i < size; i++)
            {
                int secondHalf = i < breaks[1] ? 0 : 1;
                yOut[i] = coefsSpline[secondHalf, 0];
            }

            for (int i = 1; i < nSpline; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int secondHalf = j < breaks[1] ? 0 : 1;
                    yOut[j] = yOut[j] * (j - breaks[1] * secondHalf) + coefsSpline[secondHalf, i];
                }
            }

            return 0;
        }
    }
}
