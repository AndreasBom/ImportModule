using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ImportBookings.Domain.Models
{
    public class BookingView
    {
        public BookingView(string pnr, IEnumerable<BookingService> services)
        {
            Pnr = pnr;
            Histories = new List<BookingHistory>();
            Services = services.Where(x => x.Pnr == pnr).ToList();
        }
        public string Pnr { get; set; }
        public IList<BookingHistory> Histories { get; set; }
        public IList<BookingService> Services { get; set; }
    }

    public class BookingHistory : Booking
    {
        public BookingHistory(Booking booking, IEnumerable<Passanger> passengers)
        {
            Pnr = booking.Pnr;
            PassangerId = booking.PassangerId;
            BookingDate = booking.BookingDate;
            DepartureDate = booking.DepartureDate;
            CreatedDate = booking.CreatedDate;
            LastModified = booking.LastModified;
            Status = booking.Status;
            ReasonForCancel = booking.ReasonForCancel;

            var passenger = passengers.FirstOrDefault(p => p.PassangerId == booking.PassangerId) ?? new Passanger { Email = "", PassangerId = "", Mobil = "" };

            Passanger = passenger;
        }

        public Passanger Passanger { get; set; }
        
    }

    public class Booking
    {
        public string Pnr { get; set; } //CONFIRMATION_NUM
        public DateTime BookingDate { get; set; } //BOOK_DATE
        public DateTime CreatedDate { get; set; } //CREATED_DATE
        public DateTime LastModified { get; set; } //LAST_MODIFIED_DATE
        public DateTime DepartureDate { get; set; } //DEPARTURE_DATE
        public string PassangerId { get; set; } //PASSENGER_ID
        public string Status { get; set; } //RES_SEG_STATUS_DESCRIPTION
        public string ReasonForCancel { get; set; } //CANCEL_REASON_DESCRIPTION
    }

    public sealed class BookingMapping : CsvClassMap<Booking>
    {
        public BookingMapping()
        {
            Map(m => m.Pnr).Name("CONFIRMATION_NUM");
            Map(m => m.PassangerId).Name("PASSENGER_ID");
            Map(m => m.BookingDate).Name("BOOK_DATE").TypeConverter<StringToDateTimeConverter>();
            Map(m => m.CreatedDate).Name("CREATED_DATE").TypeConverter<StringToDateTimeConverter>();
            Map(m => m.LastModified).Name("LAST_MODIFIED_DATE").TypeConverter<StringToDateTimeConverter>();
            Map(m => m.DepartureDate).Name("DEPARTURE_DATE").TypeConverter<StringToDateTimeConverter>();
            Map(m => m.Status).Name("RES_SEG_STATUS_DESCRIPTION");
            Map(m => m.ReasonForCancel).Name("CANCEL_REASON_DESCRIPTION");
        }
    }

    public class StringToDateTimeConverter : DefaultTypeConverter
    {
        private const String DateFormat = @"dd-MMM-yy";

        public override bool CanConvertFrom(Type type)
        {
            bool ret = typeof(String) == type;
            return ret;
        }

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            DateTime newDate = default(System.DateTime);
            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    newDate = DateTime.ParseExact(text, DateFormat, CultureInfo.InvariantCulture);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return newDate;
        }
    }
}
