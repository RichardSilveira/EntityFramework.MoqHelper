# EntityFramework.MoqHelper
Helper methods to work with Entity Framework 6.* and Moq libraries doing mocks of Entity Framework main methods that access database.

EntityFramework.MoqHelper is a simple library, but a lot of help when it comes to mock with Entity Framework.

To create objects through mock it needs to perform many settings, especially for test scenarios where queries are made.
Many of these codes required were encapsulated in that library and some logic to work with lists of objects and queries them were made. Using a fluent interface you can perform different settings to mock DbContext and DbSet objects.

To mock EF objects, the library was chosen to Moq because it is a stable and very popular library of Mock in general.

The focus of this library is to facilitate the developer's life when testing using EF without accessing the database directly.

To mock in EF you must configure each method related to each operation you want to make the database available in DbSet as DbSet.Add, DbSet.Find and DbSet.Remove, for example.

Using this library it can make it through something like:
```cs
[TestMethod]
public void Customer_CRUD_operations_basics()
{
    var customer = new Customer() { Name = "Foo Bar", Address = "Los Angeles, CA" };

    var customers = new List<Customer>();
    var mockSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Customer>()
                                                    .SetupForQueryOn(customers)
                                                    .WithAdd(customers, "CustomerID")//overwritten to simulate behavior of auto-increment database
                                                    .WithFind(customers, "CustomerID")
                                                    .WithRemove(customers);

    var mockContext = EntityFrameworkMoqHelper.CreateMockForDbContext<DemoContext, Customer>(mockSet);

    var customerService = new CustomerService(mockContext.Object);

    customerService.Insert(customer);

    customers.Should().Contain(x => x.CustomerID == customer.CustomerID);

    //Testing GetByID (and DbSet.Find) method
    customerService.GetByID(customer.CustomerID).Should().NotBeNull();

    //Testing Remove method
    customerService.Remove(customer);

    customerService.GetByID(customer.CustomerID).Should().BeNull();
}
```
  
### Nuget Package
This repository is a source code and samples about how to use for nuget package EntityFramework.MoqHelper

To install this package, run the following command in the Package Manager Console:

**Install-Package EntityFramework.MoqHelper**

For better understanding of mock with Entity Framework 6 read this article <a href="http://www.codeproject.com/Tips/1045590/Testing-with-mock-on-Entity-Framework" target="_blank">Testing with mock on Entity Framework 6</a> posted on codeproject:
