using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoCamperBackendV2.Models;

public partial class TblUser
{
    public int FldUserId { get; set; }

    public string? FldName { get; set; }

    public string? FldPhoneNumber { get; set; }

    public string? FldAdress { get; set; }

    public bool? FldIsAdmin { get; set; }

    public string? FldEncryptedPassword { get; set; }

    public string? FldEmail { get; set; }

    [JsonIgnore]
    public virtual ICollection<TblBooking> TblBookings { get; set; } = new List<TblBooking>();
    [JsonIgnore]
    public virtual ICollection<TblSpace> TblSpaces { get; set; } = new List<TblSpace>();
}
