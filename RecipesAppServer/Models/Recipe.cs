using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

public partial class Recipe
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string RecipesName { get; set; } = null!;

    [StringLength(100)]
    public string RecipeDescription { get; set; } = null!;

    [StringLength(30)]
    public string RecipeImage { get; set; } = null!;

    public int MadeBy { get; set; }

    public int Rating { get; set; }

    [StringLength(5)]
    public string IsKosher { get; set; } = null!;

    [StringLength(5)]
    public string IsGloten { get; set; } = null!;

    [StringLength(20)]
    public string TimeOfDay { get; set; } = null!;

    [InverseProperty("Recipe")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Recipe")]
    public virtual ICollection<Level> Levels { get; set; } = new List<Level>();

    [ForeignKey("MadeBy")]
    [InverseProperty("Recipes")]
    public virtual User MadeByNavigation { get; set; } = null!;

    [InverseProperty("Recipe")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
