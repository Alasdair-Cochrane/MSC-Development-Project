using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelsController(IModelService modelService) {
            _modelService = modelService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var element = _modelService.GetByiD(id);
            if (element == null)
            {
                return NotFound();
            }
            return Ok(element);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_modelService.GetAll());
        }


        [HttpPost]
        public IActionResult Add(EquipmentModelDTO model)
        {
            var newModel = _modelService.Add(model);
            return CreatedAtAction(nameof(Get), new { id = newModel.Id }, newModel);
        }

        [HttpPut]
        public IActionResult Update(EquipmentModelDTO model)
        {
            return Ok(_modelService.Update(model));
        }


    }
}
