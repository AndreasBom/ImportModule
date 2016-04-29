using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using CsvHelper;
using ImportBookings.Domain.Models;

namespace ImportBookings.Domain
{
    /// <summary>
    /// Reads text files and populates models
    /// </summary>
    public class CsvFileManager
    {
        public IEnumerable<Source> PopulateModel(IList<string> fileList)
        {
            string bookingFileName = Globals.BookingFileName;
            string passengerFileName = Globals.PassengerFileName;
            string chargesFileName = Globals.ChargesFileName;

            var files = new List<Source>();

            if (fileList.Any())
            {
                var f = from file in fileList
                .Where(file => file.Contains(bookingFileName) ||
                               file.Contains(passengerFileName) ||
                               file.Contains(chargesFileName))
                        select new Source
                        {
                            Path = file,
                            Identification = file.Split('_')[2],
                            Date =
                                DateTime.ParseExact(file.Split('_')[3].Split('.')[0], "yyyyMMdd", CultureInfo.InvariantCulture)
                        };
                files = f.ToList();
            }
            return files.ToList();
        }

        /// <summary>
        /// Reading csv file where file name contains 'Segs'
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public IEnumerable<Booking> ReadBookingsFile(string pathToFile)
        {
            //List<int> tal1 = new List<int>();
            using (TextReader textReader = File.OpenText(pathToFile))
            using (var csvReader = new CsvReader(textReader))
            {
                csvReader.Configuration.RegisterClassMap<BookingMapping>();
                csvReader.Configuration.Delimiter = "|";

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<Booking>();
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Reading csv file where file name contains 'PAX'
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public IEnumerable<Passanger> ReadPassangersFile(string pathToFile)
        {
            using (TextReader textReader = File.OpenText(pathToFile))
            using (var csvReader = new CsvReader(textReader))
            {
                csvReader.Configuration.RegisterClassMap<PassangerMapping>();
                csvReader.Configuration.Delimiter = "|";

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<Passanger>();
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Reading csv file where file name contains 'Charges'
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public IEnumerable<BookingService> ReadServicesFile(string pathToFile)
        {
            using (TextReader textReader = File.OpenText(pathToFile))
            using (var csvReader = new CsvReader(textReader))
            {
                csvReader.Configuration.RegisterClassMap<BookingServiceMapping>();
                csvReader.Configuration.Delimiter = "|";

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<BookingService>();
                    yield return record;
                }
            }
        }
    }
}
