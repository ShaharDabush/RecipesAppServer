using Microsoft.AspNetCore.Mvc;
using RecipesAppServer.Models;
using RecipesAppServer.DTO;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RecipesAppServer.Controllers;

[Route("api")]
[ApiController]
public class RecipesAppAPIController : ControllerBase
{
    //a variable to hold a reference to the db context!
    private RecipesAppDbContext context;
    //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
    private IWebHostEnvironment webHostEnvironment;
    //Use dependency injection to get the db context and web host into the constructor
    public RecipesAppAPIController(RecipesAppDbContext context, IWebHostEnvironment env)
    {
        this.context = context;
        this.webHostEnvironment = env;
    }

    [HttpGet]
    [Route("TestServer")]
    public ActionResult<string> TestServer()
    {
        return Ok("Server Responded Successfully");
    }

    #region Backup / Restore
    [HttpGet("Backup")]
    public async Task<IActionResult> Backup()
    {
        string path = $"{this.webHostEnvironment.WebRootPath}\\..\\DBScripts\\backup.bak";

        bool success = await BackupDatabaseAsync(path);
        if (success)
        {
            return Ok("Backup was successful");
        }
        else
        {
            return BadRequest("Backup failed");
        }
    }

    [HttpGet("Restore")]
    public async Task<IActionResult> Restore()
    {
        string path = $"{this.webHostEnvironment.WebRootPath}\\..\\DBScripts\\backup.bak";

        bool success = await RestoreDatabaseAsync(path);
        if (success)
        {
            return Ok("Restore was successful");
        }
        else
        {
            return BadRequest("Restore failed");
        }
    }
    //this function backup the database to a specified path
    private async Task<bool> BackupDatabaseAsync(string path)
    {
        try
        {

            //Get the connection string
            string? connectionString = context.Database.GetConnectionString();
            //Get the database name
            string databaseName = context.Database.GetDbConnection().Database;
            //Build the backup command
            string command = $"BACKUP DATABASE {databaseName} TO DISK = '{path}'";
            //Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open the connection
                await connection.OpenAsync();
                //Create a command
                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                {
                    //Execute the command
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }

    //THis function restore the database from a backup in a certain path
    private async Task<bool> RestoreDatabaseAsync(string path)
    {
        try
        {
            //Get the connection string
            string? connectionString = context.Database.GetConnectionString();
            //Get the database name
            string databaseName = context.Database.GetDbConnection().Database;
            //Build the restore command
            string command = $@"
                USE master;
                ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE {databaseName} FROM DISK = '{path}' WITH REPLACE;
                ALTER DATABASE {databaseName} SET MULTI_USER;";

            //Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open the connection
                await connection.OpenAsync();
                //Create a command
                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                {
                    //Execute the command
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    #endregion

    [HttpPost("login")]
    public IActionResult Login([FromBody] DTO.LoginInfo loginDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Get model user class from DB with matching email. 
            Models.User? modelsUser = context.GetUser(loginDto.UserEmail);

            //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
            if (modelsUser == null || modelsUser.UserPassword != loginDto.Password)
            {
                return Unauthorized();
            }

            //Login suceed! now mark login in session memory!
            HttpContext.Session.SetString("loggedInUser", modelsUser.Email);

            DTO.User dtoUser = new DTO.User(modelsUser);
            dtoUser.UserImage = GetUserImageVirtualPath(dtoUser.Id);
            return Ok(dtoUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] DTO.RegisterInfo registerInfoDto)
    {
        try
        {

            HttpContext.Session.Clear();
            //Logout any previous login attempt
            //Checking if needed for new storage 
            Random randForCode = new Random();
            string Code = "";
            for (int i = 0; i < 5; i++)
            {
                int Rand = randForCode.Next(1, 10);
                Code += Convert.ToString(Rand);

            }
            Models.Storage? modelsStorage;
            Models.User modelsUser = registerInfoDto.UserInfo.GetModels();
            modelsUser.StorageId = null;
            context.Users.Add(modelsUser);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            modelsUser = context.GetUser(registerInfoDto.UserInfo.Email);



            if (!registerInfoDto.IsNewStorage)
            {
                // Find the Storage by Storage code 
                modelsStorage = context.GetStorageByCode(registerInfoDto.StorageCodeInfo);
                if (modelsStorage == null)
                {
                    return Unauthorized();
                }
            }
            else
            {
                //Create storage class
                modelsStorage = registerInfoDto.StorageInfo.GetModels();
                modelsStorage.StorageCode = Code;
                modelsStorage.Manager = modelsUser.Id;
                context.Storages.Add(modelsStorage);
                context.SaveChanges();
                modelsStorage = context.GetStorageByCode(Code);


            }
            modelsUser.StorageId = modelsStorage.Id;
            context.SaveChanges();

            //User and Storage were added!

            DTO.User dtoUser = new DTO.User(modelsUser);
            DTO.Storage dtoStorage = new DTO.Storage(modelsStorage);
            dtoUser.UserImage = GetUserImageVirtualPath(dtoUser.Id);
            DTO.RegisterInfo registerInfo = new RegisterInfo(dtoUser, dtoStorage,dtoStorage.StorageCode,true);
            if (Ok(dtoUser).StatusCode == 200 && Ok(dtoStorage).StatusCode == 200)
            {
                return Ok(registerInfo);
            }
            else { return Unauthorized(); }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpGet("getRecipes")]
    public IActionResult GetRecipes()
    {
        try
        {

            List<Models.Recipe> ModelsRecipes = new List<Models.Recipe>();
            List<DTO.Recipe> DTORecipes = new List<DTO.Recipe>();
            ModelsRecipes = context.GetAllRecipe();
            foreach (Models.Recipe recipe in ModelsRecipes)
            {
                DTORecipes.Add(new DTO.Recipe(recipe));
            }
            foreach(DTO.Recipe r in DTORecipes)
            {
                r.RecipeImage = GetRecipeImageVirtualPath(r.Id,r.RecipesName);
            }
            return Ok(DTORecipes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("getAllergys")]
    public IActionResult GetAllergys()
    {
        try
        {

            List<Models.Allergy> ModelsAllergies = new List<Models.Allergy>();
            List<DTO.Allergy> DTOAllergies = new List<DTO.Allergy>();
            ModelsAllergies = context.GetAllAllergy();
            foreach (Models.Allergy allergy in ModelsAllergies)
            {
                DTOAllergies.Add(new DTO.Allergy(allergy));
            }
            return Ok(DTOAllergies);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("getRecipesAmountByuser")]
    public IActionResult GetRecipesAmountByuser([FromBody] int userId)
    {
        try
        {
            List<Models.Recipe> ModelsRecipes = new List<Models.Recipe>();
            List<DTO.Recipe> DTORecipes = new List<DTO.Recipe>();
            ModelsRecipes = context.GetRecipesByUser(userId);
            int Amount = 0;
            if (ModelsRecipes == null)
            {
                return Ok(Amount);
            }
            foreach (Models.Recipe recipe in ModelsRecipes)
            {
                Amount++;
            }
            return Ok(Amount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getCommentsAmountByuser")]
    public IActionResult GetCommentsAmountByuser([FromBody] int userId)
    {
        try
        {
            List<Models.Comment> ModelsComments = new List<Models.Comment>();
            List<DTO.Comment> DTOComments = new List<DTO.Comment>();
            ModelsComments = context.GetCommentsByUser(userId);
            int Amount = 0;
            if (ModelsComments == null)
            {
                return Ok(Amount);
            }
            foreach (Models.Comment Comment in ModelsComments)
            {
                Amount++;
            }
            return Ok(Amount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getRatingsAmountByuser")]
    public IActionResult GetRatingsAmountByuser([FromBody] int userId)
    {
        try
        {
            List<Models.Rating> ModelsRatings = new List<Models.Rating>();
            List<DTO.Rating> DTORatings = new List<DTO.Rating>();
            ModelsRatings = context.GetRatingsByUser(userId);
            int Amount = 0;
            if (ModelsRatings == null)
            {
                return Ok(Amount);
            }
            foreach (Models.Rating Rating in ModelsRatings)
            {
                Amount++;
            }
            return Ok(Amount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getLevelsByRecipe")]
    public IActionResult GetLevelsByRecipe([FromBody] int recipeId)
    {
        try
        {

            List<Models.Level> ModelsLevels = new List<Models.Level>();
            List<DTO.Level> DTOLevels = new List<DTO.Level>();
            ModelsLevels = context.GetLevelByRecipe(recipeId);
            foreach (Models.Level level in ModelsLevels)
            {
                DTOLevels.Add(new DTO.Level(level));
            }
            return Ok(DTOLevels);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getUsers")]
    public IActionResult GetUsers()
    {
        try
        {

            List<Models.User> ModelsUsers = new List<Models.User>();
            List<DTO.User> DTOUsers = new List<DTO.User>();
            ModelsUsers = context.GetAllUser();
            foreach (Models.User user in ModelsUsers)
            {
                DTOUsers.Add(new DTO.User(user));
            }
            foreach(DTO.User u in DTOUsers)
            {
                u.UserImage = GetUserImageVirtualPath(u.Id);
            }
            return Ok(DTOUsers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getAllIngredients")]
    public IActionResult GetAllIngredients()
    {
        try
        {

            List<Models.Ingredient> ModelsIngredients = new List<Models.Ingredient>();
            List<DTO.Ingredient> DTOIngredients = new List<DTO.Ingredient>();
            ModelsIngredients = context.GetAllIngredient();
            foreach (Models.Ingredient ingredient in ModelsIngredients)
            {
                DTOIngredients.Add(new DTO.Ingredient(ingredient));
            }
            foreach(DTO.Ingredient i in DTOIngredients)
            {
                i.IngredientImage = GetIngredientImageVirtualPath(i.IngredientName);
            }
            return Ok(DTOIngredients);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getIngredientsByRecipe")]
    public IActionResult GetIngredientsByRecipe([FromBody] int recipeId)
    {
        try
        {

            List<Models.Ingredient> ModelsIngredients = new List<Models.Ingredient>();
            List<DTO.Ingredient> DTOIngredients = new List<DTO.Ingredient>();
            ModelsIngredients = context.GetIngredientByRecipe(recipeId);
            foreach (Models.Ingredient ingredient in ModelsIngredients)
            {
                DTOIngredients.Add(new DTO.Ingredient(ingredient));
            }
            foreach (DTO.Ingredient i in DTOIngredients)
            {
                i.IngredientImage = GetIngredientImageVirtualPath(i.IngredientName);
            }
            return Ok(DTOIngredients);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("getIngredientsByBarcode")]
    public IActionResult GetIngredientsByBarcode([FromBody] string barcode)
    {
        try
        {
            Models.Ingredient? ModelsIngredient = context.GetIngredientByBarcode(barcode);
            if(ModelsIngredient== null)
            {
                return Ok(null);
            }
            DTO.Ingredient? ingredientDTO = new DTO.Ingredient(ModelsIngredient);
            ingredientDTO.IngredientImage = GetIngredientImageVirtualPath(ingredientDTO.IngredientName);
            return Ok(ingredientDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getIngredientRecipesByRecipe")]
    public IActionResult GetIngredientRecipesByRecipe([FromBody] int recipeId)
    {
        try
        {

            List<Models.Ingredient> ModelsIngredients = new List<Models.Ingredient>();
            List<Models.IngredientRecipe> ModelIngredientRecipes = new List<Models.IngredientRecipe>();
            List<DTO.IngredientRecipe> DTOIngredientRecipes = new List<DTO.IngredientRecipe>();
            ModelsIngredients = context.GetIngredientByRecipe(recipeId);
            ModelIngredientRecipes = context.GetIngredientRecipeByRecipe(recipeId, ModelsIngredients);
            foreach (Models.IngredientRecipe ingredientrecipe in ModelIngredientRecipes)
            {
                DTOIngredientRecipes.Add(new DTO.IngredientRecipe(ingredientrecipe));
            }
            return Ok(DTOIngredientRecipes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getIngredientsByStorage")]
    public IActionResult getIngredientsByStorage([FromBody] int storageId)
    {
        try
        {
            List<DTO.Ingredient> DTOIngredients = new List<DTO.Ingredient>();
            Models.Storage storage = context.GetStorageById(storageId);
            foreach (Models.Ingredient i in storage.Ingredients)
            {
                DTOIngredients.Add(new DTO.Ingredient(i));
            }
            foreach (DTO.Ingredient i in DTOIngredients)
            {
                i.IngredientImage = GetIngredientImageVirtualPath(i.IngredientName);
            }
            return Ok(DTOIngredients);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("getUsersbyStorage")]
    public IActionResult GetUsersbyStorage([FromBody] int userId)
    {
        try
        {

            List<Models.User> ModelsUsers = new List<Models.User>();
            List<DTO.User> DTOUsers = new List<DTO.User>();
            Models.User user = context.GetUserById(userId);
            Models.Storage storage = context.GetStorageById(user.StorageId);
            if (storage == null)
            {
                return Ok(new List<DTO.User>());
            }
            Models.User StorageAdmin = context.GetAdminByStorage(storage,storage.Id);
            ModelsUsers = context.GetUsersByStorage(user.StorageId);
            DTOUsers.Add(new DTO.User(StorageAdmin));
            foreach (Models.User u in ModelsUsers)
            {
                if(u.Id != storage.Manager)
                {
                    DTOUsers.Add(new DTO.User(u));
                }
            }
            foreach (DTO.User u2 in DTOUsers)
            {
                u2.UserImage = GetUserImageVirtualPath(u2.Id);
            }
            return Ok(DTOUsers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("getRatingbyRecipe")]
    public IActionResult GetRatingbyRecipe([FromBody] int recipeId)
    {
        try
        {
            Models.Recipe ModelsRecipe = context.GetRecipeById(recipeId);
            int rating = ModelsRecipe.Rating;
            return Ok(rating);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("getAllergysbyUser")]
    public IActionResult GetAllergysbyUser([FromBody] int userId)
    {
        try
        {
            List<Models.Allergy> ModelsAllergys = new List<Models.Allergy>();
            ModelsAllergys = context.GetAllergiesByUser(userId);
            if(ModelsAllergys == null)
            {
                ModelsAllergys = new List<Models.Allergy>();
            }
            List<DTO.Allergy> DTOAllergies = new List<DTO.Allergy>();
            foreach (Models.Allergy a in ModelsAllergys)
            {
                DTOAllergies.Add(new DTO.Allergy(a));
            }
            return Ok(DTOAllergies);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("changeName")]
    public IActionResult ChangeName([FromBody]  DTO.User newUser)
    {
        try
        {
            Models.User user = context.GetUserById(newUser.Id);
            user.UserName = newUser.UserName;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("changeMail")]
    public IActionResult ChangeMail([FromBody] DTO.User newUser)
    {
        try
        {

            Models.User user = context.GetUserById(newUser.Id);
            user.Email = newUser.Email;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("changeStorageName")]
    public IActionResult ChangeStorageName([FromBody] DTO.Storage newStorage)
    {
        try
        {

            Models.Storage storage = context.GetStorageByStorage(newStorage.Id);
            storage.StorageName = newStorage.StorageName;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("removeStorageMember")]
    public IActionResult RemoveStorageMember([FromBody] int userId)
    {
        try
        {
            Models.User user1 = context.GetUserById(userId);
            user1.StorageId = null;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("removeStorageIngredient")]
    public IActionResult RemoveStorageIngredient([FromBody] int ingredientId, [FromQuery] int storageId)
    {
        try
        {
            Models.Storage storage = context.GetStorageById(storageId);
            foreach (Models.Ingredient i in storage.Ingredients)
            {
                if(i.Id == ingredientId)
                {
                    storage.Ingredients.Remove(i);
                    context.SaveChanges();
                    return Ok();
                }
            }
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("removeStorageIngredients")]
    public IActionResult RemoveStorageIngredients([FromBody] List<DTO.Ingredient> ingredients, [FromQuery] int storageId)
    {
        try
        {
            Models.Storage storage = context.GetStorageById(storageId);
            List<Models.Ingredient> ingredientsModels = new List<Models.Ingredient>(); 
            Models.Ingredient ingredient = null;
            foreach(DTO.Ingredient i in ingredients)
            {
                ingredientsModels.Add(i.GetModels());
            }
            foreach (Models.Ingredient i in ingredientsModels)
            {
                ingredient =  storage.Ingredients.Where(si => si.Id == i.Id).FirstOrDefault();
                storage.Ingredients.Remove(ingredient);
                context.SaveChanges();
            }
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("addIngredietToStorage")]
    public IActionResult AddIngredietToStorage([FromBody] int ingredientId, [FromQuery] int storageId)
    {
        try
        {
            Models.Storage storage = context.GetStorageById(storageId);
            Models.Ingredient ingredient = context.GetIngredientById(ingredientId);
            storage.Ingredients.Add(ingredient);
            context.SaveChanges();
            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("changeManager")]
    public IActionResult ChangeManager([FromBody] int userId)
    {
        try
        {
            Models.User user = context.GetUserById(userId);
            Models.Storage storage = context.GetStorageById(user.StorageId);
            storage.Manager = userId;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("deleteStorage")]
    public IActionResult DeleteStorage([FromBody] DTO.Storage storage)
    {
        try
        {
            Models.Storage storage1 = context.GetStorageById(storage.Id);
            context.Storages.Remove(storage1);
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("getStorageByUser")]
    public IActionResult GetStorageByuser([FromBody] int userId)
    {
        try
        {
            Models.User user = context.GetUserById(userId);
            Models.Storage? storage = context.GetStorageById(user.StorageId);
            DTO.Storage storage1 = new DTO.Storage(storage);
            return Ok(storage1);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("updateUser")]
    public IActionResult UpdateUser([FromBody] DTO.User user)
    {
        try
        {
            Models.User updateUser = context.GetUserById(user.Id);
            updateUser.UserName = user.UserName;
            updateUser.Email = user.Email;
            updateUser.UserImage = user.UserImage;
            updateUser.IsKohser = user.IsKohser;
            updateUser.UserPassword = user.UserPassword;
            updateUser.UserName = user.UserName;
            updateUser.Vegetarianism = user.Vegetarianism;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("saveRecipe")]
    public IActionResult SaveRecipe([FromBody] DTO.SaveRecipeInfo saveRecipeInfo)
    {
        try
        {
            Models.Recipe recipeModels = saveRecipeInfo.RecipeInfo.GetModels();
            context.SetAllergies(recipeModels,saveRecipeInfo.RecipeInfo.Allergies);
            context.Recipes.Add(recipeModels);
            context.SaveChanges();
            DTO.IngredientRecipe ingredientRecipeDto = new DTO.IngredientRecipe();
            foreach (DTO.IngredientRecipe i in saveRecipeInfo.IngredientsInfo)
            {
                i.RecipeId = recipeModels.Id;
                Models.IngredientRecipe recipeIngredientModels = i.GetModels();
                recipeIngredientModels.Recipe = recipeModels;
                recipeIngredientModels.Ingredient = context.GetIngredientById(recipeIngredientModels.IngredientId);
                context.IngredientRecipes.Add(recipeIngredientModels);
                context.SaveChanges();
            }
            foreach (DTO.Level l in saveRecipeInfo.LevelsInfo)
            {
                l.RecipeId = recipeModels.Id;
                Models.Level levelModels = l.GetModels();
                levelModels.Id = 0;
                context.Levels.Add(levelModels);
                context.SaveChanges();

            }
            saveRecipeInfo.RecipeInfo.RecipeImage = GetUserImageVirtualPath(saveRecipeInfo.RecipeInfo.Id);
            return Ok(saveRecipeInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("saveAllergy")]
    public IActionResult saveAllergy([FromBody] List<DTO.Allergy> allergies, [FromQuery] int userId)
    {
        try
        {
            List<Models.Allergy> allergyUsers = new List<Models.Allergy>();
            Models.User user = context.GetUserById(userId);
            foreach (DTO.Allergy a in allergies)
            {
                Models.Allergy au = context.Allergies.Where(x => x.Id == a.Id).FirstOrDefault();
                allergyUsers.Add(au);
            }
            user.Allergies = allergyUsers;
            context.Users.Update(user);
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("saveIngredient")]
    public IActionResult SaveIngredient([FromBody] DTO.Ingredient newIngredient, [FromQuery] int storageId)
    {
        try
        {
            Models.Ingredient modelIngredient = newIngredient.GetModels();
            context.Ingredients.Add(modelIngredient);
            context.SaveChanges();
            Models.Storage storage = context.GetStorageById(storageId);
            storage.Ingredients.Add(modelIngredient);
            context.SaveChanges();
            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("saveRating")]
    public IActionResult SaveRating([FromBody] DTO.Rating newRating)
    {
        try
        {
            Models.Rating modelRating = new Models.Rating();
            if (context.GetRateByRecipeAndUser(newRating.RecipeId, newRating.UserId) == null)
            {
               modelRating = newRating.GetModels();
               context.Ratings.Add(modelRating);
               context.SaveChanges();
            }
            else
            {
                modelRating = context.GetRateByRecipeAndUser(newRating.RecipeId,newRating.UserId);
                modelRating.Rate = newRating.Rate;
                context.SaveChanges();
            }
            Models.Recipe recipe = context.GetRecipeById(modelRating.RecipeId);
            List <Models.Rating> ratings = context.GetRatingByRecipe(newRating.RecipeId);
            double recipeRatings = 0;
            foreach(Models.Rating r in ratings)
            {
                recipeRatings = recipeRatings + r.Rate;
            }
            recipeRatings = recipeRatings / ratings.Count;
            recipe.Rating = (int)recipeRatings;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("saveNewStorage")]
    public IActionResult SaveNewStorage([FromBody] DTO.Storage newStorage)
    {
        try
        {
            Random randForCode = new Random();
            string Code = "";
            for (int i = 0; i < 5; i++)
            {
                int Rand = randForCode.Next(1, 10);
                Code += Convert.ToString(Rand);
            }
            Models.Storage? modelsStorage = new Models.Storage();
            modelsStorage = newStorage.GetModels();
            modelsStorage.StorageCode = Code;
            modelsStorage.Manager = newStorage.Manager;
            context.Storages.Add(modelsStorage);
            context.SaveChanges();
            Models.User modelsUser = context.GetUserById(newStorage.Manager);
            modelsUser.StorageId = modelsStorage.Id;
            context.SaveChanges();
            return Ok(modelsStorage.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //[HttpPost("addToStorageByCode")]
    //public IActionResult AddToStorageByCode([FromBody] string storageCode)
    //{
    //    try
    //    {
    //        return Ok();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    #region images
    private static bool IsImage(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        List<string> jpg = new List<string> { "FF", "D8" };
        List<string> bmp = new List<string> { "42", "4D" };
        List<string> gif = new List<string> { "47", "49", "46" };
        List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
        List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

        List<string> bytesIterated = new List<string>();

        for (int i = 0; i < 8; i++)
        {
            string bit = stream.ReadByte().ToString("X2");
            bytesIterated.Add(bit);

            bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
            if (isImage)
            {
                return true;
            }
        }

        return false;
    }
    

    private string GetRecipeImageVirtualPath(int recipeId, string recipeName)
    {
        string virtualPath = $"/recipeImages/{recipeId}_{recipeName}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\recipeImages\\{recipeId}_{recipeName}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\recipeImages\\{recipeId}_{recipeName}.jpg";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".jpg";
            }
            else
            {
                virtualPath = $"/recipeImages/default.png";
            }
        }

        return virtualPath;
    }

    [HttpPost("uploadRecipeImage")]
    public async Task<IActionResult> UploadRecipeImage(IFormFile file, [FromQuery] string recipeName, [FromQuery] int madeBy)
    {
        //Read all files sent
        long imagesSize = 0;
        try
        {
            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\recipeImages\\{madeBy}_{recipeName}{extention}";
                string virtualFilePath = $"/recipeImages/{madeBy}_{recipeName}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                        return BadRequest("File sent is not an image");
                    }

                }

                return Ok(virtualFilePath);

            }

            return BadRequest("File in size 0");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    private string GetUserImageVirtualPath(int userId)
    {
        string virtualPath = $"/userImages/{userId}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\userImages\\{userId}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\userImages\\{userId}.jpg";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".jpg";
            }
            else
            {
                virtualPath = $"/userImages/default.png";
            }
        }

        return virtualPath;
    }

    [HttpPost("uploadUserImage")]
    public async Task<IActionResult> UploadUserImage(IFormFile file, [FromQuery] string userId)
    {
        //Read all files sent
        long imagesSize = 0;
        try
        {
            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\userImages\\{userId}{extention}";
                string virtualFilePath = $"/userImages/{userId}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                        return BadRequest("File sent is not an image");
                    }

                }

                return Ok(virtualFilePath);

            }

            return BadRequest("File in size 0");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    private string GetIngredientImageVirtualPath(string ingredientName)
    {
        string virtualPath = $"/ingredientImages/{ingredientName}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\ingredientImages\\{ingredientName}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\ingredientImages\\{ingredientName}.jpg";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".jpg";
            }
            else
            {
                virtualPath = $"/ingredientImages/default.png";
            }
        }

        return virtualPath;
    }

    [HttpPost("uploadIngredientImage")]
    public async Task<IActionResult> UploadIngredientImage(IFormFile file, [FromQuery] string IngredientName)
    {
        //Read all files sent
        long imagesSize = 0;
        try
        {
            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\IngredientImages\\{IngredientName}{extention}";
                string virtualFilePath = $"/IngredientImages/{IngredientName}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                        return BadRequest("File sent is not an image");
                    }

                }

                return Ok(virtualFilePath);

            }

            return BadRequest("File in size 0");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    #endregion
}

