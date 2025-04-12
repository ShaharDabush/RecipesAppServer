using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Table("Allergy")]
public partial class Allergy
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string AllergyName { get; set; } = null!;

    [ForeignKey("AllergyId")]
    [InverseProperty("Allergies")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
