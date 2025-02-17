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

    [HttpPost("getStorageByUser")]
    public IActionResult GetStorageByuser([FromBody] int storageId)
    {
        try
        {
            Models.Storage storage = context.GetStorageByStorage(storageId);
            DTO.Storage storage1 = new DTO.Storage(storage);
            return Ok(storage1);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

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

    private string GetRecipeImageVirtualPath(int userId, string recipeName)
    {
        string virtualPath = $"/recipeImages/{userId}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\recipeImages\\{userId}_{recipeName}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\recipeImages\\{userId}_{recipeName}.jpg";
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
    public async Task<IActionResult> UploadUserImage(IFormFile file, [FromQuery] string UserId)
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
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\userImages\\{UserId}{extention}";
                string virtualFilePath = $"/userImages/{UserId}{extention}";

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
    private string GetIngredientImageVirtualPath(int ingredientId)
    {
        string virtualPath = $"/ingredientImages/{ingredientId}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\ingredientImages\\{ingredientId}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\ingredientImages\\{ingredientId}.jpg";
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

