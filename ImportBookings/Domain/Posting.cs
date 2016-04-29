using System.Collections.Generic;
using System.Net;
using System.Reflection;
using ImportBookings.Exceptions;
using log4net;

namespace ImportBookings.Domain
{
    /// <summary>
    /// Post to Apisci
    /// </summary>
    public static class Posting
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// For Testing!
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="action"></param>
        /// <param name="data"></param>
        public static void SendTest(string reference, string action, IDictionary<string, string> data)
        {
            var dataString = "";

            foreach (var item in data)
            {
                dataString += item.Key + ": " + item.Value + ", ";
            }
            
            Logger.Info($"Reference: {reference}, Action: {action}, Data: {dataString}");

        }

        /// <summary>
        /// POST to Apisci
        /// </summary>
        /// <param name="reference">PNR</param>
        /// <param name="action">Booking complete, modified, inbound, outbound, canceled</param>
        /// <param name="data">Dictionary with email, bookingdate, depaturedate, PNR, mobile, services</param>
        public static void Send(string reference, string action, IDictionary<string, string> data)
        {
            //var serviceUrl = ConfigurationManager.AppSettings["Apisci.ServiceFacade"];
            //var apiKey = ConfigurationManager.AppSettings["Apisci.ApiKey"];

            var serviceUrl = @"http://localhost:40170/api/Apisci";
            var apiKey = "12345";

            var httpClient = new EasyHttp.Http.HttpClient
            {
                ThrowExceptionOnHttpError = false
            };


            var response = httpClient.Post(serviceUrl, new
            {
                Reference = reference,
                ApiKey = apiKey,
                Action = action,
                Data = data
            }, "application/json");


            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new SendErrorException();
            }
        }

    }
}
