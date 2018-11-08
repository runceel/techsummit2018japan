using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace PSFApp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string DataFilePath { get; } = "data.txt";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(DataFilePath))
            {
                textBlock.Text = File.ReadAllText(DataFilePath);
            }
            else
            {
                textBlock.Text = "Not found";
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(DataFilePath, DateTimeOffset.Now.ToString());
            }
            catch (Exception ex)
            {
                textBlock.Text = ex.Message;
            }
        }
    }
}
