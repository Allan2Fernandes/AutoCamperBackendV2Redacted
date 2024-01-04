namespace AutoCamperBackendV2.DataTransferObjects
{
    public class CreateBookingDTO
    {

        public int? FldUserId { get; set; }

        public int? FldSpaceId { get; set; }

        public DateTime? FldReservationStart { get; set; }

        public DateTime? FldReservationEnd { get; set; }

        public DateTime? FldCancellation { get; set; }

        public bool? FldIsAccepted { get; set; }
    }
}
