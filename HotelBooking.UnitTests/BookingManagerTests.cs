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

        private static DateTime d = DateTime.Today;
        private static DateTime firstInMonth1 = new DateTime(d.Year, d.Month, 1).AddMonths(1);
        private static DateTime firstInMonth2 = new DateTime(d.Year, d.Month, 1).AddMonths(2);

        public BookingManagerTests(){

            fakeBookingRepository = new Mock<IRepository<Booking>>();

            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1, CustomerId = 1, RoomId = 1, IsActive = true,
                    StartDate = firstInMonth1,
                    EndDate = firstInMonth2
                },
                new Booking
                {
                    Id = 2, CustomerId = 2, RoomId = 2, IsActive = true,
                    StartDate = firstInMonth1,
                    EndDate = firstInMonth2
                }
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
            var booking = new Booking
            {
                Id = 1, CustomerId = 1, RoomId = 1, IsActive = true,
                StartDate = firstInMonth1,
                EndDate = firstInMonth2
            };

            // Act
            var created = bookingManager.CreateBooking(booking);

            // Assert
            Assert.False(created);
        }

        [Fact]
        public void CreateBooking_RoomAvailable_ReturnTrue()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1, CustomerId = 1, RoomId = 1, IsActive = true,
                StartDate = firstInMonth2.AddDays(1),
                EndDate = firstInMonth2.AddDays(2)
            };

            // Act
            var created = bookingManager.CreateBooking(booking);

            // Assert
            Assert.True(created);
        }

        [Fact]
        public void CreateBooking_RoomAvailable_CreatedBooking()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1, CustomerId = 1, RoomId = 1, IsActive = true,
                StartDate = firstInMonth2.AddDays(1),
                EndDate = firstInMonth2.AddDays(2)
            };
            // Act
            var created = bookingManager.CreateBooking(booking);
            // Assert
            fakeBookingRepository.Verify(x => x.Add(booking), Times.Once());
        }

        [Fact]
        public void GetFullyOccupiedDates_StartDateLaterThanEndDate_ThrowsException()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(2);
            var endDate = DateTime.Today.AddDays(1);

            // Assert
            Assert.Throws<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(startDate, endDate));
        }

        public static IEnumerable<object[]> VacantPeriodTestData()
        {
            yield return new object[] { firstInMonth1.AddDays(-2), firstInMonth1.AddDays(-1), 0 }; // Vacant period BEFORE known bookings.
            yield return new object[] { firstInMonth1.AddDays(-1), firstInMonth1, 1 };             // One day intersecting start of known bookings.
            yield return new object[] { firstInMonth2, firstInMonth2.AddDays(1), 1 };              // One day intersecting end of known bookings.
            yield return new object[] { firstInMonth2.AddDays(1), firstInMonth2.AddDays(2), 0 };   // Vacant period AFTER known bookings.
        }
        
        [Theory]
        [MemberData(nameof(VacantPeriodTestData))]
        public void GetFullyOccupiedDates_VacantPeriod_ReturnsNumberOfOccupiedDates(string startDate, string endDate, int numOccupiedDates)
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
