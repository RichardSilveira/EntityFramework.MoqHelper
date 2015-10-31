using DemoClientApp.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClientApp.Domain
{
    public class CustomerService
    {
        private DemoContext _context;
        private DbSet<Customer> _dbSet;

        public CustomerService(DemoContext context = null)
        {
            _context = context ?? new DemoContext();
            _dbSet = _context.Set<Customer>();
        }

        public Customer GetByID(int ID)
        {
            return _dbSet.Find(ID);
        }

        public void Insert(Customer customer)
        {
            //Bussines Rules
            if (string.IsNullOrEmpty(customer.Name))
                throw new Exception("Customer name is required");

            _dbSet.Add(customer);

            _context.SaveChanges();
        }

        public void Remove(Customer customer)
        {
            _dbSet.Remove(customer);
            _context.SaveChanges();
        }
    }
}
