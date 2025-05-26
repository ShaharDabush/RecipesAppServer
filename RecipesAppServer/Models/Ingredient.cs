using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

public partial class Ingredient
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string IngredientName { get; set; } = null!;

    [StringLength(1500)]
    public string IngredientImage { get; set; } = null!;

    public bool IsKosher { get; set; }

    public bool IsGloten { get; set; }

    public bool IsMeat { get; set; }

    public bool IsDairy { get; set; }

    [StringLength(200)]
    public string Barcode { get; set; } = null!;

    [InverseProperty("Ingredient")]
    public virtual ICollection<IngredientRecipe> IngredientRecipes { get; set; } = new List<IngredientRecipe>();

    [ForeignKey("IngredientId")]
    [InverseProperty("Ingredients")]
    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();
}
