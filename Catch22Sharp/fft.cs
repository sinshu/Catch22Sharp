using System;
using System.Numerics;

namespace Catch22Sharp
{
    public static class Fft
    {
        public static void twiddles(Span<Complex> a)
        {
            double pi = Math.PI;
            int size = a.Length;
            for (int i = 0; i < size; i++)
            {
                double angle = -pi * i / size;
                a[i] = Complex.Exp(new Complex(0.0, angle));
            }
        }

        private static void RecursiveFft(Complex[] a, Complex[] outBuf, int size, int step, Complex[] tw, int aOffset, int outOffset)
        {
            if (step < size)
            {
                RecursiveFft(outBuf, a, size, step * 2, tw, outOffset, aOffset);
                RecursiveFft(outBuf, a, size, step * 2, tw, outOffset + step, aOffset + step);

                for (int i = 0; i < size; i += 2 * step)
                {
                    int outIndex = outOffset + i;
                    Complex t = tw[i] * outBuf[outIndex + step];
                    a[aOffset + i / 2] = outBuf[outIndex] + t;
                    a[aOffset + (i + size) / 2] = outBuf[outIndex] - t;
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
            RecursiveFft(aArray, outArray, size, 1, twArray, 0, 0);
            for (int i = 0; i < size; i++)
            {
                a[i] = aArray[i];
            }
        }
    }
}
