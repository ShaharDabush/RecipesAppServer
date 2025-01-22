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
    public User? GetUserById(int Id)
    {
        return this.Users.Where(u => u.Id == Id)
                            .FirstOrDefault();
    }
    public Storage? GetStorageById(int? Id)
    {
        return this.Storages.Where(s => s.Id == Id)
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
    public List<Recipe>? GetRecipesByUser(int UserId)
    {
        return this.Recipes.Where(r => r.MadeBy == UserId).ToList();
    }
    public List<Comment>? GetCommentsByUser(int UserId)
    {
        return this.Comments.Where(c => c.UserId == UserId).ToList();
    }
    public List<Rating>? GetRatingsByUser(int UserId)
    {
        return this.Ratings.Where(r => r.UserId == UserId).ToList();
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
    public List<User>? GetUsersByStorage(int? StorageId)
    {
        return this.Users.Where(u => u.StorageId == StorageId).ToList();
    }
    public User? GetAdminByStorage(Storage? Storage,int StorageId)
    {
        List<User> users = this.Users.Where(u => u.StorageId == StorageId).ToList();
        foreach (User u in users)
        {
            if(u.Id == Storage.Manager)
            {
                return u;
            }
           
        }
        return null;
    }



}

