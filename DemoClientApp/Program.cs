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

                var customer = new Customer() { Name = "Lore Ipsum", Address = "Los Angeles, CA" };

                var customerService = new CustomerService();

                customerService.Insert(customer);

                Console.WriteLine("Inserting Customer...Done.");

                Console.WriteLine("All basic CRUD operations done, give a look at 'DemoClientUnitTest' project to understand how EntityFramework.MoqHelper works");

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine("Stack: " + exc.StackTrace);
            }

            Console.ReadKey();
        }
    }
}
