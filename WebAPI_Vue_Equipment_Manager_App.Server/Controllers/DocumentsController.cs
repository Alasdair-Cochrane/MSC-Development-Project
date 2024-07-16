using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService) { 
            
            _documentService = documentService;        
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UploadItemDocument(IFormFile file, int id)
        {
            string ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf")
            {
                return BadRequest();
            }
             string filename = $"-I-{id}-" + Guid.NewGuid().ToString() + ext;

            if (await _documentService.ValidateAsync(file))
            {
                string? url = Url.Link("GetItemDocument", new { url = filename});
                var document = await _documentService.UploadAsync(id, url,  file, filename);
                return CreatedAtAction(nameof(GetItemDocument), new { url = filename } , document);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{url}", Name="GetItemDocument")]
        public async Task<IActionResult> GetItemDocument(string url)
        {
            var file = await _documentService.RetrieveAsync(url);
            if (file == null) { return NotFound(); }

            return File(file, "application/pdf");
        }

    }
}
