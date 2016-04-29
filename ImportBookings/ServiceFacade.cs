using System;
using System.Collections.Generic;
using System.Linq;
using ImportBookings.Domain;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Models;
using ImportBookings.Domain.Repositories;


namespace ImportBookings
{
    public class ServiceFacade
    {
        private readonly CsvFileManager _csvFileManager = new CsvFileManager();
        private readonly IProcessing _processing;
        private readonly IProcessedDataRepository _dataRepository;
        private readonly ISettingsRepository _settingsRepository;

        public IList<Source> FileSetsWithMissingFiles { get; set; } = new List<Source>();
        public IList<IGrouping<DateTime, Source>> FileSetsNonChronological { get; set; } = new List<IGrouping<DateTime, Source>>();


        public ServiceFacade()
            : this(new Processing(), new ProcessedDataRepository(), new SettingsRepository())
        {
            //Empty
        }

        public ServiceFacade(IProcessing processing, IProcessedDataRepository dataRepository, SettingsRepository settingsRepository)
        {
            _processing = processing;
            _dataRepository = dataRepository;
            _settingsRepository = settingsRepository;
        }

        
        public ServiceFacade(IProcessing processing, IProcessedDataRepository dataRepository, ISettingsRepository settingsRepository)
        {
            _processing = processing;
            _dataRepository = dataRepository;
            _settingsRepository = settingsRepository;
        }

        public void AddOrUpdateLastProcessedDate(string value)
        {
            _settingsRepository.AddOrUpdateSetting(Globals.LastProcessedKey, value);
        }

        public string GetLastProcessedDate()
        {
            return _settingsRepository.GetValue(Globals.LastProcessedKey);
        }
        
        public IEnumerable<IGrouping<DateTime, Source>> GetNewSetsOfFiles(IList<string> listOfFiles)
        {
            var bookingFileName = Globals.BookingFileName;
            var passengerFileName = Globals.PassengerFileName;
            var chargesFileName = Globals.ChargesFileName;

            var fileList = new List<IGrouping<DateTime, Source>>(3);

            var filesGroupedByDate = _csvFileManager
                .PopulateModel(listOfFiles)
                .GroupBy(f => f.Date).ToList().OrderBy(x => x.Key);

            FileSetsWithMissingFiles.Clear();
            FileSetsNonChronological.Clear();

            foreach (var fileSet in filesGroupedByDate)
            {
                //If all three needed csv files is present
                if (fileSet.Any(b => b.Identification == bookingFileName &&
                                     fileSet.Any(p => p.Identification == passengerFileName) &&
                                     fileSet.Any(c => c.Identification == chargesFileName)))
                {
                    fileList.Add(fileSet);
                }
                else
                {
                    foreach (var source in fileSet)
                    {
                        FileSetsWithMissingFiles.Add(
                            new Source
                            {
                                Date = source.Date,
                                Identification = source.Identification,
                                Path = source.Path
                            });
                    }
                }
            }

            return fileList.OrderBy(f => f.Key).ToList();
        }

        public void AddProcessedData(ProcessedData data)
        {
            _dataRepository.AddData(data);
        }

        public void RemoveData(ProcessedData data)
        {
            _dataRepository.DeleteData(data.Id);
        }

        public Queue<ProcessedData> GetUnSentData()
        {
            var queue = new Queue<ProcessedData>(_dataRepository.GetAllData());
            return queue;
        }

        public void Save()
        {
            _settingsRepository.Save();
            _dataRepository.Save();
        }

        public Queue<ProcessedData> ProcessFile(IEnumerable<Source> fileSet)
        {
            try
            {
               return  _processing.Process(fileSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
