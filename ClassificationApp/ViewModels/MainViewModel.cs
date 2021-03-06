﻿using AttributesExtraction;
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
using KeywordsExtraction;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClassificationApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _labelName = "places";
        private int _nearestNeighboursNumber = 2;
        private int _coldStartSamples = 100;
        private int _samplesToClassify = 1000;
        private ExtractorType _extractorType;
        private MetricType _metricType;
        private string _directoryFilePath = @"C:\Users\Mateusz\Desktop\reuters";
        private const string _chosenDataPath = @".\data\data.json";
        private int _filesInDirectory;
        private List<string> _listOfFiles;
        private List<RawSample> _listOfRawSamples;
        private List<PreProcessedSample> _listOfPreProcessedSamples = new List<PreProcessedSample>();
        private List<PreProcessedSample> _listOfLearningSamples = new List<PreProcessedSample>();
        private ConcurrentQueue<DataSample> _concurrentBagOfDataSamples;
        private bool _shouldUseJsonDataFile;
        private bool _isDataSgm;
        private bool _showNGramInput = false;
        private int _nForNGram = 2;
        private readonly List<ClassifiedDataSample> _listOfClassifiedSamples = new List<ClassifiedDataSample>();
        private readonly JsonSerializer _serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented
        };

        private StopWordsFilter _stopWordsFilter = new StopWordsFilter(WellKnownNames.StopWords);
        private Lemmatizer _lemmatizer = new Lemmatizer();
        private IAttributeExtractor _extractor;
        private decimal _learningDataRatio = new decimal(0.4);

        #region observable props
        public int NForNGram
        {
            get => _nForNGram;
            set => SetProperty(ref _nForNGram, value);
        }
        public bool ShowNGramInput
        {
            get => _showNGramInput;
            set => SetProperty(ref _showNGramInput, value);
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
            set
            {
                SetProperty(ref _extractorType, value);
                ShowNGramInput = value == ExtractorType.NGram;
            }
        }
        
        public MetricType MetricType
        {
            get => _metricType;
            set => SetProperty(ref _metricType, value);
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
        public bool IsDataSgm
        {
            get => _isDataSgm;
            set => SetProperty(ref _isDataSgm, value);
        }
        public List<PreProcessedSample> ListOfPreProcessedSamples
        {
            get => _listOfPreProcessedSamples;
            set => SetProperty(ref _listOfPreProcessedSamples, value);
        }
        public int SamplesToClassify
        {
            get => _samplesToClassify;
            set => SetProperty(ref _samplesToClassify, value);
        }

        public decimal LearningDataRatio
        {
            get => _learningDataRatio;
            set => SetProperty(ref _learningDataRatio, value);
        }
        #endregion

        #region Commands
        public IRaiseCanExecuteCommand OpenDirectoryCommand { get; }
        public IRaiseCanExecuteCommand LoadFilesCommand { get; }
        public IRaiseCanExecuteCommand ExtractCommand { get; }
        public IRaiseCanExecuteCommand ExtractMoreCommand { get; }
        public IRaiseCanExecuteCommand ClassifyCommand { get; }
        public IRaiseCanExecuteCommand ResultCommand { get; }
        public IRaiseCanExecuteCommand ChangeUseJsonDataFile { get; }
        public IRaiseCanExecuteCommand ChangeIsDataSgm { get; }
        #endregion

        public ConcurrentBag<ClassifiedDataSample> ConcurrentBagOfClassifiedSamples { get; set; } = new ConcurrentBag<ClassifiedDataSample>();
        public int HowManyPopularKeywords { get; set; } = 15;
        public int HowManyUnpopularKeywords { get; set; } = 5;

        public MainViewModel()
        {
            OpenDirectoryCommand = new RelayCommand(ReadDirectoryPath);
            LoadFilesCommand = new RelayCommand(LoadFiles);
            ExtractCommand = new RelayCommand(ExtractNew);
            ExtractMoreCommand = new RelayCommand(ExtractAdditional);
            ClassifyCommand = new RelayCommand(ClassifySamples);
            ResultCommand = new RelayCommand(ShowResults);
            ChangeUseJsonDataFile = new RelayCommand(() => _shouldUseJsonDataFile = !_shouldUseJsonDataFile);
            ChangeIsDataSgm = new RelayCommand(() => _isDataSgm = !_isDataSgm);
        }

        private void ReadDirectoryPath()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowDialog();
            DirectoryFilePath = folderDialog.SelectedPath;
        }

        private void LoadFiles()
        {
            ListOfPreProcessedSamples = new List<PreProcessedSample>();
            if (ShouldUseJsonDataFile)
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                using (JsonReader reader = new JsonTextReader(File.OpenText(Path.ChangeExtension(Path.Combine(DirectoryFilePath, "data"), "json"))))
                {
                    ListOfPreProcessedSamples = serializer.Deserialize<List<PreProcessedSample>>(reader);
                }
                FilesInDirectory = 1;
            }
            else
            {
                if (_isDataSgm)
                {
                    _listOfFiles = Directory.GetFiles(DirectoryFilePath).Where(p => Path.GetExtension(p) == ".sgm").ToList();
                    FilesInDirectory = _listOfFiles.Count;
                    _listOfRawSamples = new List<RawSample>();
                    foreach (string path in _listOfFiles)
                    {
                        _listOfRawSamples.AddRange(DataSamplesReader.ReadAllSamples(path, LabelName));
                    }
                    TryFilterLabels();
                }
                else
                {
                    _listOfRawSamples = OneLinerReader.ReadAllSamples(
                        Directory.GetFiles(DirectoryFilePath).First(p => Path.GetFileName(p) == "data.txt"),
                        LabelName);
                    FilesInDirectory = 1;
                }
                var allPreProcessedSamples = _listOfRawSamples
                    .Select(s => _lemmatizer.LemmatizeSample(_stopWordsFilter.Filter(s))).ToList();

                //split data into learn and test
                _listOfLearningSamples = allPreProcessedSamples.Take((int)(LearningDataRatio * allPreProcessedSamples.Count)).ToList();
                ListOfPreProcessedSamples = allPreProcessedSamples.Skip((int)(LearningDataRatio * allPreProcessedSamples.Count)).ToList();

                var randomizer = new Random();
                ListOfPreProcessedSamples = ListOfPreProcessedSamples.OrderBy(s => randomizer.Next()).ToList();

                //create and save new keywords
                KeywordExtractor.Extract(_listOfLearningSamples);

                //save results to json
                using (StreamWriter file = File.CreateText(Path.ChangeExtension(Path.Combine(DirectoryFilePath, "data"), "json")))
                {
                    _serializer.Serialize(file, ListOfPreProcessedSamples);
                }
            }
        }

        private void TryFilterLabels()
        {
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
        }

        private void ExtractNew()
        {
            _extractor = ResolveExtractor();
            _concurrentBagOfDataSamples = new ConcurrentQueue<DataSample>(_extractor.Extract(ListOfPreProcessedSamples));
        }

        private void ExtractAdditional()
        {
            _extractor = ResolveExtractor();
            var samplesWithNewAttributes = new ConcurrentQueue<DataSample>(_extractor.Extract(ListOfPreProcessedSamples));
            _concurrentBagOfDataSamples = new ConcurrentQueue<DataSample>(
                _concurrentBagOfDataSamples
                    .Zip(samplesWithNewAttributes,
                        (o, n) => new DataSample(
                            new AttributesDictionary(o.Attributes.Values
                                .Concat(n.Attributes.Values)
                                .GroupBy(a => a.Key)
                                .Select(group => group.First())
                                .ToDictionary(kv => kv.Key, kv => kv.Value)),
                            o.Labels)));
        }

        private void ClassifySamples()
        {
            if (_coldStartSamples + _samplesToClassify > _concurrentBagOfDataSamples.Count)
            {
                MessageBox.Show($"There's only {_concurrentBagOfDataSamples.Count} samples, cannot take {_coldStartSamples + _samplesToClassify}");
                return;
            }
            var learnedData = new SamplesCollection(
                _concurrentBagOfDataSamples
                .Take(_coldStartSamples)
                .ToList());

            ConcurrentBagOfClassifiedSamples = new ConcurrentBag<ClassifiedDataSample>();
            _concurrentBagOfDataSamples
                .Skip(_coldStartSamples)
                .Take(_samplesToClassify)
                .AsParallel()
                .ForAll(s => ConcurrentBagOfClassifiedSamples.Add(
                    NearestNeighboursClassifier.Classify(
                        s,
                        learnedData,
                        _nearestNeighboursNumber,
                        ResolveMetric()))
                    );
        }

        private void ShowResults()
        {
            if (ConcurrentBagOfClassifiedSamples != null)
            {
                ResultsWindow resultsWindow = new ResultsWindow(new ResultsViewModel(ConcurrentBagOfClassifiedSamples.ToList()));
                resultsWindow.Show();
            }
        }

        private IAttributeExtractor ResolveExtractor()
        {
            switch (_extractorType)
            {
                case ExtractorType.Count:
                    return new CountExtractor(LoadKeywords(), "Counted-key-words");
                case ExtractorType.TFMWords:
                    return new TFMExtractor();
                case ExtractorType.TFMKeyWords:
                    return new KeyWordsExtractor(LoadKeywords());
                case ExtractorType.KeywordsIndex:
                    return new KeywordIndexExtractor(LoadKeywords());
                case ExtractorType.NGram:
                    return new NGramExtractor(3);
                default:
                    throw new NotSupportedException($"Cannot construct this extractor type: {_extractorType.ToString()}");
            }
        }
        
        private IMetric ResolveMetric()
        {
            switch (_metricType)
            {
                case MetricType.Chebyshev:
                    return new ChebyshevMetric();
                case MetricType.Euclidean:
                    return new EuclideanMetric();
                case MetricType.Manhattan:
                    return new ManhattanMetric();
                case MetricType.Cosin:
                    return new CosMetric();
                default:
                    throw new NotSupportedException($"Cannot construct this extractor type: {_metricType.ToString()}");
            }
        }

        private List<string> LoadKeywords()
        {
            var keyWords = KeywordsSelector.LoadKeywords(HowManyPopularKeywords, HowManyUnpopularKeywords);
            using (StreamWriter file = File.CreateText(Path.ChangeExtension(Path.Combine(Directory.GetCurrentDirectory(), "keywords"), "json")))
            {
                _serializer.Serialize(file, keyWords);
            }

            return keyWords;
        }
    }
}
