using HandwriteApp.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Media;
using Windows.Storage;

namespace HandwriteApp.ViewModels
{
    public class MainViewModel : Observable
    {
        private HandwriteModel Model { get; set; }

        public ObservableCollection<RecognizedResult> Results { get; } = new ObservableCollection<RecognizedResult>();

        public MainViewModel()
        {
        }

        public void ClearLogs()
        {
            Results.Clear();
        }

        public async Task RecognizeAsync(VideoFrame image)
        {
            await InitializeModelAsync();
            var output = await Model.EvaluateAsync(new HandwriteInput
            {
                Input3 = ImageFeatureValue.CreateFromVideoFrame(image),
            });
            var scores = output.Plus214_Output_0.GetAsVectorView().ToList();
            var answer = scores.IndexOf(scores.Max());
            Results.Add(new RecognizedResult
            {
                Result = answer.ToString(),
                Zero = scores[0],
                One = scores[1],
                Two = scores[2],
                Three = scores[3],
                Four = scores[4],
                Five = scores[5],
                Six = scores[6],
                Seven = scores[7],
                Eight = scores[8],
                Nine = scores[9],
                Timestamp = DateTimeOffset.Now,
            });
        }

        private async Task InitializeModelAsync()
        {
            if (Model != null)
            {
                return;
            }

            var modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/mnist.onnx"));
            Model = await HandwriteModel.CreateFromStreamAsync(modelFile);
        }
    }

    public class RecognizedResult
    {
        public string Result { get; set; }
        public float Zero { get; set; }
        public float One { get; set; }
        public float Two { get; set; }
        public float Three { get; set; }
        public float Four { get; set; }
        public float Five { get; set; }
        public float Six { get; set; }
        public float Seven { get; set; }
        public float Eight { get; set; }
        public float Nine { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
