using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImportBookings.Domain
{
    public static class PersistentData
    {
        private static readonly string PathToFile;
        private static readonly XDocument Doc;
        static PersistentData()
        {
            PathToFile = AppDomain.CurrentDomain.BaseDirectory + "AppMemory.xml";
            if (!File.Exists(PathToFile))
            {
                var doc = new XDocument(
                            new XElement("ImportBookings",
                                new XElement("lastProcessed", ""), 
                                new XElement("lastSent")));
                doc.Save(PathToFile);
            }
            Doc = XDocument.Load(PathToFile);
        }

        public static void WriteLastProcessedDateToFile(string value)
        {
            Doc.Element("ImportBookings").Element("lastProcessed").Value = value;
            Doc.Save(PathToFile);
        }

        public static string ReadLastProcessedDateFromFile()
        {
            return Doc.Element("ImportBookings").Element("lastProcessed").Value;
        }

        public static void WriteLastSentDateToFile(string value)
        {
            Doc.Element("ImportBookings").Element("lastSent").Value = value;
            Doc.Save(PathToFile);
        }

        public static string ReadLastSentDateFromFile()
        {
            return Doc.Element("ImportBookings").Element("lastSent").Value;
        }
    }
}
