using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportBookings.Domain.DAL.Entities;

namespace ImportBookings.Domain.Repositories
{
    public interface ISettingsRepository : IDisposable
    {
        IDictionary<string, string> GetAllKeysValues();
        string GetValue(string key); 
        void AddOrUpdateSetting(string key, string value);
        void AddOrUpdateSetting(Settings keyValue);
        void DeleteSetting(string id);
        void Save();
    }
}
