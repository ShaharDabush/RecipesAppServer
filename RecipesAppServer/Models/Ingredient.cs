﻿using System;
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

    [StringLength(30)]
    public string IngredientImage { get; set; } = null!;

    public int KindId { get; set; }

    [StringLength(20)]
    public string MeatOrDariy { get; set; } = null!;

    [StringLength(5)]
    public string IsKosher { get; set; } = null!;

    [StringLength(5)]
    public string IsGloten { get; set; } = null!;

    [StringLength(200)]
    public string Barkod { get; set; } = null!;

    [InverseProperty("Ingredient")]
    public virtual ICollection<Barkod> Barkods { get; set; } = new List<Barkod>();

    [ForeignKey("KindId")]
    [InverseProperty("Ingredients")]
    public virtual Kind Kind { get; set; } = null!;
}