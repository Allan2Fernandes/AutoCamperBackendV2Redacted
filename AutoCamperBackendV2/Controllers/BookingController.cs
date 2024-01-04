using AutoCamperBackendV2.DataTransferObjects;
using AutoCamperBackendV2.Functions;
using AutoCamperBackendV2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AutoCamperBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ParkInPeaceProjectContext Context;
        EmailSenderAdapter EmailSender = new EmailSenderAdapter();
        BookingServices Services;

        public BookingController(ParkInPeaceProjectContext context)
        {
            Context = context;
            this.Services = new BookingServices(context);    
        }


        [HttpPost(nameof(CreateBooking))]
        public async Task<ActionResult<string>> CreateBooking(CreateBookingDTO RequestBody)
        {
            // Create a booking object which can be added to the database
            var BookingToCreate = new TblBooking
            {
                FldUserId = RequestBody.FldUserId,
                FldSpaceId = RequestBody.FldSpaceId,
                FldReservationStart = RequestBody.FldReservationStart,
                FldReservationEnd = RequestBody.FldReservationEnd,
                FldCancellation = null,
            };

            // Add it to the database
            //await Context.TblBookings.AddAsync(BookingToCreate);
            //await Context.SaveChangesAsync();
            Services.CreateBookingInTable(BookingToCreate);

            // Send the an email to the space owner

            // Find the space owner's email
            var SpaceOwnerQuery = from Spaces in Context.TblSpaces
                                  join Users in Context.TblUsers
                                  on
                                  Spaces.FldUserId equals Users.FldUserId
                                  where Spaces.FldSpaceId == RequestBody.FldSpaceId
                                  select Users;


            string OwnersEmailAddress = "";
            string OwnersName = "";
            foreach (var item in SpaceOwnerQuery)
            {
                OwnersEmailAddress = item.FldEmail.ToString();
                OwnersName += item.FldName.ToString();
            }
            EmailSender.SendEmail(OwnersEmailAddress, "Pending Booking", $"<html><body><h1>Hello {OwnersName}</h1><p>One of your parking spaces has been booked. Logged in to confirm or reject the booking</p></body></html>", null);

            return Ok(new {BookingID = BookingToCreate.FldBookingId});
        }

        [HttpGet(nameof(getBookingOnUserID) + "/{UserID}")]
        public async Task<ActionResult<List<object>>> getBookingOnUserID(int UserID)
        {      
            List<object> QueryResults = Services.GetBookingsDetailsOnUserID(UserID);         
            return Ok(QueryResults);
        }

        [HttpPost(nameof(cancelBooking) + "/{BookingID}")]
        public async Task<ActionResult<string>> cancelBooking(int BookingID)
        {
            // Find the booking using the booking ID

            // Set the cancellation date to Date.Now

            var booking = Services.FindBookingOnID(BookingID);

            if(booking == null)
            {
                return Ok("Booking not found");
            }

            Services.CancelBooking(booking);

            // Find the space owner's Email ID
            var FindSpaceOwnerQuery = from Bookings in Context.TblBookings
                                      join Spaces in Context.TblSpaces
                                      on Bookings.FldSpaceId equals Spaces.FldSpaceId
                                      join Users in Context.TblUsers
                                      on Spaces.FldUserId equals Users.FldUserId
                                      select Users;
            string SpaceOwnerEmail = "";
            string OwnersName = "";
            foreach (var item in FindSpaceOwnerQuery)
            {
                SpaceOwnerEmail = item.FldEmail;
                OwnersName = item.FldName;
            }

            EmailSender.SendEmail(SpaceOwnerEmail, "Cancelled Booking", $"<html><body><h1>Hello {OwnersName}</h1><p>A booking for one of you parking spaces has been cancelled</p></body></html>", null);
            return Ok("Cancelled");
        }


        [HttpGet(nameof(GetPendingBookingsOnUserID) + "/{UserID}")]
        public async Task<ActionResult<List<object>>> GetPendingBookingsOnUserID(int UserID)
        {
            List<object> QueryResults = Services.GetPendingBookingsOfUser(UserID);    
            return Ok(QueryResults);
        }


        [HttpPost(nameof(ConfirmBookingDecision))]
        public async Task<ActionResult<string>> ConfirmBookingDecision(ConfirmBookingDecisionDTO Request)
        {
            // Find the booking
            var BookingToUpdate = Services.FindBookingOnID(Request.FldBookingId);
            if(BookingToUpdate == null)
            {
                return Ok("Booking Not Found");
            }
            Services.ConfirmBookingDecision(BookingToUpdate, Request.FldIsAccepted);

            // Update the user who booked the space by emailing them.

            var BookingCreatorQuery = from Bookings in Context.TblBookings
                                      join Users in Context.TblUsers
                                      on
                                      Bookings.FldUserId equals Users.FldUserId
                                      where Bookings.FldBookingId == Request.FldBookingId
                                      select Users;

            string BookingCreatorEmail = "";
            foreach (var item in BookingCreatorQuery)
            {
                BookingCreatorEmail = item.FldEmail.ToString();
            }

            var BookingDetails = from Bookings in Context.TblBookings
                                    join Spaces in Context.TblSpaces
                                    on
                                    Bookings.FldSpaceId equals Spaces.FldSpaceId
                                    join SpaceOwners in Context.TblUsers
                                    on Spaces.FldUserId equals SpaceOwners.FldUserId
                                    join BookingCreators in Context.TblUsers
                                    on Bookings.FldUserId equals BookingCreators.FldUserId
                                    where Bookings.FldBookingId == Request.FldBookingId
                                    select new
                                    {
                                        BookingCreators = BookingCreators,
                                        Bookings = Bookings,
                                        Spaces = Spaces,
                                        SpaceOwners = SpaceOwners
                                    };
            string BookerEmail = "";
            string BookerName = "";
            string SpaceOwnerEmail = "";
            string SpaceOwnerName = "";
            DateTime BookingStartDate;
            DateTime BookingEndDate;
            DateTime InvoiceDate;
            TimeSpan BookingDays;
            int BookingDaysCount;
            int BookingPrice;
            string trimmedInvoiceDate = "";
            string InvoiceContent = "";

            foreach (var item in BookingDetails)
            {
                SpaceOwnerEmail = item.SpaceOwners.FldEmail.ToString();
                SpaceOwnerName = item.SpaceOwners.FldName.ToString();
                BookerEmail = item.BookingCreators.FldEmail.ToString();
                BookerName = item.BookingCreators.FldName.ToString();
                BookingStartDate = (DateTime)item.Bookings.FldReservationStart;
                BookingEndDate = (DateTime)item.Bookings.FldReservationEnd;
                BookingDays = (DateTime)item.Bookings.FldReservationEnd - (DateTime)item.Bookings.FldReservationStart;
                BookingDaysCount = BookingDays.Days;
                InvoiceDate = DateTime.Now;
                BookingPrice = (int)(item.Spaces.FldPrice * BookingDaysCount);

                trimmedInvoiceDate = InvoiceDate.ToString("dd-MM-yyyy");

                InvoiceContent = "Owner: " + SpaceOwnerName + "\n" + "Booker: " + BookerName + "\n" + "Booking Duration (days): " + BookingDaysCount + "\n" + "Price: " + BookingPrice;



            }

            PdfGenerator pdfGenerator = new PdfGenerator();
            string FileName = pdfGenerator.CreatePDF("Invoice", trimmedInvoiceDate, BookerName, InvoiceContent);
            string FilePath = FileName;
            byte[] PDFBytes = System.IO.File.ReadAllBytes(FilePath);
            Debug.WriteLine(FilePath);

            EmailSender.SendEmail(BookingCreatorEmail, "Booking Confirmation", $"Your booking has been {((bool)Request.FldIsAccepted ? "Accepted" : "Rejected")}", $"./{FilePath}");
            System.IO.File.Delete(FilePath);

            return Ok("Booking Updated");
        }
    }


}
