namespace RecipesAppServer.DTO
{
    public class Storage
    {
        public int Id { get; set; }

        public string StorageName { get; set; } = null!;

        public string StorageCode { get; set; } = null!;

        public int Manager { get; set; }

        public List<Ingredient> Ingredients { get; set; }


        public Storage() { }
        public Storage(Models.Storage modelStorage)
        {
            this.Id = modelStorage.Id;
            this.StorageName = modelStorage.StorageName;
            this.Manager = modelStorage.Manager;
            this.StorageCode = modelStorage.StorageCode;
            this.Ingredients = new List<Ingredient>();
            foreach (Models.Ingredient i in modelStorage.Ingredients)
            {
                DTO.Ingredient ingredient= new DTO.Ingredient();
                ingredient.Id = i.Id;
                this.Ingredients.Add(ingredient);
            }
        }

        public Models.Storage GetModels()
        {
            Models.Storage modelStorage = new Models.Storage()
            {
                Id = this.Id,
                StorageName = this.StorageName,
                Manager = this.Manager,
                StorageCode = this.StorageCode,
               

        };

            return modelStorage;
        }
    }
}
