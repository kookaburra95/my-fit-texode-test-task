using System;
using System.Collections.Generic;
using System.IO;
using DevExpress.Mvvm;
using MyFitTexodeTest.Model;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using MyFitTexodeTest.Services;

namespace MyFitTexodeTest.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly string _path = $"{Environment.CurrentDirectory}\\Data";
        private Data _selectedDataItem;
        private FIleIOService _fileIoService;

        public SeriesCollection SeriesCollection { get; set; }
        public DelegateCommand ExportJsonCommand { get; private set; }
        public DelegateCommand ExportXmlCommand { get; private set; }
        public DelegateCommand ExportCsvCommand { get; private set; }

        public List<Data> Data { get; }

        public MainWindowViewModel()
        {
            var allRecords = Record.GetAllRecords();

            var groupRecords = (from record in allRecords
                                group record by record.User into g
                let avg = g.Average(u => u.Steps)
                let max = g.Max(u => u.Steps)
                let min = g.Min(u => u.Steps)
                select new Data
                {
                    UserName = g.Key,
                    AvgStep = (int)avg,
                    MaxStep = max,
                    MinStep = min,
                    Steps = (
                            from r in allRecords where r.User == g.Key select r.Steps)
                        .Take(new DirectoryInfo(_path).GetFiles().Length).ToList(),
                    Rank = (
                            from r in allRecords where r.User == g.Key select r.Rank).ToList().First(),
                    Status = (
                            from r in allRecords where r.User == g.Key select r.Status).ToList().First()
                }).ToList();

            Data = groupRecords;
            ExportJsonCommand = new DelegateCommand(ExportJson);
            ExportXmlCommand = new DelegateCommand(ExportXml);
            ExportCsvCommand = new DelegateCommand(ExportCsv);
        }

        private void ExportCsv()
        {
            if (SelectedDataItem != null)
            {
                _fileIoService = new FIleIOService($"{Environment.CurrentDirectory}\\{SelectedDataItem.UserName}.csv");

                try
                {
                    _fileIoService.ExportDataCsv(SelectedDataItem);
                    MessageBox.Show("Экспорт в CSV завершен!");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Не выбран пользователь!");
            }
        }

        private void ExportXml()
        {
            if (SelectedDataItem != null)
            {
                _fileIoService = new FIleIOService($"{Environment.CurrentDirectory}\\{SelectedDataItem.UserName}.xml");

                try
                {
                    _fileIoService.ExportDataXml(SelectedDataItem);
                    MessageBox.Show("Экспорт в XML завершен!");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Не выбран пользователь!");
            }
        }

        private void ExportJson()
        {
            if (SelectedDataItem != null)
            {
                _fileIoService = new FIleIOService($"{Environment.CurrentDirectory}\\{SelectedDataItem.UserName}.json");

                try
                {
                    _fileIoService.ExportDataJson(SelectedDataItem);
                    MessageBox.Show("Экспорт в JSON завершен!");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Не выбран пользователь!");
            }
        }

        public Data SelectedDataItem
        {
            get => _selectedDataItem;
            set
            {
                _selectedDataItem = value;

                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = _selectedDataItem.Steps.AsChartValues()
                    }
                };
            }
        }
    }
}
        