using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoCamperBackendV2.Models;

public partial class TblPrivateDiscussion
{
    public int FldPrivateDiscussionId { get; set; }

    public string? FldUser1Email { get; set; }

    public string? FldUser2Email { get; set; }
    [JsonIgnore]
    public virtual ICollection<TblMessage> TblMessages { get; set; } = new List<TblMessage>();
}
