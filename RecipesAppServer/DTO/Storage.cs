﻿namespace RecipesAppServer.DTO
{
    public class Storage
    {
        public int Id { get; set; }

        public string StorangeName { get; set; } = null!;

        public int Manager { get; set; }

        public Storage() { }
        public Storage(Models.Storage modelUser)
        {
            this.Id = modelUser.Id;
            this.StorangeName = modelUser.StorangeName;
            this.Manager = modelUser.Manager;
        }

        public Models.Storage GetModels()
        {
            Models.Storage modelsUser = new Models.Storage()
            {
                Id = this.Id,
                StorangeName = this.StorangeName,
                Manager = this.Manager
            };

            return modelsUser;
        }
    }
}
