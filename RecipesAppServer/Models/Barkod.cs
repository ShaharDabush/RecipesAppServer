using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Table("Barkod")]
public partial class Barkod
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    public string BarkodImage { get; set; } = null!;

    public int IngredientId { get; set; }

    [StringLength(30)]
    public string IngredientImage { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("Barkods")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
