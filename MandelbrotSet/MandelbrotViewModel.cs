using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MandelbrotSet.Annotations;

namespace MandelbrotSet
{
    public interface IMandelbrotViewModel
    {
        double RealStart { get; set; }
        double RealEnd { get; set; }
        double ImaginaryStart { get; set; }
        double ImaginaryEnd { get; set; }
    }

    public class MandelbrotViewModel : IMandelbrotViewModel, INotifyPropertyChanged
    {
        #region Properties
        private double realStart = -2, realEnd = 1, imaginaryStart = -1, imaginaryEnd = 1;

        public double RealStart
        {
            get => realStart;
            set { realStart = value; OnPropertyChanged(nameof(RealStart)); }
        }
        public double RealEnd
        {
            get => realEnd;
            set { realEnd = value; OnPropertyChanged(nameof(RealEnd)); }
        }
        public double ImaginaryStart
        {
            get => imaginaryStart;
            set { imaginaryStart = value; OnPropertyChanged(nameof(ImaginaryStart)); }
        }
        public double ImaginaryEnd
        {
            get => imaginaryEnd;
            set { imaginaryEnd = value; OnPropertyChanged(nameof(ImaginaryEnd)); }
        }
        #endregion
        
        public ICommand click = new Command<object>(val => true, val => MessageBox.Show("Hallo Welt " + val));

        public ICommand Click => click;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
