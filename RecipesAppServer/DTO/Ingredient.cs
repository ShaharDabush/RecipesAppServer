namespace RecipesAppServer.DTO
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string IngredientName { get; set; } = null!;

        public string IngredientImage { get; set; } = null!;

        public int KindId { get; set; }

        public string MeatOrDariy { get; set; } = null!;

        public string IsKosher { get; set; } = null!;

        public string IsGloten { get; set; } = null!;

        public string Barkod { get; set; } = null!;

        public Ingredient() { }
        public Ingredient(Models.Ingredient modelUser)
        {
            this.Id = modelUser.Id;
            this.IngredientName = modelUser.IngredientName;
            this.IngredientImage = modelUser.IngredientImage;
            this.KindId = modelUser.KindId;
            this.MeatOrDariy = modelUser.MeatOrDariy;
            this.IsKosher = modelUser.IsKosher;
            this.IsGloten = modelUser.IsGloten;
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
                MeatOrDariy = this.MeatOrDariy,
                IsKosher = this.IsKosher,
                IsGloten = this.IsGloten,
                Barkod = this.Barkod
            };

            return modelsUser;
        }
    }
}
