using System;
using System.Linq;
using ImportBookings;
using ImportBookings.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestImportModul
{
    [TestClass]
    public class ServiceUnitTest
    {
        private ServiceFacade _serviceFacade;
        //[TestInitialize]
        //public void SetUp()
        //{
        //    var pathToFolder = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\ftp3";
        //    _serviceFacade = new ServiceFacade(new ProcessingFakeClass(), new ProcessedDataRepository(), new SettingsRepositoryFakeClass());
        //}

        //[TestMethod]
        //public void Test_GetNewSetsOfFiles_Count_MissingFileSets()
        //{
        //    _serviceFacade.GetNewSetsOfFiles();
        //    var missingFilesCount = _serviceFacade.FileSetsWithMissingFiles.Count;

        //    Assert.AreEqual(2, missingFilesCount);
        //}


    }
}
