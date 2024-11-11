using Microsoft.AspNetCore.Mvc;
using RecipesAppServer.Models;
using RecipesAppServer.DTO;
using System.Text.Json;

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
            modelsUser.StorageId = 1;
            context.Users.Add(modelsUser);
            context.SaveChanges();
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

