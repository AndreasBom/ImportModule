using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using ImportWeb.DAL.Entities;

namespace ImportWeb.DAL
{
    public class ImportContext : DbContext
    {
        public ImportContext()
            :base("name=ImportWebConnString")
        {
            //Empty   
        }

        public virtual DbSet<ProcessedData> ProcessedData { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}