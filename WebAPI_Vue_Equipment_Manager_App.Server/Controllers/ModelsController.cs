﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Add(EquipmentModelDTO model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var newModel = _modelService.Add(model);
            return CreatedAtAction(nameof(Get), new { id = newModel.Id }, newModel);
        }

        [HttpPut]
        public async Task<IActionResult> Update(EquipmentModelDTO model)
        {
            var updated = await _modelService.UpdateAsync(model);
            return Ok(updated);
        }


    }
}
