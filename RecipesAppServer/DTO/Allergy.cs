namespace RecipesAppServer.DTO
{
    public class Allergy
    {
        public int Id { get; set; }

        public string AllergyName { get; set; } = null!;

        public Allergy() { }
        public Allergy(Models.Allergy modelUser)
        {
            this.Id = modelUser.Id;
            this.AllergyName = modelUser.AllergyName;
        }

        public Models.Allergy GetModels()
        {
            Models.Allergy modelsUser = new Models.Allergy()
            {
                Id = this.Id,
                AllergyName = this.AllergyName,
            };

            return modelsUser;
        }
    }
}
