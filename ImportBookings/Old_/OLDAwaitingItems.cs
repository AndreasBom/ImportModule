using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImportBookings
{
    public class DataModel
    {
        public string Reference { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
    }

    public static class AwaitingItems
    {
        private static string _pathToFile = AppDomain.CurrentDomain.BaseDirectory + "queue.json";

        public static void SaveProcessedNotSentData(string reference, string action, IDictionary<string, string> processedData)
        {
            var jsonString = "";

            if (File.Exists(_pathToFile))
            {
                jsonString = File.ReadAllText(_pathToFile);
            }
   
            var jsonList = JsonConvert.DeserializeObject<List<DataModel>>(jsonString) ?? new List<DataModel>();


            jsonList.Add(new DataModel
            {
                Reference = reference,
                Action = action,
                Data = JsonConvert.SerializeObject(processedData)
            });

            jsonString = JsonConvert.SerializeObject(jsonList);

            File.WriteAllText(_pathToFile, jsonString);
        }

        public static void SaveProcessedNotSentData(IEnumerable<DataModel> listToSave)
        {
            var jsonList = listToSave.Select(item => new DataModel
            {
                Reference = item.Reference,
                Action = item.Action,
                Data = item.Data
            }).ToList();

            var jsonString = JsonConvert.SerializeObject(jsonList);
            File.WriteAllText(_pathToFile, jsonString);
        }

        public static IEnumerable<DataModel> RetriveProcessedNotSentData()
        {
            IEnumerable<DataModel> jsonList = new List<DataModel>();
            if (File.Exists(_pathToFile))
            {
                var jsonString = File.ReadAllText(_pathToFile);
                jsonList = from obj in JArray.Parse(jsonString)
                    select new DataModel
                    {
                        Reference = obj.Value<string>("Reference"),
                        Action = obj.Value<string>("Action"),
                        Data = obj.Value<string>("Data")
                    };

            }
            return jsonList.ToList();
        }
    }
}
