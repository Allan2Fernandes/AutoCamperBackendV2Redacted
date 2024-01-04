namespace AutoCamperBackendV2.DataTransferObjects
{
    public class UpdateUserDetailsDTO
    {
        public int FldUserId { get; set; }
        public string? FldName { get; set; }

        public string? FldPhoneNumber { get; set; }

        public string? FldAddress { get; set; }

        public string? FldPassword { get; set; }

        public string? FldEmail { get; set; }
    }
}
