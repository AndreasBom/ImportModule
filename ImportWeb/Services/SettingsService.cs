using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImportWeb.Repositories;

namespace ImportWeb.Services
{
    public class SettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public string LastProcessedDate
        {
            get { return _settingsRepository.GetValue(Globals.LastProcessedKey); }
            set { _settingsRepository.AddOrUpdateSetting(Globals.LastProcessedKey, value); }
        }

        public SettingsService()
            :this(new SettingsRepository())
        {
            
        }

        public SettingsService(ISettingsRepository repository)
        {
            _settingsRepository = repository;
        }

        

    }
}