using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;

namespace HotelBooking.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly HotelBookingContext db;

        public CustomerRepository(HotelBookingContext context)
        {
            db = context;
        }

        public Customer Add(Customer entity)
        {
            db.Add(entity);
            db.SaveChanges();
            return entity;
        }

        public Customer Edit(Customer entity)
        {
            var OrigCustomer = Get(entity.Id);
            OrigCustomer.Name = entity.Name;
            OrigCustomer.Email = entity.Email;

            db.SaveChanges();
            return OrigCustomer;
        }

        public Customer Get(int id)
        {
            return db.Customer.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customer.ToList();
        }

        public Customer Remove(int id)
        {
            var customerToRemove = Get(id);
            db.Customer.Remove(customerToRemove);
            db.SaveChanges();
            return customerToRemove;
        }
    }
}
