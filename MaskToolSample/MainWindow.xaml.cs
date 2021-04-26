using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MaskToolSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            maskControl.AppPath = "mspaint.exe";
        }

        private void ActiveMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //maskControl.ActiveMainWindow();
            maskControl.IsAppWindowActive = false;
        }

        private void ActiveOtherAppWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //maskControl.ActiveOtherWindow();
            maskControl.IsAppWindowActive = true;
        }

    }
}
