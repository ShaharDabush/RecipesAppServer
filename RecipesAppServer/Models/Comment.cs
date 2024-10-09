using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

public partial class Comment
{
    [Key]
    public int Id { get; set; }

    [Column("Comment")]
    [StringLength(300)]
    public string Comment1 { get; set; } = null!;

    public int UserId { get; set; }

    public int RecipeId { get; set; }

    [ForeignKey("RecipeId")]
    [InverseProperty("Comments")]
    public virtual Recipe Recipe { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Comments")]
    public virtual User User { get; set; } = null!;
}
