using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly HotelBookingContext db;

        public BookingRepository(HotelBookingContext context)
        {
            db = context;
        }

        public Booking Add(Booking entity)
        {
            db.Booking.Add(entity);
            db.SaveChanges();
            return null;
        }

        public Booking Edit(Booking entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
            return null;
        }

        public Booking Get(int id)
        {
            return db.Booking.Include(b => b.Customer).Include(b => b.Room).FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Booking> GetAll()
        {
            return db.Booking.Include(b => b.Customer).Include(b => b.Room).ToList();
        }

        public Booking Remove(int id)
        {
            var booking = db.Booking.FirstOrDefault(b => b.Id == id);
            db.Booking.Remove(booking);
            db.SaveChanges();
            return null;
        }

    }
}
