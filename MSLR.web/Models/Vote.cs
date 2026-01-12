using System;
using System.Collections.Generic;

namespace MSLR.web.Models;

public partial class Vote
{
    public int VoteId { get; set; }

    public int VoterId { get; set; }

    public int ReferendumId { get; set; }

    public int OptionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ReferendumOption Option { get; set; } = null!;

    public virtual Referendum Referendum { get; set; } = null!;

    public virtual Voter Voter { get; set; } = null!;
}
