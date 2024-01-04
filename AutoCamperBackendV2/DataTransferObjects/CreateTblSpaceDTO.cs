namespace AutoCamperBackendV2.DataTransferObjects
{
    public class CreateTblSpaceDTO
    {
        public int FldSpaceId { get; set; }

        public int? FldPrice { get; set; }

        public string? FldAddress { get; set; }

        public int? FldUserId { get; set; }

        public double? FldLength { get; set; }

        public double? FldWidth { get; set; }

        public double? FldHeight { get; set; }

        public double? FldLongitude { get; set; }

        public double? FldLatitude { get; set; }

        public bool? FldSewageDisposal { get; set; }

        public bool? FldElectricity { get; set; }

        public int? FldCancellationDuration { get; set; }

        public double? FldCancellationPenalty { get; set; }

        public string[]? ListOfImageEncodings { get; set; }
    }
}
