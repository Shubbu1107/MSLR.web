using System;
using System.Collections.Generic;

namespace MSLR.web.Models;

public partial class Referendum
{
    public int ReferendumId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ReferendumOption> ReferendumOptions { get; set; } = new List<ReferendumOption>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
