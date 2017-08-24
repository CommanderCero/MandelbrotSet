using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MandelbrotSet.Model
{
    public class MandelbrotMap
    {
        #region Variables
        public static int MaximumIterations = 80;

        public int[][] Map { get; set; }

        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

        public double RealStart { get; private set; }
        public double RealEnd { get; private set; }
        public double ImaginaryStart { get; private set; }
        public double ImaginaryEnd { get; private set; }
        #endregion

        public MandelbrotMap(int mapWidth, int mapHeight, double realStart, double realEnd, double imaginaryStart, double imaginaryEnd)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;

            RealStart = realStart;
            RealEnd = realEnd;
            ImaginaryStart = imaginaryStart;
            ImaginaryEnd = imaginaryEnd;

            GenerateMap();
        }

        public void Move(double dx, double dy)
        {
            var stepSizeX = (RealEnd - RealStart) / MapWidth;
            var stepSizeY = (ImaginaryEnd - ImaginaryStart) / MapHeight;
            
            //Remove excess
            dx -= dx % stepSizeX;
            dy -= dy % stepSizeY;

            var newRealStart = RealStart + dx;
            var newRealEnd = RealEnd + dx;
            var newImaginaryStart = ImaginaryStart + dy;
            var newImaginaryEnd = ImaginaryEnd + dy;
            
            var newArr = new int[MapHeight][];
            var copyPosition = (int)(dx < 0 ? 0 : dx - 1);
            var calculatePosition = (int)(dx > 0 ? 0 : MapHeight - dx - 1);
            for (var y = 0; y < dx / stepSizeX; y++)
            {
                newArr[calculatePosition + y] = new int[MapWidth];
                for (var x = 0; x < MapWidth; x++)
                {
                    newArr[y][x] = GetIterations(new Complex(newRealStart + stepSizeX * x, newImaginaryStart + stepSizeY * y));
                }
            }

            for (var y = 0; y < MapHeight - dx - 1; y++)
            {
                newArr[copyPosition + y] = new int[MapWidth];
                for (var x = 0; x < MapWidth; x++)
                {
                    newArr[y][x] = Map[y][x];
                }
            }

            RealStart = newRealStart;
            RealEnd = newRealEnd;
            ImaginaryStart = newImaginaryStart;
            ImaginaryEnd = newImaginaryEnd;
            Map = newArr;
        }

        private void GenerateMap()
        {
            var dx = (RealEnd - RealStart) / MapWidth;
            var dy = (ImaginaryEnd - ImaginaryStart) / MapHeight;

            var arr = new int[MapHeight][];
            for (var y = 0; y < MapHeight; y++)
            {
                arr[y] = new int[MapWidth];
                for (var x = 0; x < MapWidth; x++)
                {
                    arr[y][x] = GetIterations(new Complex(RealStart + dx * x, ImaginaryStart + dy * y));
                }
            }

            Map = arr;
        }

        //Calculates if the given c is bounded for the Mandelbrotformula
        //Zn+1 = Zn * Zn + c
        private static int GetIterations(Complex c)
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
    }
}
