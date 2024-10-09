using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Keyless]
[Table("IngredientStorage")]
public partial class IngredientStorage
{
    public int IngredientId { get; set; }

    public int RecipeId { get; set; }

    public int Amount { get; set; }

    [StringLength(20)]
    public string MeasureUnits { get; set; } = null!;

    [ForeignKey("IngredientId")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("RecipeId")]
    public virtual Recipe Recipe { get; set; } = null!;
}
