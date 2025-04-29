using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Table("Rating")]
public partial class Rating
{
    [Key]
    public int Id { get; set; }

    public double Rate { get; set; }

    public int UserId { get; set; }

    public int RecipeId { get; set; }

    [ForeignKey("RecipeId")]
    [InverseProperty("Ratings")]
    public virtual Recipe Recipe { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Ratings")]
    public virtual User User { get; set; } = null!;
}
