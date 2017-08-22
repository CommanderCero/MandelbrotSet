﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MandelbrotSet.Annotations;
using MandelbrotSet.Model;

namespace MandelbrotSet
{

    public class MandelbrotViewModel : IMandelbrotSetViewModel, INotifyPropertyChanged
    {
        public MandelbrotMap Map { get; set; }

        public Command<MouseDragEvent> OnDrag { get; }

        public MandelbrotViewModel()
        {
            Map = new MandelbrotMap(900, 700, -2, 1, -1, 1);
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

        public void Move()
        {
            Map.Move(1, 0);
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
