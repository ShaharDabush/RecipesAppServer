namespace RecipesAppServer.DTO
{
    public class Storage
    {
        public int Id { get; set; }

        public string StorageName { get; set; } = null!;

        public string StorageCode { get; set; } = null!;

        public int Manager { get; set; }

        public Storage() { }
        public Storage(Models.Storage modelStorage)
        {
            this.Id = modelStorage.Id;
            this.StorageName = modelStorage.StorageName;
            this.Manager = modelStorage.Manager;
            this.StorageCode = modelStorage.StorageCode;
        }

        public Models.Storage GetModels()
        {
            Models.Storage modelStorage = new Models.Storage()
            {
                Id = this.Id,
                StorageName = this.StorageName,
                Manager = this.Manager,
                StorageCode = this.StorageCode
                
            };

            return modelStorage;
        }
    }
}
