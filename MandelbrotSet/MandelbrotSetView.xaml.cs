using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MandelbrotSet.Model;

namespace MandelbrotSet
{
    public class MouseDragEvent : EventArgs
    {
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }

        public MouseDragEvent(double x, double y)
        {
            DeltaX = x;
            DeltaY = y;
        }
    }

    public partial class MandelbrotSetView : UserControl
    {
        #region DependencyProperties
        public static readonly DependencyProperty MapProperty = 
            DependencyProperty.Register("Map", typeof(int[][]), typeof(MandelbrotSetView), new PropertyMetadata(null, PropertyChangedCallback));
        public static readonly DependencyProperty MapWidthProperty =
            DependencyProperty.Register("MapWidth", typeof(int), typeof(MandelbrotSetView), new PropertyMetadata(0, PropertyChangedCallback));
        public static readonly DependencyProperty MapHeightProperty =
            DependencyProperty.Register("MapHeight", typeof(int), typeof(MandelbrotSetView), new PropertyMetadata(0, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if(dependencyObject is MandelbrotSetView obj && obj.IsLoaded)
            {
                obj.Render();
            }
        }

        public int[][] Map
        {
            get => (int[][]) GetValue(MapProperty);
            set => SetValue(MapProperty, value);
        }

        public int MapWidth
        {
            get => (int)GetValue(MapWidthProperty);
            set => SetValue(MapProperty, value);
        }

        public int MapHeight
        {
            get => (int)GetValue(MapHeightProperty);
            set => SetValue(MapProperty, value);
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

        public void Render()
        {
            if (Map == null)
            {
                Display.Source = null;
                return;
            }

            //Initialize image and stuff
            var nStride = (MapWidth * PixelFormats.Bgra32.BitsPerPixel + 7) / 8;
            var ImageDimentions = new Int32Rect(0, 0, MapWidth, MapHeight);
            var ImageArr = new byte[MapHeight * nStride];

            //Calculate stepsize
            var map = Map;
            for (var x = 0; x < MapWidth; x++)
            {
                for (var y = 0; y < MapHeight; y++)
                {
                    var index = (y * MapWidth + x) * 4;
                    var m = map[y][x];
                    var t = (255 * m / MandelbrotMap.MaximumIterations);
                    var color = 255 - (int)(m * 255.0 / MandelbrotMap.MaximumIterations);

                    ImageArr[index + 0] = (byte)color;   //Blue
                    ImageArr[index + 1] = (byte)color;   //Green
                    ImageArr[index + 2] = (byte)color;   //Red
                    ImageArr[index + 3] = 255; //Alpha
                }
            }

            //Push your data to a Bitmap
            var BmpToWriteOn = new WriteableBitmap(MapWidth, MapHeight, 96, 96, PixelFormats.Bgra32, null);
            BmpToWriteOn.WritePixels(ImageDimentions, ImageArr, nStride, 0, 0);

            //Push your bitmap to Xaml Image
            Display.Source = BmpToWriteOn;
        }

        private bool dragging;
        private Point dragStart;

        private void Display_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            dragStart = e.GetPosition(this);
        }

        private void Display_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
        }

        private void Display_OnMouseLeave(object sender, MouseEventArgs e)
        {
            dragging = false;
            var vm = (MandelbrotViewModel)DataContext;
            var dragEventArgs = CreateEvent(dragStart, e.GetPosition(this));
            if (vm.OnDrag != null && vm.OnDrag.CanExecute(dragEventArgs))
                vm.OnDrag.Execute(dragEventArgs);
        }

        private void Display_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging)
                return;

            var currPosition = e.GetPosition(this);
            var vm = (MandelbrotViewModel)DataContext;
            var dragEventArgs = CreateEvent(dragStart, currPosition);

            if (vm.OnDrag == null || !vm.OnDrag.CanExecute(dragEventArgs))
                return;

            vm.OnDrag.Execute(dragEventArgs);
            dragStart = currPosition;
        }

        private MouseDragEvent CreateEvent(Point start, Point end)
        {
            var dx = end.X - start.X;
            var dy = end.Y - start.Y;

            return new MouseDragEvent(dx / ActualWidth, dy / ActualHeight);
        }
    }
}
