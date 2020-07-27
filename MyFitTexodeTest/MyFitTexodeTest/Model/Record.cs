using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using MyFitTexodeTest.Services;

namespace MyFitTexodeTest.Model
{
    public class Record : INotifyPropertyChanged
    {
        private static readonly string _path = $"{Environment.CurrentDirectory}\\Data";
        private static List<Record> _recordsList = new List<Record>();
        private static readonly List<Record> _allRecordsList = new List<Record>();
        private static FIleIOService _fileIoService;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Rank { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public int Steps { get; set; }

        public static List<Record> GetAllRecords()
        {
            var numOfFiles = new DirectoryInfo(_path).GetFiles().Length;

            for (int i = 1; i <= numOfFiles; i++)
            {
                _fileIoService = new FIleIOService(_path + $"\\day{i}.json");

                try
                {
                    _recordsList = _fileIoService.LoadData();
                    _allRecordsList.AddRange(_recordsList);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            return _allRecordsList;
        }
    }
}
