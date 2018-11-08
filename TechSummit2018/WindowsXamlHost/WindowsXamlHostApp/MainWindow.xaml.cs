using Microsoft.Toolkit.Wpf.UI.XamlHost;
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
using UWP = Windows.UI.Xaml.Controls;

namespace WindowsXamlHostApp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private UWP.InkCanvas _inkCanvas;
        private UWP.InkToolbar _inkToolbar;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InkCanvas_ChildChanged(object sender, EventArgs e)
        {
            _inkCanvas = ((WindowsXamlHost)sender).Child as UWP.InkCanvas;
            SetupInkControls();
        }

        private void InkToolbar_ChildChanged(object sender, EventArgs e)
        {
            _inkToolbar = ((WindowsXamlHost)sender).Child as UWP.InkToolbar;
            SetupInkControls();
        }

        private void SetupInkControls()
        {
            if (_inkCanvas == null || _inkToolbar == null)
            {
                return;
            }

            _inkToolbar.TargetInkCanvas = _inkCanvas;
            inkToolbarHost_LayoutUpdated(null, EventArgs.Empty);
            inkCanvasHost_LayoutUpdated(null, EventArgs.Empty);
        }

        private void inkToolbarHost_LayoutUpdated(object sender, EventArgs e)
        {
            if (_inkToolbar != null)
            {
                return;
            }

            _inkToolbar.Width = inkToolbarHost.Width;
            _inkToolbar.Height = inkToolbarHost.Height;
        }

        private void inkCanvasHost_LayoutUpdated(object sender, EventArgs e)
        {
            if (_inkCanvas != null)
            {
                return;
            }

            _inkCanvas.Width = inkCanvasHost.Width;
            _inkCanvas.Height = inkCanvasHost.Height;
        }
    }
}
