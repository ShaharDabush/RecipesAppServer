using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Index("Email", Name = "UQ__Users__A9D105342D9610D1", IsUnique = true)]
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

    [StringLength(1500)]
    public string? UserImage { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? IsKohser { get; set; }

    [StringLength(20)]
    public string? Vegetarianism { get; set; }

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

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
}
