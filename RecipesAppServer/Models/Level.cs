using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

public partial class Level
{
    [Key]
    public int Id { get; set; }

    [StringLength(300)]
    public string TextLevel { get; set; } = null!;

    public int LevelCount { get; set; }

    public int RecipeId { get; set; }

    [ForeignKey("RecipeId")]
    [InverseProperty("Levels")]
    public virtual Recipe Recipe { get; set; } = null!;
}
