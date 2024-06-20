using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _maintenanceService.GetAllAsync();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var found = await _maintenanceService.GetAsync(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }
        [HttpPut]
        public async Task<IActionResult> Update(MaintenanceDTO entry)
        {
            var updated = await _maintenanceService.UpdateAsync(entry);
            if (updated == null)
            {
                return BadRequest();
            }
            return Ok(updated);
        }
        [HttpPost]
        public async Task<IActionResult> Insert(MaintenanceDTO entry)
        {
            var added = await _maintenanceService.AddAsync(entry);
            if (added == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _maintenanceService.DeleteAsync(id);
            return Ok();
        }

    }
}
