using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestImportModul
{
    [TestClass]
    public class ProcessedDataRepositoryUnitTest
    {

        [TestMethod]
        public void AddData_Successfully()
        {
            var mockSet = new Mock<DbSet<ProcessedData>>();

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(m => m.ProcessedData).Returns(mockSet.Object);

            var settings = new ProcessedDataRepository(mockContext.Object);

            settings.AddData(new ProcessedData {Action = "action", Data = "data", Reference = "reference"});
            settings.Save();

            mockSet.Verify(m => m.Add(It.IsAny<ProcessedData>()), Times.Once);
            
        }

        [TestMethod]
        public void Save_Successfully()
        {
            var mockSet = new Mock<DbSet<ProcessedData>>();

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(m => m.ProcessedData).Returns(mockSet.Object);

            var settings = new ProcessedDataRepository(mockContext.Object);

            settings.AddData(new ProcessedData { Action = "action", Data = "data", Reference = "reference" });
            settings.Save();

            mockSet.Verify(m => m.Add(It.IsAny<ProcessedData>()), Times.Once);
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);

        }

        [TestMethod]
        public void Delete_Successfully()
        {
            var mockSet = new Mock<DbSet<ProcessedData>>();

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(m => m.ProcessedData).Returns(mockSet.Object);

            var processedRepo = new ProcessedDataRepository(mockContext.Object);

            processedRepo.DeleteData(1);

            mockSet.Verify(m => m.Remove(It.IsAny<ProcessedData>()), Times.Once);
        }

        [TestMethod]
        public void GetAllData_successfully()
        {
            var data = new List<ProcessedData>
            {
                new ProcessedData() {Action = "action1", Reference = "reference1", Data = "data1"},
                new ProcessedData() {Action = "action2", Reference = "reference2", Data = "data2"},
                new ProcessedData() {Action = "action3", Reference = "reference3", Data = "data3"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ProcessedData>>();
            mockSet.As<IQueryable<ProcessedData>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<ProcessedData>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ProcessedData>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ProcessedData>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(c => c.ProcessedData).Returns(mockSet.Object);

            var service = new ProcessedDataRepository(mockContext.Object);
            var result = service.GetAllData().ToList();


            Assert.AreEqual(3, result.Count());

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual("action"+ (i + 1), result[i].Action);
                Assert.AreEqual("reference" + (i + 1), result[i].Reference);
                Assert.AreEqual("data" + (i + 1), result[i].Data);
            }
            
        }


    }
}
