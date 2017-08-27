using MandelbrotSet.Annotations;
using MandelbrotSet.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Point = System.Windows.Point;

namespace MandelbrotSet
{
    public class MandelbrotViewModel : INotifyPropertyChanged
    {
        private MandelbrotMap map;

        public int[][] Map => map.Map;
        public int MapWidth => map.MapWidth;
        public int MapHeight => map.MapHeight;

        public void OnDrag(Point vec)
        {
            map.Move(0, -vec.Y / MapHeight);
            map.Move(-vec.X / MapWidth, 0);
            OnPropertyChanged(nameof(Map));
        }

        public MandelbrotViewModel()
        {
            map = new MandelbrotMap(900, 700, -2, 1, -1, 1);
            //OnDrag = new Command<MouseDragEvent>(
            //    dragEvent =>
            //    {
            //        return counter++ % 5 == 0;
            //    },
            //    dragEvent =>
            //    {
            //        double width = RealEnd - RealStart;
            //        double height = ImaginaryEnd - ImaginaryStart;
            //        RealStart -= width * dragEvent.DeltaX;
            //        RealEnd -= width * dragEvent.DeltaX;
            //        ImaginaryStart -= height * dragEvent.DeltaY;
            //        ImaginaryEnd -= height * dragEvent.DeltaY;
            //    });
        }

        public void Move(double x)
        {
            map.Move(0, x);
            map.Move(x, 0);
            OnPropertyChanged(nameof(Map));
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
