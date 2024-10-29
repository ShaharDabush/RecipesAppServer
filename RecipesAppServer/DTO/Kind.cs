namespace RecipesAppServer.DTO
{
    public class Kind
    {
        public int Id { get; set; }

        public string KindName { get; set; } = null!;

        public Kind() { }
        public Kind(Models.Kind modelUser)
        {
            this.Id = modelUser.Id;
            this.KindName = modelUser.KindName;
        }

        public Models.Kind GetModels()
        {
            Models.Kind modelsUser = new Models.Kind()
            {
                Id = this.Id,
                KindName = this.KindName,
            };

            return modelsUser;
        }
    }
}
