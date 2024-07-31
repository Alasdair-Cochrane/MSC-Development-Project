using BrunoZell.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
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
        private readonly IDocumentService _documentService;
        public ItemsController(IItemService itemSerivce, ILogger<ItemsController> logger, 
            IImageService imageService, IUserService userService, IDocumentService documentService)
        {
            _itemSerivce = itemSerivce;
            _logger = logger;
            _imageService = imageService;
            _userService = userService;
            _documentService = documentService;
        }

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
            if(string.IsNullOrEmpty(extension))
            {
                extension = ".jpeg";
            };
         
            string fileName = Guid.NewGuid().ToString() + extension;

            try
            {
                await _imageService.Upload(file, fileName);
                var url = Url.Link("GetImage", new {url = fileName});
                return url;
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

        [HttpPost("{id}/image")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile image)
        {
            var url = await UploadImage(image);
            if(url == null) return StatusCode(500);
            await _itemSerivce.UpdateImageUrl(id, url);
            return Ok(url);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/image/{url}", Name="GetImage")]
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

        [HttpPost("{id}/documents")]
        public async Task<IActionResult> UploadItemDocument(IFormFile file, int id)
        {

            string ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf")
            {
                return BadRequest();
            }
            string uri = $"I-{id}-" + Guid.NewGuid().ToString() + ext;

            var document = new Document { FileName = file.FileName, URL = uri };
            if (await _documentService.ValidateAsync(file))
            {
                 await _documentService.UploadAsync(file, uri);
            }
            else
            {
                return BadRequest("Document Invalid");

            }
            try
            {
                var itemDoc = await  _itemSerivce.CreateItemDocumentAsync(document,id);
                return Ok(itemDoc);

            }
            catch
            {
                await _documentService.RemoveAsync(uri, false);
                throw;
            }
        }

        [HttpGet("statuses/names")]
        public async Task<IActionResult> GetStatusNames()
        {
            var statuses = await _itemSerivce.GetStatusCategoryNames();
           
            if (statuses.IsNullOrEmpty())
            {
                return StatusCode(500);
            }
            else
            {
                return Ok(statuses);
            }
        }
        [HttpGet("statuses")]
        public async Task<IActionResult> GetAllItemsByStatus()
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);

            var list = await _itemSerivce.GetQuantityByStatusAsync(user.Id);
            return Ok(list);
        }

    }
}



