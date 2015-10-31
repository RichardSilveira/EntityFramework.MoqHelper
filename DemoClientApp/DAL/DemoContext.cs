using DemoClientApp.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClientApp.DAL
{
    public class DemoContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }

        public DemoContext() : base(@"Data Source=NOTE-RICHARD\SHADOW;Initial Catalog=DemoForMoqHelper;Integrated Security=true;")
        {
            //todo:update datasource value to '.\SQLEXPRESS' for convention
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
