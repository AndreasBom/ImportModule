using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using EasyHttp.Infrastructure;
using ImportBookings.Domain;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Models;
using ImportBookings.Domain.Repositories;
using ImportBookings.Exceptions;
using Newtonsoft.Json;

namespace ImportBookings.Dev_Env
{
    public class ProcessingFakeClass : PB
    {
        //For testing
        private readonly string _filePathToQueuedData = AppDomain.CurrentDomain.BaseDirectory + "queue.json";
        public bool FailToSend { get; set; } = false;
        private readonly IProcessedDataRepository _dataRepository = new ProcessedDataRepository();

        public Dictionary<string, int> SentToApisci = new Dictionary<string, int>
        {
            {"BookingComplete", 0 },
            {"OutboundSegmentBooked", 0 },
            {"InboundSegmentBooked", 0 },
             {"BookingModified", 0 },
            {"OutboundSegmentModified", 0 },
            {"InboundSegmentModified", 0 },
            {"BookingCanceled", 0 },
            {"ActionUnknown", 0 },
        };


        protected override void Send(string reference, string action, IDictionary<string, string> data)
        {
            if (FailToSend)
            {
                var d = new ProcessedData
                {
                    Reference = reference,
                    Action = action,
                    Data = JsonConvert.SerializeObject(data)
                };
                _dataRepository.AddData(d);
                _dataRepository.Save();
                throw new SendErrorException();
            }
            


            //Dev-code
            switch (action)
            {
                case "BookingCompleted":
                    SentToApisci["BookingComplete"] += 1;
                    break;
                case "OutboundSegmentBooked":
                    SentToApisci["OutboundSegmentBooked"] += 1;
                    break;
                case "InboundSegmentBooked":
                    SentToApisci["InboundSegmentBooked"] += 1;
                    break;
                case "BookingModified":
                    SentToApisci["BookingModified"] += 1;
                    break;
                case "OutboundSegmentModified":
                    SentToApisci["OutboundSegmentModified"] += 1;
                    break;
                case "InboundSegmentModified":
                    SentToApisci["InboundSegmentModified"] += 1;
                    break;
                case "BookingCanceled":
                    SentToApisci["BookingCanceled"] += 1;
                    break;
                default:
                    SentToApisci["ActionUnknown"] += 1;
                    break;
            }


        }
    }
}
