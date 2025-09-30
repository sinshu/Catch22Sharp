//
//  butterworth.cs
//
//  Ported from butterworth.c
//  Created by Carl Henning Lubba on 23/09/2018.
//

using System;
using System.Numerics;

namespace Catch22Sharp
{
    internal static class Butterworth
    {
        public static void poly(ReadOnlySpan<Complex> x, Span<Complex> @out)
        {
            int size = x.Length;

            // initialise
            @out[0] = Complex.One;
            for (int i = 1; i < size + 1; i++)
            {
                @out[i] = Complex.Zero;
            }

            Complex[] outTempArray = new Complex[size + 1];
            Span<Complex> outTemp = outTempArray.AsSpan();
            Span<Complex> outSpan = @out.Slice(0, size + 1);

            for (int i = 1; i < size + 1; i++)
            {
                outSpan.CopyTo(outTemp);

                for (int j = 1; j < i + 1; j++)
                {
                    Complex temp1 = x[i - 1] * outTemp[j - 1];
                    Complex temp2 = @out[j];
                    @out[j] = temp2 - temp1;
                }
            }
        }

        public static void filt(ReadOnlySpan<double> y, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> @out)
        {
            /* Filter a signal y with the filter coefficients a and b, output to array out.*/

            double offset = y[0];
            int size = y.Length;
            int nCoeffs = a.Length;

            for (int i = 0; i < size; i++)
            {
                @out[i] = 0;
                for (int j = 0; j < nCoeffs; j++)
                {
                    if (i - j >= 0)
                    {
                        @out[i] += b[j] * (y[i - j] - offset);
                        @out[i] -= a[j] * @out[i - j];
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                @out[i] += offset;
            }
        }

        public static void reverse_array(Span<double> a)
        {
            /* Reverse the order of the elements in an array. Write back into the input array.*/

            int size = a.Length;
            double temp;
            for (int i = 0; i < size / 2; i++)
            {
                temp = a[i];
                a[i] = a[size - i - 1];
                a[size - 1 - i] = temp;
            }
        }

        public static void filt_reverse(ReadOnlySpan<double> y, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> @out)
        {
            /* Filter a signal y with the filter coefficients a and b _in reverse order_, output to array out.*/

            int size = y.Length;

            double[] yTempArray = y.ToArray();
            Span<double> yTemp = yTempArray.AsSpan();

            reverse_array(yTemp);

            double offset = yTemp[0];
            int nCoeffs = a.Length;

            for (int i = 0; i < size; i++)
            {
                @out[i] = 0;
                for (int j = 0; j < nCoeffs; j++)
                {
                    if (i - j >= 0)
                    {
                        @out[i] += b[j] * (yTemp[i - j] - offset);
                        @out[i] -= a[j] * @out[i - j];
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                @out[i] += offset;
            }

            reverse_array(@out);
        }
    }
}
