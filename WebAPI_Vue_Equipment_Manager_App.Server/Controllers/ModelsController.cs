using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    //Used only for autocomplete data when filling in forms

    public class ModelsController : ControllerBase
    {
        private readonly IEquipmentModelService _modelService;
        private readonly IUserService _userService;

        public ModelsController(IEquipmentModelService modelService, IUserService userService)
        {
            _modelService = modelService;
            _userService = userService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _modelService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);

            var categories = await _modelService.GetCategoriesAsync(user.Id);
            if(categories.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(categories);
        }

    }
}
