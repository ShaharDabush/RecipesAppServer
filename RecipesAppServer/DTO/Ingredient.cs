namespace RecipesAppServer.DTO
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string IngredientName { get; set; } = null!;

        public string IngredientImage { get; set; } = null!;

        public int KindId { get; set; }

        public bool IsKosher { get; set; } 

        public bool IsGloten { get; set; } 

        public bool IsMeat { get; set; }

        public bool IsDairy { get; set; }

        public string Barkod { get; set; } = null!;

        public Ingredient() { }
        public Ingredient(Models.Ingredient modelUser)
        {
            this.Id = modelUser.Id;
            this.IngredientName = modelUser.IngredientName;
            this.IngredientImage = modelUser.IngredientImage;
            this.KindId = modelUser.KindId;
            this.IsKosher = modelUser.IsKosher;
            this.IsGloten = modelUser.IsGloten;
            this.IsMeat = modelUser.IsMeat;
            this.IsDairy = modelUser.IsDairy;
            this.Barkod = modelUser.Barkod;
        }

        public Models.Ingredient GetModels()
        {
            Models.Ingredient modelsUser = new Models.Ingredient()
            {
                Id = this.Id,
                IngredientName = this.IngredientName,
                IngredientImage = this.IngredientImage,
                KindId = this.KindId,
                IsKosher = this.IsKosher,
                IsGloten = this.IsGloten,
                IsMeat = this.IsMeat,
                IsDairy = this.IsDairy,
                Barkod = this.Barkod
            };

            return modelsUser;
        }
    }
}
