using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;
        private readonly IUserService _userService;


        public UnitsController(IUnitService unitService, IUserService userService)
        {
            _unitService = unitService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool adminOnly = true, bool flat = true) {

            var user = await _userService.GetCurrentUserAsync(HttpContext);
            IEnumerable<UnitDTO> units;

            if (flat)
            {
                units = await _unitService.GetAllAsync(user.Id);
            }
            else
            {
                if (adminOnly)
                {
                    units = await _unitService.GetAdminRoleUnits(user.Id);
                }
                else
                {
                    units = await _userService.GetRelevantUnits(user.Id);
                }
            }
            return Ok(units);
        }

        [HttpGet ("public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicUnits()
        {
            var units = await _unitService.GetPublicUnitsAsync();
            if (units.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(units);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var found = await _unitService.GetParentsById(id);
            if (found == null) {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(UnitDTO unit)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            var added = await _unitService.AddAsync(unit, user.Id);

            if (added == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UnitDTO unit)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);

            var updated = await _unitService.UpdateAsync(unit, user.Id);
            if (updated == null)
            {
                return BadRequest();
            }
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);

            await _unitService.DeleteAsync(id, user.Id);
            return Ok();
        }
    }
}
