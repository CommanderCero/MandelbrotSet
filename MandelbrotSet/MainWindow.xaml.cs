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

        public void RightClick(object sender, RoutedEventArgs e)
        {
            var vm = (MandelbrotViewModel)DataContext;
            vm.Move(0.2);
        }

        public void LeftClick(object sender, RoutedEventArgs e)
        {
            var vm = (MandelbrotViewModel)DataContext;
            vm.Move(-0.2);
        }
    }
}
