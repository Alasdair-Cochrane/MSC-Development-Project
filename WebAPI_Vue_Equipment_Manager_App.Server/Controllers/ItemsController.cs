﻿using BrunoZell.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Migrations;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemSerivce;
        private readonly ILogger<ItemsController> _logger;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;

        public ItemsController(IItemService itemSerivce, ILogger<ItemsController> logger, IImageService imageService, IUserService userService)
        {
            _itemSerivce = itemSerivce;
            _logger = logger;
            _imageService = imageService;
            _userService = userService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var items = await _itemSerivce.GetAllAsync();
        //    return Ok(items);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _itemSerivce.GetByIdAsync(id);
            if (item != null)
            {
                return Ok(item);
            }
            return NotFound();
        }
        
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]ItemQuery query)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);

            var results =  await _itemSerivce.Search(query, user.Id);
            if(results != null)
            {
                return Ok(results);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Insert([FromForm]ItemPostDTO item, IFormFile? image)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            
            if(image != null)
            {
                item.ImageURL = await UploadImage(image);
            }
            else
            {
            }
            if (!ModelState.IsValid) return BadRequest();
            var added = await _itemSerivce.AddAsync(item.ToItemDTOFromPost(), user.Id);
            if (added != null)
            {
                return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
            }
            return StatusCode(500);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ItemDTO item)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);

            var updated = await _itemSerivce.UpdateAsync(item, user.Id);
            if (updated != null)
            {
                return Ok(updated);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] ItemDTO item)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            await _itemSerivce.DeleteAsync(item, user.Id);
            return Ok();
        }

        private async Task<string?> UploadImage(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
         
            string fileName = Guid.NewGuid().ToString() + extension;

            try
            {
                await _imageService.Upload(file, fileName);
                var uri = Url.Link("GetImage", new {url = fileName});
                return uri;
            }
            catch (Exception ex)
            {
                throw new ImageUploadException("Failed to add Image", ex);
            }
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImageById(int id)
        {
            string? url = await _itemSerivce.GetImageUrl(id);
            if(url == null) return NotFound();
            var image = await _imageService.Retrieve(url);

            if(image == null) return NotFound();

            var returnFile = File(image, "image/jpeg");
            return Ok(returnFile);
        }

        [HttpGet]
        [Route("/image/{url}", Name="GetImage")]
        public async Task<IActionResult> GetImageByUrl(string url)
        {
            var image = await _imageService.Retrieve(url);
            
            if(image == null) return NotFound();
            
             return File(image, Image.Jpeg);
        }

        [HttpGet("csv")]
        public async Task<IActionResult> GetCsvExport([FromQuery]IEnumerable<int>? unitIds)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);

            var csvStream = await _itemSerivce.GetExport(user.Id, unitIds);

            return File(csvStream, "application/octet-stream", "Equipment.csv");
        }

    }
}



