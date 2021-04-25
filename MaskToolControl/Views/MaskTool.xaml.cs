using MaskToolControl.Model;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaskToolControl.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class MaskTool : UserControl, IDisposable
    {
        public MaskTool()
        {
            InitializeComponent();
        }

        public MaskToolModel MaskToolModel { get; set; }

        public string AppPath
        {
            get { return (string)GetValue(AppPathProperty); }
            set { SetValue(AppPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AppPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AppPathProperty =
            DependencyProperty.Register("AppPath", typeof(string), typeof(MaskTool), new PropertyMetadata(""));



        public bool IsAppWindowActive
        {
            get { return (bool)GetValue(IsAppWindowActiveProperty); }
            set { SetValue(IsAppWindowActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAppWindowActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAppWindowActiveProperty =
            DependencyProperty.Register("IsAppWindowActive", typeof(bool), typeof(MaskTool), new PropertyMetadata(false, OnIsAppWindowActiveChanged));

        private static void OnIsAppWindowActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue)
            {
                ((MaskTool)sender).ActiveOtherWindow();
            }
            else
            {
                ((MaskTool)sender).ActiveMainWindow();
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            var helper = new WindowInteropHelper(Window.GetWindow(window));
            MaskToolModel = new MaskToolModel(AppPath, helper.Handle);
            var loacation = this.TransformToAncestor(window).Transform(new Point(0, 0));
            MaskToolModel.LoadApplicationWindow((int)loacation.X, (int)loacation.Y, (int)this.ActualWidth, (int)this.ActualHeight);
        }

        public void ActiveMainWindow()
        {
            MaskToolModel.ActiveMainWindow();
        }

        public void ActiveOtherWindow()
        {
            MaskToolModel.ActiveOtherApp();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            MaskToolModel.Dispose();
        }

        public void Dispose()
        {
            MaskToolModel.Dispose();
        }

    }
}
