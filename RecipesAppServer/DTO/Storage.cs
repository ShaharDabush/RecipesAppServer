namespace RecipesAppServer.DTO
{
    public class Storage
    {
        public int Id { get; set; }

        public string StorangeName { get; set; } = null!;

        public string StorageCode { get; set; } = null!;

        public int Manager { get; set; }

        public Storage() { }
        public Storage(Models.Storage modelStorage)
        {
            this.Id = modelStorage.Id;
            this.StorangeName = modelStorage.StorageName;
            this.Manager = modelStorage.Manager;
        }

        public Models.Storage GetModels()
        {
            Models.Storage modelStorage = new Models.Storage()
            {
                Id = this.Id,
                StorageName = this.StorangeName,
                Manager = this.Manager,
                StorageCode = "ADBCE"
                
            };

            return modelStorage;
        }
    }
}
