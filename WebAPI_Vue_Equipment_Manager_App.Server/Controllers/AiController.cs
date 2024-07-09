using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private ILabelInterpretationService _interpretationService;

        public AiController(ILabelInterpretationService service)
        {
           _interpretationService = service;
        }

        [HttpPost]

        public async Task<IActionResult> ReadLabel(IFormFile image)
        {
            return  Ok( await _interpretationService.InterpretLabelAsync(image));
        }


    }
}
