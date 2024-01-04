using AutoCamperBackendV2.DataTransferObjects;
using AutoCamperBackendV2.Functions;
using AutoCamperBackendV2.Models;
using AutoCamperBackendV2.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace XUnitTestProject
{
    public class BookingTests
    {
        private readonly ITestOutputHelper output;
        public BookingTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CreateBookingInTableTest() 
        {
            var mockSet = new Mock<DbSet<TblBooking>>();
            var mockContext = new Mock<ParkInPeaceProjectContext>();

            mockContext.Setup(m => m.TblBookings).Returns(mockSet.Object);

            var service = new BookingServices(mockContext.Object);
            service.CreateBookingInTable(new TblBooking());

            mockSet.Verify(m => m.Add(It.IsAny<TblBooking>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void FindBookingOnIDTest()
        {
            var data = new List<TblBooking>
            {
                new TblBooking
                {
                    FldBookingId = 1,
                    FldUserId = 10

                },
                new TblBooking
                {
                    FldBookingId = 2,
                    FldUserId = 20
                },
                new TblBooking
                {
                    FldBookingId = 3,
                    FldUserId = 30
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TblBooking>>();
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblBookings).Returns(mockSet.Object);

            var Service = new BookingServices(mockContext.Object);
            var QueriedBooking = Service.FindBookingOnID(3);

            Assert.Equal(30, QueriedBooking.FldUserId);
        }

        [Fact]
        public void CancelBookingTest()
        {
            var data = new List<TblBooking>
            {
                new TblBooking
                {
                    FldBookingId = 1,
                    FldUserId = 10

                },
                new TblBooking
                {
                    FldBookingId = 2,
                    FldUserId = 20
                },
                new TblBooking
                {
                    FldBookingId = 3,
                    FldUserId = 30
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TblBooking>>();
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblBookings).Returns(mockSet.Object);

            var Service = new BookingServices(mockContext.Object);

            var BookingToCancel = Service.FindBookingOnID(2);

            Service.CancelBooking(BookingToCancel);

            // Verify that the booking was updated
            mockContext.Verify(m => m.SaveChanges(), Times.Once);

            // Re-query the updated booking and verify that the cancellation time is not null

            var CancelledBooking = Service.FindBookingOnID(2);

            // Verify a second time by actually checking the cancellation time
            Assert.NotNull(CancelledBooking.FldCancellation);
        }

        [Fact]
        public void ConfirmBookingDecisionTest()
        {
            var data = new List<TblBooking>
            {
                new TblBooking
                {
                    FldBookingId = 1,
                    FldUserId = 10

                },
                new TblBooking
                {
                    FldBookingId = 2,
                    FldUserId = 20
                },
                new TblBooking
                {
                    FldBookingId = 3,
                    FldUserId = 30
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TblBooking>>();
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TblBooking>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblBookings).Returns(mockSet.Object);

            var Service = new BookingServices(mockContext.Object);

            var BookingToAccept = Service.FindBookingOnID(1);
            var BookingToReject = Service.FindBookingOnID(2);

            Service.ConfirmBookingDecision(BookingToAccept, true);
            Service.ConfirmBookingDecision(BookingToReject, false);

            // Verify that the booking was updated
            mockContext.Verify(m => m.SaveChanges(), Times.Exactly(2)); // It is twice

            // Re-query the bookings and check if they were accepted and rejected

            var AcceptedBooking = Service.FindBookingOnID(1);
            var RejectedBooking = Service.FindBookingOnID(2);   

            Assert.Equal(AcceptedBooking.FldIsAccepted, true);
            Assert.Equal(RejectedBooking.FldIsAccepted, false);
        }

        [Fact]
        public void GetPendingBookingsOfUsersSpaces()
        {
            var SpaceData = new List<TblSpace>
            {
                new TblSpace
                {
                    FldSpaceId = 1,
                    FldAddress = "Address1",
                    FldUserId = 1,
                },
                new TblSpace
                {
                    FldSpaceId = 2,
                    FldAddress = "Address2",
                    FldUserId = 1,
                },
                new TblSpace
                {
                    FldSpaceId = 3,
                    FldAddress = "Address3",
                    FldUserId = 2,
                }
            }.AsQueryable();

            var BookingsData = new List<TblBooking>
            {
                new TblBooking
                {
                    FldBookingId = 1,
                    FldSpaceId = 1,
                    FldIsAccepted = true,

                },
                new TblBooking
                {
                    FldBookingId = 2,
                    FldSpaceId = 2,
                    FldIsAccepted = false,
                },
                new TblBooking
                {
                    FldBookingId = 3,
                    FldSpaceId = 2
                },
                new TblBooking
                {
                    FldBookingId = 4,
                    FldSpaceId = 3,
                    FldIsAccepted = true
                }
            }.AsQueryable();

            var SpaceMockSet = new Mock<DbSet<TblSpace>>();
            SpaceMockSet.As<IQueryable<TblSpace>>().Setup(m => m.Provider).Returns(SpaceData.Provider);
            SpaceMockSet.As<IQueryable<TblSpace>>().Setup(m => m.Expression).Returns(SpaceData.Expression);
            SpaceMockSet.As<IQueryable<TblSpace>>().Setup(m => m.ElementType).Returns(SpaceData.ElementType);
            SpaceMockSet.As<IQueryable<TblSpace>>().Setup(m => m.GetEnumerator()).Returns(() => SpaceData.GetEnumerator());

            var BookingsMockSet = new Mock<DbSet<TblBooking>>();
            BookingsMockSet.As<IQueryable<TblBooking>>().Setup(m => m.Provider).Returns(BookingsData.Provider);
            BookingsMockSet.As<IQueryable<TblBooking>>().Setup(m => m.Expression).Returns(BookingsData.Expression);
            BookingsMockSet.As<IQueryable<TblBooking>>().Setup(m => m.ElementType).Returns(BookingsData.ElementType);
            BookingsMockSet.As<IQueryable<TblBooking>>().Setup(m => m.GetEnumerator()).Returns(() => BookingsData.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblSpaces).Returns(SpaceMockSet.Object);
            mockContext.Setup(c => c.TblBookings).Returns(BookingsMockSet.Object);

            var Service = new BookingServices(mockContext.Object);

            var PendingBookings = Service.GetPendingBookingsOfUser(1);
            Assert.Equal(1, PendingBookings.Count);

            object firstItem = PendingBookings[0];
            Type itemType = firstItem.GetType();

            output.WriteLine($"Type of the first item: {itemType}");

            // Output all properties for inspection
            foreach (var property in itemType.GetProperties())
            {
                var value = property.GetValue(firstItem);
                output.WriteLine($"{property.Name}: {value}");

                if (property.Name == "Booking")
                {
                    // It is of type TblBooking
                    var fldBookingId = value.GetType().GetProperty("FldBookingId").GetValue(value);
                    Assert.Equal(3, fldBookingId);
                }
            }
        }
    }
}
