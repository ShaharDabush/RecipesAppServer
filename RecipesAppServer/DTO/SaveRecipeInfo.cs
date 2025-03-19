namespace RecipesAppServer.DTO
{
    public class SaveRecipeInfo
    {
        public Recipe RecipeInfo { get; set; }
        public List<Level> LevelsInfo { get; set; }
        public List<IngredientRecipe> IngredientsInfo { get; set; }
    }
}
