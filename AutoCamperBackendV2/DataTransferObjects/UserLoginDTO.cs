namespace AutoCamperBackendV2.DataTransferObjects
{
    public class UserLoginDTO
    {
        public int FldUserId { get; set; }

        public string FldEncryptedPassword { get; set; }

        public string FldEmail { get; set; }
    }
}
