using System.Collections.Generic;
using System.Data.Entity;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;

namespace ImportBookings.Domain.Repositories
{
    /// <summary>
    /// Repository for data that is processed, but not sent
    /// </summary>
    public class ProcessedDataRepository : IProcessedDataRepository
    {
        private readonly ImportContext _context;

        public ProcessedDataRepository()
            :this(new ImportContext())
        {
            //Empty
        }

        public ProcessedDataRepository(ImportContext context)
        {
            _context = context;
        }

        public IEnumerable<ProcessedData> GetAllData()
        {
            return _context.ProcessedData;
        }

        public void AddData(ProcessedData data)
        {
            _context.ProcessedData.Add(data);
        }

        public void AddData(IEnumerable<ProcessedData> dataCollection)
        {
            foreach (var data in dataCollection)
            {
                _context.ProcessedData.Add(data);
            }
        }

        public void DeleteData(int id)
        {
            var data = _context.ProcessedData.Find(id);
            _context.ProcessedData.Remove(data);
        }

        public void UpdateData(ProcessedData data)
        {
            _context.Entry(data).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProcessedDataRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
