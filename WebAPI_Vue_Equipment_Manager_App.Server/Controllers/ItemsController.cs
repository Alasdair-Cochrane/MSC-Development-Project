using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemSerivce;

        public ItemsController(IItemService itemSerivce)
        {
            _itemSerivce = itemSerivce;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemSerivce.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _itemSerivce.GetByIdAsync(id);
            if (item != null)
            {
                return Ok(item);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ItemDTO item)
        {
            if (!ModelState.IsValid) return BadRequest(item);
            Debug.WriteLine(JsonSerializer.Serialize(item));
            var added = await _itemSerivce.AddAsync(item);
            if (added != null)
            {
                return CreatedAtAction(nameof(Get), new { id = item.Id }, added);
            }
            return BadRequest();

        }

        [HttpPut]
        public async Task<IActionResult> Update(ItemDTO item)
        {
            var updated = await _itemSerivce.UpdateAsync(item);
            if (updated != null)
            {
                return Ok(updated);
            }
            return BadRequest();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemSerivce.DeleteByIdAsync(id);
            return Ok();
        }
    }
}
