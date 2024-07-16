using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {

            var list = await _unitService.GetAllAsync();
            return Ok(list);
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
            var added = await _unitService.AddAsync(unit);
            if (added == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UnitDTO unit)
        {
            var updated = await _unitService.UpdateAsync(unit);
            if (updated == null)
            {
                return BadRequest();
            }
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitService.DeleteAsync(id);
            return Ok();
        }
    }
}
