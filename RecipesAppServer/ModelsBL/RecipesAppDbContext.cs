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
}

