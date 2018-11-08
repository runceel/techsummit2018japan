using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;

namespace CrossDeviceSample.WPF
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private MainWindowViewModel ViewModel { get; set; }
        private AppServiceConnection Connection { get; set; }

        public async Task ConnectAppServiceAsync()
        {
            if (Connection != null)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    ViewModel.Status = $"Already connected.";
                });
                return;
            }

            var connection = new AppServiceConnection
            {
                AppServiceName = "CommunicationService",
                PackageFamilyName = Package.Current.Id.FamilyName,
            };
            var status = await connection.OpenAsync();
            await Dispatcher.InvokeAsync(() =>
            {
                ViewModel.Status = $"App service connection status: {status}";
            });

            if (status == AppServiceConnectionStatus.Success)
            {
                Connection = connection;
                Connection.RequestReceived += async (s, args) =>
                {
                    var requestMessage = args.Request.Message;
                    if (requestMessage.TryGetValue("value", out var x))
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            ViewModel.Message = $"Received message: {x}";
                        });
                    }
                };
            }
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Debug.WriteLine(Package.Current.Id.FamilyName);
            // VM setup
            var param = e.Args.FirstOrDefault() ?? "techsummit2018demo:?message=Launch with no arguments.";
            var query = ParseQuery(param);

            ViewModel = new MainWindowViewModel();
            if (query.TryGetValue("message", out var message))
            {
                ViewModel.Message = message;
            }

            MainWindow = new MainWindow
            {
                DataContext = ViewModel,
            };

            MainWindow.Show();
        }

        private static IDictionary<string, string> ParseQuery(string arg)
        {
            var n = HttpUtility.ParseQueryString(new Uri(arg).Query ?? "");
            return n.Keys.OfType<string>().ToDictionary(x => x, x => n[x]);
        }
    }
}
