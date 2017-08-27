using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MandelbrotSet.Model;

namespace MandelbrotSet
{
    public class MandelbrotBitmapConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 || !(values[0] is int[][] arr) || !(values[1] is int width) || !(values[2] is int height))
                return null;

            //Initialize image and stuff
            var nStride = (width * PixelFormats.Bgra32.BitsPerPixel + 7) / 8;
            var ImageDimentions = new Int32Rect(0, 0, width, height);
            var ImageArr = new byte[height * nStride];

            //Calculate stepsize
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var index = (y * width + x) * 4;
                    var m = arr[y][x];
                    var t = (255 * m / MandelbrotMap.MaximumIterations);
                    var color = 255 - (int)(m * 255.0 / MandelbrotMap.MaximumIterations);

                    ImageArr[index + 0] = (byte)color;   //Blue
                    ImageArr[index + 1] = (byte)color;   //Green
                    ImageArr[index + 2] = (byte)color;   //Red
                    ImageArr[index + 3] = 255; //Alpha
                }
            }

            //Push your data to a Bitmap
            var BmpToWriteOn = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            BmpToWriteOn.WritePixels(ImageDimentions, ImageArr, nStride, 0, 0);

            return BmpToWriteOn;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
