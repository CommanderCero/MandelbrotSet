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

            RealStart += dx;
            RealEnd += dx;
            ImaginaryStart += dy;
            ImaginaryEnd += dy;

            var pixeldx = (int)(dx / stepSizeX);
            var pixeldy = (int)(dy / stepSizeY);
            var newArr = new int[MapHeight][];
            for (var i = 0; i < newArr.Length; i++)
                newArr[i] = new int[MapWidth];

            //TODO Refactor Move
            #region dx
            if (pixeldx > 0)
            {
                pixeldx = Math.Abs(pixeldx);
                //Calculate new
                for (var y= 0; y < MapHeight; y++)
                {
                    for (var x = MapWidth - pixeldx; x < MapWidth; x++)
                    {
                        newArr[y][x] = GetIterations(new Complex(RealStart + x * stepSizeX, ImaginaryStart + y * stepSizeY));
                    }
                }

                //Copy
                for (var y = 0; y < MapHeight; y++)
                {
                    for (var x = pixeldx; x < MapWidth; x++)
                    {
                        newArr[y][x - pixeldx] = Map[y][x];
                    }
                }
            }
            else if(pixeldx < 0)
            {
                pixeldx = Math.Abs(pixeldx);
                //Calculate new
                for (var y = 0; y < MapHeight; y++)
                {
                    for (var x = 0; x < pixeldx; x++)
                    {
                        newArr[y][x] = GetIterations(new Complex(RealStart + x * stepSizeX, ImaginaryStart + y * stepSizeY));
                    }
                }

                //Copy
                for (var y = 0; y < MapHeight; y++)
                {
                    for (var x = 0; x < MapWidth - pixeldx; x++)
                    {
                        newArr[y][pixeldx + x] = Map[y][x];
                    }
                }
            }
            #endregion
            
            if (pixeldy > 0)
            {
                pixeldy = Math.Abs(pixeldy);
                //Calculate new
                for (var x = 0; x < MapWidth; x++)
                {
                    for (var y = MapHeight - pixeldy; y < MapHeight; y++)
                    {
                        newArr[y][x] = GetIterations(new Complex(RealStart + x * stepSizeX, ImaginaryStart + y * stepSizeY));
                    }
                }

                //Copy
                for (var x = 0; x < MapWidth; x++)
                {
                    for (var y = pixeldy; y < MapHeight; y++)
                    {
                        newArr[y - pixeldy][x] = Map[y][x];
                    }
                }
            }
            else if (pixeldy < 0)
            {
                pixeldy = Math.Abs(pixeldy);
                //Calculate new
                for (var x = 0; x < MapWidth; x++)
                {
                    for (var y = 0; y < pixeldy; y++)
                    {
                        newArr[y][x] = GetIterations(new Complex(RealStart + x * stepSizeX, ImaginaryStart + y * stepSizeY));
                    }
                }

                //Copy
                for (var x = 0; x < MapWidth; x++)
                {
                    for (var y = 0; y < MapHeight - pixeldy; y++)
                    {
                        newArr[y + pixeldy][x] = Map[y][x];
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
