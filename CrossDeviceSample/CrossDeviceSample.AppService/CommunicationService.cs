using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace CrossDeviceSample.AppService
{
    public sealed class CommunicationService : IBackgroundTask
    {
        private static string ClientFamilyName { get; } = "fb16b4dd-f06d-4f94-a790-1bc4860ec651_5ppbtxp1sbcde";

        private static Dictionary<Guid, (AppServiceConnection con, BackgroundTaskDeferral deferral)> WpfAppConnections { get; } = new Dictionary<Guid, (AppServiceConnection con, BackgroundTaskDeferral deferral)>();
        private static Dictionary<Guid, (AppServiceConnection con, BackgroundTaskDeferral deferral)> GraphAppConnections { get; } = new Dictionary<Guid, (AppServiceConnection con, BackgroundTaskDeferral deferral)>();

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine($"Run invoked: {GetHashCode()}, {Process.GetCurrentProcess().Id}");
            if (!(taskInstance.TriggerDetails is AppServiceTriggerDetails details))
            {
                return;
            }

            if (details.CallerPackageFamilyName == ClientFamilyName)
            {
                var d = taskInstance.GetDeferral();
                taskInstance.Canceled += WpfTaskInstance_Canceled;
                var con = details.AppServiceConnection;
                con.RequestReceived += WpfAppConnection_RequestReceived;
                con.ServiceClosed += WpfAppConnection_ServiceClosed;

                WpfAppConnections.Add(taskInstance.InstanceId, (con, d));
            }
            else
            {
                var d = taskInstance.GetDeferral();
                taskInstance.Canceled += GraphTaskInstance_Canceled;
                var con = details.AppServiceConnection;
                con.RequestReceived += GraphAppConnection_RequestReceived;

                GraphAppConnections.Add(taskInstance.InstanceId, (con, d));
            }
        }

        private void WpfAppConnection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            Debug.WriteLine($"WpfAppConnection closed: {args.Status}");
        }

        private async void GraphAppConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var d = args.GetDeferral();
            var receivedMessage = args.Request.Message;
            if (receivedMessage.TryGetValue("message", out var x))
            {
                foreach (var wpfConInfo in WpfAppConnections.Values)
                {
                    var sendMessage = new ValueSet();
                    sendMessage.Add("message", x);
                    await wpfConInfo.con.SendMessageAsync(sendMessage);
                }
            }

            var responseMessage = new ValueSet();
            responseMessage.Add("timestamp", DateTimeOffset.Now);
            responseMessage.Add("machineCount", WpfAppConnections.Count);
            var responseStatus = await args.Request.SendResponseAsync(responseMessage);
            Debug.WriteLine(responseStatus);
            d.Complete();
        }

        private void GraphTaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (GraphAppConnections.Remove(sender.InstanceId, out var x))
            {
                x.deferral.Complete();
                x.con.RequestReceived -= GraphAppConnection_RequestReceived;
            }
        }

        private void WpfAppConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
        }

        private void WpfTaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (WpfAppConnections.Remove(sender.InstanceId, out var x))
            {
                x.deferral.Complete();
                x.con.RequestReceived -= WpfAppConnection_RequestReceived;
            }
        }
    }
}
