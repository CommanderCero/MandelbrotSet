using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MandelbrotSet.Model
{
    public static class MandelbrotHelper
    {
        public static int MaximumIterations = 80;

        //Calculates if the given c is bounded for the Mandelbrotformula
        //Zn+1 = Zn * Zn + c
        public static int GetIterations(Complex c)
        {
            var z = Complex.Zero;
            var n = 0;
            while (z.Magnitude <= 2 && n < MaximumIterations)
            {
                z = z * z + c;
                n++;
            }

            return n;
        }

        public static int[][] Generate2DMap(int width, int height, double realStart, double realEnd, double imaginaryStart, double imaginaryEnd)
        {
            var dx = (realEnd - realStart) / width;
            var dy = (imaginaryEnd - imaginaryStart) / height;

            var arr = new int[height][];
            for (var y = 0; y < height; y++)
            {
                arr[y] = new int[width];
                for (var x = 0; x < width; x++)
                {
                    arr[y][x] = GetIterations(new Complex(realStart + dx * x, imaginaryStart + dy * y));
                }
            }

            return arr;
        }
    }
}
