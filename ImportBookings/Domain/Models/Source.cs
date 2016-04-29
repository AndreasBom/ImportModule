using System;

namespace ImportBookings.Domain.Models
{
    /// <summary>
    /// Mapping id, date and path from file name
    /// </summary>
    public class Source
    {
        public string Path { get; set; }
        public string Identification { get; set; }
        public DateTime Date { get; set; }
    }
}
