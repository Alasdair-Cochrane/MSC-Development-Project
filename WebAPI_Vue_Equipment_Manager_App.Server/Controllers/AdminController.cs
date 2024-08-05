using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IItemService _itemService;

        public AdminController(IItemService itemService)
        {
            _itemService = itemService;
        }
        [HttpPost]
        public async  Task<IActionResult> Insert(IEnumerable<ItemPostDTO> items)
        {
           var result = await _itemService.AddManyAsync(items.Select(x => x.ToItemDTOFromPost()));
            return Ok(result);
        }
    }
}
