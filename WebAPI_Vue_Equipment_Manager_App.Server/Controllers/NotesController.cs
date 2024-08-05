using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/items/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        private readonly IUserService _userService;

        public NotesController(INotesService notesService, IUserService userService)
        {
            _notesService = notesService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertNote(ItemNotePostDTO note)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            var post = note.ToEntity(user);
            var added = await _notesService.AddNoteAsync(post);
            return Ok(added);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            await _notesService.DeleteNoteAsync(id, user.Id);
            return NoContent();
        }
        [HttpGet]
        [Route("~/api/items/{itemId}/notes")]
        public async Task<IActionResult> GetNotes(int itemId)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            var notes = await _notesService.GetNotesForItemAsync(itemId, user.Id);
            return Ok(notes);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotesBeforeNow([FromQuery] int daysBeforeNow)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            var notes = await _notesService.GetNotesBeforeNowAsync(daysBeforeNow, user.Id);
            return Ok(notes);
        }
    }
}
