using System.Collections.Generic;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.Fakes
{
    public class FakeRoomRepository : IRepository<Room>
    {
        // This field is exposed so that a unit test can validate that the
        // Add method was invoked.
        public bool addWasCalled = false;

        public Room Add(Room entity)
        {
            addWasCalled = true;
            return null;
        }

        // This field is exposed so that a unit test can validate that the
        // Edit method was invoked.
        public bool editWasCalled = false;

        public Room Edit(Room entity)
        {
            editWasCalled = true;
            return null;
        }

        public Room Get(int id)
        {
            return new Room { Id = 1, Description = "A" };
        }

        public IEnumerable<Room> GetAll()
        {
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };
            return rooms;
        }

        // This field is exposed so that a unit test can validate that the
        // Remove method was invoked.
        public bool removeWasCalled = false;

        public Room Remove(int id)
        {
            // Hehehe
            removeWasCalled = true;
            return null;
        }
    }
}
