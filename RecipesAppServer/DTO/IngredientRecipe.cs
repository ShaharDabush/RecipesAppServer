using RecipesAppServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipesAppServer.DTO
{
    public class IngredientRecipe
    {
        public int IngredientId { get; set; }

        public int RecipeId { get; set; }

        public int Amount { get; set; }

        public string MeasureUnits { get; set; } = null!;
        public IngredientRecipe() { }
        public IngredientRecipe(Models.IngredientRecipe modelUser)
        {
            this.IngredientId = modelUser.IngredientId;
            this.RecipeId = modelUser.RecipeId;
            this.Amount = modelUser.Amount;
            this.MeasureUnits = modelUser.MeasureUnits;
        }

        public Models.IngredientRecipe GetModels()
        {
            Models.IngredientRecipe modelsUser = new Models.IngredientRecipe()
            {
                IngredientId = this.IngredientId,
                RecipeId = this.RecipeId,
                Amount = this.Amount,
                MeasureUnits = this.MeasureUnits
            };

            return modelsUser;
        }
    }
}
