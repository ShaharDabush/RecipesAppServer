namespace RecipesAppServer.DTO
{
    public class RegisterInfo
    {
        public User UserInfo { get; set; }
        public Storage StorageInfo { get; set; }
        public string StorageCodeInfo { get; set; }
        public bool IsNewStorage { get; set; }

        public RegisterInfo(User userInfo, Storage storageInfo, string storageCodeInfo, bool isNewStorage)
        {
            UserInfo = userInfo;
            StorageInfo = storageInfo;
            StorageCodeInfo = storageCodeInfo;
            IsNewStorage = isNewStorage;
        }
    }
}
