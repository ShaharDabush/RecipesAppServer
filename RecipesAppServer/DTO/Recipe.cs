namespace RecipesAppServer.DTO
{
    public class Recipe
    {
        public int Id { get; set; }

        public string RecipesName { get; set; } = null!;

        public string RecipeDescription { get; set; } = null!;

        public string RecipeImage { get; set; } = null!;

        public string Type { get; set; }   

        public int MadeBy { get; set; }

        public int Rating { get; set; }

        public bool IsKosher { get; set; }

        public bool IsGloten { get; set; } 
        public int HowManyMadeIt { get; set; }

        public bool ContainsMeat { get; set; }

        public bool ContainsDairy { get; set; }

        public string TimeOfDay { get; set; } = null!;
        
        public List<Allergy> Allergies { get; set; }

        public Recipe() { }
        public Recipe(Models.Recipe modelRecipe)
        {
            this.Id = modelRecipe.Id;
            this.RecipesName = modelRecipe.RecipesName;
            this.RecipeDescription = modelRecipe.RecipeDescription;
            this.RecipeImage = modelRecipe.RecipeImage;
            this.Type = modelRecipe.Type;
            this.MadeBy = modelRecipe.MadeBy;
            this.Rating = modelRecipe.Rating;
            this.IsKosher = modelRecipe.IsKosher;
            this.IsGloten = modelRecipe.IsGloten;
            this.HowManyMadeIt = modelRecipe.HowManyMadeIt;
            this.ContainsMeat = modelRecipe.ContainsMeat;
            this.ContainsDairy = modelRecipe.ContainsDairy;
            this.TimeOfDay = modelRecipe.TimeOfDay;
            this.Allergies = new List<Allergy>();
            foreach (Models.Allergy a in modelRecipe.Allergies)
            {
                DTO.Allergy allergy = new DTO.Allergy();
                allergy.Id = a.Id;
                allergy.AllergyName = a.AllergyName;
                this.Allergies.Add(allergy);
            }

        }

        public Models.Recipe GetModels()
        {
            Models.Recipe modelsRecipe = new Models.Recipe()
            {
                Id = this.Id,
                RecipesName = this.RecipesName,
                RecipeDescription = this.RecipeDescription,
                RecipeImage = this.RecipeImage,
                Type = this.Type,
                MadeBy = this.MadeBy,
                Rating = this.Rating,
                IsKosher = this.IsKosher,
                IsGloten = this.IsGloten,
                HowManyMadeIt = this.HowManyMadeIt,
                ContainsMeat = this.ContainsMeat,
                ContainsDairy = this.ContainsDairy,
                TimeOfDay = this.TimeOfDay

            };
            return modelsRecipe;
        }
    }
}
