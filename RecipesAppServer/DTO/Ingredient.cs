namespace RecipesAppServer.DTO
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string IngredientName { get; set; } = null!;

        public string IngredientImage { get; set; } = null!;

        public bool IsKosher { get; set; } 

        public bool IsGloten { get; set; } 

        public bool IsMeat { get; set; }

        public bool IsDairy { get; set; }

        public string Barcode { get; set; } = null!;

        public Ingredient() { }
        public Ingredient(Models.Ingredient modelIngredient)
        {
            this.Id = modelIngredient.Id;
            this.IngredientName = modelIngredient.IngredientName;
            this.IngredientImage = modelIngredient.IngredientImage;
            this.IsKosher = modelIngredient.IsKosher;
            this.IsGloten = modelIngredient.IsGloten;
            this.IsMeat = modelIngredient.IsMeat;
            this.IsDairy = modelIngredient.IsDairy;
            this.Barcode = modelIngredient.Barcode;
        }

        public Models.Ingredient GetModels()
        {
            Models.Ingredient modelIngredient = new Models.Ingredient()
            {
                Id = this.Id,
                IngredientName = this.IngredientName,
                IngredientImage = this.IngredientImage,
                IsKosher = this.IsKosher,
                IsGloten = this.IsGloten,
                IsMeat = this.IsMeat,
                IsDairy = this.IsDairy,
                Barcode = this.Barcode
            };

            return modelIngredient;
        }
    }
}
