namespace RecipesAppServer.DTO
{
    public class Recipe
    {
        public int Id { get; set; }

        public string RecipesName { get; set; } = null!;

        public string RecipeDescription { get; set; } = null!;

        public string RecipeImage { get; set; } = null!;

        public int MadeBy { get; set; }

        public int Rating { get; set; }

        public string IsKosher { get; set; } = null!;

        public string IsGloten { get; set; } = null!;
        public int HowManyMadeIt { get; set; }

        public int ContainsMeat { get; set; }

        public int ContainsDairy { get; set; }

        public string TimeOfDay { get; set; } = null!;

        public Recipe() { }
        public Recipe(Models.Recipe modelRecipe)
        {
            this.Id = modelRecipe.Id;
            this.RecipesName = modelRecipe.RecipesName;
            this.RecipeDescription = modelRecipe.RecipeDescription;
            this.RecipeImage = modelRecipe.RecipeImage;
            this.MadeBy = modelRecipe.MadeBy;
            this.Rating = modelRecipe.Rating;
            this.IsKosher = modelRecipe.IsKosher;
            this.IsGloten = modelRecipe.IsGloten;
            this.TimeOfDay = modelRecipe.TimeOfDay;

        }

        public Models.Recipe GetModels()
        {
            Models.Recipe modelsRecipe = new Models.Recipe()
            {
                Id = this.Id,
                RecipesName = this.RecipesName,
                RecipeDescription = this.RecipeDescription,
                RecipeImage = this.RecipeImage,
                MadeBy = this.MadeBy,
                Rating = this.Rating,
                IsKosher = this.IsKosher,
                IsGloten = this.IsGloten,
                TimeOfDay = this.TimeOfDay

            };

            return modelsRecipe;
        }
    }
}
