using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace ImportBookings.Domain
{
    public class FileListener : IFileListener
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDictionary<Guid, FileSystemWatcher> _fileSystemWatchers = new Dictionary<Guid, FileSystemWatcher>();

        public void StartListening(Guid id, string directory, string filter, Action<string> fileArrived)
        {
            if (_fileSystemWatchers.ContainsKey(id))
            {
                Logger.Error($"A watcher with id: {id} has already been added!");
                throw new InvalidOperationException($"A watcher with id: {id} has already been added!");
            }
               
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var files = Directory.GetFiles(directory, filter).OrderBy(File.GetCreationTimeUtc).ToList();

            foreach (var file in files)
            {
                Logger.Info($"Initializing with existing file: \"{file}\"");
                var currentFile = file;
                fileArrived(currentFile);
                //var task = new Task(() => fileArrived(currentFile));
                //try
                //{
                //    //task.RunSynchronously();
                //    task.Start();
                //    task.Wait();
                //}
                //catch (AggregateException ae)
                //{
                //    Logger.Error(ae);
                //}
            }

            var fileSystemWatcher = new FileSystemWatcher(directory, filter)
            {
                EnableRaisingEvents = true
            };

            fileSystemWatcher.Created += (sender, eventArgs) =>
            {
                Logger.Info($"New file arrived: \"{eventArgs.FullPath}\"");
                fileArrived(eventArgs.FullPath);
            };
        }





        public void StopListeners()
        {
            Logger.Info("Disposing existing file listeners");

            foreach (var fileSystemWatcher in _fileSystemWatchers)
                fileSystemWatcher.Value.Dispose();

            _fileSystemWatchers.Clear();
        }

        public void StopListener(Guid id)
        {
            if (!_fileSystemWatchers.ContainsKey(id))
            {
                Logger.Error($"A watcher with id: {id} has not been added!");
                throw new InvalidOperationException($"A watcher with id: {id} has not been added!");
            }

            Logger.Info("Disposing existing file listeners");
            var fileSystemWatcher = _fileSystemWatchers[id];

            fileSystemWatcher.Dispose();
            _fileSystemWatchers.Remove(id);
        }
    }
}
