using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoCamperBackendV2.Models;

public partial class TblSpace
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

    public bool? FldIsActive { get; set; }
    [JsonIgnore]
    public virtual TblUser? FldUser { get; set; }
    [JsonIgnore]
    public virtual ICollection<TblBooking> TblBookings { get; set; } = new List<TblBooking>();
    [JsonIgnore]
    public virtual ICollection<TblSpaceImage> TblSpaceImages { get; set; } = new List<TblSpaceImage>();
}
