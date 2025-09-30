using System;
using System.Numerics;

namespace Catch22Sharp
{
    public static class Fft
    {
        public static void twiddles(Span<Complex> a)
        {
            int size = a.Length;
            double PI = 3.14159265359;

            for (int i = 0; i < size; i++)
            {
                Complex tmp = Complex.Zero - PI * i / size * Complex.ImaginaryOne;
                a[i] = Complex.Exp(tmp);
                //a[i] = cexp(-I * M_PI * i / size);
            }
        }

        private static void _fft(Complex[] a, Complex[] @out, int size, int step, Complex[] tw, int aOffset, int outOffset)
        {
            if (step < size)
            {
                _fft(@out, a, size, step * 2, tw, outOffset, aOffset);
                _fft(@out, a, size, step * 2, tw, outOffset + step, aOffset + step);

                for (int i = 0; i < size; i += 2 * step)
                {
                    int idx = outOffset + i;
                    Complex t = tw[i] * @out[idx + step];
                    a[aOffset + i / 2] = @out[idx] + t;
                    a[aOffset + (i + size) / 2] = @out[idx] - t;
                }
            }
        }

        public static void fft(Span<Complex> a, Span<Complex> tw)
        {
            int size = a.Length;
            Complex[] aArray = a.ToArray();
            Complex[] outArray = new Complex[size];
            Array.Copy(aArray, outArray, size);
            Complex[] twArray = tw.ToArray();
            _fft(aArray, outArray, size, 1, twArray, 0, 0);
            for (int i = 0; i < size; i++)
            {
                a[i] = aArray[i];
            }
        }
    }
}
