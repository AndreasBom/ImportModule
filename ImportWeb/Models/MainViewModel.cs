using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.UI.WebControls;
using ImportWeb.Services;

namespace ImportWeb.Models
{
    public class MainViewModel
    {
        private SettingsService _settingsService;

        public MainViewModel()
        {
            _settingsService = new SettingsService();
        }

        [DataType(DataType.Date)]
        [DisplayName("Sista processdatum (filnamnets datum)")]
        public DateTime LastProcessedDate
        {
            get { return DateTime.ParseExact(_settingsService.LastProcessedDate, "yyyyMMdd", CultureInfo.InvariantCulture); }
            set { _settingsService.LastProcessedDate = value.ToString("yyyMMdd"); }
        }


    }
}