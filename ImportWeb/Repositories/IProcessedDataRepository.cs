using System;
using System.Collections.Generic;
using ImportWeb.DAL.Entities;

namespace ImportWeb.Repositories
{
    public interface IProcessedDataRepository :IDisposable
    {
        IEnumerable<ProcessedData> GetAllData();
        void AddData(ProcessedData data);
        void AddData(IEnumerable<ProcessedData> dataCollection);
        void UpdateData(ProcessedData data);
        void DeleteData(int id);
        void Save();
    }
}
