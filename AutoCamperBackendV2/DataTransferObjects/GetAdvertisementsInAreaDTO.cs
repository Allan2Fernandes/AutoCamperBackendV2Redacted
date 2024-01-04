namespace AutoCamperBackendV2.DataTransferObjects
{
    public class GetAdvertisementsInAreaDTO
    {
        public double? FldLongitude { get; set; }

        public double? FldLatitude { get; set; }

        public double? FldPrice { get; set; }

        public DateTime? FldReservationStart { get; set; }

        public DateTime? FldReservationEnd { get; set; }
    }
}
