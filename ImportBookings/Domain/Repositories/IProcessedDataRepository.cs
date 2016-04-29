using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;

namespace ImportBookings.Domain.Repositories
{
    public interface IProcessedDataRepository : IDisposable
    {
        IEnumerable<ProcessedData> GetAllData(); 
        void AddData(ProcessedData data);
        void AddData(IEnumerable<ProcessedData> dataCollection);
        void UpdateData(ProcessedData data);
        void DeleteData(int id);
        void Save();
    }
}
