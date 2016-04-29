using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportWeb.DAL.Entities;

namespace ImportWeb.Repositories
{
    public interface ISettingsRepository : IDisposable
    {
        IDictionary<string, string> GetAllKeysValues();
        string GetValue(string key);
        void AddOrUpdateSetting(string key, string value);
        void AddOrUpdateSetting(Settings keyValue);
        IEnumerable<Settings> GetAllSettings();
        void DeleteSetting(string id);
        void Save();
    }
}
