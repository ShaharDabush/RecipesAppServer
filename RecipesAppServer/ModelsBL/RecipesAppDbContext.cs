using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipesAppServer.Models;

public partial class RecipesAppDbContext : DbContext
{
    public User? GetUser(string email)
    {
        return this.Users.Where(u => u.Email == email)
                            .FirstOrDefault();
    }
    public Storage? GetStorage(string StorageCode)
    {
        return this.Storages.Where(s => s.StorageCode == StorageCode)
                    .FirstOrDefault();
    }

    public List<Recipe>? GetAllRecipe()
    {
        return this.Recipes.ToList();
    }


}

