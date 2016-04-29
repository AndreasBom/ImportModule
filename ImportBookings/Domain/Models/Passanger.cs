using CsvHelper.Configuration;

namespace ImportBookings.Domain.Models
{
    public class Passanger
    {
        public string PassangerId { get; set; } //PASSENGER_ID
        public string Mobil { get; set; } //PAX_CELL_PH2
        public string Email { get; set; } //PAX_EMAIL_ADDR2

    }

    public sealed class PassangerMapping : CsvClassMap<Passanger>
    {
        public PassangerMapping()
        {
            Map(m => m.PassangerId).Name("PASSENGER_ID");
            Map(m => m.Mobil).Name("PAX_CELL_PH2");
            Map(m => m.Email).Name("PAX_EMAIL_ADDR2");
        }
    }
}
