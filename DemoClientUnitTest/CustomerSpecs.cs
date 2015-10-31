using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoClientApp.Domain;
using System.Collections.Generic;
using EntityFramework.MoqHelper;
using DemoClientApp.DAL;
using Moq;

namespace DemoClientUnitTest
{
    [TestClass]
    public class CustomerSpecs
    {
        [TestMethod]
        public void Customer_insert_checking_EF_implementation_details()
        {
            var customer = new Customer() { Name = "Foo Bar", Address = "Los Angeles, CA" };

            var customers = new List<Customer>();
            var mockSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Customer>()
                                                            .SetupForQueryOn(customers)
                                                            .WithAdd(customers);

            var mockContext = EntityFrameworkMoqHelper.CreateMockForDbContext<DemoContext, Customer>(mockSet);

            var customerService = new CustomerService(mockContext.Object);

            customerService.Insert(customer);

            //Checking how many times 'DbSet.Add' and 'DbContext.SaveChanges' was called
            mockSet.Verify(m => m.Add(It.IsAny<Customer>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Customer_insert_checking_property_customerID()
        {
            var customer = new Customer() { Name = "Foo Bar", Address = "Los Angeles, CA" };

            var customers = new List<Customer>();
            var mockSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Customer>()
                                                            .SetupForQueryOn(customers)
                                                            .WithAdd(customers, "CustomerID");//overwritten to simulate behavior of auto-increment database

            var mockContext = EntityFrameworkMoqHelper.CreateMockForDbContext<DemoContext, Customer>(mockSet);

            var customerService = new CustomerService(mockContext.Object);

            customerService.Insert(customer);

            //Instead of checking implementations details, it was checking bussines rules
            Assert.IsTrue(customer.CustomerID > 0);
        }

        //todo:Create another method with Remove and Find operations
    }
}
