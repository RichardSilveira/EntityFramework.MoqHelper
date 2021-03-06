﻿using DemoClientApp.Domain;
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

        public DemoContext() : base(@"Data Source=.\SQLEXPRESS;Initial Catalog=DemoForMoqHelper;Integrated Security=true;")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
