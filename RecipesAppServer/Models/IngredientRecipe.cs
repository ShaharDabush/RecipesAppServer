using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[PrimaryKey("IngredientId", "RecipeId")]
[Table("IngredientRecipe")]
public partial class IngredientRecipe
{
    [Key]
    public int IngredientId { get; set; }

    [Key]
    public int RecipeId { get; set; }

    public int Amount { get; set; }

    [StringLength(20)]
    public string MeasureUnits { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientRecipes")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("RecipeId")]
    [InverseProperty("IngredientRecipes")]
    public virtual Recipe Recipe { get; set; } = null!;
}
