using System;
using System.Collections.Generic;
using System.Linq;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Models;
using ImportBookings.Domain.Repositories;
using Newtonsoft.Json;


namespace ImportBookings.Domain
{
    /// <summary>
    /// Extracts and assambles data from multiple files 
    /// </summary>
    public class Processing : IProcessing
    {
        protected IEnumerable<Booking> BookingInfo;
        protected IEnumerable<BookingService> ServiceInfo;
        protected IEnumerable<Passanger> PassangersInfo;
        protected CsvFileManager CsvFileManager;
        protected IProcessedDataRepository DataRepository;

        public Processing()
        {
            CsvFileManager = new CsvFileManager();
        }

        private void PopulateModelFromCsv(IEnumerable<Source> fileSet)
        {
            var pathToBookings = (from file in fileSet
                                  where file.Identification == Globals.BookingFileName
                                  select file.Path).FirstOrDefault();
            var pathToServices = (from file in fileSet
                                  where file.Identification == Globals.ChargesFileName
                                  select file.Path).FirstOrDefault();
            var pathToPassenger = (from file in fileSet
                                   where file.Identification == Globals.PassengerFileName
                                   select file.Path).FirstOrDefault();

            BookingInfo = CsvFileManager.ReadBookingsFile(pathToBookings).ToList();
            ServiceInfo = CsvFileManager.ReadServicesFile(pathToServices).ToList();
            PassangersInfo = CsvFileManager.ReadPassangersFile(pathToPassenger).ToList();
        }

        public Queue<ProcessedData> Process(IEnumerable<Source> fileSet)
        {
            PopulateModelFromCsv(fileSet);
            var queueProcessedData = new Queue<ProcessedData>();

            //Retrieve all bookings
            var bookings = GetBookingViewList();

            foreach (var booking in bookings.Where(b => b.Histories.Any(p => p.Passanger.Email.Length > 0)))//Exclude bookings with missing Email
            {
                string reference = booking.Pnr;
                var segments = ExtractSegments(booking).ToList();
                var services = ExtractServices(booking).ToList();
                var histories = ExtractHistories(booking).ToList();

                //Booking is ACTIVE
                if (booking.Histories.All(b => b.Status == "ACTIVE"))
                {
                    var data = GetData(reference, histories, services);

                    queueProcessedData.Enqueue(new ProcessedData { Reference = reference, Action = "BookingCompleted", Data = JsonConvert.SerializeObject(data) });
                    queueProcessedData.Enqueue(new ProcessedData { Reference = reference, Action = "OutboundSegmentBooked", Data = JsonConvert.SerializeObject(data) });

                    if (segments.Count > 1)
                    {
                        data["DepatureDate"] = histories.LastOrDefault().DepartureDate.ToShortDateString();

                        queueProcessedData.Enqueue(new ProcessedData { Reference = reference, Action = "InboundSegmentBooked", Data = JsonConvert.SerializeObject(data) });

                    }

                }
                //Booking is CANCELED (terminated) or modified
                else
                {
                    var groupedByStatus = from b in booking.Histories
                                           .GroupBy(g => g.Status)
                                          select b;

                    //Booking is modified (first CANCELED then ACTIVE) 
                    if (groupedByStatus.FirstOrDefault().Key == "CANCELED" && groupedByStatus.Any(a => a.Key == "ACTIVE"))
                    {
                        var data = GetData(reference, histories, services);

                        queueProcessedData.Enqueue(new ProcessedData { Reference = reference, Action = "BookingModified", Data = JsonConvert.SerializeObject(data) });
                        queueProcessedData.Enqueue(new ProcessedData { Reference = reference, Action = "OutboundSegmentModified", Data = JsonConvert.SerializeObject(data) });


                        if (segments.Count > 1)
                        {
                            data["DepatureDate"] = histories.LastOrDefault().DepartureDate.ToShortDateString();

                            queueProcessedData.Enqueue(new ProcessedData { Reference = reference, Action = "InboundSegmentModified", Data = JsonConvert.SerializeObject(data) });

                        }
                    }
                    //Booking is canceled
                    else
                    {
                        foreach (var bookingHistory in booking.Histories.GroupBy(h=>h.DepartureDate))
                        {
                            var historySeg = from h in histories
                                where h.DepartureDate == bookingHistory.Key
                                select h;
                            {
                                var data = GetData(reference, historySeg, services);

                                queueProcessedData.Enqueue(new ProcessedData { Reference = reference, Action = "BookingCanceled", Data = JsonConvert.SerializeObject(data) });
                            }
                           
                        }
                        

                    }
                }

            }

            return queueProcessedData;
        }

        private Dictionary<string, string> GetData(string reference, IEnumerable<BookingHistory> histories, IEnumerable<string> services)
        {
            var data = new Dictionary<string, string>
                    {
                        {"Email", histories.FirstOrDefault().Passanger.Email},
                        {"BookingDate", histories.FirstOrDefault().BookingDate.ToShortDateString()},
                        {"DepatureDate", histories.FirstOrDefault().DepartureDate.ToShortDateString()},
                        {"PNR", reference},
                        {"Mobil", histories.FirstOrDefault().Passanger.Mobil},
                        {"Services", string.Join(",", services.Distinct())}
                    };

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns a booking-view with booking history, PNR, and passanger information </returns>
        private IEnumerable<BookingView> GetBookingViewList()
        {
            var bookings = new List<BookingView>();

            foreach (var booking in BookingInfo.GroupBy(x => x.Pnr))
            {
                var bookingView = new BookingView(booking.Key, ServiceInfo);
                foreach (var bookingHistory in booking.Select(b => new BookingHistory(b, PassangersInfo)))
                {
                    bookingView.Histories.Add(bookingHistory);
                }
                bookings.Add(bookingView);
            }

            return bookings;    
        }

        private IEnumerable<BookingHistory> ExtractHistories(BookingView booking)
        {
            return booking.Histories.ToList();
        }

        private IEnumerable<string> ExtractServices(BookingView booking)
        {
            return from service in booking.Services
                   where !service.ServiceCode.Contains("AIR") &&
                         !service.ServiceCode.Contains("TAX")
                   select service.ServiceCode.Trim();
        }

        private IEnumerable<IGrouping<DateTime, BookingHistory>> ExtractSegments(BookingView booking)
        {
            return from segment in booking.Histories
                   .GroupBy(seg => seg.DepartureDate)
                   select segment;
        }

    }
}
