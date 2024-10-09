using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Keyless]
[Table("AllergyUser")]
public partial class AllergyUser
{
    public int UserId { get; set; }

    public int AllergyId { get; set; }

    [ForeignKey("AllergyId")]
    public virtual Allergy Allergy { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
