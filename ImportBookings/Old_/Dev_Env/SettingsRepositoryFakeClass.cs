using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Repositories;

namespace ImportBookings.Dev_Env
{
    public class SettingsRepositoryFakeClass : ISettingsRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetAllKeysValues()
        {
            throw new NotImplementedException();
        }

        public string GetValue(string key)
        {
            //for testing
            return "20160216";
        }

        public void AddSetting(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void AddSetting(Settings keyValue)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateSetting(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateSetting(Settings keyValue)
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting(string id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
