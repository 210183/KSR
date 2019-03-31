using ClassificationApp.Base;
using FileSamplesRead;
using FileSamplesRead.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ClassificationApp.ViewModels
{
    class MainViewModel : BindableBase
    {
        private decimal _percentageOfLearningFiles;
        private string _labelName;
        private int _nearestNeighboursNumber;
        private int _coldStartSamples;
        //private ExtractorType _extractorType;
        private string _directoryFilePath;
        private int _filesInDirectory;
        private List<string> _listOfFiles;
        private List<RawSample> _listOfRawSamples;

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
        //public ExtractorType ExtractorType
        //{
        //    get => _extractorType;
        //    set => SetProperty(ref _extractorType, value);
        //}
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
        #endregion

        #region Commands
        public IRaiseCanExecuteCommand OpenDirectoryCommand { get; }
        public IRaiseCanExecuteCommand LoadFilesCommand { get; }
        public IRaiseCanExecuteCommand ExtractCommand { get; }
        public IRaiseCanExecuteCommand ClassifyCommand { get; }
        public IRaiseCanExecuteCommand ResultCommand { get; }
        #endregion

        public MainViewModel()
        {
            OpenDirectoryCommand = new RelayCommand(ReadDirectoryPath);
            LoadFilesCommand = new RelayCommand(LoadFiles);
            ExtractCommand = new RelayCommand(ExtractFromSamples);
            ClassifyCommand = new RelayCommand(ClassifySamples);
            ResultCommand = new RelayCommand(ShowResults);
        }


        private void ReadDirectoryPath()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowDialog();
            DirectoryFilePath = folderDialog.SelectedPath;

            _listOfFiles = Directory.GetFiles(DirectoryFilePath).Where(p => Path.GetExtension(p) == ".sgm").ToList();
            FilesInDirectory = _listOfFiles.Count;

        }

        private void LoadFiles()
        {
            var dataReader = new DataSamplesReader();
            _listOfRawSamples = new List<RawSample>();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            foreach (string path in _listOfFiles)
            {
                _listOfRawSamples = dataReader.ReadAllSamples(path, LabelName);
                using (StreamWriter file = File.CreateText(Path.ChangeExtension(path, "json")))
                {
                    serializer.Serialize(file, _listOfRawSamples);
                }
            }
        }

        private void ExtractFromSamples()
        {
            throw new NotImplementedException();
        }

        private void ClassifySamples()
        {
            throw new NotImplementedException();
        }

        private void ShowResults()
        {
            throw new NotImplementedException();
        }



    }
}
