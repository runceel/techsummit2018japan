using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace CrossDeviceSample.AppService
{
    public sealed class CrossDeviceAppService : IBackgroundTask
    {
        private TaskManager TaskManager { get; } = new TaskManager();
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // TaskManager.Register(taskInstance);
            Debug.WriteLine($"CrossDeviceAppService#Run called: {taskInstance.InstanceId}");
        }
    }

    class TaskManager
    {
        private readonly Dictionary<Guid, TaskInfo> _taskInfos = new Dictionary<Guid, TaskInfo>();
        public void Register(IBackgroundTaskInstance instance)
        {
            if (_taskInfos.ContainsKey(instance.InstanceId))
            {
                return;
            }

            var taskInfo = TaskInfo.CreateFromTaskInstance(instance);
            if (taskInfo == null)
            {
                return;
            }

            _taskInfos.Add(instance.InstanceId, taskInfo);
            taskInfo.Received += TaskInfo_Received;
            taskInfo.Closed += TaskInfo_Closed;
        }

        private void TaskInfo_Closed(Guid id)
        {
            if (_taskInfos.Remove(id, out var taskInfo))
            {
                taskInfo.Received -= TaskInfo_Received;
                taskInfo.Closed -= TaskInfo_Closed;
            }
        }

        private async void TaskInfo_Received((TaskInfo sender, AppServiceRequest request) args)
        {
            if (args.sender.IsRemote)
            {
                // from graph
                var command = new CrossDeviceMessage(args.request.Message);
                foreach (var task in _taskInfos.Values.Where(x => !x.IsRemote).ToArray())
                {
                    await task.Connection.SendMessageAsync(command.ToValueSet());
                }
            }
            else
            {
                // from local
                // no op
            }
        }
    }

    class TaskInfo
    {
        public event Action<(TaskInfo sender, AppServiceRequest request)> Received;
        public event Action<Guid> Closed;
        public Guid Id { get; }
        public bool IsRemote { get; }
        public AppServiceConnection Connection { get; }
        private BackgroundTaskDeferral Deferral { get; }
        private TaskInfo(IBackgroundTaskInstance instance, AppServiceTriggerDetails details)
        {
            Id = instance.InstanceId;
            Connection = details.AppServiceConnection;
            IsRemote = details.IsRemoteSystemConnection;
            Connection.RequestReceived += Connection_RequestReceived;
            Deferral = instance.GetDeferral();
            instance.Canceled += Instance_Canceled;
        }

        private void Instance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            Deferral?.Complete();
            Closed?.Invoke(Id);
        }

        private void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            Received?.Invoke((this, args.Request));
        }

        public static TaskInfo CreateFromTaskInstance(IBackgroundTaskInstance instance)
        {
            if (!(instance.TriggerDetails is AppServiceTriggerDetails details))
            {
                return null;
            }

            return new TaskInfo(instance, details);
        }
    }
}
