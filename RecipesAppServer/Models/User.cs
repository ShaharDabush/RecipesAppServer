using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Index("Email", Name = "UQ__Users__A9D10534A8C978B6", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string UserPassword { get; set; } = null!;

    [StringLength(30)]
    public string? UserImage { get; set; }

    public int? IsAdmin { get; set; }

    public int? StorageId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("User")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [InverseProperty("MadeByNavigation")]
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    [ForeignKey("StorageId")]
    [InverseProperty("Users")]
    public virtual Storage? Storage { get; set; }

    [InverseProperty("ManagerNavigation")]
    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();
}
