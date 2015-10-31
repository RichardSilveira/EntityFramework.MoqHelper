using DemoClientApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting example...");

                var customer = new Customer() { Name = "Lorem Ipsum", Address = "Los Angeles, CA" };

                var customerService = new CustomerService();

                customerService.Insert(customer);

                Console.WriteLine("Inserting Customer...Done.");

                var customerFromDataBase = customerService.GetByID(customer.CustomerID);

                customerService.Remove(customer);

                Console.WriteLine("Some basic CRUD operations done. Give a look at 'DemoClientUnitTest' project to understand how EntityFramework.MoqHelper works");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine("Stack: " + exc.StackTrace);

                Console.WriteLine("");
                Console.WriteLine("CURRENT DATA SOURCE (hard coded on DemoContext class): " + @"Data Source=.\SQLEXPRESS;Initial Catalog=DemoForMoqHelper;Integrated Security=true;");
            }

            Console.ReadKey();
        }
    }
}
