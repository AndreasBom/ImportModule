using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using EasyHttp.Infrastructure;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Models;
using ImportBookings.Domain.Repositories;
using ImportBookings.Exceptions;
using JsonFx.Serialization.Resolvers;
using Newtonsoft.Json;

namespace ImportBookings.Domain
{
    //public class Processing : ProcessingBase
    //{
        
    //    /// <summary>
    //    /// HTTP POST to Apisci
    //    /// </summary>
    //    /// <param name="reference"></param>
    //    /// <param name="action"></param>
    //    /// <param name="data"></param>
    //    protected override void Send(string reference, string action, IDictionary<string, string> data)
    //    {
    //        //var serviceUrl = ConfigurationManager.AppSettings["Apisci.Service"];
    //        //var apiKey = ConfigurationManager.AppSettings["Apisci.ApiKey"];

    //        var serviceUrl = @"http://localhost:40170/api/Apisci";
    //        var apiKey = "12345";

    //        var httpClient = new EasyHttp.Http.HttpClient
    //        {
    //            ThrowExceptionOnHttpError = false
    //        };


    //        var response = httpClient.Post(serviceUrl, new
    //        {
    //            Reference = reference,
    //            ApiKey = apiKey,
    //            Action = action,
    //            Data = data
    //        }, "application/json");

    //        if (response.StatusCode != HttpStatusCode.OK)
    //        {
    //            //Saving processed data to database
    //            var processedData = new ProcessedData
    //            {
    //                Reference = reference,
    //                Action = action,
    //                Data = JsonConvert.SerializeObject(data)
    //            };

    //            DataRepository.AddData(processedData);
    //            DataRepository.Save();

    //            throw new SendErrorException();
    //        }
    //    }


    //}
}

