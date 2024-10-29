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

        public string TimeOfDay { get; set; } = null!;

        public Recipe() { }
        public Recipe(Models.Recipe modelUser)
        {
            this.Id = modelUser.Id;
            this.RecipesName = modelUser.RecipesName;
            this.RecipeDescription = modelUser.RecipeDescription;
            this.RecipeImage = modelUser.RecipeImage;
            this.MadeBy = modelUser.MadeBy;
            this.Rating = modelUser.Rating;
            this.IsKosher = modelUser.IsKosher;
            this.IsGloten = modelUser.IsGloten;
            this.TimeOfDay = modelUser.TimeOfDay;

        }

        public Models.Recipe GetModels()
        {
            Models.Recipe modelsUser = new Models.Recipe()
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

            return modelsUser;
        }
    }
}
