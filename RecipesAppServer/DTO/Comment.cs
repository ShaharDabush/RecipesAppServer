namespace RecipesAppServer.DTO
{
    public class Comment
    {
        public int Id { get; set; }
        public string Comment1 { get; set; } = null!;

        public int UserId { get; set; }

        public int RecipeId { get; set; }
        public Comment() { }    
        public Comment(Models.Comment modelUser)
        {
            this.Id = modelUser.Id;
            this.Comment1 = modelUser.Comment1;
            this.UserId = modelUser.UserId;
            this.RecipeId = modelUser.RecipeId;
        }

        public Models.Comment GetModels()
        {
            Models.Comment modelsUser = new Models.Comment()
            {
                Id = this.Id,
                Comment1 = this.Comment1,
                UserId = this.UserId,
                RecipeId = this.RecipeId
            };

            return modelsUser;
        }
    }
}
