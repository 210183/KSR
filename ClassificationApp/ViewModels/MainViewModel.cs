using AttributesExtraction;
using AttributesExtraction.Extractors;
using Classification;
using Classification.Metrics;
using ClassificationApp.Base;
using ClassificationApp.Views;
using Core;
using Core.Models;
using Core.Models.Concrete;
using DataPreprocessing;
using FileSamplesRead;
using FileSamplesRead.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClassificationApp.ViewModels


{
    internal class MainViewModel : BindableBase
    {
        private decimal _percentageOfLearningFiles;
        private string _labelName = "places";
        private int _nearestNeighboursNumber = 2;
        private int _coldStartSamples = 100;
        private ExtractorType _extractorType;
        private string _directoryFilePath = @"C:\Users\Mateusz\Desktop\reuters_przetworzone";
        private int _filesInDirectory;
        private List<string> _listOfFiles;
        private List<RawSample> _listOfRawSamples;
        private List<PreProcessedSample> _listOfPreProcessedSamples = new List<PreProcessedSample>();
        private SamplesCollection _listOfDataSamples;
        private ConcurrentBag<DataSample> _concurrentBagOfDataSamples;
        private bool _shouldUseJsonDataFile;

        private readonly List<ClassifiedDataSample> _listOfClassifiedSamples = new List<ClassifiedDataSample>();
        private readonly ConcurrentBag<ClassifiedDataSample> _concurrentBagOfClassifiedSamples = new ConcurrentBag<ClassifiedDataSample>();
        private readonly JsonSerializer _serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented
        };

        private StopWordsFilter _stopWordsFilter = new StopWordsFilter(WellKnownNames.StopWords);
        private Lemmatizer _lemmatizer = new Lemmatizer();
        private IAttributeExtractor _extractor;
        private IMetric _metric = new ManhattanMetric();

        #region observable props
        public decimal PercentageOfLearningFiles
        {
            get => _percentageOfLearningFiles;
            set => SetProperty(ref _percentageOfLearningFiles, value);
        }
        public string LabelName
        {
            get => _labelName;
            set => SetProperty(ref _labelName, value);
        }
        public int NearestNeighboursNumber
        {
            get => _nearestNeighboursNumber;
            set => SetProperty(ref _nearestNeighboursNumber, value);
        }
        public int ColdStartSamples
        {
            get => _coldStartSamples;
            set => SetProperty(ref _coldStartSamples, value);
        }
        public ExtractorType ExtractorType
        {
            get => _extractorType;
            set => SetProperty(ref _extractorType, value);
        }
        public string DirectoryFilePath
        {
            get => _directoryFilePath;
            private set => SetProperty(ref _directoryFilePath, value);
        }
        public int FilesInDirectory
        {
            get => _filesInDirectory;
            private set => SetProperty(ref _filesInDirectory, value);
        }

        public bool ShouldUseJsonDataFile
        {
            get => _shouldUseJsonDataFile;
            set => SetProperty(ref _shouldUseJsonDataFile, value);
        }
        #endregion

        #region Commands
        public IRaiseCanExecuteCommand OpenDirectoryCommand { get; }
        public IRaiseCanExecuteCommand LoadFilesCommand { get; }
        public IRaiseCanExecuteCommand ExtractCommand { get; }
        public IRaiseCanExecuteCommand ClassifyCommand { get; }
        public IRaiseCanExecuteCommand ResultCommand { get; }
        public IRaiseCanExecuteCommand ChangeUseJsonDataFile { get; }
        #endregion

        public MainViewModel()
        {
            OpenDirectoryCommand = new RelayCommand(ReadDirectoryPath);
            LoadFilesCommand = new RelayCommand(LoadFiles);
            ExtractCommand = new RelayCommand(ExtractFromSamples);
            ClassifyCommand = new RelayCommand(ClassifySamples);
            ResultCommand = new RelayCommand(ShowResults);
            ChangeUseJsonDataFile = new RelayCommand(() => _shouldUseJsonDataFile = !_shouldUseJsonDataFile);
        }


        private void ReadDirectoryPath()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowDialog();
            DirectoryFilePath = folderDialog.SelectedPath;
        }

        private void LoadFiles()
        {
            _listOfPreProcessedSamples = new List<PreProcessedSample>();
            if (ShouldUseJsonDataFile)
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                using (JsonReader reader = new JsonTextReader(File.OpenText(Path.ChangeExtension(Path.Combine(DirectoryFilePath, "data"), "json"))))
                {
                    _listOfPreProcessedSamples = serializer.Deserialize<List<PreProcessedSample>>(reader);
                }
            }
            else
            {
                _listOfFiles = Directory.GetFiles(DirectoryFilePath).Where(p => Path.GetExtension(p) == ".sgm").ToList();
                FilesInDirectory = _listOfFiles.Count;
                _listOfRawSamples = new List<RawSample>();
                foreach (string path in _listOfFiles)
                {
                    _listOfRawSamples.AddRange(DataSamplesReader.ReadAllSamples(path, LabelName));
                }

                //load labels filter
                string labelsFilterPath = Path.Combine(Directory.GetCurrentDirectory(), "labelsFilter.json");
                if (File.Exists(labelsFilterPath))
                {
                    using (JsonReader file = new JsonTextReader(File.OpenText(labelsFilterPath)))
                    {
                        var labelsFilter = _serializer.Deserialize<List<string>>(file);
                        _listOfRawSamples = _listOfRawSamples
                            .Where(s => s.Labels.Values.Count == 1 && labelsFilter.Contains(s.Labels.Values.Single().Name))
                            .ToList();
                    }
                }

                foreach (var sample in _listOfRawSamples)
                {
                    _listOfPreProcessedSamples.Add(_lemmatizer.LemmatizeSample(_stopWordsFilter.Filter(sample)));
                }

                //save results to json
                using (StreamWriter file = File.CreateText(Path.ChangeExtension(Path.Combine(DirectoryFilePath, "data"), "json")))
                {
                    _serializer.Serialize(file, _listOfPreProcessedSamples);
                }
            }
        }

        private void ExtractFromSamples()
        {
            _extractor = new TFMExtractor();
            //_listOfDataSamples = new SamplesCollection(_extractor.Extract(_listOfPreProcessedSamples));
            _concurrentBagOfDataSamples = new ConcurrentBag<DataSample>(_extractor.Extract(_listOfPreProcessedSamples));
        }

        private void ClassifySamples()
        {
            var randomizer = new Random();
            var learnedData = new SamplesCollection(_concurrentBagOfDataSamples
                .OrderBy(s => randomizer.Next())
                .Take(_coldStartSamples).ToList());
            //_listOfDataSamples.Samples.Skip(_coldStartSamples)
            //    .ToList()
            //    .ForEach(s => _listOfClassifiedSamples.Add(
            //        NearestNeighboursClassifier.Classify(
            //            s,
            //            learnedData,
            //            _nearestNeighboursNumber,
            //            _metric
            //    )));

            _concurrentBagOfDataSamples
                .Skip(_coldStartSamples)
                .AsParallel()
                .ForAll(s => _concurrentBagOfClassifiedSamples.Add(
                    NearestNeighboursClassifier.Classify( 
                        s,
                        learnedData,
                        _nearestNeighboursNumber,
                        _metric))
                    );
        }

        private void ShowResults()
        {
            if (_concurrentBagOfClassifiedSamples != null)
            {
                ResultsWindow resultsWindow = new ResultsWindow(new ResultsViewModel(_concurrentBagOfClassifiedSamples.ToList()));
                resultsWindow.Show();
            }
        }
    }
}
