using BrunoZell.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
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

        public ItemsController(IItemService itemSerivce, ILogger<ItemsController> logger, IImageService imageService)
        {
            _itemSerivce = itemSerivce;
            _logger = logger;
            _imageService = imageService;
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
            var results =  await _itemSerivce.Search(query);
            if(results != null)
            {
                return Ok(results);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Insert([FromForm]ItemPostDTO item, IFormFile? image)
        {
            
            if(image != null)
            {
                item.ImageURL = await UploadImage(image);
            }
            else
            {
            }

            if (!ModelState.IsValid) return BadRequest();
            var added = await _itemSerivce.AddAsync(item.ToItemDTOFromPost());
            if (added != null)
            {
                return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
            }
            return StatusCode(500);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemSerivce.DeleteByIdAsync(id);
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

    }
}



