using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private ILabelRecognitionService _labelRecognitionService;

        public AiController(ILabelRecognitionService labelRecognitionService)
        {
            _labelRecognitionService = labelRecognitionService;
        }

        [HttpPost]

        public async Task<IActionResult> ReadLabel(IFormFile image)
        {
            return  Ok( await _labelRecognitionService.ReadImageAsync(image));
        }


    }
}
