namespace RecipesAppServer.DTO
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string? UserImage { get; set; }

        public string ProfileImagePath { get; set; } = "";

        public int? StorageId { get; set; }

        public int? IsAdmin { get; set; }

        public User() { }
        public User(Models.User modelUser)
        {
            this.Id = modelUser.Id;
            this.UserName = modelUser.UserName;
            this.Email = modelUser.Email;
            this.UserPassword = modelUser.UserPassword;
            this.UserImage = modelUser.UserImage;
            this.StorageId = modelUser.StorageId;
        }

        public Models.User GetModels()
        {
            Models.User modelsUser = new Models.User()
            {
                Id = this.Id,
                UserName = this.UserName,
                Email = this.Email,
                UserPassword = this.UserPassword,
                UserImage = this.UserImage,
                StorageId = this.StorageId,
            };

            return modelsUser;
        }
    }
}
