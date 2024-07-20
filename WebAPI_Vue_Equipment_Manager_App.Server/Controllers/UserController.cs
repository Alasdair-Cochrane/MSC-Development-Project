using Google.Api.Gax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Migrations;

//https://www.reddit.com/r/dotnet/comments/18e2dto/using_custom_user_model_with_net_8_identity_api/
namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IUnitService _unitService;

        public UsersController(SignInManager<User> signInManager, UserManager<User> userManager, IUserService userService, IUnitService unitService)
        {
            _userManager = userManager;
            _userService = userService;
            _unitService = unitService;
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

            if (!result.Succeeded)
            {
                return BadRequest(new { result.Errors });
            }

            //if the user has not chosen or created a assoicated Unit then just return
            if(newUser.Organisation == null)
            {
                return Ok();
            }
            user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user == null)
            {
                throw new DataInsertionException($"New User ID could not be found from email : {newUser.Email}");
            }

            try
            {

                //get the new user's chosen root unit
                var unit = newUser.Organisation;
                //if the chosen unit already exists - and the ID matches existing unit- then assing the user as public
                if (unit.Id != null)
                {
                    var foundUnit = await _unitService.GetAsync(unit.Id.Value);
                    if (foundUnit == null)
                    {
                        throw new DataInsertionException("Selected Unit ID does not match any current entry");
                    }
                    await _userService.AssignUserPublicOnCreate(user.Id, unit.Id.Value);
                }
                //check that the unit does not already exists by Name and if not create the unit then assign new user as admin
                else
                {
                    var exstingUnit = await _unitService.FindByName(unit.Name);
                    if (exstingUnit != null)
                    {
                        throw new DataInsertionException($"Unit with name {exstingUnit.Name} already exists");
                    }
                    await _unitService.AddAsync(newUser.Organisation, user.Id);
                }

                return Ok(new { FirstName = user.FirstName, LastName = user.LastName, Unit = newUser.Organisation.Name ?? "" });
            }
            catch(Exception ex)
            {
                await _userManager.DeleteAsync(user);
                throw;
            }

        }



        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> OnLogIn()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new UnauthorisedOperationException("Current User could not be found from request context\"");
            }
            var dto = await _userService.GetUserDetailsAsync(user);

            return Ok(dto);
        }

        [HttpPost("assignments")]
        public async Task<IActionResult> AssignUser(AssignmentDTO assignment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new UnauthorisedOperationException("Current User could not be found from request context");
            }
            var created = await _userService.AssignUser(user.Id, assignment.UserId, assignment.RoleId, assignment.UnitId);

            return Ok();
        }

       
    }
}
