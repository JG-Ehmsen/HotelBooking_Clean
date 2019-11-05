using HotelBooking.Core;
using Moq;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace HotelBooking.SpecflowTests
{
    [Binding]
    public class GetFullyOccupiedDatesSteps
    {
        private IBookingManager bookingManager;

        Mock<IRepository<Booking>> fakeBookingRepository;
        Mock<IRepository<Room>> fakeRoomRepository;

        private static DateTime d = DateTime.Today;
        private static DateTime tenYearsLater = new DateTime(d.Year, d.Month, 1).AddYears(10);

        DateTime startDate;
        DateTime endDate;
        int amountFullyOccupiedDates;

        public GetFullyOccupiedDatesSteps()
        {

            fakeBookingRepository = new Mock<IRepository<Booking>>();

            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1, CustomerId = 1, RoomId = 1, IsActive = true,
                    StartDate = d,
                    EndDate = tenYearsLater
                }
            };

            // Implement fake GetAll() method.
            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);

            fakeBookingRepository.Setup(x => x.Add(It.IsAny<Booking>())).Returns(bookings[0]);


            fakeRoomRepository = new Mock<IRepository<Room>>();

            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" }
            };

            // Implement fake GetAll() method.
            fakeRoomRepository.Setup(x => x.GetAll()).Returns(rooms);

            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);
        }

        [Given(@"I have entered a date range start date '(.*)'")]
        public void GivenIHaveEnteredADateRangeStartDate(string _startDate)
        {
            startDate = DateTime.Parse(_startDate);
        }
        
        [Given(@"I have entered a date range end date '(.*)'")]
        public void GivenIHaveEnteredADateRangeEndDate(string _endDate)
        {
            endDate = DateTime.Parse(_endDate);
        }
        
        [When(@"I press find fully occupied dates")]
        public void WhenIPressFindFullyOccupiedDates()
        {
            amountFullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate).Count;
        }
        
        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int expectedFullyOccupiedDates)
        {
            Assert.Equal(expectedFullyOccupiedDates, amountFullyOccupiedDates);
        }
    }
}
