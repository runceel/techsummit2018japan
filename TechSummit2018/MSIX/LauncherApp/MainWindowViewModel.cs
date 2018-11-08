using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LauncherApp
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private DelegateCommand _launchCommand;
        public DelegateCommand LaunchCommand =>
            _launchCommand ?? (_launchCommand = new DelegateCommand(ExecuteLaunchCommand));

        private void ExecuteLaunchCommand()
        {
            LaunchImpl();
        }

        [Conditional("UWP")]
        private static void LaunchImpl()
        {
            var package = Windows.ApplicationModel
                .Package
                .Current
                .Dependencies
                .FirstOrDefault(x => x.IsOptional);
            if (package == null)
            {
                MessageBox.Show("会社固有設定の入った変更パッケージをインストールしてください。");
                return;
            }

            Process.Start(Path.Combine(package.InstalledLocation.Path, "SubApp.exe"));
        }

        public void LoadSettingsFromRegistry()
        {
            var registryKeyName = @"SOFTWARE\MsixDemoApp";
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = hklm.OpenSubKey(registryKeyName))
            {
                Title = key?.GetValue("BrandText") as string ?? $"レジストリーに値がみつかりません: {registryKeyName} の BrandText";
            }
        }
    }
}
