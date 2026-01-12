using System;
using System.Collections.Generic;

namespace MSLR.web.Models;

public partial class Voter
{
    public int VoterId { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Scc { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ValidScc SccNavigation { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
