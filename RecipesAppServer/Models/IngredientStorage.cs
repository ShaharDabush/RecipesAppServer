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

    public int StorageId { get; set; }

    [ForeignKey("IngredientId")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("StorageId")]
    public virtual Storage Storage { get; set; } = null!;
}
