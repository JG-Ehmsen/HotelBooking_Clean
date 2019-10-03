using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            //IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            Mock<IRepository<Booking>> fakeBookingRepository;

            fakeBookingRepository = new Mock<IRepository<Booking>>();

            var bookings = new List<Booking>
            {
                new Booking { Id = 1, CustomerId = 1, RoomId = 1, IsActive = true, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 2, 1) },
                new Booking { Id = 2, CustomerId = 2, RoomId = 2, IsActive = true, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 2, 1) }
            };

            // Implement fake GetAll() method.
            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);

            fakeBookingRepository.Setup(x => x.Add(It.IsAny<Booking>())).Returns(bookings[0]);

            //IRepository<Room> roomRepository = new FakeRoomRepository();
            Mock<IRepository<Room>> fakeRoomRepository;

            fakeRoomRepository = new Mock<IRepository<Room>>();

            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            // Implement fake GetAll() method.
            fakeRoomRepository.Setup(x => x.GetAll()).Returns(rooms);

            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = DateTime.Today;
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(date, date));
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

    }
}
