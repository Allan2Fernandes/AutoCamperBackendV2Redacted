using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoCamperBackendV2.Models;

public partial class TblBooking
{
    public int FldBookingId { get; set; }

    public int? FldUserId { get; set; }

    public int? FldSpaceId { get; set; }

    public DateTime? FldReservationStart { get; set; }

    public DateTime? FldReservationEnd { get; set; }

    public DateTime? FldCancellation { get; set; }

    public bool? FldIsAccepted { get; set; }
    [JsonIgnore]
    public virtual TblSpace? FldSpace { get; set; }
    [JsonIgnore]
    public virtual TblUser? FldUser { get; set; }
}
