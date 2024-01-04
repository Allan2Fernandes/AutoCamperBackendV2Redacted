using AutoCamperBackendV2.DataTransferObjects;
using AutoCamperBackendV2.Functions;

namespace AutoCamperBackendV2.Services
{
    public class BookingServices
    {
        ParkInPeaceProjectContext context;

        public BookingServices(ParkInPeaceProjectContext context)
        {
            this.context = context;
        }

        public void CreateBookingInTable(TblBooking BookingToCreate)
        {
            // Add it to the database
            context.TblBookings.Add(BookingToCreate);
            context.SaveChanges();
        }

        public List<object> GetBookingsDetailsOnUserID(int UserID)
        {
            var query = from bookings in context.TblBookings
                        join space in context.TblSpaces on bookings.FldSpaceId equals space.FldSpaceId
                        join SpaceOwner in context.TblUsers on space.FldUserId equals SpaceOwner.FldUserId
                        where bookings.FldUserId == UserID
                        select new
                        {
                            Booking = bookings,
                            Space = space,
                            SpaceOwner = SpaceOwner
                        };


            List<object> QueryResults = new List<object>();
            foreach (var item in query)
            {
                QueryResults.Add(new
                {
                    BookingID = item.Booking.FldBookingId,
                    UserID = item.Booking.FldUserId,
                    SpaceID = item.Booking.FldSpaceId,
                    ReservationStartDate = item.Booking.FldReservationStart,
                    ReservationEndDate = item.Booking.FldReservationEnd,
                    CancellationDateTime = item.Booking.FldCancellation,
                    isAccepted = item.Booking.FldIsAccepted,
                    SpaceAddress = item.Space.FldAddress,
                    SpacePrice = item.Space.FldPrice,
                    SpaceOwnerName = item.SpaceOwner.FldName,
                    SpaceOwnerPhoneNumber = item.SpaceOwner.FldPhoneNumber,
                    SpaceOwnerEmail = item.SpaceOwner.FldEmail

                });
            }
            return QueryResults;
        }

        public TblBooking? FindBookingOnID(int BookingID)
        {
            var booking = context.TblBookings.SingleOrDefault(eachBooking => eachBooking.FldBookingId == BookingID);
            return booking;
        }

        public void CancelBooking(TblBooking BookingToCancel)
        {
            BookingToCancel.FldCancellation = DateTime.Now;
            context.SaveChanges();
        }

        public List<object> GetPendingBookingsOfUser(int UserID)
        {
            var Query = from Space in context.TblSpaces
                        join Booking in context.TblBookings
                        on
                        Space.FldSpaceId equals Booking.FldSpaceId
                        // Space owner needs to be notified when there are pending bookings
                        where Space.FldUserId == UserID 
                        && Booking.FldIsAccepted == null
                        select new
                        {
                            Space,
                            Booking,
                        };

            List<object> QueryResults = new List<object>();

            foreach (var item in Query)
            {
                QueryResults.Add(item);
            }
            return QueryResults;
        }

        public void ConfirmBookingDecision(TblBooking BookingToUpdate, bool? Decision)
        {
            BookingToUpdate.FldIsAccepted = Decision;
            context.SaveChanges();
        }
    }
}
