using HotelBooking.Core;
using Moq;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace HotelBooking.SpecflowTests
{
    [Binding]
    public class CreateBookingSteps
    {
        private IBookingManager bookingManager;

        Mock<IRepository<Booking>> fakeBookingRepository;
        Mock<IRepository<Room>> fakeRoomRepository;

        private static DateTime d = DateTime.Today;
        private static DateTime firstInMonth1 = new DateTime(d.Year, d.Month, 1).AddMonths(1);
        private static DateTime firstInMonth2 = new DateTime(d.Year, d.Month, 1).AddMonths(2);

        DateTime startDate;
        DateTime endDate;
        int customerId;
        bool bookingResult;

        public CreateBookingSteps()
        {

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

        [Given(@"I have entered a start date '(.*)'")]
        public void GivenIHaveEnteredAStartDate(string _startDate)
        {
            startDate = DateTime.Parse(_startDate);
        }
        
        [Given(@"I have entered a end date '(.*)'")]
        public void GivenIHaveEnteredAEndDate(string _endDate)
        {
            endDate = DateTime.Parse(_endDate);
        }
        
        [Given(@"I have entered a customer id (.*)")]
        public void GivenIHaveEnteredACustomerId(int _customerId)
        {
            customerId = _customerId;
        }
        
        [When(@"I press button Create booking")]
        public void WhenIPressButtonCreateBooking()
        {
            var booking = new Booking
            {
                CustomerId = customerId,
                StartDate = startDate,
                EndDate = endDate
            };

            bookingResult = bookingManager.CreateBooking(booking);
           
        }
        
        [Then(@"The result should be (.*)")]
        public void ThenTheResultShouldBe(string p0)
        {
            Assert.True(bookingResult); 
        }
    }
}
