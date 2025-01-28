using Microsoft.AspNetCore.Mvc;
using RecipesAppServer.Models;
using RecipesAppServer.DTO;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Threading;

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
            dtoUser.UserImage = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("register")]
    public IActionResult Register([FromBody]DTO.RegisterInfo registerInfoDto)
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
                modelsStorage = context.GetStorage(registerInfoDto.StorageCodeInfo);
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
                modelsStorage = context.GetStorage(Code);
                
                
            }
            modelsUser.StorageId = modelsStorage.Id;
            context.SaveChanges();

            //User and Storage were added!
            
            DTO.User dtoUser = new DTO.User(modelsUser);
            DTO.Storage dtoStorage = new DTO.Storage(modelsStorage);
            dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            if (Ok(dtoUser).StatusCode == 200 && Ok(dtoStorage).StatusCode == 200)
            {
                return Ok();
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
            foreach(Models.Recipe recipe in ModelsRecipes)
            {
                DTORecipes.Add(new DTO.Recipe(recipe));
            }
            return Ok(DTORecipes);
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
            return Ok(DTOUsers);
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

    [HttpPost("removeStorageMember")]
    public IActionResult RemoveStorageMember([FromBody] DTO.User user)
    {
        try
        {
            Models.User user1 = context.GetUserById(user.Id);
            user.StorageId = 9999;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    //this function check which profile image exist and return the virtual path of it.
    //if it does not exist it returns the default profile image virtual path
    private string GetProfileImageVirtualPath(int userId)
    {
        string virtualPath = $"/profileImages/{userId}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.jpg";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".jpg";
            }
            else
            {
                virtualPath = $"/profileImages/default.png";
            }
        }

        return virtualPath;
    }



}

