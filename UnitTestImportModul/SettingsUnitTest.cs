using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ImportBookings;
using ImportBookings.Domain.DAL;
using ImportBookings.Domain.DAL.Entities;
using ImportBookings.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace UnitTestImportModul
{
    [TestClass]
    public class SettingsUnitTest
    {
        [TestMethod]
        public void AddOrUpdate_Settings_Adding_key_Value()
        {
            var mockSet = new Mock<DbSet<Settings>>();

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(m => m.Settings).Returns(mockSet.Object);

            var settings = new SettingsRepository(mockContext.Object);

            settings.AddOrUpdateSetting("testKey", "testValue");
            settings.Save();

            mockSet.Verify(m=>m.Add(It.IsAny<Settings>()), Times.Once);
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddOrUpdate_Settings_Adding_SettingsObject()
        {
            var mockSet = new Mock<DbSet<Settings>>();

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(m => m.Settings).Returns(mockSet.Object);

            var settings = new SettingsRepository(mockContext.Object);

            settings.AddOrUpdateSetting(new Settings {Key = "TestKEY", Value = "TestValue"});

            mockSet.Verify(m=>m.Add(It.IsAny<Settings>()), Times.Once);

        }

        [TestMethod]
        public void Save_Settings()
        {
            var mockSet = new Mock<DbSet<Settings>>();

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(m => m.Settings).Returns(mockSet.Object);

            var settings = new SettingsRepository(mockContext.Object);

            settings.AddOrUpdateSetting(new Settings { Key = "TestKEY", Value = "TestValue" });
            settings.Save();
            
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);

        }

        [TestMethod]
        public void DeleteSettings()
        {
            var mockSet = new Mock<DbSet<Settings>>();

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(m => m.Settings).Returns(mockSet.Object);

            var settings = new SettingsRepository(mockContext.Object);
            settings.DeleteSetting("keyTest");

            mockSet.Verify(m => m.Remove(It.IsAny<Settings>()), Times.Once);
        }

        [TestMethod]
        public void GetAllKeyValues_AssertValues()
        {
            var data = new List<Settings>
            {
                new Settings() {Key = "key1", Value = "value1"},
                new Settings() {Key = "key2", Value = "value2"},
                new Settings {Key = "key3", Value = "value3"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Settings>>();
            mockSet.As<IQueryable<Settings>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Settings>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Settings>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Settings>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ImportContext>();
            mockContext.Setup(c => c.Settings).Returns(mockSet.Object);

            var service = new SettingsRepository(mockContext.Object);
            var result = service.GetAllKeysValues();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("value1", result["key1"]);
            Assert.AreEqual("value2", result["key2"]);
            Assert.AreEqual("value3", result["key3"]);
        }
    }
}
