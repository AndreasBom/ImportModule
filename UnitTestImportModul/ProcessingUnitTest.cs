using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ImportBookings.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportBookings.Domain.Models;

namespace UnitTestImportModul
{

    [TestClass]
    public class ProcessingUnitTest
    {
        private static readonly string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string BookingInfoFilePath = BaseDir + @"\App_Data\ftp\ENH_Segs_20160218.txt";
        private static readonly string PassangerInfoFilePath = BaseDir + @"\App_Data\ftp\ENH_PAX_20160217.txt";
        private static readonly string ServiceInfoFilePath = BaseDir + @"\App_Data\ftp\ENH_Charges_20160217.txt";

        //[TestMethod]
        //public void Test_ExpectedActionsUsedBy_SendMethod_ToApisci_Using_ProcessMethod()
        //{
        //    var bookingFileName = "Segs";
        //    var passengerFileName = "PAX";
        //    var chargesFileName = "Charges";
        //    var csvFileManager = new CsvFileManager();
        //    var pathToFolder = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\ftp4";

        //    var fileList = new List<IGrouping<DateTime, Source>>(3);
        //    var filesGroupedByDate = csvFileManager.PopulateModel(pathToFolder).GroupBy(f => f.Date).ToList();

        //    foreach (var file in filesGroupedByDate)
        //    {
        //        if (file.Any(b => b.Identification == bookingFileName &&
        //                          file.Any(p => p.Identification == passengerFileName) &&
        //                          file.Any(c => c.Identification == chargesFileName)))
        //        {
        //            fileList.Add(file);
        //        }
        //    }

        //    fileList = fileList.OrderBy(f => f.Key).ToList();

        //    var processing = new ProcessingFakeClass();

        //    foreach (var fileSet in fileList)
        //    {
        //        processing.Process(fileSet);
        //    }
            

        //    //Expected result:
        //    var expected = new Dictionary<string, int>
        //    {
        //        {"BookingComplete", 2 },
        //        {"OutboundSegmentBooked", 2 },
        //        {"InboundSegmentBooked", 1 },
        //         {"BookingModified", 1 },
        //        {"OutboundSegmentModified", 1 },
        //        {"InboundSegmentModified", 1 },
        //        {"BookingCanceled", 1 },
        //        {"ActionUnknown", 0 },
        //    };

        //    foreach (var actual in processing.SentToApisci)
        //    {
        //        Assert.AreEqual(expected[actual.Key], actual.Value);
        //    }
        //}

    }
}
