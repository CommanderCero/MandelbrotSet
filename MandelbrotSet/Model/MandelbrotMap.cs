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

            var pixeldx = (int)(dx / stepSizeX);
            var newArr = new int[MapHeight][];
            if (pixeldx > 0)
            {
                for (var i = 0; i < MapHeight; i++)
                {
                    newArr[i] = new int[MapWidth];
                    for (var x = pixeldx; x < MapWidth; x++)
                    {
                        newArr[i][x - pixeldx] = Map[i][x];
                    }
                }
            }
            else
            {
                for (var i = 0; i < MapHeight; i++)
                {
                    newArr[i] = new int[MapWidth];
                    for (var x = 0; x < MapWidth - pixeldx; x++)
                    {
                        newArr[i][pixeldx + x - 1] = Map[i][x];
                    }
                }
            }
            

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
