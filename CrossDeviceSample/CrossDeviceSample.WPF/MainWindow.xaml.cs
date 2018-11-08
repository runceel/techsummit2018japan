using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;

namespace CrossDeviceSample.WPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine(Package.Current.Id.FamilyName);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeConnectionAsync();
        }
    }
}
