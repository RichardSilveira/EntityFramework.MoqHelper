namespace DemoClientApp.Migrations
{
    using DAL;
    using Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DemoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DemoContext context)
        {
            context.Customers.Add(new Customer() { Name = "Foo" });
            context.Customers.Add(new Customer() { Name = "Bar" });
        }
    }
}
