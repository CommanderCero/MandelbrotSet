using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MandelbrotSet.Model;

namespace MandelbrotSet
{
    /// <summary>
    /// Interaktionslogik für MandelbrotSet.xaml
    /// </summary>
    public partial class MandelbrotSetView : UserControl
    {
        #region DependencyProperties
        public static readonly DependencyProperty RealStartProperty = 
           DependencyProperty.Register("RealStart", typeof(double), typeof(MandelbrotSetView), new PropertyMetadata(-2.0, PropertyChangedCallback));

        public static readonly DependencyProperty RealEndProperty =
            DependencyProperty.Register("RealEnd", typeof(double), typeof(MandelbrotSetView), new PropertyMetadata(1.0, PropertyChangedCallback));

        public static readonly DependencyProperty ImaginaryStartProperty =
            DependencyProperty.Register("ImaginaryStart", typeof(double), typeof(MandelbrotSetView), new PropertyMetadata(-1.0, PropertyChangedCallback));

        public static readonly DependencyProperty ImaginaryEndProperty =
            DependencyProperty.Register("ImaginaryEnd", typeof(double), typeof(MandelbrotSetView), new PropertyMetadata(1.0, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if(dependencyObject is MandelbrotSetView obj)
            {
                obj.Render();
            }
        }

        public double RealStart
        {
            get => (double)GetValue(RealStartProperty);
            set => SetValue(RealStartProperty, value);
        }

        public double RealEnd
        {
            get => (double)GetValue(RealEndProperty);
            set => SetValue(RealEndProperty, value);
        }

        public double ImaginaryStart
        {
            get => (double)GetValue(ImaginaryStartProperty);
            set => SetValue(ImaginaryStartProperty, value);
        }

        public double ImaginaryEnd
        {
            get => (double)GetValue(ImaginaryEndProperty);
            set => SetValue(ImaginaryEndProperty, value);
        }
        #endregion

        public MandelbrotSetView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Render();
        }

        private void Render()
        {
            if (!IsLoaded)
                return;

            //Initialize image and stuff
            var width = (int)ActualWidth;
            var height = (int)ActualHeight;
            var nStride = (width * PixelFormats.Bgra32.BitsPerPixel + 7) / 8;
            var ImageDimentions = new Int32Rect(0, 0, width, height);
            var ImageArr = new byte[height * nStride];

            //Calculate stepsize
            var dre = (RealEnd - RealStart) / width;
            var dim = (ImaginaryEnd - ImaginaryStart) / height;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var index = (y * width + x) * 4;
                    
                    var c = new Complex(RealStart + dre * x, ImaginaryStart + dim * y);
                    var m = MandelbrotHelper.GetIterations(c);
                    var t = (255 * m / MandelbrotHelper.MaximumIterations);
                    var color = 255 - (int)(m * 255.0 / MandelbrotHelper.MaximumIterations);

                    ImageArr[index + 0] = (byte)color;   //Blue
                    ImageArr[index + 1] = (byte)color;   //Green
                    ImageArr[index + 2] = (byte)color;   //Red
                    ImageArr[index + 3] = 255; //Alpha
                }
            }

            //Push your data to a Bitmap
            var BmpToWriteOn = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            BmpToWriteOn.WritePixels(ImageDimentions, ImageArr, nStride, 0, 0);

            //Push your bitmap to Xaml Image
            Display.Source = BmpToWriteOn;
        }
    }
}
