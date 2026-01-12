using System;
using System.Collections.Generic;

namespace MSLR.web.Models;

public partial class ValidScc
{
    public string Scc { get; set; } = null!;

    public bool IsUsed { get; set; }

    public virtual Voter? Voter { get; set; }
}
