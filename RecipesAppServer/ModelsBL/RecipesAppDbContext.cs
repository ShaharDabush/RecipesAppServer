using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RecipesAppServer.DTO;

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
    public List<Level>? GetLevelByRecipe(int RecipeId)
    {
        return this.Levels.Where(l => l.RecipeId == RecipeId).ToList();
    }
    public List<Ingredient>? GetAllIngredient()
    {
        return this.Ingredients.ToList();
    }
    public List<Ingredient>? GetIngredientByRecipe(int RecipeId)
    {
        List<IngredientRecipe> l =  this.IngredientRecipes.Where(i => i.RecipeId == RecipeId).ToList();
        List<Ingredient> IngredientList = new List<Ingredient>();
        foreach (IngredientRecipe ir in l)
        {
            IngredientList.Add(this.Ingredients.Where(i => i.Id == ir.IngredientId).FirstOrDefault());
        }
        return IngredientList;

    }
    public List<User>? GetAllUser()
    {
        return this.Users.ToList();
    }


}

