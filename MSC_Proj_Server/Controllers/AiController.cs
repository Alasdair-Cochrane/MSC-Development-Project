using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Startup;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{

    //Used for reading relevant equiment data from user provided images, and returning results to the user

    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [Authorize]
    public class AiController : ControllerBase
    {
        private ILabelInterpretationService _interpretationService;
        private readonly ILogger<AiController> _logger;

        public AiController(ILabelInterpretationService service, ILogger<AiController> logger)
        {
           _interpretationService = service;
            _logger = logger;
        }

        [HttpPost]

        public async Task<IActionResult> ReadLabel(IFormFile image)
        {
            var result = await _interpretationService.InterpretLabelAsync(image);
            return  Ok(result);
        }


    }
}

