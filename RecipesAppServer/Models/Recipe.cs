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

    [StringLength(1000)]
    public string RecipeDescription { get; set; } = null!;

    [StringLength(1500)]
    public string RecipeImage { get; set; } = null!;

    [StringLength(100)]
    public string Type { get; set; } = null!;

    public int MadeBy { get; set; }

    public int Rating { get; set; }

    public bool IsKosher { get; set; }

    public bool IsGloten { get; set; }

    public int HowManyMadeIt { get; set; }

    public bool ContainsMeat { get; set; }

    public bool ContainsDairy { get; set; }

    [StringLength(20)]
    public string TimeOfDay { get; set; } = null!;

    [InverseProperty("Recipe")]
    public virtual ICollection<IngredientRecipe> IngredientRecipes { get; set; } = new List<IngredientRecipe>();

    [InverseProperty("Recipe")]
    public virtual ICollection<Level> Levels { get; set; } = new List<Level>();

    [ForeignKey("MadeBy")]
    [InverseProperty("Recipes")]
    public virtual User MadeByNavigation { get; set; } = null!;

    [InverseProperty("Recipe")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [ForeignKey("RecipeId")]
    [InverseProperty("Recipes")]
    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
}
