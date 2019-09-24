using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly HotelBookingContext db;

        public RoomRepository(HotelBookingContext context)
        {
            db = context;
        }

        public Room Add(Room entity)
        {
            db.Add(entity);
            db.SaveChanges();
            return entity;
        }

        public Room Edit(Room entity)
        {
            var OrigRoom = Get(entity.Id);
            OrigRoom.Description = entity.Description;

            db.SaveChanges();
            return OrigRoom;
        }

        public Room Get(int id)
        {
            // The FirstOrDefault method below returns null
            // if there is no room with the specified Id.
            return db.Room.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Room> GetAll()
        {
            return db.Room.ToList();
        }

        public Room Remove(int id)
        {
            // The Single method below throws an InvalidOperationException
            // if there is not exactly one room with the specified Id.
            var room = db.Room.Single(r => r.Id == id);
            db.Room.Remove(room);
            db.SaveChanges();
            return room;
        }
    }
}
