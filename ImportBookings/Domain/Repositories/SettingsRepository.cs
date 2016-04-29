using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Exceptions;

namespace ImportBookings.Domain.Repositories
{
    /// <summary>
    /// Repository for settings
    /// </summary>
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ImportContext _context;

        public SettingsRepository()
            :this(new ImportContext())
        {
            //Empty
        }

        public SettingsRepository(ImportContext context)
        {
            _context = context;
        }

        public string GetValue(string key)
        {
            var keyValue = this._context.Settings.Find(key);
            return keyValue.Value;
        }

        public IDictionary<string, string> GetAllKeysValues()
        {
            var dic = (from keyValue in _context.Settings
                       select new
                       {
                           keyValue.Key,
                           keyValue.Value
                       }).ToDictionary(kv => kv.Key, kv => kv.Value);

            return dic;
        }

        private void AddSetting(string key, string value)
         {
            var s = new Settings
            {
                Key = key,
                Value = value
            };
            _context.Settings.Add(s);
        }

        private void AddSetting(Settings keyValue)
        {
            _context.Settings.Add(keyValue);
        }

        public void DeleteSetting(string key)
        {
            var s = _context.Settings.Find(key);
            _context.Settings.Remove(s);
        }

        public void AddOrUpdateSetting(string key, string value)
        {
            if (_context.Settings.Find(key) != null)
            {
                var entity = _context.Settings.Find(key);
                entity.Value = value;
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                AddSetting(key, value);
            }

        }

        public void AddOrUpdateSetting(Settings keyValue)
        {
            if (_context.Settings.Find(keyValue.Key) != null)
            {
                _context.Entry(keyValue).State = EntityState.Modified;
            }
            else
            {
                AddSetting(keyValue);
            }
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new KeyAlreadyExistsException(ex.InnerException.ToString());
            }
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
        // ~SettingsRepository() {
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
