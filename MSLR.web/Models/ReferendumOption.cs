using System;
using System.Collections.Generic;

namespace MSLR.web.Models;

public partial class ReferendumOption
{
    public int OptionId { get; set; }

    public int ReferendumId { get; set; }

    public string OptionText { get; set; } = null!;

    public virtual Referendum Referendum { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
