﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)  { 
            
            _documentService = documentService;        
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf")
            {
                return BadRequest();
            }
             string uri = "-D-" + Guid.NewGuid().ToString() + ext;

            if (await _documentService.ValidateAsync(file))
            {
                 await _documentService.UploadAsync(file, uri);
                return CreatedAtAction(nameof(GetDocument), new { url = uri });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{url}", Name="GetDocument")]
        public async Task<IActionResult> GetDocument(string url)
        {
            var file = await _documentService.RetrieveAsync(url);
            if (file == null) { return NotFound(); }

            return File(file, "application/pdf");
        }

        [HttpDelete("{uri}")]
        public async Task<IActionResult> DeleteDocument(string uri)
        {
            throw new NotImplementedException();
        }

    }
}
