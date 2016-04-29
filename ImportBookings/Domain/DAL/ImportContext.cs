using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportBookings.Domain.DAL.Entities;

namespace ImportBookings.Domain.DAL
{
    public class ImportContext : DbContext
    {
        public ImportContext()
            :base("name=ApisciImport")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ImportContext>());
        }


        public virtual DbSet<ProcessedData> ProcessedData { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
