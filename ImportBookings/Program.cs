using System;
using System.IO;
using System.Reflection;
using ImportBookings.Domain.Repositories;
using log4net;
using Topshelf;
using Topshelf.FileSystemWatcher;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ImportBookings
{   
    public class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ImportService Service = new ImportService();
        private static readonly string Dir = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\ftp5";

        public static void Main(string[] args)
        {
            /* For testing. Setting last processed date */
            var settings = new SettingsRepository();
            settings.AddOrUpdateSetting(Globals.LastProcessedKey, "20160218");
            settings.Save();
       
            /********************** Web ServiceFacade ************************/
            HostFactory.Run(x =>
            {

                x.UseLog4Net();
                x.Service<Program>(s =>
                {
                    
                    s.ConstructUsing(() => new Program());
                    s.BeforeStartingService(b => { BeforeStarting(); });
                    s.WhenStarted((service, host) => true);
                    s.WhenFileSystemCreated(config =>
                        config.AddDirectory(dir =>
                        {
                            dir.Path = Dir;
                            dir.FileFilter = "*.txt";
                            dir.CreateDir = false;
                            dir.NotifyFilters = NotifyFilters.FileName;
                        }), FileArrived);
                    s.WhenStopped((service, host) => true);
                });
                x.RunAsLocalSystem();

                x.SetDescription("ImportService missing data to Apisci");
                x.SetDisplayName("Apisci ImportService Module");
                x.SetServiceName("ApisciImportModule");
            });

        }
        /**************************************************************************/

        /// <summary>
        /// Will run on every new file arriving in folder
        /// </summary>
        private static void FileArrived(TopshelfFileSystemEventArgs topshelfFileSystemEventArgs)
        {
            Logger.Info("New file arrived");
            Service.AddFilesToQueue(topshelfFileSystemEventArgs.FullPath);
            Service.RunProcess();
        }

        /// <summary>
        /// Will process files that are in folder when service starts
        /// </summary>
        private static void BeforeStarting()
        {
            var initialFiles = Directory.GetFiles(Dir);
            var runProcess = false;
            foreach (var file in initialFiles)
            {
                Service.AddFilesToQueue(file);
                runProcess = true;
            }

            if (runProcess)
            {
                Service.RunProcess();
            }
        }

    }
}
