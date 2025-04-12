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

    public int KindId { get; set; }

    public bool IsKosher { get; set; }

    public bool IsGloten { get; set; }

    public bool IsMeat { get; set; }

    public bool IsDairy { get; set; }

    [StringLength(200)]
    public string Barkod { get; set; } = null!;

    [InverseProperty("Ingredient")]
    public virtual ICollection<Barkod> Barkods { get; set; } = new List<Barkod>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<IngredientRecipe> IngredientRecipes { get; set; } = new List<IngredientRecipe>();

    [ForeignKey("KindId")]
    [InverseProperty("Ingredients")]
    public virtual Kind Kind { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("Ingredients")]
    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();
}
