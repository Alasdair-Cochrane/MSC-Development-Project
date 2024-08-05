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
        private readonly IUnitRepository _unitRepository;

        public NotesService(INoteRepository noteRepository, IUnitRepository unitRepository)
        {
            _noteRepository = noteRepository;
            _unitRepository = unitRepository;
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
            
            list = await _noteRepository.AssignUserCanDeleteAsync(list, userId);
            return list;
        }
        public async Task DeleteNoteAsync(int noteId, int userId)
        {
            if(await _noteRepository.CheckUserCanDeleteAsync(noteId, userId)) 
            {
                await _noteRepository.DeleteNoteAsync(noteId);
            }
            else
            {
                throw new UnauthorizedAccessException($"User {userId} is not authorised to delete note {noteId}");
            }
        }

        public async Task<IEnumerable<ItemNoteDTO>> GetNotesBeforeNowAsync(int days, int userId)
        {
            var units = await _unitRepository.GetAllRelevantUnitIdsToUserAsync(userId);
            var notes = await _noteRepository.GetNotesInTimePeriodAsync(days, units);
            return notes;
        }

    }
    public interface INotesService
    {
        Task<ItemNoteDTO> AddNoteAsync(ItemNote note);
        Task DeleteNoteAsync(int noteId, int userId);
        Task<IEnumerable<ItemNoteDTO>> GetNotesBeforeNowAsync(int days, int userId);
        Task<IEnumerable<ItemNoteDTO>> GetNotesForItemAsync(int itemId, int userId);
    }

}
