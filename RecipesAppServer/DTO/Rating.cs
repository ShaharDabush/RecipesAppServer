namespace RecipesAppServer.DTO
{
    public class Rating
    {
        public int Id { get; set; }

        public int Rate { get; set; }

        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public Rating() { }
        public Rating(Models.Rating modelUser)
        {
            this.Id = modelUser.Id;
            this.Rate = modelUser.Rate;
            this.UserId = modelUser.UserId;
            this.RecipeId = modelUser.RecipeId; ;
        }

        public Models.Rating GetModels()
        {
            Models.Rating modelsUser = new Models.Rating()
            {
                Id = this.Id,
                Rate = this.Rate,
                UserId = this.UserId,
                RecipeId = this.RecipeId,
            };

            return modelsUser;
        }
    }
}
