using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoCamperBackendV2.Models;

public partial class TblMessage
{
    public int FldMessageId { get; set; }

    public DateTime? FldTimeSent { get; set; }

    public string? FldMessageText { get; set; }

    public int? FldPrivateDiscussionId { get; set; }

    public bool? FldMessageDirection { get; set; }
    [JsonIgnore]
    public virtual TblPrivateDiscussion? FldPrivateDiscussion { get; set; }
}
