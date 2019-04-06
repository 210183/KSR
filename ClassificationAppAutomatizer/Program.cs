using ClassificationApp.ViewModels;

namespace ClassificationAppAutomatizer
{
    class Program
    {
        private const int K_Max = 5;
        private const int ColdStart_Min = 5;
        private const int ColdStart_Step = 5;
        private const int ColdStart_Max = 5;
        private const int SamplesToClassify = 5;
        private const int ClassificationTries = 5;
        private const int KeywordsExtractionTries = 5;

        static void Main(string[] args)
        {
            MainViewModel context = new MainViewModel();
        }
    }
}
