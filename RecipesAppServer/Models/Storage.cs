using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

[Table("Storage")]
public partial class Storage
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    public string StorageName { get; set; } = null!;

    [StringLength(5)]
    public string StorageCode { get; set; } = null!;

    public int Manager { get; set; }

    [ForeignKey("Manager")]
    [InverseProperty("Storages")]
    public virtual User ManagerNavigation { get; set; } = null!;

    [InverseProperty("Storage")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
