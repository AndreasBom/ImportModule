using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Models;

namespace ImportBookings.Domain
{
    public interface IProcessing
    {
        Queue<ProcessedData> Process(IEnumerable<Source> fileSet);
    }
}
