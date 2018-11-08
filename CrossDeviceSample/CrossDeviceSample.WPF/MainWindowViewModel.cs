using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Windows.ApplicationModel.AppService;

namespace CrossDeviceSample.WPF
{
    public class MainWindowViewModel : BindableBase
    {
        private AppServiceConnection Connection { get; set; }
        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _receivedMessage;
        public string ReceivedMessage
        {
            get { return _receivedMessage; }
            set { SetProperty(ref _receivedMessage, value); }
        }

        public ObservableCollection<History> Histories { get; } = new ObservableCollection<History>();

        public MainWindowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(Histories, new object());
        }

        public async Task InitializeConnectionAsync()
        {
            if (Connection != null)
            {
                return;
            }

            Connection = new AppServiceConnection
            {
                PackageFamilyName = Consts.CommunicationServiceHost,
                AppServiceName = Consts.CommunicationServiceName,
            };
            Connection.RequestReceived += Connection_RequestReceived;
            Connection.ServiceClosed += Connection_ServiceClosed;

            while (true)
            {
                var status = await Connection.OpenAsync();
                Status = $"{status}";
                if (status == AppServiceConnectionStatus.Success)
                {
                    break;
                }

                await Task.Delay(2000);
            }
        }

        private async void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            Debug.WriteLine("Closed!!");
            Connection = null;
            await InitializeConnectionAsync();
        }

        private void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var d = args.GetDeferral();
            if (args.Request.Message.TryGetValue("message", out var x))
            {
                Histories.Insert(0, new History { Value = ReceivedMessage });
                ReceivedMessage = $"{x}";
            }
            d.Complete();
        }
    }

    public class History
    {
        public string Value { get; set; }

        public override string ToString() => Value;
    }
}
