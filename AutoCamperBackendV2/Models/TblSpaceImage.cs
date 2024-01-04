using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoCamperBackendV2.Models;

public partial class TblSpaceImage
{
    public int FldSpaceImagesId { get; set; }

    public string? FldB64encoding { get; set; }

    public int? FldSpaceId { get; set; }
    [JsonIgnore]
    public virtual TblSpace? FldSpace { get; set; }
}
