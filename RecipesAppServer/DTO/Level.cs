namespace RecipesAppServer.DTO
{
    public class Level
    {
        public int Id { get; set; }

        public string TextLevel { get; set; } = null!;

        public int LevelCount { get; set; }

        public int RecipeId { get; set; }

        public Level() { }
        public Level(Models.Level modelUser)
        {
            this.Id = modelUser.Id;
            this.TextLevel = modelUser.TextLevel;
            this.LevelCount = modelUser.LevelCount;
            this.RecipeId = modelUser.RecipeId;;
        }

        public Models.Level GetModels()
        {
            Models.Level modelsUser = new Models.Level()
            {
                Id = this.Id,
                TextLevel = this.TextLevel,
                LevelCount = this.LevelCount,
                RecipeId = this.RecipeId,
            };

            return modelsUser;
        }
    }
}
