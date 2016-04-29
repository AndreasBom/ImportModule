using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ImportBookings.Domain;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Repositories;
using ImportBookings.Exceptions;
using log4net;
using Newtonsoft.Json;
using Polly;

namespace ImportBookings
{
    public class ImportService
    {
        #region private properties
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ServiceFacade _serviceFacade;
        private readonly IList<string> _fileList = new List<string>();
        #endregion

        public ImportService()
        {
            _serviceFacade = new ServiceFacade(new Processing(), new ProcessedDataRepository(), new SettingsRepository());
        }

        public void AddFilesToQueue(string file)
        {
            if (file.Contains(Globals.BookingFileName) ||
                file.Contains(Globals.ChargesFileName) ||
                file.Contains(Globals.PassengerFileName))
            {
                _fileList.Add(file);
            }

        }

        public void RunProcess()
        {
            //If any files are in queue to be sent
            var savedData = _serviceFacade.GetUnSentData();
            var hasChanged = false;

            if (savedData.Any())
            {
                Logger.Info("Unsent data found.");

                //Send
                ProcessedData item = new ProcessedData();
                try
                {
                    while (savedData.Count > 0)
                    {
                        item = savedData.Dequeue();
                        Policy
                            .Handle<SendErrorException>()
                            .WaitAndRetry(2, retryCount => TimeSpan.FromSeconds(1))
                            .Execute(() =>
                                        Posting.SendTest(item.Reference, item.Action,
                                        JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Data)));
                        _serviceFacade.RemoveData(item);
                        hasChanged = true;
                        Logger.Info("Successfully sent all data");
                    }


                }
                catch (Exception ex)
                {
                    Logger.Error("Could not send files");
                    //Save processed but not sent data to database
                    while (savedData.Count > 0)
                    {
                        _serviceFacade.AddProcessedData(item);
                        item = savedData.Dequeue();
                    }
                    _serviceFacade.Save();
                }

                finally
                {
                    if (hasChanged)
                    {
                        _serviceFacade.Save();
                    }
                }
            }

            //No saved data exists
            if (!savedData.Any())
            {
                Logger.Info("LastProcessedFile: " + _serviceFacade.GetLastProcessedDate());
                var listOfFileSets = _serviceFacade.GetNewSetsOfFiles(_fileList).ToList();
                hasChanged = false;
                var listOfFileSetsBackup = listOfFileSets;
                var processedQueue = new Queue<ProcessedData>();
                
                //Process
                if (listOfFileSets.Any())
                {
                    

                    foreach (var fileSet in listOfFileSets.OrderBy(f => f.Key))
                    {
                        var lastProcessedFileSet = DateTime.ParseExact(_serviceFacade.GetLastProcessedDate(), "yyyyMMdd",
                            CultureInfo.InvariantCulture);

                        if (fileSet.Key == lastProcessedFileSet.AddDays(1))
                        {
                            Logger.Info("Will process new sets of files");
                            //Prepare files before sending
                            processedQueue = _serviceFacade.ProcessFile(fileSet);
                            _serviceFacade.AddOrUpdateLastProcessedDate(fileSet.Key.ToString("yyyyMMdd"));
                            hasChanged = true;
                        }
                    }

                    //Send
                    ProcessedData item = new ProcessedData();
                    try
                    {
                        while(processedQueue.Count > 0)
                        {
                            item = processedQueue.Dequeue();
                            Policy
                                .Handle<SendErrorException>()
                                .WaitAndRetry(2, retryCount => TimeSpan.FromSeconds(1))
                                .Execute(() =>
                                            Posting.SendTest(item.Reference, item.Action,
                                            JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Data))
                                            );

                            Logger.Info("Successfully sent all data");
                        }
                    }

                    catch (Exception ex)
                    {   
                        Logger.Error("Could not send files");
                        //Save processed but not sent data to database
                        while (processedQueue.Count > 0)
                        {
                            _serviceFacade.AddProcessedData(item);
                            item = processedQueue.Dequeue();
                        }
                        _serviceFacade.Save();
                    }

                    finally
                    {
                        if (hasChanged)
                        {
                            _serviceFacade.Save();
                        }
                    }
                }
            }
        }
    }
}
