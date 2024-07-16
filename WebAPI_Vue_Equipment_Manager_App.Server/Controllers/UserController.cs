using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

//https://www.reddit.com/r/dotnet/comments/18e2dto/using_custom_user_model_with_net_8_identity_api/
namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO newUser)
        {
            var user = new User {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = newUser.Email,            
            };
            
           var result = await _userManager.CreateAsync(user, newUser.Password);
            if(result.Succeeded)
            {
                return Ok(new { Message = "Registration Successful" });
            }
            return BadRequest( new { Errors = result.Errors});
        }

        [HttpGet()]
        public async Task<IActionResult> GetUserDetails()
        {
            return Ok();
        }

        [HttpGet("assignments")]
        public async Task<IActionResult> GetAssignments(IEnumerable<int> userIDs)
        {
            return Ok();
        }

       
    }
}
