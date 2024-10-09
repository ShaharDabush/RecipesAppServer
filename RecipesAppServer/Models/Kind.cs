using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Table("Kind")]
public partial class Kind
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string KindName { get; set; } = null!;

    [InverseProperty("Kind")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
