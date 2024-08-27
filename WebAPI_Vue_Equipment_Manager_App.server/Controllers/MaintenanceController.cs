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
    [Route("api/items/[controller]")]
    [Authorize]

    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IDocumentService _documentService;
        private readonly IUserService _userService;

        public MaintenanceController(IMaintenanceService maintenanceService, IDocumentService documentService, IUserService userService)
        {
            _maintenanceService = maintenanceService;
            _documentService = documentService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]int daysBeforeNow)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            var list = await _maintenanceService.GetAllAsync(daysBeforeNow, user.Id);
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
        [HttpGet]
        [Route("~/api/items/{id}/maintenance")]
        public async Task<IActionResult> GetMaintenanceForItem(int id)
        {
            var list = await _maintenanceService.GetAllForItemAsync(id);
            if (list.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(list);
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
        public async Task<IActionResult> Insert([FromBody] MaintenanceDTO entry)
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
            return NoContent();
        }

        [HttpGet("categories/names")]
        public async Task<IActionResult> GetCategoryNames()
        {
            var names = await _maintenanceService.GetCategoryNamesAsync();
            if (names.IsNullOrEmpty())
            {
                return StatusCode(500);
            }
            return Ok(names);
        }
        [HttpPost("{id}/documents")]
        public async Task<IActionResult> AddFile(IFormFile file, int id)
        {
            string ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf")
            {
                return BadRequest();
            }
            //create uri
            string uri = $"M-{id}-" + Guid.NewGuid().ToString() + ext;

            //create document metadata for database
            var document = new Document { FileName = file.FileName, URL = uri };

            //file validation not implemented - returns true - for virus scanning etc.
            if (await _documentService.ValidateAsync(file))
            {
                await _documentService.UploadAsync(file, uri);
            }
            else
            {
                return BadRequest("Document Invalid");

            }

            //will upload document and insert record into database
            //if upload fails it will remove database record
            try
            {
                var itemDoc = await _maintenanceService.CreateDocumentAsync(document, id);
                return Ok(itemDoc);

            }
            catch
            {
                await _documentService.RemoveAsync(uri, false);
                throw;
            }

        }

        [HttpGet("{id}/documents")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var result = await _maintenanceService.GetDocumentsAsync(id);

            return Ok(result);
        }



    }
}
