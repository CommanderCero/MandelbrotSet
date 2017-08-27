using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MandelbrotSet.Model;

namespace MandelbrotSet
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Point startPoint;
        private bool dragging;
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition((IInputElement)sender);
            dragging = true;
        }

        private void UIElement_OnMouseLeftButtonUp_(object sender, MouseButtonEventArgs e)
        {
            var endPoint = e.GetPosition((IInputElement) sender);
            ((MandelbrotViewModel)DataContext).OnDrag(new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y));
            dragging = false;
        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging)
                return;
            
            var endPoint = e.GetPosition((IInputElement)sender);
            if (endPoint.Equals(startPoint))
                return;

            ((MandelbrotViewModel)DataContext).OnDrag(new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y));
            startPoint = endPoint;
            
        }
    }
}
