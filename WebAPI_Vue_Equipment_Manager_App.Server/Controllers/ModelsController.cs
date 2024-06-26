using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly IEquipmentModelService _modelService;

        public ModelsController(IEquipmentModelService modelService) {
            _modelService = modelService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var element = await _modelService.GetByiDAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            return Ok(element);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _modelService.GetAllAsync();
            return Ok(results);
        }


        [HttpPost]
        public async Task<IActionResult> Add(EquipmentModelDTO model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var newModel = await _modelService.AddAsync(model);
            if(newModel == null) {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = newModel.Id }, newModel);
        }

        [HttpPut]
        public async Task<IActionResult> Update(EquipmentModelDTO model)
        {
            var updated = await _modelService.UpdateAsync(model);
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _modelService.DeleteAsync(id);
            return Ok();
        }


    }
}
