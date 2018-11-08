using System;
using HandwriteApp.ViewModels;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using WinML.Samples;

namespace HandwriteApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        private Helper Helper { get; } = new Helper();

        public MainPage()
        {
            InitializeComponent();

            inkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(new InkDrawingAttributes
            {
                Color = Colors.White,
                Size = new Size(22, 22),
                IgnorePressure = true,
                IgnoreTilt = true,
            });
        }

        private async void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            var image = await Helper.GetHandWrittenImage(inkCanvasHost);
            await ViewModel.RecognizeAsync(image);
        }

        private void ClearMenu_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }
    }
}
