using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using ImportBookings.Domain;
using ImportBookings.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestImportModul
{
    [TestClass]
    public class CsvFileManagerUnitTest
    {
        private CsvFileManager _csvFileManager;
        private static string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static string bookingInfoFilePath = baseDir + @"\App_Data\ftp\ENH_Segs_20160217.txt";
        private static string passangerInfoFilePath = baseDir + @"\App_Data\ftp\ENH_PAX_20160217.txt";
        private static string serviceInfoFilePath = baseDir + @"\App_Data\ftp\ENH_Charges_20160217.txt";




        [TestInitialize]
        public void SetUp()
        {
            _csvFileManager = new CsvFileManager();
        }

        [TestMethod]
        public void Test_CsvFileManager_Read_ENH_PAX_20160217_txt_IntoModelClass()
        {
            var passangersInfo = _csvFileManager.ReadPassangersFile(passangerInfoFilePath);

            Assert.IsTrue(passangersInfo.Any());
        }



        [TestMethod]
        public void Test_CsvFileManager_Read_ENH_Segs_20160217_txt_IntoModelClass()
        {
            var bookingInfo = _csvFileManager.ReadBookingsFile(bookingInfoFilePath);

            Assert.IsTrue(bookingInfo.Any());
        }

        [TestMethod]
        public void Read_CsvFilesAnd_CreateListWith_BookingView()
        {
            var bookingInfo = _csvFileManager.ReadBookingsFile(bookingInfoFilePath).ToList();
            var serviceInfo = _csvFileManager.ReadServicesFile(serviceInfoFilePath).ToList();
            var passangersInfo = _csvFileManager.ReadPassangersFile(passangerInfoFilePath).ToList();

            var bookings = new List<BookingView>();

            foreach (var booking in bookingInfo.GroupBy(x => x.Pnr))
            {
                var bookingView = new BookingView(booking.Key,serviceInfo);
                foreach (var booking1 in booking)
                {
                    var bookingHistory = new BookingHistory(booking1, passangersInfo);
                    bookingView.Histories.Add(bookingHistory);
                }
                bookings.Add(bookingView);
            }

            Assert.IsTrue(bookings.Any());
        }

        [TestMethod]
        public void Test_CsvFileManager_Read_ENH_Charges_20160217_txt_IntoModelClass()
        {
            var serviceInfo = _csvFileManager.ReadServicesFile(serviceInfoFilePath);

            Assert.IsTrue(serviceInfo.Any());
        }

        //[TestMethod]
        //public void Test_CsvFileManager_TryToReadFilesInEmptyFolder()
        //{
        //    var fileList = _csvFileManager.PopulateModel(baseDir + @"\App_Data\ftp2");

        //    Assert.AreEqual(0, fileList.Count());
        //}


    }
}
