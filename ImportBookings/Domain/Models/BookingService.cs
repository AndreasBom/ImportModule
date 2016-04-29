using CsvHelper.Configuration;

namespace ImportBookings.Domain.Models
{
    public class BookingService
    {
        public string Pnr { get; set; }
        public string ServiceCode { get; set; }
    }

    public sealed class BookingServiceMapping : CsvClassMap<BookingService>
    {
        public BookingServiceMapping()
        {
            Map(m => m.Pnr).Name("CONFIRMATION_NUM");
            Map(m => m.ServiceCode).Name("CODE_TYPE");
        }
    }
}
