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

        Mock<IRepository<Booking>> fakeBookingRepository;
        Mock<IRepository<Room>> fakeRoomRepository;


        public BookingManagerTests(){

            fakeBookingRepository = new Mock<IRepository<Booking>>();

            var bookings = new List<Booking>
            {
                new Booking { Id = 1, CustomerId = 1, RoomId = 1, IsActive = true, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 2, 1) },
                new Booking { Id = 2, CustomerId = 2, RoomId = 2, IsActive = true, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 2, 1) }
            };

            // Implement fake GetAll() method.
            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);

            fakeBookingRepository.Setup(x => x.Add(It.IsAny<Booking>())).Returns(bookings[0]);


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

        [Fact]
        public void CreateBooking_NoRoomsAvailable_ReturnFalse()
        {
            // Arrange
            var booking = new Booking { Id = 1, CustomerId = 1, RoomId = 1, IsActive = true, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 2, 1) };

            // Act
            var created = bookingManager.CreateBooking(booking);

            // Assert
            Assert.False(created);
        }

        [Fact]
        public void CreateBooking_RoomAvailable_ReturnTrue()
        {
            // Arrange
            var booking = new Booking { Id = 1, CustomerId = 1, RoomId = 1, IsActive = true, StartDate = new DateTime(2021, 1, 1), EndDate = new DateTime(2021, 2, 1) };

            // Act
            var created = bookingManager.CreateBooking(booking);

            // Assert
            Assert.True(created);
        }

        [Fact]
        public void CreateBooking_RoomAvailable_CreatedBooking()
        {
            // Arrange
            var booking = new Booking { Id = 1, CustomerId = 1, RoomId = 1, IsActive = true, StartDate = new DateTime(2021, 1, 1), EndDate = new DateTime(2021, 2, 1) };
            // Act
            var created = bookingManager.CreateBooking(booking);
            // Assert
            fakeBookingRepository.Verify(x => x.Add(booking), Times.Once());
        }

        [Fact]
        public void GetFullyOccupiedDates_StartDateLaterThanEndDate_ThrowsException()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(5);
            var endDate = DateTime.Today.AddDays(4);

            // Assert
            Assert.Throws<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(startDate, endDate));
        }

        [Theory]
        [InlineData("30/12/2019", "31/12/2019", 0)] // Vacant period BEFORE known bookings.
        [InlineData("31/12/2019", "1/1/2020", 1)] // One day intersecting start of known bookings.
        [InlineData("1/2/2020", "2/2/2020", 1)] // One day intersecting end of known bookings.
        [InlineData("30/12/2021", "31/12/2021", 0)] // Vacant period AFTER known bookings.
        public void GetFullyOccupiedDates_VacantPeriod_ReturnsEmptyList(string startDate, string endDate, int numOccupiedDates)
        {
            // Arrange
            var start = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);

            // Act
            var occupiedDates = bookingManager.GetFullyOccupiedDates(start, end);

            // Assert
            Assert.Equal(numOccupiedDates, occupiedDates.Count);
        }



    }
}
