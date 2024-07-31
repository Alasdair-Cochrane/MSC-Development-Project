using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{

    public class NotesService : INotesService
    {
        private readonly INoteRepository _noteRepository;

        public NotesService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<ItemNoteDTO> AddNoteAsync(ItemNote note)
        {
            var added = await _noteRepository.AddNoteAsync(note);
            return added;
        }

        public async Task<IEnumerable<ItemNoteDTO>> GetNotesForItemAsync(int itemId, int userId)
        {
            var list = await _noteRepository.GetNoteDTOsForItemAsync(itemId);
            if (list.IsNullOrEmpty())
            {
                return list;
            }
            
            list = await _noteRepository.AssignUserCanDelete(list, userId);
            return list;
        }
        public async Task DeleteNoteAsync(int noteId, int userId)
        {
            if(await _noteRepository.CheckUserCanDelete(noteId, userId)) 
            {
                await _noteRepository.DeleteNote(noteId);
            }
            else
            {
                throw new UnauthorizedAccessException($"User {userId} is not authorised to delete note {noteId}");
            }
        }

    }
    public interface INotesService
    {
        Task<ItemNoteDTO> AddNoteAsync(ItemNote note);
        Task DeleteNoteAsync(int noteId, int userId);
        Task<IEnumerable<ItemNoteDTO>> GetNotesForItemAsync(int itemId, int userId);
    }

}
