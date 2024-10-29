namespace RecipesAppServer.DTO
{
    public class Barkod
    {
        public int Id { get; set; }

        public string BarkodImage { get; set; } = null!;

        public int IngredientId { get; set; }

        public Barkod() { }
        public Barkod(Models.Barkod modelUser)
        {
            this.Id = modelUser.Id;
            this.BarkodImage = modelUser.BarkodImage;
            this.IngredientId = modelUser.IngredientId;
        }

        public Models.Barkod GetModels()
        {
            Models.Barkod modelsUser = new Models.Barkod()
            {
                Id = this.Id,
                BarkodImage = this.BarkodImage,
                IngredientId = this.IngredientId
            };

            return modelsUser;
        }
    }
}
